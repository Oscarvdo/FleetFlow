using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Dashboard;

namespace FleetFlow.Dispatch.WinForms.Controls.Dashboard;

public partial class DashboardControl : UserControl
{
    private readonly IDashboardService? _dashboardService;

    public DashboardControl()
    {
        InitializeComponent();
        btnRefresh.Click += btnRefresh_Click;
    }

    public DashboardControl(
        IDashboardService dashboardService)
        : this()
    {
        _dashboardService = dashboardService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_dashboardService is not null)
        {
            await LoadDashboardAsync();
        }
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        if (_dashboardService is null)
        {
            return;
        }

        SetBusyState(true);

        try
        {
            DashboardSummary summary =
                await _dashboardService.GetSummaryAsync();

            lblActiveTripsValue.Text =
                summary.ActiveTrips.ToString("N0");

            lblAvailableDriversValue.Text =
                summary.AvailableDrivers.ToString("N0");

            lblAvailableVehiclesValue.Text =
                summary.AvailableVehicles.ToString("N0");

            lblPendingLoadsValue.Text =
                summary.PendingLoads.ToString("N0");

            lblDelayedTripsValue.Text =
                summary.DelayedTrips.ToString("N0");

            lblActiveIncidentsValue.Text =
                summary.ActiveIncidents.ToString("N0");

            lblTrackedVehiclesValue.Text =
                summary.TrackedVehicles.ToString("N0");

            DateTime generatedUtc = DateTime.SpecifyKind(
                summary.GeneratedAtUtc,
                DateTimeKind.Utc);

            lblUpdated.Text =
                $"Updated {generatedUtc.ToLocalTime():g}";
        }
        catch (Exception)
        {
            lblUpdated.Text = "Unable to load dashboard";
            lblUpdated.ForeColor = Color.Firebrick;
        }
        finally
        {
            SetBusyState(false);
        }
    }

    private void SetBusyState(bool isBusy)
    {
        btnRefresh.Enabled = !isBusy;
        btnRefresh.Text = isBusy
            ? "Loading..."
            : "Refresh";

        UseWaitCursor = isBusy;
    }
}