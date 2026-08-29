using FleetFlow.Application.Dashboard;

namespace FleetFlow.Application.Abstractions.Dashboard;

public interface IDashboardService
{
    Task<DashboardSummary> GetSummaryAsync(
        CancellationToken cancellationToken = default);
}