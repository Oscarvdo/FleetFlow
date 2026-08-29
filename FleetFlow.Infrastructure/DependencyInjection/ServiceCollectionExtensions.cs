using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Security;
using FleetFlow.Infrastructure.Dashboard;
using FleetFlow.Infrastructure.Data;
using FleetFlow.Infrastructure.Dispatch;
using FleetFlow.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace FleetFlow.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFleetFlowInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "The FleetFlow database connection string is required.",
                nameof(connectionString));
        }

        services.AddSingleton<IDbConnectionFactory>(
            new SqlConnectionFactory(connectionString));

        services.AddSingleton<
            IAuthenticationService,
            SqlAuthenticationService>();

        services.AddSingleton<
            IDashboardService,
            SqlDashboardService>();

        services.AddSingleton<
            IDispatchBoardService,
            SqlDispatchBoardService>();

        return services;
    }
}