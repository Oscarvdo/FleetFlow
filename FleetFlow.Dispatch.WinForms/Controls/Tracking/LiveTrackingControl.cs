using System.Text.Json;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Tracking;
using Microsoft.Web.WebView2.Core;

namespace FleetFlow.Dispatch.WinForms.Controls.Tracking;

public partial class LiveTrackingControl : UserControl
{
    private static readonly JsonSerializerOptions JsonOptions =
        new()
        {
            PropertyNamingPolicy =
                JsonNamingPolicy.CamelCase
        };

    private readonly ILiveTrackingService? _trackingService;
    private readonly ILiveTrackingSimulationEngine? _simulationEngine;
    private readonly bool _canManageSimulation;
    private readonly System.Windows.Forms.Timer _refreshTimer;

    private IReadOnlyList<LiveTrackingVehicleItem> _vehicles =
        Array.Empty<LiveTrackingVehicleItem>();

    private bool _mapReady;
    private bool _loading;
    private bool _initializingMap;
    private bool _eventsSubscribed;
    private int _refreshPending;

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public LiveTrackingControl()
    {
        InitializeComponent();

        _refreshTimer =
    new System.Windows.Forms.Timer(components)
    {
        Interval = 2000
    };

        WireControlEvents();
        UpdateSimulationButtons();
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public LiveTrackingControl(
        ILiveTrackingService trackingService,
        ILiveTrackingSimulationEngine simulationEngine,
        bool canManageSimulation = true)
        : this()
    {
        _trackingService = trackingService;
        _simulationEngine = simulationEngine;
        _canManageSimulation = canManageSimulation;

        ConfigurePermissions();
        SubscribeToSimulationEvents();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (DesignMode ||
            _trackingService is null ||
            _initializingMap)
        {
            return;
        }

        _initializingMap = true;

        try
        {
            await InitializeMapAsync();
            await RefreshTrackingAsync();
            _refreshTimer.Start();
        }
        catch (Exception exception)
        {
            ShowError(
                "Live Tracking could not be initialized.",
                exception);
        }
        finally
        {
            _initializingMap = false;
        }
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);

        if (DesignMode)
        {
            return;
        }

        if (Visible &&
            _trackingService is not null &&
            _mapReady)
        {
            _refreshTimer.Start();
            ScheduleRefresh();
        }
        else
        {
            _refreshTimer.Stop();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        if (Disposing)
        {
            _refreshTimer.Stop();
            UnsubscribeFromSimulationEvents();
        }

        base.OnHandleDestroyed(e);
    }

    private void WireControlEvents()
    {
        _refreshTimer.Tick += refreshTimer_Tick;
        btnRefresh.Click += btnRefresh_Click;
        btnStart.Click += btnStart_Click;
        btnPause.Click += btnPause_Click;
        btnStop.Click += btnStop_Click;
        btnFitAll.Click += btnFitAll_Click;
        dgvVehicles.SelectionChanged +=
            dgvVehicles_SelectionChanged;
    }

    private void ConfigurePermissions()
    {
        btnStart.Visible = _canManageSimulation;
        btnPause.Visible = _canManageSimulation;
        btnStop.Visible = _canManageSimulation;

        nudVehicleCount.Enabled = _canManageSimulation;
        nudTimeScale.Enabled = _canManageSimulation;
    }

    private void SubscribeToSimulationEvents()
    {
        if (_simulationEngine is null ||
            _eventsSubscribed)
        {
            return;
        }

        _simulationEngine.TelemetryProduced +=
            simulationEngine_TelemetryProduced;

        _simulationEngine.StatusChanged +=
            simulationEngine_StatusChanged;

        _eventsSubscribed = true;
    }

    private void UnsubscribeFromSimulationEvents()
    {
        if (_simulationEngine is null ||
            !_eventsSubscribed)
        {
            return;
        }

        _simulationEngine.TelemetryProduced -=
            simulationEngine_TelemetryProduced;

        _simulationEngine.StatusChanged -=
            simulationEngine_StatusChanged;

        _eventsSubscribed = false;
    }

    private async Task InitializeMapAsync()
    {
        string mapFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "MapAssets",
            "live-tracking-map.html");

        if (!File.Exists(mapFilePath))
        {
            throw new FileNotFoundException(
                "The Live Tracking map file was not copied to the output directory.",
                mapFilePath);
        }

        await webViewMap.EnsureCoreWebView2Async();

        webViewMap.CoreWebView2.Settings.AreDefaultContextMenusEnabled =
            false;

        webViewMap.CoreWebView2.Settings.AreDevToolsEnabled =
            true;

        webViewMap.CoreWebView2.Settings.IsStatusBarEnabled =
            false;

        webViewMap.CoreWebView2.WebMessageReceived +=
            webViewMap_WebMessageReceived;

        Uri mapUri = new(mapFilePath);

        webViewMap.CoreWebView2.Navigate(
            mapUri.AbsoluteUri);

        lblLastUpdate.Text = "Loading OpenStreetMap...";
    }

    private async void webViewMap_WebMessageReceived(
        object? sender,
        CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            string json = e.WebMessageAsJson;

            using JsonDocument document =
                JsonDocument.Parse(json);

            JsonElement root =
                document.RootElement;

            if (!root.TryGetProperty(
                    "type",
                    out JsonElement typeElement))
            {
                return;
            }

            string? messageType =
                typeElement.GetString();

            if (string.Equals(
                    messageType,
                    "map-ready",
                    StringComparison.OrdinalIgnoreCase))
            {
                _mapReady = true;
                lblLastUpdate.Text = "Map ready";

                await SendVehiclesToMapAsync();
                return;
            }

            if (string.Equals(
                    messageType,
                    "vehicle-selected",
                    StringComparison.OrdinalIgnoreCase) &&
                root.TryGetProperty(
                    "vehicleId",
                    out JsonElement vehicleElement) &&
                vehicleElement.TryGetInt64(
                    out long vehicleId))
            {
                SelectVehicleInGrid(vehicleId);
                await LoadSelectedTripAsync(vehicleId);
            }
        }
        catch (Exception exception)
        {
            ShowError(
                "The map message could not be processed.",
                exception);
        }
    }

    private async void refreshTimer_Tick(
        object? sender,
        EventArgs e)
    {
        await RefreshTrackingAsync();
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await RefreshTrackingAsync();
    }

    private async void btnStart_Click(
        object? sender,
        EventArgs e)
    {
        if (_simulationEngine is null ||
            !_canManageSimulation)
        {
            return;
        }

        SetToolbarBusy(true);

        try
        {
            CreateSimulationRunRequest request =
                new()
                {
                    Name =
                        $"FleetFlow Simulation {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    ScenarioCode =
                        "NORMAL_OPERATION",
                    RandomSeed =
                        Environment.TickCount,
                    TimeScale =
                        nudTimeScale.Value,
                    UpdateIntervalMilliseconds =
                        1000,
                    PlannedVehicleCount =
                        Decimal.ToInt32(
                            nudVehicleCount.Value),
                    ConfigurationJson =
                        """
                        {
                          "source": "FleetFlow.Dispatch.WinForms",
                          "mode": "MULTI_TASK",
                          "map": "OPENSTREETMAP"
                        }
                        """
                };

            SimulationRunCommandResult run =
                await _simulationEngine.StartAsync(request);

            lblSimulationStatus.Text =
                $"RUNNING — Run {run.SimulationRunId}";

            UpdateSimulationButtons();

            await RefreshTrackingAsync();
        }
        catch (Exception exception)
        {
            ShowError(
                "The simulation could not be started.",
                exception);
        }
        finally
        {
            SetToolbarBusy(false);
            UpdateSimulationButtons();
        }
    }

    private async void btnPause_Click(
        object? sender,
        EventArgs e)
    {
        if (_simulationEngine is null ||
            !_simulationEngine.IsRunning)
        {
            return;
        }

        SetToolbarBusy(true);

        try
        {
            if (_simulationEngine.IsPaused)
            {
                await _simulationEngine.ResumeAsync();
            }
            else
            {
                await _simulationEngine.PauseAsync();
            }

            UpdateSimulationButtons();
        }
        catch (Exception exception)
        {
            ShowError(
                "The simulation state could not be changed.",
                exception);
        }
        finally
        {
            SetToolbarBusy(false);
            UpdateSimulationButtons();
        }
    }

    private async void btnStop_Click(
        object? sender,
        EventArgs e)
    {
        if (_simulationEngine is null ||
            !_simulationEngine.IsRunning)
        {
            return;
        }

        DialogResult confirmation =
            MessageBox.Show(
                "Do you want to stop the current simulation?",
                "FleetFlow",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        SetToolbarBusy(true);

        try
        {
            await _simulationEngine.StopAsync();
            await RefreshTrackingAsync();
        }
        catch (Exception exception)
        {
            ShowError(
                "The simulation could not be stopped.",
                exception);
        }
        finally
        {
            SetToolbarBusy(false);
            UpdateSimulationButtons();
        }
    }

    private async void btnFitAll_Click(
        object? sender,
        EventArgs e)
    {
        await ExecuteMapCommandAsync(
            "window.fleetFlowMap.fitAll();");
    }

    private async void dgvVehicles_SelectionChanged(
        object? sender,
        EventArgs e)
    {
        if (dgvVehicles.CurrentRow?.DataBoundItem
            is not LiveTrackingVehicleItem vehicle)
        {
            lblSelectedVehicle.Text =
                "No vehicle selected";

            return;
        }

        lblSelectedVehicle.Text =
            BuildSelectedVehicleText(vehicle);

        await ExecuteMapCommandAsync(
            $"window.fleetFlowMap.selectVehicle({vehicle.VehicleId}, true);");

        await LoadSelectedTripAsync(
            vehicle.VehicleId);
    }

    private void simulationEngine_TelemetryProduced(
        IReadOnlyList<VehicleTelemetryUpdate> telemetry)
    {
        ScheduleRefresh();
    }

    private void simulationEngine_StatusChanged(
        string status)
    {
        if (IsDisposed ||
            Disposing)
        {
            return;
        }

        if (InvokeRequired)
        {
            BeginInvoke(
                new Action<string>(
                    simulationEngine_StatusChanged),
                status);

            return;
        }

        lblSimulationStatus.Text = status;
        UpdateSimulationButtons();
        ScheduleRefresh();
    }

    private void ScheduleRefresh()
    {
        if (IsDisposed ||
            Disposing)
        {
            return;
        }

        if (Interlocked.Exchange(
                ref _refreshPending,
                1) == 1)
        {
            return;
        }

        void QueueRefresh()
        {
            _ = RefreshScheduledAsync();
        }

        if (InvokeRequired)
        {
            BeginInvoke(
                new Action(QueueRefresh));
        }
        else
        {
            QueueRefresh();
        }
    }

    private async Task RefreshScheduledAsync()
    {
        try
        {
            await RefreshTrackingAsync();
        }
        finally
        {
            Interlocked.Exchange(
                ref _refreshPending,
                0);
        }
    }

    private async Task RefreshTrackingAsync()
    {
        if (_trackingService is null ||
            _loading ||
            IsDisposed)
        {
            return;
        }

        _loading = true;
        btnRefresh.Enabled = false;

        try
        {
            _vehicles =
                await _trackingService.GetMapStateAsync(
                    includeOffline: true,
                    offlineAfterSeconds: 60,
                    simulationRunId: null);

            long? selectedVehicleId =
                GetSelectedVehicleId();

            dgvVehicles.DataSource =
                _vehicles.ToList();

            lblVehicleSummary.Text =
                BuildVehicleSummary(_vehicles);

            RestoreVehicleSelection(
                selectedVehicleId);

            await SendVehiclesToMapAsync();

            lblLastUpdate.Text =
                $"Updated {DateTime.Now:g}";
        }
        catch (Exception exception)
        {
            ShowError(
                "Live Tracking positions could not be loaded.",
                exception);
        }
        finally
        {
            _loading = false;
            btnRefresh.Enabled = true;
        }
    }

    private async Task SendVehiclesToMapAsync()
    {
        if (!_mapReady)
        {
            return;
        }

        string vehiclesJson =
            JsonSerializer.Serialize(
                _vehicles,
                JsonOptions);

        await ExecuteMapCommandAsync(
            $"window.fleetFlowMap.setVehicles({vehiclesJson});");
    }

    private async Task LoadSelectedTripAsync(
        long vehicleId)
    {
        if (_trackingService is null ||
            !_mapReady)
        {
            return;
        }

        LiveTrackingVehicleItem? vehicle =
            _vehicles.FirstOrDefault(
                item => item.VehicleId == vehicleId);

        if (vehicle?.TripId is not long tripId)
        {
            await ExecuteMapCommandAsync(
                "window.fleetFlowMap.setRoute([]);");

            await ExecuteMapCommandAsync(
                "window.fleetFlowMap.setStops([]);");

            return;
        }

        try
        {
            Task<IReadOnlyList<LiveTrackingRoutePoint>>
                routeTask =
                    _trackingService.GetTripRouteAsync(
                        tripId);

            Task<IReadOnlyList<LiveTrackingStopItem>>
                stopsTask =
                    _trackingService.GetTripStopsAsync(
                        tripId);

            await Task.WhenAll(
                routeTask,
                stopsTask);

            IReadOnlyList<LiveTrackingRoutePoint> route =
                await routeTask;

            IReadOnlyList<LiveTrackingStopItem> stops =
                await stopsTask;

            string routeJson =
                JsonSerializer.Serialize(
                    route,
                    JsonOptions);

            string stopsJson =
                JsonSerializer.Serialize(
                    stops,
                    JsonOptions);

            await ExecuteMapCommandAsync(
                $"window.fleetFlowMap.setRoute({routeJson});");

            await ExecuteMapCommandAsync(
                $"window.fleetFlowMap.setStops({stopsJson});");
        }
        catch (Exception exception)
        {
            ShowError(
                "The selected trip route could not be loaded.",
                exception);
        }
    }

    private async Task ExecuteMapCommandAsync(
        string script)
    {
        if (!_mapReady ||
            webViewMap.CoreWebView2 is null)
        {
            return;
        }

        await webViewMap.CoreWebView2
            .ExecuteScriptAsync(script);
    }

    private void SelectVehicleInGrid(
        long vehicleId)
    {
        foreach (DataGridViewRow row
                 in dgvVehicles.Rows)
        {
            if (row.DataBoundItem
                is LiveTrackingVehicleItem vehicle &&
                vehicle.VehicleId == vehicleId)
            {
                row.Selected = true;
                dgvVehicles.CurrentCell =
                    row.Cells[0];

                return;
            }
        }
    }

    private long? GetSelectedVehicleId()
    {
        return dgvVehicles.CurrentRow?.DataBoundItem
            is LiveTrackingVehicleItem vehicle
                ? vehicle.VehicleId
                : null;
    }

    private void RestoreVehicleSelection(
        long? vehicleId)
    {
        if (vehicleId.HasValue)
        {
            SelectVehicleInGrid(
                vehicleId.Value);

            return;
        }

        if (dgvVehicles.Rows.Count > 0)
        {
            dgvVehicles.Rows[0].Selected = true;
            dgvVehicles.CurrentCell =
                dgvVehicles.Rows[0].Cells[0];
        }
    }

    private void UpdateSimulationButtons()
    {
        bool running =
            _simulationEngine?.IsRunning == true;

        bool paused =
            _simulationEngine?.IsPaused == true;

        btnStart.Enabled =
            _canManageSimulation &&
            !running;

        btnPause.Enabled =
            _canManageSimulation &&
            running;

        btnPause.Text =
            paused
                ? "Resume"
                : "Pause";

        btnStop.Enabled =
            _canManageSimulation &&
            running;

        nudVehicleCount.Enabled =
            _canManageSimulation &&
            !running;

        nudTimeScale.Enabled =
            _canManageSimulation &&
            !running;

        if (_simulationEngine is null)
        {
            lblSimulationStatus.Text =
                "SIMULATION UNAVAILABLE";
        }
        else if (!running &&
                 lblSimulationStatus.Text.StartsWith(
                     "RUNNING",
                     StringComparison.OrdinalIgnoreCase))
        {
            lblSimulationStatus.Text =
                "READY";
        }
    }

    private void SetToolbarBusy(bool busy)
    {
        btnRefresh.Enabled = !busy;
        btnFitAll.Enabled = !busy;
        UseWaitCursor = busy;

        if (busy)
        {
            lblLastUpdate.Text =
                "Processing...";
        }
    }

    private static string BuildVehicleSummary(
        IReadOnlyList<LiveTrackingVehicleItem> vehicles)
    {
        int moving =
            vehicles.Count(
                vehicle =>
                    string.Equals(
                        vehicle.TrackingStatus,
                        "MOVING",
                        StringComparison.OrdinalIgnoreCase));

        int offline =
            vehicles.Count(
                vehicle =>
                    vehicle.IsOffline);

        return
            $"{vehicles.Count} vehicles · {moving} moving · {offline} offline";
    }

    private static string BuildSelectedVehicleText(
        LiveTrackingVehicleItem vehicle)
    {
        string trip =
            string.IsNullOrWhiteSpace(vehicle.TripNumber)
                ? "No active trip"
                : vehicle.TripNumber;

        return $"{vehicle.UnitNumber} · {trip}";
    }

    private static void ShowError(
        string message,
        Exception exception)
    {
        MessageBox.Show(
            $"{message}\n\n{exception.Message}",
            "FleetFlow",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}