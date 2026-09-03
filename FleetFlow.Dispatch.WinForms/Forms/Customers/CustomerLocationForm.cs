using System.Globalization;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Customers;

namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

public partial class CustomerLocationForm : Form
{
    private readonly long _customerId;
    private readonly ICustomerService? _customerService;
    private readonly CustomerLocationItem? _location;

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public CustomerLocationForm()
    {
        InitializeComponent();
        ConfigureLocationTypes();

        btnSave.Click += btnSave_Click;
        btnCancel.Click += btnCancel_Click;
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public CustomerLocationForm(
        long customerId,
        ICustomerService customerService,
        CustomerLocationItem? location = null)
        : this()
    {
        _customerId = customerId;
        _customerService = customerService;
        _location = location;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        ConfigureForm();

        if (_location is not null)
        {
            DisplayLocation(_location);
        }
    }

    private void ConfigureLocationTypes()
    {
        cboLocationType.Items.Clear();
        cboLocationType.Items.Add("CUSTOMER");
        cboLocationType.Items.Add("TERMINAL");
        cboLocationType.Items.Add("FUEL");
        cboLocationType.Items.Add("REST_AREA");
        cboLocationType.Items.Add("OTHER");
        cboLocationType.SelectedItem = "CUSTOMER";
    }

    private void ConfigureForm()
    {
        if (_location is null)
        {
            Text = "FleetFlow — New Customer Location";
            lblTitle.Text = "New Location";
            btnSave.Text = "Save Location";
            return;
        }

        Text = $"FleetFlow — Edit {_location.LocationCode}";
        lblTitle.Text = "Edit Location";
        btnSave.Text = "Update Location";
    }

    private void DisplayLocation(CustomerLocationItem location)
    {
        txtLocationCode.Text = location.LocationCode;
        cboLocationType.SelectedItem = location.LocationType;
        txtLocationName.Text = location.LocationName;
        txtAddress1.Text = location.Address1;
        txtAddress2.Text = location.Address2;
        txtCity.Text = location.City;
        txtState.Text = location.StateCode;
        txtPostalCode.Text = location.PostalCode;

        txtLatitude.Text = location.Latitude?.ToString(
            CultureInfo.CurrentCulture);

        txtLongitude.Text = location.Longitude?.ToString(
            CultureInfo.CurrentCulture);

        txtContactName.Text = location.ContactName;
        txtContactPhone.Text = location.ContactPhone;
        chkBilling.Checked = location.IsBillingLocation;
    }

    private async void btnSave_Click(
        object? sender,
        EventArgs e)
    {
        if (_customerService is null)
        {
            MessageBox.Show(
                "The customer service is not available.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        if (!TryBuildRequest(
                out SaveCustomerLocationRequest? request))
        {
            lblMessage.Text = "Review the highlighted fields.";
            return;
        }

        SetBusy(true);

        try
        {
            await _customerService.SaveLocationAsync(request!);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"The location could not be saved.\n\n{exception.Message}",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private bool TryBuildRequest(
        out SaveCustomerLocationRequest? request)
    {
        errorProvider.Clear();
        request = null;

        bool valid = true;

        valid &= Required(
            txtLocationCode,
            "Location code is required.");

        if (cboLocationType.SelectedItem is null)
        {
            errorProvider.SetError(
                cboLocationType,
                "Location type is required.");

            valid = false;
        }

        valid &= Required(
            txtLocationName,
            "Location name is required.");

        valid &= Required(
            txtAddress1,
            "Street address is required.");

        valid &= Required(
            txtCity,
            "City is required.");

        valid &= Required(
            txtState,
            "State is required.");

        valid &= Required(
            txtPostalCode,
            "Postal code is required.");

        string stateCode = txtState.Text.Trim();

        if (stateCode.Length > 0 && stateCode.Length != 2)
        {
            errorProvider.SetError(
                txtState,
                "Use the two-letter state code.");

            valid = false;
        }

        decimal? latitude = ParseCoordinate(
            txtLatitude,
            -90M,
            90M,
            ref valid);

        decimal? longitude = ParseCoordinate(
            txtLongitude,
            -180M,
            180M,
            ref valid);

        if (!valid)
        {
            FocusFirstInvalidControl();
            return false;
        }

        request = new SaveCustomerLocationRequest
        {
            LocationId = _location?.LocationId,
            CustomerId = _customerId,
            LocationCode =
                txtLocationCode.Text.Trim().ToUpperInvariant(),
            LocationType =
                cboLocationType.SelectedItem?.ToString() ?? "CUSTOMER",
            LocationName =
                txtLocationName.Text.Trim(),
            Address1 =
                txtAddress1.Text.Trim(),
            Address2 =
                Optional(txtAddress2.Text),
            City =
                txtCity.Text.Trim(),
            StateCode =
                stateCode.ToUpperInvariant(),
            PostalCode =
                txtPostalCode.Text.Trim(),
            Latitude = latitude,
            Longitude = longitude,
            ContactName =
                Optional(txtContactName.Text),
            ContactPhone =
                Optional(txtContactPhone.Text),
            IsBillingLocation =
                chkBilling.Checked,
            ExpectedRowVersion =
                _location?.RowVersion
        };

        return true;
    }

    private bool Required(
        TextBox control,
        string message)
    {
        if (!string.IsNullOrWhiteSpace(control.Text))
        {
            return true;
        }

        errorProvider.SetError(control, message);
        return false;
    }

    private decimal? ParseCoordinate(
        TextBox control,
        decimal minimum,
        decimal maximum,
        ref bool valid)
    {
        string text = control.Text.Trim();

        if (text.Length == 0)
        {
            return null;
        }

        bool parsed = decimal.TryParse(
            text,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out decimal value);

        if (parsed && value >= minimum && value <= maximum)
        {
            return value;
        }

        errorProvider.SetError(
            control,
            $"Enter a value between {minimum} and {maximum}.");

        valid = false;
        return null;
    }

    private void FocusFirstInvalidControl()
    {
        Control[] controls =
        {
            txtLocationCode,
            cboLocationType,
            txtLocationName,
            txtAddress1,
            txtCity,
            txtState,
            txtPostalCode,
            txtLatitude,
            txtLongitude
        };

        foreach (Control control in controls)
        {
            if (!string.IsNullOrEmpty(errorProvider.GetError(control)))
            {
                control.Focus();
                return;
            }
        }
    }

    private void SetBusy(bool busy)
    {
        pnlFields.Enabled = !busy;
        btnSave.Enabled = !busy;
        btnCancel.Enabled = !busy;
        UseWaitCursor = busy;

        lblMessage.Text = busy
            ? "Saving location..."
            : string.Empty;
    }

    private void btnCancel_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    private static string? Optional(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}