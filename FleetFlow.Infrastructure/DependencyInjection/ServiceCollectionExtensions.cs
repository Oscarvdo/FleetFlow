using FleetFlow.Application.Abstractions.Security;
using FleetFlow.Infrastructure.Abstractions.Persistence;
using FleetFlow.Infrastructure.Data;
using FleetFlow.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetFlow.Infrastructure.DependencyInjection
{
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

            return services;
        }
    }
}
