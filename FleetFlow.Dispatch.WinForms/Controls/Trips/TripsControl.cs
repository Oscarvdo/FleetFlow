using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Trips;

namespace FleetFlow.Dispatch.WinForms.Controls.Trips;

public partial class TripsControl : UserControl
{
    private readonly ITripListService? _tripListService;

    private IReadOnlyList<TripListItem> _allTrips = [];

    public event Action<long>? TripOpenRequested;

    public TripsControl()
    {
        InitializeComponent();

        ConfigureGrid();
        ConfigureStatusFilter();

        btnRefresh.Click += btnRefresh_Click;
        txtSearch.TextChanged += FilterChanged;
        cboStatus.SelectedIndexChanged += FilterChanged;
        dgvTrips.CellFormatting += dgvTrips_CellFormatting;
        dgvTrips.CellDoubleClick += dgvTrips_CellDoubleClick;
    }

    public TripsControl(
        ITripListService tripListService)
        : this()
    {
        _tripListService = tripListService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_tripListService is not null)
        {
            await LoadTripsAsync();
        }
    }

    private void ConfigureGrid()
    {
        dgvTrips.AutoGenerateColumns = false;
        dgvTrips.Columns.Clear();

        dgvTrips.Columns.AddRange(
            new DataGridViewTextBoxColumn
            {
                Name = "colTripNumber",
                DataPropertyName = "TripNumber",
                HeaderText = "TRIP",
                FillWeight = 95F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                DataPropertyName = "TripStatus",
                HeaderText = "STATUS",
                FillWeight = 110F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colLoadNumber",
                DataPropertyName = "LoadNumber",
                HeaderText = "LOAD",
                FillWeight = 90F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colCustomer",
                DataPropertyName = "Customer",
                HeaderText = "CUSTOMER",
                FillWeight = 155F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colPickup",
                DataPropertyName = "ScheduledPickupUtc",
                HeaderText = "PICKUP",
                FillWeight = 115F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colDelivery",
                DataPropertyName = "ScheduledDeliveryUtc",
                HeaderText = "DELIVERY",
                FillWeight = 115F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colStops",
                DataPropertyName = "TotalStops",
                HeaderText = "STOPS",
                FillWeight = 55F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colProgress",
                DataPropertyName = "ProgressPercent",
                HeaderText = "PROGRESS",
                FillWeight = 70F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colDistance",
                DataPropertyName = "PlannedDistanceMiles",
                HeaderText = "MILES",
                FillWeight = 75F,
                ReadOnly = true
            });
    }

    private void ConfigureStatusFilter()
    {
        cboStatus.DisplayMember =
            nameof(TripStatusFilter.DisplayName);

        cboStatus.ValueMember =
            nameof(TripStatusFilter.StatusCode);

        cboStatus.Items.AddRange(
        [
            new TripStatusFilter(
            null,
            "All statuses"),

        new TripStatusFilter(
            "PLANNED",
            "Planned"),

        new TripStatusFilter(
            "OFFERED",
            "Offered to Driver"),

        new TripStatusFilter(
            "ASSIGNED",
            "Assigned"),

        new TripStatusFilter(
            "EN_ROUTE_TO_PICKUP",
            "En Route to Pickup"),

        new TripStatusFilter(
            "AT_PICKUP",
            "At Pickup"),

        new TripStatusFilter(
            "LOADED",
            "Loaded"),

        new TripStatusFilter(
            "EN_ROUTE_TO_DELIVERY",
            "En Route to Delivery"),

        new TripStatusFilter(
            "AT_DELIVERY",
            "At Delivery"),

        new TripStatusFilter(
            "DELIVERED",
            "Delivered"),

        new TripStatusFilter(
            "COMPLETED",
            "Completed"),

        new TripStatusFilter(
            "DELAYED",
            "Delayed"),

        new TripStatusFilter(
            "INCIDENT_REPORTED",
            "Incident Reported"),

        new TripStatusFilter(
            "VEHICLE_BREAKDOWN",
            "Vehicle Breakdown"),

        new TripStatusFilter(
            "CANCELLED",
            "Cancelled")
        ]);

        cboStatus.SelectedIndex = 0;
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await LoadTripsAsync();
    }

    private void FilterChanged(
        object? sender,
        EventArgs e)
    {
        ApplyLocalFilter();
    }

    private async Task LoadTripsAsync()
    {
        if (_tripListService is null)
        {
            return;
        }

        SetBusyState(true);

        try
        {
            _allTrips =
                await _tripListService.GetTripsAsync();

            ApplyLocalFilter();

            lblStatus.ForeColor =
                Color.FromArgb(106, 116, 130);

            lblStatus.Text =
                $"Updated {DateTime.Now:g}";
        }
        catch (Exception exception)
        {
            lblStatus.ForeColor = Color.Firebrick;
            lblStatus.Text = "Unable to load trips";

            MessageBox.Show(
                $"The trips could not be loaded.\n\n" +
                exception.Message,
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            SetBusyState(false);
        }
    }

    private void ApplyLocalFilter()
    {
        string searchText =
            txtSearch.Text.Trim();

        string? statusCode =
            (cboStatus.SelectedItem
                as TripStatusFilter)
            ?.StatusCode;

        IEnumerable<TripListItem> filtered =
            _allTrips;

        if (!string.IsNullOrWhiteSpace(statusCode))
        {
            filtered = filtered.Where(
                trip =>
                    string.Equals(
                        trip.TripStatusCode,
                        statusCode,
                        StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filtered = filtered.Where(
                trip =>
                    Contains(
                        trip.TripNumber,
                        searchText) ||

                    Contains(
                        trip.LoadNumber,
                        searchText) ||

                    Contains(
                        trip.CustomerNumber,
                        searchText) ||

                    Contains(
                        trip.Customer,
                        searchText) ||

                    Contains(
                        trip.TripStatus,
                        searchText));
        }

        List<TripListItem> records =
            filtered.ToList();

        dgvTrips.DataSource = null;
        dgvTrips.AutoGenerateColumns = false;
        dgvTrips.DataSource = records;

        lblCount.Text =
            records.Count == 1
                ? "1 trip"
                : $"{records.Count:N0} trips";
    }

    private static bool Contains(
        string? value,
        string searchText)
    {
        return value?.Contains(
            searchText,
            StringComparison.OrdinalIgnoreCase) == true;
    }

    private void dgvTrips_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        if (dgvTrips.Rows[e.RowIndex].DataBoundItem
            is not TripListItem selectedTrip)
        {
            return;
        }

        TripOpenRequested?.Invoke(
            selectedTrip.TripId);
    }

    private void dgvTrips_CellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex < 0)
        {
            return;
        }

        string propertyName =
            dgvTrips.Columns[e.ColumnIndex]
                .DataPropertyName;

        if (propertyName is
            "ScheduledPickupUtc" or
            "ScheduledDeliveryUtc" &&
            e.Value is DateTime utcValue)
        {
            DateTime utc =
                DateTime.SpecifyKind(
                    utcValue,
                    DateTimeKind.Utc);

            e.Value =
                utc.ToLocalTime().ToString("g");

            e.FormattingApplied = true;
            return;
        }

        if (propertyName == "ProgressPercent" &&
            e.Value is decimal progress)
        {
            e.Value = $"{progress:0.#}%";
            e.FormattingApplied = true;
            return;
        }

        if (propertyName ==
                "PlannedDistanceMiles" &&
            e.Value is decimal distance)
        {
            e.Value = $"{distance:N1}";
            e.FormattingApplied = true;
        }
    }

    private void SetBusyState(bool isBusy)
    {
        btnRefresh.Enabled = !isBusy;
        txtSearch.Enabled = !isBusy;
        cboStatus.Enabled = !isBusy;

        btnRefresh.Text =
            isBusy
                ? "Loading..."
                : "Refresh";

        UseWaitCursor = isBusy;
    }

    private sealed record TripStatusFilter(
        string? StatusCode,
        string DisplayName);
}