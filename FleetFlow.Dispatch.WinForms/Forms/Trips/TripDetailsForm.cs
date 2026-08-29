using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Trips;

namespace FleetFlow.Dispatch.WinForms.Forms.Trips;

public partial class TripDetailsForm : Form
{
    private readonly long _tripId;
    private readonly ITripDetailsService? _tripDetailsService;

    public TripDetailsForm()
    {
        InitializeComponent();

        btnRefresh.Click += btnRefresh_Click;
        btnClose.Click += btnClose_Click;
    }

    public TripDetailsForm(
        long tripId,
        ITripDetailsService tripDetailsService)
        : this()
    {
        _tripId = tripId;
        _tripDetailsService = tripDetailsService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_tripDetailsService is null)
        {
            return;
        }

        await LoadTripDetailsAsync();
    }

    private async Task LoadTripDetailsAsync()
    {
        if (_tripDetailsService is null)
        {
            return;
        }

        SetLoadingState(true);

        try
        {
            TripDetailsResult? result =
                await _tripDetailsService.GetByIdAsync(_tripId);

            if (result is null)
            {
                MessageBox.Show(
                    "The selected trip could not be found.",
                    "Trip Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Close();
                return;
            }

            DisplayTrip(result);
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"The trip details could not be loaded.\n\n{exception.Message}",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DisplayTrip(TripDetailsResult result)
    {
        TripDetails trip = result.Trip;

        Text = $"FleetFlow — {trip.TripNumber}";

        lblTripNumber.Text = trip.TripNumber;
        lblTripStatus.Text = trip.TripStatus;

        lblCustomerValue.Text =
            $"{trip.CustomerNumber} — {trip.CompanyName}";

        lblLoadValue.Text = trip.LoadNumber;

        lblDescriptionValue.Text =
            string.IsNullOrWhiteSpace(trip.LoadDescription)
                ? "No description"
                : trip.LoadDescription;

        lblScheduleValue.Text =
            $"{FormatDate(trip.ScheduledPickupUtc)} → " +
            $"{FormatDate(trip.ScheduledDeliveryUtc)}";

        lblDistanceValue.Text =
            trip.ActualDistanceMiles.HasValue
                ? $"{trip.ActualDistanceMiles:N1} / " +
                  $"{trip.PlannedDistanceMiles:N1} mi"
                : $"{trip.PlannedDistanceMiles:N1} mi planned";

        lblProgressValue.Text =
            $"{trip.CompletedStops} of {trip.TotalStops} stops " +
            $"({trip.ProgressPercent:N0}%)";

        dgvStops.DataSource = null;
        dgvStops.DataSource = result.Stops.ToList();

        dgvHistory.DataSource = null;
        dgvHistory.DataSource = result.StatusTimeline.ToList();

        lblMessage.Text =
            $"Updated {DateTime.Now:MMM d, yyyy h:mm:ss tt}";
    }

    private static string FormatDate(DateTime dateTime)
    {
        DateTime localDateTime =
            dateTime.Kind == DateTimeKind.Utc
                ? dateTime.ToLocalTime()
                : dateTime;

        return localDateTime.ToString("MMM d, yyyy h:mm tt");
    }

    private void SetLoadingState(bool isLoading)
    {
        UseWaitCursor = isLoading;
        btnRefresh.Enabled = !isLoading;

        if (isLoading)
        {
            lblMessage.Text = "Loading trip details...";
        }
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await LoadTripDetailsAsync();
    }

    private void btnClose_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }
}