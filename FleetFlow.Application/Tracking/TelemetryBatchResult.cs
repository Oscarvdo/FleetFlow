namespace FleetFlow.Application.Tracking;

public sealed class TelemetryBatchResult
{
    public int InsertedRows { get; init; }

    public long SubmittedRows { get; init; }

    public int DuplicateRows =>
        Math.Max(
            0,
            checked((int)SubmittedRows) - InsertedRows);

    public bool AllRowsInserted =>
        InsertedRows == SubmittedRows;
}