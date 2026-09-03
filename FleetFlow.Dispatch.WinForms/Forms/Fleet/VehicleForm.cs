using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Fleet;

namespace FleetFlow.Dispatch.WinForms.Forms.Fleet;

public partial class VehicleForm : Form
{
    private readonly IFleetCommandService?
        _service;

    private readonly FleetOverviewVehicleItem?
        _vehicle;

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public VehicleForm()
    {
        InitializeComponent();
        ConfigureStatusOptions();
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public VehicleForm(
        IFleetCommandService service,
        FleetOverviewVehicleItem? vehicle = null)
        : this()
    {
        _service = service;
        _vehicle = vehicle;

        ConfigureForm(vehicle);

        if (vehicle is not null)
        {
            DisplayVehicle(vehicle);
        }
    }

    /// <summary>
    /// Configura los estados permitidos para vehículos.
    /// </summary>
    private void ConfigureStatusOptions()
    {
        cboStatus.Items.AddRange(
        [
            "AVAILABLE",
            "ASSIGNED",
            "IN_TRANSIT",
            "MAINTENANCE",
            "OUT_OF_SERVICE"
        ]);

        cboStatus.SelectedItem = "AVAILABLE";
    }

    /// <summary>
    /// Configura el título para creación o edición.
    /// </summary>
    private void ConfigureForm(
        FleetOverviewVehicleItem? vehicle)
    {
        if (vehicle is null)
        {
            Text = "FleetFlow — New Vehicle";
            lblTitle.Text = "New Vehicle";
            return;
        }

        Text =
            $"FleetFlow — Edit {vehicle.UnitNumber}";

        lblTitle.Text = "Edit Vehicle";
    }

    /// <summary>
    /// Muestra los datos del vehículo seleccionado.
    /// </summary>
    private void DisplayVehicle(
        FleetOverviewVehicleItem vehicle)
    {
        txtUnitNumber.Text =
            vehicle.UnitNumber;

        txtVin.Text =
            vehicle.Vin;

        txtModelYear.Text =
            vehicle.ModelYear.ToString();

        txtMake.Text =
            vehicle.Make;

        txtModel.Text =
            vehicle.Model;

        txtLicensePlate.Text =
            vehicle.LicensePlate;

        txtLicenseState.Text =
            vehicle.LicenseState;

        txtMaxPayload.Text =
            vehicle.MaxPayloadLbs.ToString("0.##");

        txtOdometer.Text =
            vehicle.CurrentOdometerMiles.ToString("0.##");

        cboStatus.SelectedItem =
            vehicle.StatusCode;
    }

    private async void btnSave_Click(
        object? sender,
        EventArgs e)
    {
        if (_service is null)
        {
            return;
        }

        if (!ValidateRequiredFields())
        {
            return;
        }

        if (!TryReadNumericValues(
                out short modelYear,
                out decimal maxPayload,
                out decimal odometer))
        {
            return;
        }

        btnSave.Enabled = false;
        UseWaitCursor = true;

        try
        {
            SaveVehicleRequest request =
                new SaveVehicleRequest
                {
                    VehicleId =
                        _vehicle?.VehicleId,

                    UnitNumber =
                        txtUnitNumber.Text.Trim(),

                    Vin =
                        txtVin.Text.Trim().ToUpperInvariant(),

                    ModelYear =
                        modelYear,

                    Make =
                        txtMake.Text.Trim(),

                    Model =
                        txtModel.Text.Trim(),

                    LicensePlate =
                        txtLicensePlate.Text
                            .Trim()
                            .ToUpperInvariant(),

                    LicenseState =
                        txtLicenseState.Text
                            .Trim()
                            .ToUpperInvariant(),

                    MaxPayloadLbs =
                        maxPayload,

                    CurrentOdometerMiles =
                        odometer,

                    StatusCode =
                        cboStatus.SelectedItem?
                            .ToString()
                        ?? "AVAILABLE",

                    ExpectedRowVersion =
                        _vehicle?.RowVersion
                };

            await _service.SaveVehicleAsync(request);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"Vehicle could not be saved.\n\n" +
                exception.Message,
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            UseWaitCursor = false;
        }
    }

    private bool ValidateRequiredFields()
    {
        if (string.IsNullOrWhiteSpace(
                txtUnitNumber.Text) ||
            string.IsNullOrWhiteSpace(
                txtVin.Text) ||
            string.IsNullOrWhiteSpace(
                txtModelYear.Text) ||
            string.IsNullOrWhiteSpace(
                txtMake.Text) ||
            string.IsNullOrWhiteSpace(
                txtModel.Text) ||
            string.IsNullOrWhiteSpace(
                txtLicensePlate.Text) ||
            string.IsNullOrWhiteSpace(
                txtLicenseState.Text) ||
            string.IsNullOrWhiteSpace(
                txtMaxPayload.Text) ||
            string.IsNullOrWhiteSpace(
                txtOdometer.Text))
        {
            MessageBox.Show(
                "Complete all required vehicle fields.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        if (txtVin.Text.Trim().Length != 17)
        {
            MessageBox.Show(
                "VIN must contain exactly 17 characters.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtVin.Focus();
            return false;
        }

        if (txtLicenseState.Text.Trim().Length != 2)
        {
            MessageBox.Show(
                "Plate state must contain two letters.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtLicenseState.Focus();
            return false;
        }

        return true;
    }

    private bool TryReadNumericValues(
        out short modelYear,
        out decimal maxPayload,
        out decimal odometer)
    {
        modelYear = 0;
        maxPayload = 0;
        odometer = 0;

        if (!short.TryParse(
                txtModelYear.Text,
                out modelYear) ||
            modelYear < 1980 ||
            modelYear > 2100)
        {
            MessageBox.Show(
                "Model year must be between 1980 and 2100.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtModelYear.Focus();
            return false;
        }

        if (!decimal.TryParse(
                txtMaxPayload.Text,
                out maxPayload) ||
            maxPayload <= 0)
        {
            MessageBox.Show(
                "Maximum payload must be greater than zero.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtMaxPayload.Focus();
            return false;
        }

        if (!decimal.TryParse(
                txtOdometer.Text,
                out odometer) ||
            odometer < 0)
        {
            MessageBox.Show(
                "Odometer cannot be negative.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtOdometer.Focus();
            return false;
        }

        return true;
    }

    private void btnCancel_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }
}