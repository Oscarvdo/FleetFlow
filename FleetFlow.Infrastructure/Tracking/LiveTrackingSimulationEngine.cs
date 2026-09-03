using System.Threading.Channels;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Tracking;

namespace FleetFlow.Infrastructure.Tracking;

public sealed class LiveTrackingSimulationEngine
    : ILiveTrackingSimulationEngine
{
    private const int MaximumBatchSize = 100;

    private readonly ILiveTrackingService _trackingService;
    private readonly ILiveTrackingTelemetryWriter _telemetryWriter;
    private readonly ILiveTrackingSimulationRunService _runService;
    private readonly TruckRouteInterpolator _interpolator;
    private readonly SemaphoreSlim _lifecycleLock;
    private readonly AsyncManualResetEvent _pauseGate;

    private CancellationTokenSource? _simulationCancellation;
    private Channel<VehicleTelemetryUpdate>? _telemetryChannel;
    private List<Task> _truckTasks;
    private Task? _consumerTask;
    private Task? _monitorTask;

    private long _currentSimulationRunId;
    private bool _isRunning;
    private bool _isPaused;
    private bool _disposed;

    public LiveTrackingSimulationEngine(
        ILiveTrackingService trackingService,
        ILiveTrackingTelemetryWriter telemetryWriter,
        ILiveTrackingSimulationRunService runService)
    {
        _trackingService = trackingService;
        _telemetryWriter = telemetryWriter;
        _runService = runService;
        _interpolator = new TruckRouteInterpolator();
        _lifecycleLock = new SemaphoreSlim(1, 1);
        _pauseGate = new AsyncManualResetEvent(true);
        _truckTasks = new List<Task>();
    }

    public bool IsRunning => _isRunning;

    public bool IsPaused => _isPaused;

    public long? CurrentSimulationRunId =>
        _currentSimulationRunId > 0
            ? _currentSimulationRunId
            : null;

    public int ActiveTruckCount =>
        _truckTasks.Count(task => !task.IsCompleted);

    public event Action<IReadOnlyList<VehicleTelemetryUpdate>>?
        TelemetryProduced;

    public event Action<string>?
        StatusChanged;

    public async Task<SimulationRunCommandResult> StartAsync(
        CreateSimulationRunRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ThrowIfDisposed();

        await _lifecycleLock.WaitAsync(cancellationToken);

        try
        {
            if (_isRunning)
            {
                throw new InvalidOperationException(
                    "A live tracking simulation is already running.");
            }

            IReadOnlyList<LiveTrackingSimulationCandidate> candidates =
                await _trackingService.GetSimulationCandidatesAsync(
                    cancellationToken);

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    "No assigned trips with valid route points are available.");
            }

            int requestedVehicleCount =
                Math.Clamp(
                    request.PlannedVehicleCount,
                    1,
                    candidates.Count);

            LiveTrackingSimulationCandidate[] selectedCandidates =
                candidates
                    .Take(requestedVehicleCount)
                    .ToArray();

            List<TruckSimulationState> truckStates =
                await CreateTruckStatesAsync(
                    selectedCandidates,
                    cancellationToken);

            if (truckStates.Count == 0)
            {
                throw new InvalidOperationException(
                    "No valid truck routes could be loaded.");
            }

            CreateSimulationRunRequest normalizedRequest =
                new()
                {
                    Name = request.Name,
                    ScenarioCode = request.ScenarioCode,
                    RandomSeed = request.RandomSeed,
                    TimeScale = request.TimeScale,
                    UpdateIntervalMilliseconds =
                        request.UpdateIntervalMilliseconds,
                    PlannedVehicleCount = truckStates.Count,
                    ConfigurationJson =
                        request.ConfigurationJson,
                    CreatedByAppUserId =
                        request.CreatedByAppUserId
                };

            SimulationRunCommandResult run =
                await _runService.CreateAsync(
                    normalizedRequest,
                    cancellationToken);

            _currentSimulationRunId =
                run.SimulationRunId;

            try
            {
                await _runService.SetStatusAsync(
                    run.SimulationRunId,
                    "RUNNING",
                    cancellationToken);

                _simulationCancellation =
                    new CancellationTokenSource();

                _telemetryChannel =
                    Channel.CreateUnbounded<VehicleTelemetryUpdate>(
                        new UnboundedChannelOptions
                        {
                            SingleReader = true,
                            SingleWriter = false,
                            AllowSynchronousContinuations = false
                        });

                _truckTasks = new List<Task>(
                    truckStates.Count);

                _isRunning = true;
                _isPaused = false;
                _pauseGate.Set();

                CancellationToken simulationToken =
                    _simulationCancellation.Token;

                _consumerTask =
                    ConsumeTelemetryAsync(
                        _telemetryChannel.Reader,
                        simulationToken);

                foreach (TruckSimulationState state in truckStates)
                {
                    Task truckTask =
                        RunTruckAsync(
                            state,
                            run.SimulationRunId,
                            normalizedRequest.TimeScale,
                            normalizedRequest
                                .UpdateIntervalMilliseconds,
                            _telemetryChannel.Writer,
                            simulationToken);

                    _truckTasks.Add(truckTask);
                }

                _monitorTask =
                    MonitorSimulationAsync(
                        run.SimulationRunId,
                        _truckTasks,
                        _consumerTask,
                        _telemetryChannel.Writer,
                        simulationToken);

                RaiseStatusChanged(
                    $"RUNNING — {truckStates.Count} trucks");

                return run;
            }
            catch
            {
                await TrySetRunStatusAsync(
                    run.SimulationRunId,
                    "FAILED");

                ResetRuntimeState();
                throw;
            }
        }
        finally
        {
            _lifecycleLock.Release();
        }
    }

    public async Task PauseAsync(
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        await _lifecycleLock.WaitAsync(cancellationToken);

        try
        {
            EnsureRunning();

            if (_isPaused)
            {
                return;
            }

            long runId = _currentSimulationRunId;

            await _runService.SetStatusAsync(
                runId,
                "PAUSED",
                cancellationToken);

            _pauseGate.Reset();
            _isPaused = true;

            RaiseStatusChanged(
                $"PAUSED — {ActiveTruckCount} trucks");
        }
        finally
        {
            _lifecycleLock.Release();
        }
    }

    public async Task ResumeAsync(
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        await _lifecycleLock.WaitAsync(cancellationToken);

        try
        {
            EnsureRunning();

            if (!_isPaused)
            {
                return;
            }

            long runId = _currentSimulationRunId;

            await _runService.SetStatusAsync(
                runId,
                "RUNNING",
                cancellationToken);

            _pauseGate.Set();
            _isPaused = false;

            RaiseStatusChanged(
                $"RUNNING — {ActiveTruckCount} trucks");
        }
        finally
        {
            _lifecycleLock.Release();
        }
    }

    public async Task StopAsync(
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        Task? monitorTask;
        long runId;

        await _lifecycleLock.WaitAsync(cancellationToken);

        try
        {
            if (!_isRunning)
            {
                return;
            }

            runId = _currentSimulationRunId;
            monitorTask = _monitorTask;

            _pauseGate.Set();
            _isPaused = false;
            _simulationCancellation?.Cancel();
        }
        finally
        {
            _lifecycleLock.Release();
        }

        if (monitorTask is not null)
        {
            try
            {
                await monitorTask.WaitAsync(
                    cancellationToken);
            }
            catch (OperationCanceledException)
                when (!cancellationToken.IsCancellationRequested)
            {
                // Expected when the internal simulation token is cancelled.
            }
        }

        await TrySetRunStatusAsync(
            runId,
            "CANCELLED");

        await _lifecycleLock.WaitAsync(cancellationToken);

        try
        {
            ResetRuntimeState();
            RaiseStatusChanged("CANCELLED");
        }
        finally
        {
            _lifecycleLock.Release();
        }
    }

    private async Task<List<TruckSimulationState>>
        CreateTruckStatesAsync(
            IReadOnlyList<LiveTrackingSimulationCandidate> candidates,
            CancellationToken cancellationToken)
    {
        List<TruckSimulationState> states =
            new(candidates.Count);

        foreach (LiveTrackingSimulationCandidate candidate
                 in candidates)
        {
            IReadOnlyList<LiveTrackingRoutePoint> route =
                await _trackingService.GetTripRouteAsync(
                    candidate.TripId,
                    cancellationToken);

            if (route.Count < 2)
            {
                continue;
            }

            TruckSimulationState state =
                new(
                    candidate,
                    route,
                    initialFuelPercent: 100M,
                    initialOdometerMiles: 0M);

            states.Add(state);
        }

        return states;
    }

    private async Task RunTruckAsync(
        TruckSimulationState state,
        long simulationRunId,
        decimal timeScale,
        int updateIntervalMilliseconds,
        ChannelWriter<VehicleTelemetryUpdate> writer,
        CancellationToken cancellationToken)
    {
        VehicleTelemetryUpdate initialTelemetry =
            state.CreateTelemetry(
                simulationRunId,
                DateTime.UtcNow);

        await writer.WriteAsync(
            initialTelemetry,
            cancellationToken);

        using PeriodicTimer timer =
            new(
                TimeSpan.FromMilliseconds(
                    updateIntervalMilliseconds));

        while (!state.IsCompleted)
        {
            await _pauseGate.WaitAsync(
                cancellationToken);

            bool hasNextTick =
                await timer.WaitForNextTickAsync(
                    cancellationToken);

            if (!hasNextTick)
            {
                break;
            }

            await _pauseGate.WaitAsync(
                cancellationToken);

            VehicleTelemetryUpdate telemetry =
                _interpolator.Advance(
                    state,
                    simulationRunId,
                    TimeSpan.FromMilliseconds(
                        updateIntervalMilliseconds),
                    timeScale,
                    DateTime.UtcNow);

            await writer.WriteAsync(
                telemetry,
                cancellationToken);
        }
    }

    private async Task ConsumeTelemetryAsync(
        ChannelReader<VehicleTelemetryUpdate> reader,
        CancellationToken cancellationToken)
    {
        List<VehicleTelemetryUpdate> batch =
            new(MaximumBatchSize);

        try
        {
            await foreach (
                VehicleTelemetryUpdate telemetry
                in reader.ReadAllAsync(cancellationToken))
            {
                batch.Add(telemetry);

                int desiredBatchSize =
                    Math.Clamp(
                        ActiveTruckCount,
                        1,
                        MaximumBatchSize);

                if (batch.Count >= desiredBatchSize)
                {
                    await FlushBatchAsync(
                        batch,
                        cancellationToken);
                }
            }

            if (batch.Count > 0)
            {
                await FlushBatchAsync(
                    batch,
                    cancellationToken);
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            // Cancellation is part of the normal Stop operation.
        }
    }

    private async Task FlushBatchAsync(
        List<VehicleTelemetryUpdate> batch,
        CancellationToken cancellationToken)
    {
        if (batch.Count == 0)
        {
            return;
        }

        VehicleTelemetryUpdate[] snapshot =
            batch.ToArray();

        batch.Clear();

        await _telemetryWriter.AppendBatchAsync(
            snapshot,
            cancellationToken);

        TelemetryProduced?.Invoke(snapshot);
    }

    private async Task MonitorSimulationAsync(
        long simulationRunId,
        IReadOnlyCollection<Task> truckTasks,
        Task consumerTask,
        ChannelWriter<VehicleTelemetryUpdate> writer,
        CancellationToken cancellationToken)
    {
        try
        {
            await Task.WhenAll(truckTasks);

            writer.TryComplete();

            await consumerTask;

            if (!cancellationToken.IsCancellationRequested)
            {
                await TrySetRunStatusAsync(
                    simulationRunId,
                    "COMPLETED");

                _isRunning = false;
                _isPaused = false;

                RaiseStatusChanged("COMPLETED");
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            writer.TryComplete();
            _isRunning = false;
            _isPaused = false;
        }
        catch (Exception exception)
        {
            writer.TryComplete(exception);

            await TrySetRunStatusAsync(
                simulationRunId,
                "FAILED");

            _isRunning = false;
            _isPaused = false;

            RaiseStatusChanged(
                $"FAILED — {exception.Message}");
        }
    }

    private async Task TrySetRunStatusAsync(
        long simulationRunId,
        string status)
    {
        if (simulationRunId <= 0)
        {
            return;
        }

        try
        {
            await _runService.SetStatusAsync(
                simulationRunId,
                status,
                CancellationToken.None);
        }
        catch
        {
            // Preserve the original simulation error.
        }
    }

    private void EnsureRunning()
    {
        if (!_isRunning ||
            _currentSimulationRunId <= 0)
        {
            throw new InvalidOperationException(
                "No live tracking simulation is running.");
        }
    }

    private void RaiseStatusChanged(string status)
    {
        StatusChanged?.Invoke(status);
    }

    private void ResetRuntimeState()
    {
        _simulationCancellation?.Dispose();
        _simulationCancellation = null;
        _telemetryChannel = null;
        _consumerTask = null;
        _monitorTask = null;
        _truckTasks = new List<Task>();
        _currentSimulationRunId = 0;
        _isRunning = false;
        _isPaused = false;
        _pauseGate.Set();
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_isRunning)
        {
            await StopAsync();
        }

        _disposed = true;
        _simulationCancellation?.Dispose();
        _lifecycleLock.Dispose();

        GC.SuppressFinalize(this);
    }
}