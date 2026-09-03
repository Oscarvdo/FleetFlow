using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Tracking;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Authentication;
using FleetFlow.Application.Fleet;
using FleetFlow.Dispatch.WinForms.Controls.Dashboard;
using FleetFlow.Dispatch.WinForms.Controls.Customers;
using FleetFlow.Dispatch.WinForms.Controls.Dispatch;
using FleetFlow.Dispatch.WinForms.Controls.Fleet;
using FleetFlow.Dispatch.WinForms.Controls.Loads;
using FleetFlow.Dispatch.WinForms.Controls.Tracking;
using FleetFlow.Dispatch.WinForms.Controls.Trips;
using FleetFlow.Dispatch.WinForms.Forms.Loads;
using FleetFlow.Dispatch.WinForms.Forms.Customers;
using FleetFlow.Dispatch.WinForms.Forms.Fleet;
using FleetFlow.Dispatch.WinForms.Forms.Trips;

namespace FleetFlow.Dispatch.WinForms.Forms.Main;

public partial class MainForm : Form
{
    private readonly UserSession? _session;

    private readonly IDashboardService?
        _dashboardService;

    private readonly IDispatchBoardService?
        _dispatchBoardService;

    private readonly ITripDetailsService?
        _tripDetailsService;

    private readonly ITripListService?
        _tripListService;

    private readonly ILoadListService?
        _loadListService;

    private readonly ILoadDetailsService?
        _loadDetailsService;

    private readonly ILoadCommandService?
        _loadCommandService;

    private readonly ICustomerLookupService?
        _customerLookupService;

    private readonly ICustomerService?
        _customerService;

    private readonly IFleetOverviewService?
        _fleetOverviewService;
    private readonly IFleetCommandService?
        _fleetCommandService;

    private readonly ILiveTrackingService?
        _liveTrackingService;

    private readonly ILiveTrackingSimulationEngine?
        _liveTrackingSimulationEngine;

    public MainForm()
    {
        InitializeComponent();
        WireEvents();
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// ActivatorUtilities proporciona automáticamente
    /// los servicios registrados en Infrastructure.
    /// </summary>
    public MainForm(
        UserSession session,
        IDashboardService dashboardService,
        IDispatchBoardService dispatchBoardService,
        ITripDetailsService tripDetailsService,
        ITripListService tripListService,
        ILoadListService loadListService,
        ILoadDetailsService loadDetailsService,
        ILoadCommandService loadCommandService,
        ICustomerLookupService customerLookupService,
        ICustomerService customerService,
        IFleetOverviewService fleetOverviewService,
        IFleetCommandService fleetCommandService,
        ILiveTrackingService liveTrackingService,
        ILiveTrackingSimulationEngine liveTrackingSimulationEngine)
        : this()
    {
        _session = session;
        _dashboardService = dashboardService;
        _dispatchBoardService = dispatchBoardService;
        _tripDetailsService = tripDetailsService;
        _tripListService = tripListService;
        _loadListService = loadListService;
        _loadDetailsService = loadDetailsService;
        _loadCommandService = loadCommandService;
        _customerLookupService = customerLookupService;
        _customerService = customerService;
        _fleetOverviewService = fleetOverviewService;
        _fleetCommandService = fleetCommandService;
        _liveTrackingService = liveTrackingService;
        _liveTrackingSimulationEngine = liveTrackingSimulationEngine;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_session is null)
        {
            return;
        }

        Text =
            $"FleetFlow — {_session.User.Username}";

        lblUserName.Text =
            _session.User.Username;

        lblUserRole.Text = string.Join(
            ", ",
            _session.Roles.Select(
                role =>
                    role.Code.Replace('_', ' ')));

        ApplyPermissions();
        SelectNavigationButton(btnDashboard);
        ShowDashboard();
    }

    /// <summary>
    /// Conecta los botones del menú con un único
    /// controlador de navegación.
    /// </summary>
    private void WireEvents()
    {
        btnDashboard.Click +=
            NavigationButton_Click;

        btnDispatch.Click +=
            NavigationButton_Click;

        btnTrips.Click +=
            NavigationButton_Click;

        btnLoads.Click +=
            NavigationButton_Click;

        btnCustomers.Click +=
            NavigationButton_Click;

        btnFleet.Click +=
            NavigationButton_Click;

        btnTracking.Click +=
            NavigationButton_Click;

        btnReports.Click +=
            NavigationButton_Click;

        btnAdministration.Click +=
            NavigationButton_Click;

        btnLogout.Click +=
            btnLogout_Click;
    }

    /// <summary>
    /// Muestra únicamente los módulos autorizados
    /// para el usuario autenticado.
    /// </summary>
    private void ApplyPermissions()
    {
        if (_session is null)
        {
            return;
        }

        btnDispatch.Visible =
            _session.HasPermission(
                "DISPATCH.VIEW");

        btnTrips.Visible =
            _session.HasPermission(
                "TRIPS.VIEW");

        btnLoads.Visible =
            _session.HasPermission(
                "LOADS.VIEW");

        btnCustomers.Visible =
            _session.HasPermission(
                "CUSTOMERS.VIEW");

        btnFleet.Visible =
            _session.HasPermission(
                "FLEET.VIEW");

        btnTracking.Visible =
            _session.HasPermission(
                "DISPATCH.VIEW");

        btnReports.Visible =
            _session.HasPermission(
                "REPORTS.VIEW");

        btnAdministration.Visible =
            _session.HasPermission(
                "SECURITY.USERS.VIEW") ||
            _session.HasPermission(
                "SECURITY.AUDIT.VIEW");
    }

    /// <summary>
    /// Decide cuál control debe mostrarse según
    /// el botón seleccionado.
    /// </summary>
    private void NavigationButton_Click(
        object? sender,
        EventArgs e)
    {
        if (sender is not Button selectedButton)
        {
            return;
        }

        SelectNavigationButton(selectedButton);

        lblPageTitle.Text =
            selectedButton.Text;

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

        if (selectedButton == btnTrips)
        {
            ShowTrips();
            return;
        }

        if (selectedButton == btnLoads)
        {
            ShowLoads();
            return;
        }

        if (selectedButton == btnCustomers)
        {
            ShowCustomers();
            return;
        }

        if (selectedButton == btnFleet)
        {
            ShowFleet();
            return;
        }

        if (selectedButton == btnTracking)
        {
            ShowLiveTracking();
            return;
        }

        ShowPlaceholder(
            selectedButton.Text);
    }

    /// <summary>
    /// Muestra los indicadores operacionales.
    /// </summary>
    private void ShowDashboard()
    {
        if (_dashboardService is null)
        {
            ShowPlaceholder(
                "Dashboard unavailable");

            return;
        }

        var dashboardControl =
            new DashboardControl(
                _dashboardService)
            {
                Dock = DockStyle.Fill
            };

        ShowContent(dashboardControl);
        lblPageTitle.Text = "Dashboard";
    }

    /// <summary>
    /// Muestra los viajes activos utilizados
    /// por el equipo de despacho.
    /// </summary>
    private void ShowDispatchBoard()
    {
        if (_dispatchBoardService is null)
        {
            ShowPlaceholder(
                "Dispatch Board unavailable");

            return;
        }

        var dispatchBoardControl =
            new DispatchBoardControl(
                _dispatchBoardService)
            {
                Dock = DockStyle.Fill
            };

        dispatchBoardControl.TripOpenRequested +=
            OpenTripDetails;

        ShowContent(dispatchBoardControl);
        lblPageTitle.Text = "Dispatch Board";
    }

    /// <summary>
    /// Muestra todos los viajes y sus filtros.
    /// </summary>
    private void ShowTrips()
    {
        if (_tripListService is null)
        {
            ShowPlaceholder("Trips unavailable");
            return;
        }

        var tripsControl =
            new TripsControl(
                _tripListService)
            {
                Dock = DockStyle.Fill
            };

        tripsControl.TripOpenRequested +=
            OpenTripDetails;

        ShowContent(tripsControl);
        lblPageTitle.Text = "Trips";
    }

    /// <summary>
    /// Muestra todas las cargas y sus filtros.
    /// </summary>
    private void ShowLoads()
    {
        if (_loadListService is null)
        {
            ShowPlaceholder("Loads unavailable");
            return;
        }

        var loadsControl =
            new LoadsControl(
                _loadListService)
            {
                Dock = DockStyle.Fill,
                CanCreateLoads =
                    _session?.HasPermission(
                        "LOADS.MANAGE") == true
            };

        // El doble clic abre LoadDetailsForm.
        loadsControl.LoadOpenRequested +=
            OpenLoadDetails;

        // El botón New Load solicita a MainForm
        // abrir CreateLoadForm.
        loadsControl.LoadCreateRequested +=
            OpenCreateLoad;

        ShowContent(loadsControl);
        lblPageTitle.Text = "Loads";
    }

    private void ShowCustomers()
    {
        if (_customerService is null)
        {
            ShowPlaceholder("Customers unavailable");
            return;
        }

        var customersControl = new CustomersControl(_customerService)
        {
            Dock = DockStyle.Fill,
            CanManageCustomers =
                _session?.HasPermission("CUSTOMERS.MANAGE") == true
        };

        customersControl.CustomerOpenRequested += OpenCustomerDetails;
        customersControl.CustomerEditRequested += OpenEditCustomer;
        customersControl.CustomerCreateRequested += OpenCreateCustomer;
        ShowContent(customersControl);
        lblPageTitle.Text = "Customers";
    }

    private void ShowFleet()
    {
        if (_fleetOverviewService is null)
        {
            ShowPlaceholder("Fleet unavailable");
            return;
        }

        var fleetControl = new FleetControl(_fleetOverviewService)
        {
            Dock = DockStyle.Fill,
            CanManageVehicles = _session?.HasPermission("FLEET.MANAGE") == true,
            CanManageTrailers = _session?.HasPermission("FLEET.MANAGE") == true
        };
        fleetControl.VehicleCreateRequested += OpenCreateVehicle;
        fleetControl.VehicleEditRequested += OpenEditVehicle;
        fleetControl.TrailerCreateRequested += OpenCreateTrailer;
        fleetControl.TrailerEditRequested += OpenEditTrailer;
        ShowContent(fleetControl);
        lblPageTitle.Text = "Fleet";
    }

    private void ShowLiveTracking()
    {
        if (_liveTrackingService is null ||
            _liveTrackingSimulationEngine is null)
        {
            ShowPlaceholder("Live Tracking unavailable");
            return;
        }

        bool canManageSimulation =
            _session?.HasPermission("FLEET.MANAGE") == true;

        var trackingControl = new LiveTrackingControl(
            _liveTrackingService,
            _liveTrackingSimulationEngine,
            canManageSimulation)
        {
            Dock = DockStyle.Fill
        };

        ShowContent(trackingControl);
        lblPageTitle.Text = "Live Tracking";
    }

    private async void OpenCreateVehicle(object? sender, EventArgs e)
    {
        if (_fleetCommandService is null) return;
        using var form = new VehicleForm(_fleetCommandService);
        if (form.ShowDialog(this) == DialogResult.OK && sender is FleetControl fleetControl)
            await fleetControl.RefreshFleetAsync();
    }

    private async void OpenEditVehicle(FleetOverviewVehicleItem vehicle)
    {
        if (_fleetCommandService is null) return;
        using var form = new VehicleForm(_fleetCommandService, vehicle);
        if (form.ShowDialog(this) == DialogResult.OK && pnlContentHost.Controls.OfType<FleetControl>().FirstOrDefault() is FleetControl fleetControl)
            await fleetControl.RefreshFleetAsync();
    }

    private async void OpenCreateTrailer(object? sender, EventArgs e)
    {
        if (_fleetCommandService is null) return;
        using var form = new TrailerForm(_fleetCommandService);
        if (form.ShowDialog(this) == DialogResult.OK && sender is FleetControl fleetControl)
            await fleetControl.RefreshFleetAsync();
    }

    private async void OpenEditTrailer(FleetOverviewTrailerItem trailer)
    {
        if (_fleetCommandService is null) return;
        using var form = new TrailerForm(_fleetCommandService, trailer);
        if (form.ShowDialog(this) == DialogResult.OK && pnlContentHost.Controls.OfType<FleetControl>().FirstOrDefault() is FleetControl fleetControl)
            await fleetControl.RefreshFleetAsync();
    }

    private async void OpenCreateCustomer(object? sender, EventArgs e)
    {
        if (_customerService is null ||
            _session?.HasPermission("CUSTOMERS.MANAGE") != true)
        {
            MessageBox.Show("You do not have permission to create customers.",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new CustomerForm(_customerService);
        if (form.ShowDialog(this) == DialogResult.OK &&
            sender is CustomersControl customersControl)
        {
            await customersControl.RefreshCustomersAsync();
        }
    }

    private async void OpenCustomerDetails(long customerId)
    {
        if (_customerService is null) return;

        using var form = new CustomerDetailsForm(
            customerId,
            _customerService,
            _session?.HasPermission("CUSTOMERS.MANAGE") == true);

        form.LoadOpenRequested += OpenLoadDetails;
        form.ShowDialog(this);

        if (form.WasUpdated &&
            pnlContentHost.Controls.OfType<CustomersControl>().FirstOrDefault()
                is CustomersControl customersControl)
        {
            await customersControl.RefreshCustomersAsync();
        }
    }

    private async void OpenEditCustomer(long customerId)
    {
        if (_customerService is null ||
            _session?.HasPermission("CUSTOMERS.MANAGE") != true)
        {
            MessageBox.Show("You do not have permission to edit customers.",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var customer = await _customerService.GetByIdAsync(customerId);
            if (customer is null)
            {
                MessageBox.Show("The selected customer no longer exists.",
                    "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new CustomerForm(_customerService, customer);
            if (form.ShowDialog(this) == DialogResult.OK &&
                pnlContentHost.Controls.OfType<CustomersControl>().FirstOrDefault()
                    is CustomersControl customersControl)
            {
                await customersControl.RefreshCustomersAsync();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"The customer could not be opened.\n\n{exception.Message}",
                "FleetFlow", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Abre el formulario para crear una carga.
    /// Si la operación finaliza correctamente,
    /// actualiza la lista sin cambiar de módulo.
    /// </summary>
    private async void OpenCreateLoad(
        object? sender,
        EventArgs e)
    {
        if (_session is null ||
            !_session.HasPermission("LOADS.MANAGE"))
        {
            MessageBox.Show(
                "You do not have permission to create loads.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (_loadCommandService is null ||
            _customerLookupService is null)
        {
            MessageBox.Show(
                "The load creation services are unavailable.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using var createLoadForm =
            new CreateLoadForm(
                _loadCommandService,
                _customerLookupService);

        DialogResult result =
            createLoadForm.ShowDialog(this);

        // El formulario establece DialogResult.OK
        // únicamente después de guardar en SQL Server.
        if (result == DialogResult.OK &&
            sender is LoadsControl loadsControl)
        {
            await loadsControl.RefreshLoadsAsync();
        }
    }

    /// <summary>
    /// Abre el detalle de la carga seleccionada.
    /// </summary>
    private async void OpenLoadDetails(long loadId)
    {
        if (_loadDetailsService is null ||
            _tripDetailsService is null ||
            _loadCommandService is null ||
            _customerLookupService is null)
        {
            MessageBox.Show(
                "The load details service is unavailable.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using var loadDetailsForm =
            new LoadDetailsForm(
                loadId,
                _loadDetailsService,
                _tripDetailsService,
                _loadCommandService,
                _customerLookupService,
                _session?.HasPermission(
                    "LOADS.MANAGE") == true);

        loadDetailsForm.ShowDialog(this);

        if (loadDetailsForm.WasUpdated &&
            pnlContentHost.Controls
                .OfType<LoadsControl>()
                .FirstOrDefault() is
                    LoadsControl loadsControl)
        {
            await loadsControl.RefreshLoadsAsync();
        }
    }

    /// <summary>
    /// Abre el detalle del viaje seleccionado.
    /// </summary>
    private void OpenTripDetails(long tripId)
    {
        if (_tripDetailsService is null)
        {
            MessageBox.Show(
                "The trip details service is unavailable.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using var tripDetailsForm =
            new TripDetailsForm(
                tripId,
                _tripDetailsService);

        tripDetailsForm.ShowDialog(this);
    }

    /// <summary>
    /// Muestra una pantalla temporal para módulos
    /// que todavía no se han implementado.
    /// </summary>
    private void ShowPlaceholder(
        string moduleName)
    {
        var placeholder = new Panel
        {
            BackColor =
                Color.FromArgb(244, 246, 249),

            Dock = DockStyle.Fill
        };

        var title = new Label
        {
            AutoSize = true,

            Font = new System.Drawing.Font(
                "Segoe UI",
                24F,
                FontStyle.Bold),

            ForeColor =
                Color.FromArgb(29, 39, 54),

            Location =
                new Point(38, 40),

            Text = moduleName
        };

        var description = new Label
        {
            AutoSize = true,

            Font = new System.Drawing.Font(
                "Segoe UI",
                11F),

            ForeColor =
                Color.FromArgb(92, 103, 118),

            Location =
                new Point(42, 94),

            Text =
                $"The {moduleName} module will load here."
        };

        placeholder.Controls.Add(title);
        placeholder.Controls.Add(description);

        ShowContent(placeholder);
    }

    /// <summary>
    /// Sustituye el contenido actual del área principal
    /// y libera los controles anteriores.
    /// </summary>
    private void ShowContent(Control content)
    {
        foreach (Control existingControl in
                 pnlContentHost.Controls
                     .Cast<Control>()
                     .ToArray())
        {
            existingControl.Dispose();
        }

        pnlContentHost.Controls.Clear();
        pnlContentHost.Controls.Add(content);
    }

    /// <summary>
    /// Actualiza el estilo visual del botón activo.
    /// </summary>
    private void SelectNavigationButton(
        Button selectedButton)
    {
        foreach (Button button in
                 flpNavigation.Controls
                     .OfType<Button>())
        {
            button.BackColor =
                Color.FromArgb(29, 39, 54);

            button.ForeColor =
                Color.FromArgb(220, 226, 234);

            button.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Regular);
        }

        selectedButton.BackColor =
            Color.FromArgb(243, 108, 33);

        selectedButton.ForeColor =
            Color.White;

        selectedButton.Font =
            new System.Drawing.Font(
                "Segoe UI",
                10F,
                FontStyle.Bold);
    }

    private void btnLogout_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult result =
            MessageBox.Show(
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
