using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Abstractions.Security;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Infrastructure.Customers;
using FleetFlow.Infrastructure.Dashboard;
using FleetFlow.Infrastructure.Data;
using FleetFlow.Infrastructure.Dispatch;
using FleetFlow.Infrastructure.Fleet;
using FleetFlow.Infrastructure.Loads;
using FleetFlow.Infrastructure.Security;
using FleetFlow.Infrastructure.Tracking;
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

        // Esta fábrica conserva la cadena de conexión.
        // Cada servicio crea y elimina su propia conexión SQL.
        services.AddSingleton<IDbConnectionFactory>(
            new SqlConnectionFactory(connectionString));

        // Autentica usuarios y recupera
        // sus roles y permisos.
        services.AddSingleton<
            IAuthenticationService,
            SqlAuthenticationService>();

        // Obtiene los indicadores generales
        // del dashboard.
        services.AddSingleton<
            IDashboardService,
            SqlDashboardService>();

        // Obtiene los viajes activos que aparecen
        // en el tablero de despacho.
        services.AddSingleton<
            IDispatchBoardService,
            SqlDispatchBoardService>();

        // Obtiene la información completa de un viaje,
        // incluyendo paradas e historial de estados.
        services.AddSingleton<
            ITripDetailsService,
            SqlTripDetailsService>();

        // Obtiene la lista general de viajes.
        services.AddSingleton<
            ITripListService,
            SqlTripListService>();

        // Obtiene la lista general de cargas.
        services.AddSingleton<
            ILoadListService,
            SqlLoadListService>();

        // Obtiene los detalles completos
        // de una carga seleccionada.
        services.AddSingleton<
            ILoadDetailsService,
            SqlLoadDetailsService>();

        // Ejecuta operaciones que modifican cargas,
        // comenzando con la creación de una carga.
        services.AddSingleton<
            ILoadCommandService,
            SqlLoadCommandService>();

        // Obtiene los clientes activos utilizados
        // por los controles de selección.
        services.AddSingleton<
            ICustomerLookupService,
            SqlCustomerLookupService>();

        // Proporciona el listado, detalle y operaciones
        // de mantenimiento del módulo Customers.
        services.AddSingleton<
            ICustomerService,
            SqlCustomerService>();

        // Proporciona el resumen y listado general
        // de camiones, remolques y conductores.
        services.AddSingleton<
            IFleetOverviewService,
            SqlFleetOverviewService>();

        // Ejecuta operaciones de mantenimiento
        // sobre camiones y remolques.
        services.AddSingleton<
            IFleetCommandService,
            SqlFleetCommandService>();

        // Proporciona posiciones actuales, rutas, paradas
        // y candidatos para Live Tracking.
        services.AddSingleton<
            ILiveTrackingService,
            SqlLiveTrackingService>();

        // Escribe en SQL Server los lotes de telemetría
        // producidos por los camiones simulados.
        services.AddSingleton<
            ILiveTrackingTelemetryWriter,
            SqlLiveTrackingTelemetryWriter>();

        // Crea y actualiza las ejecuciones
        // de la simulación.
        services.AddSingleton<
            ILiveTrackingSimulationRunService,
            SqlLiveTrackingSimulationRunService>();

        // Administra las simulaciones concurrentes.
        // Cada camión se ejecuta como una tarea independiente.
        services.AddSingleton<
            ILiveTrackingSimulationEngine,
            LiveTrackingSimulationEngine>();

        return services;
    }
}