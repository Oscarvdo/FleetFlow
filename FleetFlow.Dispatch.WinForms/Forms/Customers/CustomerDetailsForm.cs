using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Customers;

namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

public partial class CustomerDetailsForm : Form
{
    private readonly long _customerId;
    private readonly ICustomerService? _customerService;
    private readonly bool _canManage;
    private CustomerDetails? _details;

    public bool WasUpdated { get; private set; }
    public event Action<long>? LoadOpenRequested;

    public CustomerDetailsForm()
    {
        InitializeComponent();
        btnRefresh.Click += async (_, _) => await LoadCustomerAsync();
        btnEdit.Click += btnEdit_Click;
        btnSetActive.Click += btnSetActive_Click;
        btnNewLocation.Click += btnNewLocation_Click;
        btnEditLocation.Click += btnEditLocation_Click;
        btnLocationStatus.Click += btnLocationStatus_Click;
        dgvLocations.SelectionChanged += (_, _) => UpdateLocationActions();
        dgvLocations.CellDoubleClick += (_, e) =>
        {
            if (_canManage && e.RowIndex >= 0)
                btnEditLocation.PerformClick();
        };
        btnClose.Click += (_, _) => Close();
        dgvLoads.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0 &&
                dgvLoads.Rows[e.RowIndex].DataBoundItem is CustomerRecentLoadItem load)
            {
                LoadOpenRequested?.Invoke(load.LoadId);
            }
        };
    }

    public CustomerDetailsForm(
        long customerId,
        ICustomerService customerService,
        bool canManage) : this()
    {
        _customerId = customerId;
        _customerService = customerService;
        _canManage = canManage;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_customerService is not null)
        {
            await LoadCustomerAsync();
        }
    }

    private async Task LoadCustomerAsync()
    {
        if (_customerService is null) return;
        SetBusy(true);

        try
        {
            _details = await _customerService.GetByIdAsync(_customerId);
            if (_details is null)
            {
                MessageBox.Show("The selected customer no longer exists.",
                    "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            Display(_details);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Customer details could not be loaded.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void Display(CustomerDetails details)
    {
        Text = $"FleetFlow — {details.CustomerNumber}";
        lblCustomerNumber.Text = details.CustomerNumber;
        lblCompanyName.Text = details.CompanyName;
        lblStatus.Text = details.IsActive ? "ACTIVE" : "INACTIVE";
        lblStatus.ForeColor = details.IsActive
            ? Color.FromArgb(35, 130, 85)
            : Color.Firebrick;
        lblContactValue.Text = Value(details.ContactName);
        lblEmailValue.Text = Value(details.Email);
        lblPhoneValue.Text = Value(details.Phone);
        lblCreatedValue.Text = Local(details.CreatedAtUtc);
        lblUpdatedValue.Text = Local(details.UpdatedAtUtc);
        lblLoadsValue.Text = details.LoadCount.ToString("N0");
        lblOpenLoadsValue.Text = details.OpenLoadCount.ToString("N0");
        lblRevenueValue.Text = details.TotalRevenueAmount.ToString("C");
        dgvLocations.DataSource = details.Locations.ToList();
        dgvLoads.DataSource = details.RecentLoads.ToList();
        lblMessage.Text = $"Updated {DateTime.Now:g}";
        btnEdit.Visible = _canManage;
        btnSetActive.Visible = _canManage;
        btnSetActive.Text = details.IsActive ? "Deactivate" : "Activate";
        btnNewLocation.Visible = _canManage;
        btnEditLocation.Visible = _canManage;
        btnLocationStatus.Visible = _canManage;
        UpdateLocationActions();
    }

    private async void btnEdit_Click(object? sender, EventArgs e)
    {
        if (!_canManage || _customerService is null || _details is null) return;
        using var form = new CustomerForm(_customerService, _details);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            WasUpdated = true;
            await LoadCustomerAsync();
        }
    }

    private async void btnSetActive_Click(object? sender, EventArgs e)
    {
        if (!_canManage || _customerService is null || _details is null) return;
        bool activate = !_details.IsActive;
        string action = activate ? "activate" : "deactivate";
        if (MessageBox.Show($"Do you want to {action} this customer?",
                "FleetFlow", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        SetBusy(true);
        try
        {
            await _customerService.SetActiveAsync(
                _details.CustomerId, activate, _details.RowVersion);
            WasUpdated = true;
            await LoadCustomerAsync();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"The customer status could not be changed.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnNewLocation_Click(object? sender, EventArgs e)
    {
        if (!_canManage || _customerService is null) return;
        using var form = new CustomerLocationForm(_customerId, _customerService);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            WasUpdated = true;
            await LoadCustomerAsync();
        }
    }

    private async void btnEditLocation_Click(object? sender, EventArgs e)
    {
        if (!_canManage || _customerService is null || SelectedLocation() is not { } location)
        {
            ShowSelectLocation();
            return;
        }

        using var form = new CustomerLocationForm(_customerId, _customerService, location);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            WasUpdated = true;
            await LoadCustomerAsync();
        }
    }

    private async void btnLocationStatus_Click(object? sender, EventArgs e)
    {
        if (!_canManage || _customerService is null || SelectedLocation() is not { } location)
        {
            ShowSelectLocation();
            return;
        }

        bool activate = !location.IsActive;
        string action = activate ? "activate" : "deactivate";
        if (MessageBox.Show($"Do you want to {action} {location.LocationName}?",
                "FleetFlow", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        SetBusy(true);
        try
        {
            await _customerService.SetLocationActiveAsync(
                _customerId, location.LocationId, activate, location.RowVersion);
            WasUpdated = true;
            await LoadCustomerAsync();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"The location status could not be changed.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private CustomerLocationItem? SelectedLocation() =>
        dgvLocations.CurrentRow?.DataBoundItem as CustomerLocationItem;

    private void UpdateLocationActions()
    {
        CustomerLocationItem? location = SelectedLocation();
        btnEditLocation.Enabled = location is not null;
        btnLocationStatus.Enabled = location is not null;
        btnLocationStatus.Text = location?.IsActive == false ? "Activate" : "Deactivate";
    }

    private static void ShowSelectLocation()
    {
        MessageBox.Show("Select a location first.", "FleetFlow",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void SetBusy(bool busy)
    {
        btnRefresh.Enabled = !busy;
        btnEdit.Enabled = !busy;
        btnSetActive.Enabled = !busy;
        btnNewLocation.Enabled = !busy;
        btnEditLocation.Enabled = !busy && SelectedLocation() is not null;
        btnLocationStatus.Enabled = !busy && SelectedLocation() is not null;
        UseWaitCursor = busy;
        if (busy) lblMessage.Text = "Loading customer details...";
    }

    private static string Value(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "—" : value.Trim();

    private static string Local(DateTime utc) =>
        DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime().ToString("g");
}
