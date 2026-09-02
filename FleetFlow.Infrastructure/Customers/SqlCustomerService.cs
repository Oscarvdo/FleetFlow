using System.Data;
using System.Data.Common;
using Dapper;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Abstractions.Persistence;
using FleetFlow.Application.Customers;

namespace FleetFlow.Infrastructure.Customers;

public sealed class SqlCustomerService : ICustomerService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqlCustomerService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<CustomerListItem>> SearchAsync(
        string? searchText = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            "catalog.Customer_List",
            new
            {
                SearchText = Normalize(searchText),
                IncludeInactive = includeInactive
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        IEnumerable<CustomerListItem> customers =
            await connection.QueryAsync<CustomerListItem>(command);

        return customers.AsList();
    }

    public async Task<CustomerDetails?> GetByIdAsync(
        long customerId,
        CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(customerId));
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            "catalog.Customer_GetDetails",
            new { CustomerId = customerId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        using SqlMapper.GridReader reader =
            await connection.QueryMultipleAsync(command);

        CustomerDetails? details =
            await reader.ReadSingleOrDefaultAsync<CustomerDetails>();

        if (details is null)
        {
            return null;
        }

        details.Locations =
            (await reader.ReadAsync<CustomerLocationItem>()).AsList();

        details.RecentLoads =
            (await reader.ReadAsync<CustomerRecentLoadItem>()).AsList();

        return details;
    }

    public async Task<CustomerCommandResult> SaveAsync(
        SaveCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validate(request);

        bool isUpdate = request.CustomerId.HasValue;

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        object parameters = isUpdate
            ? new
            {
                CustomerId = request.CustomerId!.Value,
                CustomerNumber = request.CustomerNumber.Trim(),
                CompanyName = request.CompanyName.Trim(),
                ContactName = Normalize(request.ContactName),
                Email = Normalize(request.Email),
                Phone = Normalize(request.Phone),
                ExpectedRowVersion = request.ExpectedRowVersion
            }
            : new
            {
                CustomerNumber = request.CustomerNumber.Trim(),
                CompanyName = request.CompanyName.Trim(),
                ContactName = Normalize(request.ContactName),
                Email = Normalize(request.Email),
                Phone = Normalize(request.Phone)
            };

        CommandDefinition command = new(
            isUpdate
                ? "catalog.Customer_Update"
                : "catalog.Customer_Create",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<CustomerCommandResult>(command);
    }

    public async Task<CustomerCommandResult> SetActiveAsync(
        long customerId,
        bool isActive,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default)
    {
        if (customerId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(customerId));
        }

        if (expectedRowVersion is not { Length: 8 })
        {
            throw new ArgumentException(
                "A valid RowVersion is required.",
                nameof(expectedRowVersion));
        }

        await using DbConnection connection =
            _connectionFactory.CreateConnection();

        CommandDefinition command = new(
            "catalog.Customer_SetActive",
            new
            {
                CustomerId = customerId,
                IsActive = isActive,
                ExpectedRowVersion = expectedRowVersion
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<CustomerCommandResult>(command);
    }

    public async Task<CustomerLocationCommandResult> SaveLocationAsync(
        SaveCustomerLocationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateLocation(request);
        bool isUpdate = request.LocationId.HasValue;

        await using DbConnection connection = _connectionFactory.CreateConnection();

        var common = new DynamicParameters();
        if (isUpdate)
        {
            common.Add("LocationId", request.LocationId!.Value);
            common.Add("ExpectedRowVersion", request.ExpectedRowVersion);
        }

        common.Add("CustomerId", request.CustomerId);
        common.Add("LocationCode", request.LocationCode.Trim());
        common.Add("LocationType", request.LocationType.Trim());
        common.Add("LocationName", request.LocationName.Trim());
        common.Add("Address1", request.Address1.Trim());
        common.Add("Address2", Normalize(request.Address2));
        common.Add("City", request.City.Trim());
        common.Add("StateCode", request.StateCode.Trim());
        common.Add("PostalCode", request.PostalCode.Trim());
        common.Add("Latitude", request.Latitude);
        common.Add("Longitude", request.Longitude);
        common.Add("ContactName", Normalize(request.ContactName));
        common.Add("ContactPhone", Normalize(request.ContactPhone));
        common.Add("IsBillingLocation", request.IsBillingLocation);

        CommandDefinition command = new(
            isUpdate ? "catalog.CustomerLocation_Update" : "catalog.CustomerLocation_Create",
            common,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<CustomerLocationCommandResult>(command);
    }

    public async Task<CustomerLocationCommandResult> SetLocationActiveAsync(
        long customerId,
        long locationId,
        bool isActive,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default)
    {
        if (customerId <= 0 || locationId <= 0)
            throw new ArgumentOutOfRangeException(nameof(locationId));
        if (expectedRowVersion is not { Length: 8 })
            throw new ArgumentException("A valid RowVersion is required.", nameof(expectedRowVersion));

        await using DbConnection connection = _connectionFactory.CreateConnection();
        CommandDefinition command = new(
            "catalog.CustomerLocation_SetActive",
            new
            {
                CustomerId = customerId,
                LocationId = locationId,
                IsActive = isActive,
                ExpectedRowVersion = expectedRowVersion
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<CustomerLocationCommandResult>(command);
    }

    private static void Validate(SaveCustomerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerNumber))
        {
            throw new ArgumentException("Customer number is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            throw new ArgumentException("Company name is required.", nameof(request));
        }

        if (request.CustomerId.HasValue &&
            request.ExpectedRowVersion is not { Length: 8 })
        {
            throw new ArgumentException(
                "A valid RowVersion is required when updating a customer.",
                nameof(request));
        }
    }

    private static void ValidateLocation(SaveCustomerLocationRequest request)
    {
        if (request.CustomerId <= 0)
            throw new ArgumentOutOfRangeException(nameof(request.CustomerId));
        if (string.IsNullOrWhiteSpace(request.LocationCode) ||
            string.IsNullOrWhiteSpace(request.LocationName) ||
            string.IsNullOrWhiteSpace(request.Address1) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.StateCode) ||
            string.IsNullOrWhiteSpace(request.PostalCode))
            throw new ArgumentException("Complete all required location fields.", nameof(request));
        if (request.LocationId.HasValue && request.ExpectedRowVersion is not { Length: 8 })
            throw new ArgumentException("A valid RowVersion is required when updating a location.", nameof(request));
        if (request.Latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(request.Latitude));
        if (request.Longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(request.Longitude));
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
