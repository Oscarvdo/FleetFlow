using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Fleet;

namespace FleetFlow.Dispatch.WinForms.Controls.Fleet;

public partial class FleetControl : UserControl
{
    private readonly IFleetOverviewService? _fleetService;
    private FleetOverviewResult? _overview;
    public event Action<FleetOverviewVehicleItem>? VehicleEditRequested;
    public event EventHandler? VehicleCreateRequested;
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool CanManageVehicles
    {
        get => btnNewVehicle.Visible;
        set { btnNewVehicle.Visible = value; btnEditVehicle.Visible = value; }
    }

    public FleetControl()
    {
        InitializeComponent();
        ConfigureGrids();
        btnRefresh.Click += async (_, _) => await RefreshFleetAsync();
        btnNewVehicle.Click += (_, _) => VehicleCreateRequested?.Invoke(this, EventArgs.Empty);
        btnEditVehicle.Click += (_, _) => EditVehicle();
        dgvVehicles.CellDoubleClick += (_, e) => { if (CanManageVehicles && e.RowIndex >= 0) EditVehicle(); };
        chkIncludeInactive.CheckedChanged += async (_, _) => await RefreshFleetAsync();
        txtSearch.TextChanged += (_, _) => ApplyFilter();
    }

    public FleetControl(IFleetOverviewService fleetService) : this() =>
        _fleetService = fleetService;

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_fleetService is not null) await RefreshFleetAsync();
    }

    public async Task RefreshFleetAsync()
    {
        if (_fleetService is null) return;
        SetBusy(true);
        try
        {
            _overview = await _fleetService.GetOverviewAsync(chkIncludeInactive.Checked);
            DisplaySummary(_overview.Summary);
            ApplyFilter();
            lblStatus.Text = $"Updated {DateTime.Now:g}";
        }
        catch (Exception exception)
        {
            lblStatus.Text = "Unable to load fleet";
            MessageBox.Show($"Fleet information could not be loaded.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { SetBusy(false); }
    }

    private void ConfigureGrids()
    {
        Setup(dgvVehicles, ("UnitNumber", "UNIT", 75), ("Description", "VEHICLE", 150),
            ("LicensePlate", "PLATE", 80), ("CurrentOdometerMiles", "ODOMETER", 85),
            ("MaxPayloadLbs", "PAYLOAD", 85), ("Status", "STATUS", 85), ("ActiveTripNumber", "ACTIVE TRIP", 90));
        Setup(dgvTrailers, ("UnitNumber", "UNIT", 75), ("TrailerType", "TYPE", 105),
            ("LicensePlate", "PLATE", 85), ("MaxPayloadLbs", "PAYLOAD", 90),
            ("Status", "STATUS", 90), ("ActiveTripNumber", "ACTIVE TRIP", 105));
        Setup(dgvDrivers, ("DriverNumber", "DRIVER #", 75), ("FullName", "DRIVER", 145),
            ("LicenseNumber", "LICENSE", 95), ("LicenseExpirationDate", "EXPIRES", 90),
            ("Phone", "PHONE", 95), ("Status", "STATUS", 90), ("ActiveTripNumber", "ACTIVE TRIP", 100));
    }

    private static void Setup(DataGridView grid, params (string Property, string Header, float Weight)[] columns)
    {
        grid.AutoGenerateColumns = false;
        foreach ((string property, string header, float weight) in columns)
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = property, HeaderText = header, FillWeight = weight, ReadOnly = true
            });
    }

    private void DisplaySummary(FleetOverviewSummary summary)
    {
        lblVehicles.Text = $"{summary.AvailableVehicles:N0} / {summary.TotalVehicles:N0}";
        lblVehicleCaption.Text = "Available vehicles";
        lblTrailers.Text = summary.TotalTrailers.ToString("N0");
        lblDrivers.Text = $"{summary.AvailableDrivers:N0} / {summary.TotalDrivers:N0}";
        lblMaintenance.Text = summary.VehiclesInMaintenance.ToString("N0");
    }

    private void ApplyFilter()
    {
        if (_overview is null) return;
        string search = txtSearch.Text.Trim();
        dgvVehicles.DataSource = _overview.Vehicles.Where(x => Match(search, x.UnitNumber, x.Description, x.Status, x.ActiveTripNumber)).ToList();
        dgvTrailers.DataSource = _overview.Trailers.Where(x => Match(search, x.UnitNumber, x.TrailerType, x.Status, x.ActiveTripNumber)).ToList();
        dgvDrivers.DataSource = _overview.Drivers.Where(x => Match(search, x.DriverNumber, x.FullName, x.LicenseNumber, x.Status, x.ActiveTripNumber)).ToList();
    }

    private static bool Match(string search, params string?[] values) => search.Length == 0 ||
        values.Any(value => value?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);

    private void SetBusy(bool busy)
    {
        btnRefresh.Enabled = !busy;
        chkIncludeInactive.Enabled = !busy;
        txtSearch.Enabled = !busy;
        btnNewVehicle.Enabled = !busy;
        btnEditVehicle.Enabled = !busy && dgvVehicles.CurrentRow is not null;
        UseWaitCursor = busy;
        if (busy) lblStatus.Text = "Loading fleet...";
    }

    private void EditVehicle()
    {
        if (dgvVehicles.CurrentRow?.DataBoundItem is FleetOverviewVehicleItem vehicle)
            VehicleEditRequested?.Invoke(vehicle);
    }
}
