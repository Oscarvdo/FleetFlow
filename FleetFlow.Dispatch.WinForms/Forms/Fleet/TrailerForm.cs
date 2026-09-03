using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Fleet;

namespace FleetFlow.Dispatch.WinForms.Forms.Fleet;

public partial class TrailerForm : Form
{
    private readonly IFleetCommandService? _service;
    private readonly FleetOverviewTrailerItem? _trailer;

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public TrailerForm()
    {
        InitializeComponent();
        ConfigureTrailerTypes();
        ConfigureStatusOptions();
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public TrailerForm(
        IFleetCommandService service,
        FleetOverviewTrailerItem? trailer = null)
        : this()
    {
        _service = service;
        _trailer = trailer;

        ConfigureForm(trailer);

        if (trailer is not null)
        {
            DisplayTrailer(trailer);
        }
    }

    private void ConfigureTrailerTypes()
    {
        cboTrailerType.Items.Clear();
        cboTrailerType.Items.Add("DRY_VAN");
        cboTrailerType.Items.Add("REEFER");
        cboTrailerType.Items.Add("FLATBED");
        cboTrailerType.Items.Add("TANKER");
        cboTrailerType.Items.Add("OTHER");
        cboTrailerType.SelectedItem = "DRY_VAN";
    }

    private void ConfigureStatusOptions()
    {
        cboStatus.Items.Clear();
        cboStatus.Items.Add("AVAILABLE");
        cboStatus.Items.Add("ASSIGNED");
        cboStatus.Items.Add("IN_TRANSIT");
        cboStatus.Items.Add("MAINTENANCE");
        cboStatus.Items.Add("OUT_OF_SERVICE");
        cboStatus.SelectedItem = "AVAILABLE";
    }

    private void ConfigureForm(FleetOverviewTrailerItem? trailer)
    {
        if (trailer is null)
        {
            Text = "FleetFlow — New Trailer";
            lblTitle.Text = "New Trailer";
            btnSave.Text = "Save Trailer";
            return;
        }

        Text = $"FleetFlow — Edit {trailer.UnitNumber}";
        lblTitle.Text = "Edit Trailer";
        btnSave.Text = "Update Trailer";
    }

    private void DisplayTrailer(FleetOverviewTrailerItem trailer)
    {
        txtUnitNumber.Text = trailer.UnitNumber;
        txtVin.Text = trailer.Vin;
        cboTrailerType.SelectedItem = trailer.TrailerType;
        txtLicensePlate.Text = trailer.LicensePlate;
        txtLicenseState.Text = trailer.LicenseState;
        txtMaxPayload.Text = trailer.MaxPayloadLbs.ToString("0.##");
        cboStatus.SelectedItem = trailer.StatusCode;
        chkActive.Checked = trailer.IsActive;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_service is null)
        {
            MessageBox.Show(
                "The trailer service is not available.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        if (!ValidateRequiredFields(out decimal payload))
        {
            return;
        }

        btnSave.Enabled = false;
        UseWaitCursor = true;

        try
        {
            SaveTrailerRequest request = new()
            {
                TrailerId = _trailer?.TrailerId,
                UnitNumber = txtUnitNumber.Text.Trim(),
                Vin = txtVin.Text.Trim().ToUpperInvariant(),
                TrailerType =
                    cboTrailerType.SelectedItem?.ToString() ?? "DRY_VAN",
                LicensePlate =
                    txtLicensePlate.Text.Trim().ToUpperInvariant(),
                LicenseState =
                    txtLicenseState.Text.Trim().ToUpperInvariant(),
                MaxPayloadLbs = payload,
                StatusCode =
                    cboStatus.SelectedItem?.ToString() ?? "AVAILABLE",
                ExpectedRowVersion = _trailer?.RowVersion
            };

            TrailerCommandResult result =
                await _service.SaveTrailerAsync(request);

            bool originalActiveStatus = _trailer?.IsActive ?? true;

            if (chkActive.Checked != originalActiveStatus)
            {
                await _service.SetTrailerActiveAsync(
                    result.TrailerId,
                    chkActive.Checked,
                    result.RowVersion);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"Trailer could not be saved.\n\n{exception.Message}",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            btnSave.Enabled = true;
        }
    }

    private bool ValidateRequiredFields(out decimal payload)
    {
        payload = 0;

        if (string.IsNullOrWhiteSpace(txtUnitNumber.Text))
        {
            ShowValidationMessage(
                "Enter the trailer unit number.",
                txtUnitNumber);

            return false;
        }

        if (txtVin.Text.Trim().Length != 17)
        {
            ShowValidationMessage(
                "The VIN must contain exactly 17 characters.",
                txtVin);

            return false;
        }

        if (cboTrailerType.SelectedItem is null)
        {
            ShowValidationMessage(
                "Select a trailer type.",
                cboTrailerType);

            return false;
        }

        if (string.IsNullOrWhiteSpace(txtLicensePlate.Text))
        {
            ShowValidationMessage(
                "Enter the trailer license plate.",
                txtLicensePlate);

            return false;
        }

        if (txtLicenseState.Text.Trim().Length != 2)
        {
            ShowValidationMessage(
                "The license state must contain exactly two letters.",
                txtLicenseState);

            return false;
        }

        if (!decimal.TryParse(txtMaxPayload.Text.Trim(), out payload) ||
            payload <= 0)
        {
            ShowValidationMessage(
                "Enter a valid maximum payload greater than zero.",
                txtMaxPayload);

            return false;
        }

        if (cboStatus.SelectedItem is null)
        {
            ShowValidationMessage(
                "Select an operational status.",
                cboStatus);

            return false;
        }

        return true;
    }

    private static void ShowValidationMessage(
        string message,
        Control control)
    {
        MessageBox.Show(
            message,
            "FleetFlow",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        control.Focus();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }
}