using FleetFlow.Application.Abstractions.Dashboard;
using FleetFlow.Application.Abstractions.Dispatch;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Authentication;
using FleetFlow.Dispatch.WinForms.Controls.Dashboard;
using FleetFlow.Dispatch.WinForms.Controls.Dispatch;
using FleetFlow.Dispatch.WinForms.Controls.Loads;
using FleetFlow.Dispatch.WinForms.Controls.Trips;
using FleetFlow.Dispatch.WinForms.Forms.Loads;
using FleetFlow.Dispatch.WinForms.Forms.Trips;
using FleetFlow.Dispatch.WinForms.Controls.Trips;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Dispatch.WinForms.Controls.Loads;

namespace FleetFlow.Dispatch.WinForms.Forms.Main;

public partial class MainForm : Form
{
    private readonly UserSession? _session;
<<<<<<< HEAD
    private readonly IDashboardService? _dashboardService;
    private readonly IDispatchBoardService? _dispatchBoardService;
    private readonly ITripDetailsService? _tripDetailsService;
    private readonly ITripListService? _tripListService;
    // Servicio que obtiene la lista de cargas.
    private readonly ILoadListService? _loadListService;
=======

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
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)

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
<<<<<<< HEAD
     UserSession session,
     IDashboardService dashboardService,
     IDispatchBoardService dispatchBoardService,
     ITripDetailsService tripDetailsService,
     ITripListService tripListService,
     ILoadListService loadListService)
     : this()
=======
        UserSession session,
        IDashboardService dashboardService,
        IDispatchBoardService dispatchBoardService,
        ITripDetailsService tripDetailsService,
        ITripListService tripListService,
        ILoadListService loadListService,
        ILoadDetailsService loadDetailsService)
        : this()
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
    {
        _session = session;
        _dashboardService = dashboardService;
        _dispatchBoardService = dispatchBoardService;
        _tripDetailsService = tripDetailsService;
        _tripListService = tripListService;
        _loadListService = loadListService;
<<<<<<< HEAD
=======
        _loadDetailsService = loadDetailsService;
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
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
<<<<<<< HEAD
=======

>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
        if (selectedButton == btnLoads)
        {
            ShowLoads();
            return;
        }

<<<<<<< HEAD
        ShowPlaceholder(selectedButton.Text);
=======
        ShowPlaceholder(
            selectedButton.Text);
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
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

<<<<<<< HEAD
=======
    /// <summary>
    /// Muestra todos los viajes y sus filtros.
    /// </summary>
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
    private void ShowTrips()
    {
        if (_tripListService is null)
        {
            ShowPlaceholder("Trips unavailable");
            return;
        }

        var tripsControl =
<<<<<<< HEAD
            new TripsControl(_tripListService)
=======
            new TripsControl(
                _tripListService)
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
            {
                Dock = DockStyle.Fill
            };

        tripsControl.TripOpenRequested +=
            OpenTripDetails;

        ShowContent(tripsControl);
        lblPageTitle.Text = "Trips";
    }

    /// <summary>
<<<<<<< HEAD
    /// Muestra la lista de cargas dentro del área principal.
=======
    /// Muestra todas las cargas y sus filtros.
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
    /// </summary>
    private void ShowLoads()
    {
        if (_loadListService is null)
        {
            ShowPlaceholder("Loads unavailable");
            return;
        }

        var loadsControl =
<<<<<<< HEAD
            new LoadsControl(_loadListService)
=======
            new LoadsControl(
                _loadListService)
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
            {
                Dock = DockStyle.Fill
            };

<<<<<<< HEAD
        // Reutilizamos el mismo formulario de detalles
        // cuando la carga tiene un viaje relacionado.
        loadsControl.TripOpenRequested +=
            OpenTripDetails;
=======
        // El doble clic abre LoadDetailsForm.
        loadsControl.LoadOpenRequested +=
            OpenLoadDetails;
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)

        ShowContent(loadsControl);
        lblPageTitle.Text = "Loads";
    }
<<<<<<< HEAD
=======

    /// <summary>
    /// Abre el detalle de la carga seleccionada.
    /// </summary>
    private void OpenLoadDetails(long loadId)
    {
        if (_loadDetailsService is null ||
            _tripDetailsService is null)
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
                _tripDetailsService);

        loadDetailsForm.ShowDialog(this);
    }

    /// <summary>
    /// Abre el detalle del viaje seleccionado.
    /// </summary>
>>>>>>> c1969a3 (Add loads module and load details workflowAdd loads module and extended demo dataset)
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