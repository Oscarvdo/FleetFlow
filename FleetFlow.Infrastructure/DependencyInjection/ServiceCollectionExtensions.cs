using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Security;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Infrastructure.Dashboard;
using FleetFlow.Infrastructure.Data;
using FleetFlow.Infrastructure.Dispatch;
using FleetFlow.Infrastructure.Loads;
using FleetFlow.Infrastructure.Security;
using FleetFlow.Infrastructure.Trips;
using Microsoft.Extensions.DependencyInjection;

namespace FleetFlow.Infrastructure.DependencyInjection;

/// <summary>
/// Registra los servicios de Infrastructure que utilizarán
/// las demás capas de FleetFlow.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configura el acceso a SQL Server y registra las
    /// implementaciones concretas de los servicios.
    /// </summary>
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

        // Se utiliza una sola fábrica porque únicamente conserva
        // la cadena de conexión; cada operación crea su conexión.
        services.AddSingleton<IDbConnectionFactory>(
            new SqlConnectionFactory(connectionString));

        // Servicio responsable de autenticar usuarios.
        services.AddSingleton<
            IAuthenticationService,
            SqlAuthenticationService>();

        // Servicio que obtiene los indicadores del dashboard.
        services.AddSingleton<
            IDashboardService,
            SqlDashboardService>();

        // Servicio que obtiene los viajes activos del despacho.
        services.AddSingleton<
            IDispatchBoardService,
            SqlDispatchBoardService>();

        // Servicio que obtiene los detalles de un viaje.
        services.AddSingleton<
            ITripDetailsService,
            SqlTripDetailsService>();

        // Servicio que obtiene y filtra la lista de viajes.
        services.AddSingleton<
            ITripListService,
            SqlTripListService>();

        // Servicio que obtiene y filtra la lista de cargas.
        services.AddSingleton<
            ILoadListService,
            SqlLoadListService>();

        // Servicio que obtiene los detalles completos
        // de una carga seleccionada.
        services.AddSingleton<
            ILoadDetailsService,
            SqlLoadDetailsService>();

        return services;
    }
}