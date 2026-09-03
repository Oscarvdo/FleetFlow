namespace FleetFlow.Infrastructure.Tracking;

/// <summary>
/// Permite pausar y reanudar operaciones asincrónicas
/// sin bloquear un hilo físico.
/// </summary>
internal sealed class AsyncManualResetEvent
{
    private volatile TaskCompletionSource<bool> _completionSource;

    public AsyncManualResetEvent(
        bool initialState = false)
    {
        _completionSource =
            CreateCompletionSource();

        if (initialState)
        {
            _completionSource.TrySetResult(true);
        }
    }

    public bool IsSet =>
        _completionSource.Task.IsCompleted;

    public Task WaitAsync(
        CancellationToken cancellationToken = default)
    {
        Task waitTask =
            _completionSource.Task;

        if (!cancellationToken.CanBeCanceled)
        {
            return waitTask;
        }

        return waitTask.WaitAsync(
            cancellationToken);
    }

    public void Set()
    {
        _completionSource.TrySetResult(true);
    }

    public void Reset()
    {
        while (true)
        {
            TaskCompletionSource<bool> current =
                _completionSource;

            if (!current.Task.IsCompleted)
            {
                return;
            }

            TaskCompletionSource<bool> replacement =
                CreateCompletionSource();

            if (ReferenceEquals(
                    Interlocked.CompareExchange(
                        ref _completionSource,
                        replacement,
                        current),
                    current))
            {
                return;
            }
        }
    }

    private static TaskCompletionSource<bool>
        CreateCompletionSource()
    {
        return new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);
    }
}