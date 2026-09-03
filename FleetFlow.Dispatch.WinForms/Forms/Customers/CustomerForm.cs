using System.ComponentModel.DataAnnotations;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Customers;

namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

public partial class CustomerForm : Form
{
    private readonly ICustomerService? _customerService;
    private readonly CustomerDetails? _existingCustomer;

    public long? SavedCustomerId { get; private set; }

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public CustomerForm()
    {
        InitializeComponent();

        btnSave.Click += btnSave_Click;
        btnCancel.Click += btnCancel_Click;
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public CustomerForm(
        ICustomerService customerService,
        CustomerDetails? existingCustomer = null)
        : this()
    {
        _customerService = customerService;
        _existingCustomer = existingCustomer;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        ConfigureForm();

        if (_existingCustomer is not null)
        {
            DisplayCustomer(_existingCustomer);
        }
    }

    private void ConfigureForm()
    {
        if (_existingCustomer is null)
        {
            Text = "FleetFlow — New Customer";
            lblTitle.Text = "New Customer";
            lblSubtitle.Text =
                "Create a customer account for loads and locations.";
            btnSave.Text = "Save Customer";

            return;
        }

        Text =
            $"FleetFlow — Edit {_existingCustomer.CustomerNumber}";
        lblTitle.Text = "Edit Customer";
        lblSubtitle.Text =
            "Update account and primary contact information.";
        btnSave.Text = "Update Customer";
    }

    private void DisplayCustomer(CustomerDetails customer)
    {
        txtCustomerNumber.Text = customer.CustomerNumber;
        txtCompanyName.Text = customer.CompanyName;
        txtContactName.Text = customer.ContactName;
        txtEmail.Text = customer.Email;
        txtPhone.Text = customer.Phone;
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

        if (!ValidateInput())
        {
            return;
        }

        SetBusy(true);

        try
        {
            SaveCustomerRequest request = new()
            {
                CustomerId = _existingCustomer?.CustomerId,
                CustomerNumber =
                    txtCustomerNumber.Text.Trim(),
                CompanyName =
                    txtCompanyName.Text.Trim(),
                ContactName =
                    NullIfWhiteSpace(txtContactName.Text),
                Email =
                    NullIfWhiteSpace(txtEmail.Text),
                Phone =
                    NullIfWhiteSpace(txtPhone.Text),
                ExpectedRowVersion =
                    _existingCustomer?.RowVersion
            };

            CustomerCommandResult result =
                await _customerService.SaveAsync(request);

            SavedCustomerId = result.CustomerId;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"The customer could not be saved.\n\n{exception.Message}",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
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
            errorProvider.SetError(
                txtCustomerNumber,
                "Customer number is required.");

            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
        {
            errorProvider.SetError(
                txtCompanyName,
                "Company name is required.");

            valid = false;
        }

        string email = txtEmail.Text.Trim();

        if (email.Length > 0 &&
            !new EmailAddressAttribute().IsValid(email))
        {
            errorProvider.SetError(
                txtEmail,
                "Enter a valid email address.");

            valid = false;
        }

        if (!valid)
        {
            lblMessage.Text =
                "Review the highlighted fields.";

            if (string.IsNullOrWhiteSpace(txtCustomerNumber.Text))
            {
                txtCustomerNumber.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                txtCompanyName.Focus();
            }
            else
            {
                txtEmail.Focus();
            }
        }

        return valid;
    }

    private void SetBusy(bool busy)
    {
        btnSave.Enabled = !busy;
        btnCancel.Enabled = !busy;
        pnlFields.Enabled = !busy;
        UseWaitCursor = busy;

        lblMessage.Text = busy
            ? "Saving customer..."
            : string.Empty;
    }

    private void btnCancel_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    private static string? NullIfWhiteSpace(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}