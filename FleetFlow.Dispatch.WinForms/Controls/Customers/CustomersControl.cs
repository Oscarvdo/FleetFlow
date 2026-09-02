using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Customers;

namespace FleetFlow.Dispatch.WinForms.Controls.Customers;

public partial class CustomersControl : UserControl
{
    private readonly ICustomerService? _customerService;
    private IReadOnlyList<CustomerListItem> _customers = [];

    public event Action<long>? CustomerOpenRequested;
    public event Action<long>? CustomerEditRequested;
    public event EventHandler? CustomerCreateRequested;

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(
        System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool CanManageCustomers
    {
        get => btnNewCustomer.Visible;
        set
        {
            btnNewCustomer.Visible = value;
            btnEditCustomer.Visible = value;
        }
    }

    public CustomersControl()
    {
        InitializeComponent();
        ConfigureGrid();
        btnRefresh.Click += async (_, _) => await RefreshCustomersAsync();
        btnNewCustomer.Click += (_, _) => CustomerCreateRequested?.Invoke(this, EventArgs.Empty);
        btnEditCustomer.Click += (_, _) => EditSelected();
        txtSearch.TextChanged += (_, _) => ApplyFilter();
        chkIncludeInactive.CheckedChanged += async (_, _) => await RefreshCustomersAsync();
        dgvCustomers.CellDoubleClick += (_, e) => OpenRow(e.RowIndex);
        dgvCustomers.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                OpenSelected();
            }
        };
    }

    public CustomersControl(ICustomerService customerService) : this()
    {
        _customerService = customerService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_customerService is not null)
        {
            await RefreshCustomersAsync();
        }
    }

    public async Task RefreshCustomersAsync()
    {
        if (_customerService is null) return;
        SetBusy(true);

        try
        {
            _customers = await _customerService.SearchAsync(
                includeInactive: chkIncludeInactive.Checked);
            ApplyFilter();
            lblStatus.Text = $"Updated {DateTime.Now:g}";
        }
        catch (Exception exception)
        {
            lblStatus.Text = "Unable to load customers";
            MessageBox.Show(
                $"Customers could not be loaded.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ConfigureGrid()
    {
        dgvCustomers.AutoGenerateColumns = false;
        dgvCustomers.Columns.AddRange(
            Column("CustomerNumber", "CUSTOMER #", 85),
            Column("CompanyName", "COMPANY", 155),
            Column("ContactName", "CONTACT", 110),
            Column("Email", "EMAIL", 135),
            Column("Phone", "PHONE", 90),
            Column("LocationCount", "LOCATIONS", 65),
            Column("LoadCount", "LOADS", 55),
            Column("IsActive", "STATUS", 65));

        dgvCustomers.CellFormatting += (_, e) =>
        {
            if (dgvCustomers.Columns[e.ColumnIndex].DataPropertyName == "IsActive" &&
                e.Value is bool isActive)
            {
                e.Value = isActive ? "Active" : "Inactive";
                e.FormattingApplied = true;
            }
        };
    }

    private static DataGridViewTextBoxColumn Column(
        string property, string header, float weight) => new()
    {
        DataPropertyName = property,
        HeaderText = header,
        FillWeight = weight,
        ReadOnly = true
    };

    private void ApplyFilter()
    {
        string search = txtSearch.Text.Trim();
        IEnumerable<CustomerListItem> filtered = _customers;

        if (search.Length > 0)
        {
            filtered = filtered.Where(customer =>
                Contains(customer.CustomerNumber, search) ||
                Contains(customer.CompanyName, search) ||
                Contains(customer.ContactName, search) ||
                Contains(customer.Email, search) ||
                Contains(customer.Phone, search));
        }

        List<CustomerListItem> result = filtered.ToList();
        dgvCustomers.DataSource = result;
        lblCount.Text = $"{result.Count:N0} customer{(result.Count == 1 ? string.Empty : "s")}";
    }

    private static bool Contains(string? value, string search) =>
        value?.Contains(search, StringComparison.OrdinalIgnoreCase) == true;

    private void OpenRow(int rowIndex)
    {
        if (rowIndex < 0) return;
        dgvCustomers.ClearSelection();
        dgvCustomers.Rows[rowIndex].Selected = true;
        dgvCustomers.CurrentCell = dgvCustomers.Rows[rowIndex].Cells[0];
        OpenSelected();
    }

    private void OpenSelected()
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is CustomerListItem customer)
        {
            CustomerOpenRequested?.Invoke(customer.CustomerId);
        }
    }

    private void EditSelected()
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is CustomerListItem customer)
        {
            CustomerEditRequested?.Invoke(customer.CustomerId);
            return;
        }

        MessageBox.Show(
            "Select a customer before choosing Edit Selected.",
            "FleetFlow",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void SetBusy(bool busy)
    {
        btnRefresh.Enabled = !busy;
        btnNewCustomer.Enabled = !busy;
        btnEditCustomer.Enabled = !busy && dgvCustomers.CurrentRow is not null;
        txtSearch.Enabled = !busy;
        chkIncludeInactive.Enabled = !busy;
        UseWaitCursor = busy;
        if (busy) lblStatus.Text = "Loading customers...";
    }
}
