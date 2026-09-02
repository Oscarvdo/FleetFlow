using System.ComponentModel.DataAnnotations;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Customers;

namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

public partial class CustomerForm : Form
{
    private readonly ICustomerService? _customerService;
    private readonly CustomerDetails? _existingCustomer;

    public long? SavedCustomerId { get; private set; }

    public CustomerForm()
    {
        InitializeComponent();
        btnSave.Click += btnSave_Click;
        btnCancel.Click += (_, _) => Close();
    }

    public CustomerForm(
        ICustomerService customerService,
        CustomerDetails? existingCustomer = null) : this()
    {
        _customerService = customerService;
        _existingCustomer = existingCustomer;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_existingCustomer is null)
        {
            Text = "FleetFlow — New Customer";
            lblTitle.Text = "New Customer";
            lblSubtitle.Text = "Create a customer account for loads and locations.";
            return;
        }

        Text = $"FleetFlow — Edit {_existingCustomer.CustomerNumber}";
        lblTitle.Text = "Edit Customer";
        lblSubtitle.Text = "Update account and primary contact information.";
        txtCustomerNumber.Text = _existingCustomer.CustomerNumber;
        txtCompanyName.Text = _existingCustomer.CompanyName;
        txtContactName.Text = _existingCustomer.ContactName;
        txtEmail.Text = _existingCustomer.Email;
        txtPhone.Text = _existingCustomer.Phone;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_customerService is null || !ValidateInput()) return;
        SetBusy(true);

        try
        {
            CustomerCommandResult result = await _customerService.SaveAsync(
                new SaveCustomerRequest
                {
                    CustomerId = _existingCustomer?.CustomerId,
                    CustomerNumber = txtCustomerNumber.Text,
                    CompanyName = txtCompanyName.Text,
                    ContactName = NullIfWhiteSpace(txtContactName.Text),
                    Email = NullIfWhiteSpace(txtEmail.Text),
                    Phone = NullIfWhiteSpace(txtPhone.Text),
                    ExpectedRowVersion = _existingCustomer?.RowVersion
                });

            SavedCustomerId = result.CustomerId;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"The customer could not be saved.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private bool ValidateInput()
    {
        errorProvider.Clear();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(txtCustomerNumber.Text))
        {
            errorProvider.SetError(txtCustomerNumber, "Customer number is required.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
        {
            errorProvider.SetError(txtCompanyName, "Company name is required.");
            valid = false;
        }

        string email = txtEmail.Text.Trim();
        if (email.Length > 0 && !new EmailAddressAttribute().IsValid(email))
        {
            errorProvider.SetError(txtEmail, "Enter a valid email address.");
            valid = false;
        }

        return valid;
    }

    private void SetBusy(bool busy)
    {
        btnSave.Enabled = !busy;
        btnCancel.Enabled = !busy;
        pnlFields.Enabled = !busy;
        UseWaitCursor = busy;
        lblMessage.Text = busy ? "Saving customer..." : "";
    }

    private static string? NullIfWhiteSpace(string value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
