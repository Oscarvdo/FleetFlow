using Dapper;
using FleetFlow.Application.Abstractions.Security;
using FleetFlow.Application.Authentication;
using FleetFlow.Domain.Security;
using FleetFlow.Infrastructure.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FleetFlow.Infrastructure.Security
{
    public sealed class SqlAuthenticationService : IAuthenticationService
    {
        private const string InvalidCredentialsMessage =
            "Invalid username or password.";

        private readonly IDbConnectionFactory _connectionFactory;

        public SqlAuthenticationService(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<LoginResult> AuthenticateAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return LoginResult.Failure(
                    "Username and password are required.");
            }

            string normalizedUsername = request.Username
                .Trim()
                .ToUpperInvariant();

            await using DbConnection connection =
                _connectionFactory.CreateConnection();

            LoginUserRecord? loginUser =
                await connection.QuerySingleOrDefaultAsync<LoginUserRecord>(
                    new CommandDefinition(
                        commandText: "security.AppUser_GetForLogin",
                        parameters: new
                        {
                            NormalizedUsername = normalizedUsername
                        },
                        commandType: CommandType.StoredProcedure,
                        cancellationToken: cancellationToken));

            if (loginUser is null)
            {
                await RecordLoginAttemptAsync(
                    connection,
                    appUserId: null,
                    request.Username.Trim(),
                    wasSuccessful: false,
                    cancellationToken);

                return LoginResult.Failure(
                    InvalidCredentialsMessage);
            }

            if (!loginUser.IsActive)
            {
                return LoginResult.Failure(
                    "This account is inactive.");
            }

            if (loginUser.LockoutEndUtc is DateTime lockoutEndUtc &&
                lockoutEndUtc > DateTime.UtcNow)
            {
                return LoginResult.Failure(
                    $"This account is locked until {lockoutEndUtc:u}.");
            }

            bool passwordIsValid = VerifyPassword(
                request.Password,
                loginUser.PasswordHash);

            await RecordLoginAttemptAsync(
                connection,
                loginUser.AppUserId,
                loginUser.Username,
                passwordIsValid,
                cancellationToken);

            if (!passwordIsValid)
            {
                return LoginResult.Failure(
                    InvalidCredentialsMessage);
            }

            IReadOnlyList<EffectivePermissionRecord> permissionRecords =
                (
                    await connection.QueryAsync<EffectivePermissionRecord>(
                        new CommandDefinition(
                            commandText:
                            """
                        SELECT
                            AppUserId,
                            Username,
                            RoleId,
                            RoleCode,
                            PermissionId,
                            PermissionCode,
                            Module
                        FROM security.vw_UserEffectivePermissions
                        WHERE AppUserId = @AppUserId
                        ORDER BY RoleCode, PermissionCode;
                        """,
                            parameters: new
                            {
                                loginUser.AppUserId
                            },
                            cancellationToken: cancellationToken))
                ).AsList();

            IReadOnlyCollection<Role> roles = permissionRecords
                .GroupBy(record => new
                {
                    record.RoleId,
                    record.RoleCode
                })
                .Select(group => new Role
                {
                    RoleId = group.Key.RoleId,
                    Code = group.Key.RoleCode,
                    DisplayName = group.Key.RoleCode,
                    IsSystemRole = true,
                    IsActive = true
                })
                .ToArray();

            string[] permissionCodes = permissionRecords
                .Select(record => record.PermissionCode)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            var user = new AppUser
            {
                AppUserId = loginUser.AppUserId,
                Username = loginUser.Username,
                Email = loginUser.Email,
                DriverId = loginUser.DriverId,
                IsActive = loginUser.IsActive,
                MustChangePassword = loginUser.MustChangePassword,
                FailedLoginAttempts = 0,
                LockoutEndUtc = null,
                LastLoginAtUtc = DateTime.UtcNow
            };

            var session = new UserSession(
                user,
                roles,
                permissionCodes);

            return LoginResult.Success(session);
        }

        private static bool VerifyPassword(
            string password,
            string passwordHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(
                    password,
                    passwordHash);
            }
            catch
            {
                return false;
            }
        }

        private static Task RecordLoginAttemptAsync(
            DbConnection connection,
            long? appUserId,
            string usernameAttempted,
            bool wasSuccessful,
            CancellationToken cancellationToken)
        {
            return connection.ExecuteAsync(
                new CommandDefinition(
                    commandText: "security.AppUser_RecordLogin",
                    parameters: new
                    {
                        AppUserId = appUserId,
                        UsernameAttempted = usernameAttempted,
                        WasSuccessful = wasSuccessful,
                        ClientApplication = "DISPATCH_WINFORMS",
                        DeviceIdentifier = Environment.MachineName,
                        IpAddress = (string?)null,
                        LockoutMinutes = 15,
                        MaximumFailedAttempts = 5
                    },
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));
        }
    }
}
