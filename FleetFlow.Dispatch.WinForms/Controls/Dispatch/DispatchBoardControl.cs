using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Dispatch;

namespace FleetFlow.Dispatch.WinForms.Controls.Dispatch;

public partial class DispatchBoardControl : UserControl
{
    private readonly IDispatchBoardService? _dispatchBoardService;
    private IReadOnlyList<DispatchBoardItem> _allTrips = [];

    public DispatchBoardControl()
    {
        InitializeComponent();

        btnRefresh.Click += btnRefresh_Click;
        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvTrips.CellFormatting += dgvTrips_CellFormatting;
    }

    public DispatchBoardControl(
        IDispatchBoardService dispatchBoardService)
        : this()
    {
        _dispatchBoardService = dispatchBoardService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_dispatchBoardService is not null)
        {
            await LoadTripsAsync();
        }
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await LoadTripsAsync();
    }

    private void txtSearch_TextChanged(
        object? sender,
        EventArgs e)
    {
        ApplyFilter();
    }

    private async Task LoadTripsAsync()
    {
        if (_dispatchBoardService is null)
        {
            return;
        }

        SetBusyState(true);

        try
        {
            _allTrips =
                await _dispatchBoardService.GetActiveTripsAsync();

            ApplyFilter();

            lblStatus.ForeColor =
                Color.FromArgb(106, 116, 130);

            lblStatus.Text =
                $"Updated {DateTime.Now:g}";
        }
        catch (Exception)
        {
            lblStatus.ForeColor = Color.Firebrick;
            lblStatus.Text = "Unable to load dispatch board";
        }
        finally
        {
            SetBusyState(false);
        }
    }

    private void ApplyFilter()
    {
        string searchText =
            txtSearch.Text.Trim();

        IEnumerable<DispatchBoardItem> filtered =
            _allTrips;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filtered = filtered.Where(
                trip =>
                    Contains(trip.TripNumber, searchText) ||
                    Contains(trip.LoadNumber, searchText) ||
                    Contains(trip.Customer, searchText) ||
                    Contains(trip.DriverName, searchText) ||
                    Contains(trip.VehicleUnitNumber, searchText) ||
                    Contains(trip.PickupLocation, searchText) ||
                    Contains(trip.DeliveryLocation, searchText) ||
                    Contains(trip.TripStatus, searchText));
        }

        List<DispatchBoardItem> records =
            filtered.ToList();

        dgvTrips.DataSource = null;
        dgvTrips.DataSource = records;

        lblCount.Text =
            records.Count == 1
                ? "1 active trip"
                : $"{records.Count:N0} active trips";
    }

    private static bool Contains(
        string? value,
        string searchText)
    {
        return value?.Contains(
            searchText,
            StringComparison.OrdinalIgnoreCase) == true;
    }

    private void dgvTrips_CellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e)
    {
        string? propertyName =
            dgvTrips.Columns[e.ColumnIndex].DataPropertyName;

        if (propertyName is
            "ScheduledPickupUtc" or
            "ScheduledDeliveryUtc" &&
            e.Value is DateTime utcValue)
        {
            DateTime utc = DateTime.SpecifyKind(
                utcValue,
                DateTimeKind.Utc);

            e.Value = utc.ToLocalTime().ToString("g");
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

        if (propertyName == "FuelPercent" &&
            e.Value is decimal fuel)
        {
            e.Value = $"{fuel:0.#}%";
            e.FormattingApplied = true;
        }
    }

    private void SetBusyState(bool isBusy)
    {
        btnRefresh.Enabled = !isBusy;
        txtSearch.Enabled = !isBusy;

        btnRefresh.Text = isBusy
            ? "Loading..."
            : "Refresh";

        UseWaitCursor = isBusy;
    }
}