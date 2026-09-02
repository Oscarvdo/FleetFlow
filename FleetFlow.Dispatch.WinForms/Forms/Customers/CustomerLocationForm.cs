using System.Globalization;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Customers;

namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

public partial class CustomerLocationForm : Form
{
    private readonly long _customerId;
    private readonly ICustomerService? _customerService;
    private readonly CustomerLocationItem? _location;

    public CustomerLocationForm()
    {
        InitializeComponent();
        btnSave.Click += btnSave_Click;
        btnCancel.Click += (_, _) => Close();
    }

    public CustomerLocationForm(
        long customerId,
        ICustomerService customerService,
        CustomerLocationItem? location = null) : this()
    {
        _customerId = customerId;
        _customerService = customerService;
        _location = location;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        cboLocationType.Items.AddRange(["CUSTOMER", "TERMINAL", "FUEL", "REST_AREA", "OTHER"]);
        cboLocationType.SelectedItem = "CUSTOMER";

        if (_location is null)
        {
            Text = "FleetFlow — New Customer Location";
            lblTitle.Text = "New Location";
            return;
        }

        Text = $"FleetFlow — Edit {_location.LocationCode}";
        lblTitle.Text = "Edit Location";
        txtLocationCode.Text = _location.LocationCode;
        cboLocationType.SelectedItem = _location.LocationType;
        txtLocationName.Text = _location.LocationName;
        txtAddress1.Text = _location.Address1;
        txtAddress2.Text = _location.Address2;
        txtCity.Text = _location.City;
        txtState.Text = _location.StateCode;
        txtPostalCode.Text = _location.PostalCode;
        txtLatitude.Text = _location.Latitude?.ToString(CultureInfo.CurrentCulture);
        txtLongitude.Text = _location.Longitude?.ToString(CultureInfo.CurrentCulture);
        txtContactName.Text = _location.ContactName;
        txtContactPhone.Text = _location.ContactPhone;
        chkBilling.Checked = _location.IsBillingLocation;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_customerService is null || !TryBuildRequest(out SaveCustomerLocationRequest? request))
            return;

        SetBusy(true);
        try
        {
            await _customerService.SaveLocationAsync(request!);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"The location could not be saved.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private bool TryBuildRequest(out SaveCustomerLocationRequest? request)
    {
        errorProvider.Clear();
        request = null;
        bool valid = true;

        valid &= Required(txtLocationCode, "Location code is required.");
        valid &= Required(txtLocationName, "Location name is required.");
        valid &= Required(txtAddress1, "Street address is required.");
        valid &= Required(txtCity, "City is required.");
        valid &= Required(txtState, "State is required.");
        valid &= Required(txtPostalCode, "Postal code is required.");

        if (txtState.Text.Trim().Length != 2)
        {
            errorProvider.SetError(txtState, "Use the two-letter state code.");
            valid = false;
        }

        decimal? latitude = ParseCoordinate(txtLatitude, -90, 90, ref valid);
        decimal? longitude = ParseCoordinate(txtLongitude, -180, 180, ref valid);
        if (!valid) return false;

        request = new SaveCustomerLocationRequest
        {
            LocationId = _location?.LocationId,
            CustomerId = _customerId,
            LocationCode = txtLocationCode.Text,
            LocationType = cboLocationType.SelectedItem?.ToString() ?? "CUSTOMER",
            LocationName = txtLocationName.Text,
            Address1 = txtAddress1.Text,
            Address2 = Optional(txtAddress2.Text),
            City = txtCity.Text,
            StateCode = txtState.Text,
            PostalCode = txtPostalCode.Text,
            Latitude = latitude,
            Longitude = longitude,
            ContactName = Optional(txtContactName.Text),
            ContactPhone = Optional(txtContactPhone.Text),
            IsBillingLocation = chkBilling.Checked,
            ExpectedRowVersion = _location?.RowVersion
        };
        return true;
    }

    private bool Required(TextBox control, string message)
    {
        if (!string.IsNullOrWhiteSpace(control.Text)) return true;
        errorProvider.SetError(control, message);
        return false;
    }

    private decimal? ParseCoordinate(TextBox control, decimal minimum, decimal maximum, ref bool valid)
    {
        if (string.IsNullOrWhiteSpace(control.Text)) return null;
        if (decimal.TryParse(control.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal value) &&
            value >= minimum && value <= maximum)
            return value;
        errorProvider.SetError(control, $"Enter a value between {minimum} and {maximum}.");
        valid = false;
        return null;
    }

    private void SetBusy(bool busy)
    {
        pnlFields.Enabled = !busy;
        btnSave.Enabled = !busy;
        btnCancel.Enabled = !busy;
        UseWaitCursor = busy;
        lblMessage.Text = busy ? "Saving location..." : string.Empty;
    }

    private static string? Optional(string value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
