using Dapper;
using FleetFlow.Application.Dashboard; 
using FleetFlow.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using FleetFlow.Application.Abstractions.Dashboard;

namespace FleetFlow.Infrastructure.Dashboard
{
    public sealed class SqlDashboardService : IDashboardService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqlDashboardService(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<DashboardSummary> GetSummaryAsync(
            CancellationToken cancellationToken = default)
        {
            await using DbConnection connection =
                _connectionFactory.CreateConnection();

            DashboardSummary summary =
                await connection.QuerySingleAsync<DashboardSummary>(
                    new CommandDefinition(
                        commandText:
                            "dispatch.Dashboard_GetSummary",
                        commandType:
                            CommandType.StoredProcedure,
                        cancellationToken:
                            cancellationToken));

            return summary;
        }
    }
}
