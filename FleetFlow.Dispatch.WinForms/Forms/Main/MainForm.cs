using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Authentication;
using FleetFlow.Dispatch.WinForms.Controls.Dashboard;
using FleetFlow.Dispatch.WinForms.Controls.Dispatch;

namespace FleetFlow.Dispatch.WinForms.Forms.Main;

public partial class MainForm : Form
{
    private readonly UserSession? _session;
    private readonly IDashboardService? _dashboardService;
    private readonly IDispatchBoardService? _dispatchBoardService;

    public MainForm()
    {
        InitializeComponent();
        WireEvents();
    }

    public MainForm(
        UserSession session,
        IDashboardService dashboardService,
        IDispatchBoardService dispatchBoardService)
        : this()
    {
        _session = session;
        _dashboardService = dashboardService;
        _dispatchBoardService = dispatchBoardService;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_session is null)
        {
            return;
        }

        Text = $"FleetFlow — {_session.User.Username}";
        lblUserName.Text = _session.User.Username;

        lblUserRole.Text = string.Join(
            ", ",
            _session.Roles.Select(
                role => role.Code.Replace('_', ' ')));

        ApplyPermissions();
        SelectNavigationButton(btnDashboard);
        ShowDashboard();
    }

    private void WireEvents()
    {
        btnDashboard.Click += NavigationButton_Click;
        btnDispatch.Click += NavigationButton_Click;
        btnTrips.Click += NavigationButton_Click;
        btnLoads.Click += NavigationButton_Click;
        btnCustomers.Click += NavigationButton_Click;
        btnFleet.Click += NavigationButton_Click;
        btnTracking.Click += NavigationButton_Click;
        btnReports.Click += NavigationButton_Click;
        btnAdministration.Click += NavigationButton_Click;
        btnLogout.Click += btnLogout_Click;
    }

    private void ApplyPermissions()
    {
        if (_session is null)
        {
            return;
        }

        btnDispatch.Visible =
            _session.HasPermission("DISPATCH.VIEW");

        btnTrips.Visible =
            _session.HasPermission("TRIPS.VIEW");

        btnLoads.Visible =
            _session.HasPermission("LOADS.VIEW");

        btnCustomers.Visible =
            _session.HasPermission("CUSTOMERS.VIEW");

        btnFleet.Visible =
            _session.HasPermission("FLEET.VIEW");

        btnTracking.Visible =
            _session.HasPermission("DISPATCH.VIEW");

        btnReports.Visible =
            _session.HasPermission("REPORTS.VIEW");

        btnAdministration.Visible =
            _session.HasPermission("SECURITY.USERS.VIEW") ||
            _session.HasPermission("SECURITY.AUDIT.VIEW");
    }

    private void NavigationButton_Click(
        object? sender,
        EventArgs e)
    {
        if (sender is not Button selectedButton)
        {
            return;
        }

        SelectNavigationButton(selectedButton);
        lblPageTitle.Text = selectedButton.Text;

        if (selectedButton == btnDashboard)
        {
            ShowDashboard();
            return;
        }

        if (selectedButton == btnDispatch)
        {
            ShowDispatchBoard();
            return;
        }

        ShowPlaceholder(selectedButton.Text);
    }

    private void ShowDashboard()
    {
        if (_dashboardService is null)
        {
            ShowPlaceholder("Dashboard unavailable");
            return;
        }

        var dashboardControl =
            new DashboardControl(_dashboardService)
            {
                Dock = DockStyle.Fill
            };

        ShowContent(dashboardControl);
        lblPageTitle.Text = "Dashboard";
    }

    private void ShowDispatchBoard()
    {
        if (_dispatchBoardService is null)
        {
            ShowPlaceholder("Dispatch Board unavailable");
            return;
        }

        var dispatchBoardControl =
            new DispatchBoardControl(_dispatchBoardService)
            {
                Dock = DockStyle.Fill
            };

        ShowContent(dispatchBoardControl);
        lblPageTitle.Text = "Dispatch Board";
    }

    private void ShowPlaceholder(string moduleName)
    {
        var placeholder = new Panel
        {
            BackColor = Color.FromArgb(244, 246, 249),
            Dock = DockStyle.Fill
        };

        var title = new Label
        {
            AutoSize = true,
            Font = new Font(
                "Segoe UI",
                24F,
                FontStyle.Bold),
            ForeColor = Color.FromArgb(29, 39, 54),
            Location = new Point(38, 40),
            Text = moduleName
        };

        var description = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(92, 103, 118),
            Location = new Point(42, 94),
            Text = $"The {moduleName} module will load here."
        };

        placeholder.Controls.Add(title);
        placeholder.Controls.Add(description);

        ShowContent(placeholder);
    }

    private void ShowContent(Control content)
    {
        foreach (Control existingControl in
                 pnlContentHost.Controls.Cast<Control>().ToArray())
        {
            existingControl.Dispose();
        }

        pnlContentHost.Controls.Clear();
        pnlContentHost.Controls.Add(content);
    }

    private void SelectNavigationButton(Button selectedButton)
    {
        foreach (Button button in
                 flpNavigation.Controls.OfType<Button>())
        {
            button.BackColor = Color.FromArgb(29, 39, 54);
            button.ForeColor = Color.FromArgb(220, 226, 234);
            button.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Regular);
        }

        selectedButton.BackColor =
            Color.FromArgb(243, 108, 33);

        selectedButton.ForeColor = Color.White;

        selectedButton.Font = new Font(
            "Segoe UI",
            10F,
            FontStyle.Bold);
    }

    private void btnLogout_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult result = MessageBox.Show(
            "Do you want to sign out of FleetFlow?",
            "Sign Out",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            Close();
        }
    }
}