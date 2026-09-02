using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Abstractions.Trips;
using FleetFlow.Application.Loads;
using FleetFlow.Dispatch.WinForms.Forms.Trips;

namespace FleetFlow.Dispatch.WinForms.Forms.Loads;

/// <summary>
/// Presenta la información completa de una carga
/// y permite abrir su viaje relacionado.
/// </summary>
public partial class LoadDetailsForm : Form
{
    private readonly long _loadId;

    private readonly ILoadDetailsService?
        _loadDetailsService;

    private readonly ITripDetailsService?
        _tripDetailsService;

    private readonly ILoadCommandService?
        _loadCommandService;

    private readonly ICustomerLookupService?
        _customerLookupService;

    private readonly bool _canManageLoads;

    private LoadDetails? _currentDetails;

    // Se asigna después de cargar los datos.
    // Puede permanecer null si la carga no tiene viaje.
    private long? _relatedTripId;

    /// <summary>
    /// Indica que la carga fue modificada desde esta ventana.
    /// MainForm utiliza esta bandera para actualizar la lista.
    /// </summary>
    public bool WasUpdated { get; private set; }

    public LoadDetailsForm()
    {
        InitializeComponent();

        btnRefresh.Click += btnRefresh_Click;
        btnEditLoad.Click += btnEditLoad_Click;
        btnOpenTrip.Click += btnOpenTrip_Click;
        btnClose.Click += btnClose_Click;
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public LoadDetailsForm(
        long loadId,
        ILoadDetailsService loadDetailsService,
        ITripDetailsService tripDetailsService,
        ILoadCommandService loadCommandService,
        ICustomerLookupService customerLookupService,
        bool canManageLoads)
        : this()
    {
        _loadId = loadId;
        _loadDetailsService = loadDetailsService;
        _tripDetailsService = tripDetailsService;
        _loadCommandService = loadCommandService;
        _customerLookupService = customerLookupService;
        _canManageLoads = canManageLoads;

        btnEditLoad.Visible = canManageLoads;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // El constructor vacío solo lo utiliza
        // Visual Studio Designer.
        if (_loadDetailsService is not null)
        {
            await LoadDetailsAsync();
        }
    }

    /// <summary>
    /// Consulta la carga y actualiza los controles.
    /// </summary>
    private async Task LoadDetailsAsync()
    {
        if (_loadDetailsService is null)
        {
            return;
        }

        SetBusyState(true);

        try
        {
            LoadDetails? details =
                await _loadDetailsService.GetByIdAsync(
                    _loadId);

            if (details is null)
            {
                MessageBox.Show(
                    "The selected load could not be found.",
                    "Load Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Close();
                return;
            }

            DisplayDetails(details);
        }
        catch (Exception exception)
        {
            lblMessage.ForeColor = Color.Firebrick;
            lblMessage.Text =
                "Unable to load load details";

            MessageBox.Show(
                $"The load details could not be loaded.\n\n" +
                exception.Message,
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            SetBusyState(false);
        }
    }

    /// <summary>
    /// Transfiere los valores del modelo a los
    /// controles visuales del formulario.
    /// </summary>
    private void DisplayDetails(
        LoadDetails details)
    {
        _currentDetails = details;
        _relatedTripId = details.TripId;

        Text =
            $"FleetFlow — {details.LoadNumber}";

        lblLoadNumber.Text =
            details.LoadNumber;

        lblLoadStatus.Text =
            details.LoadStatus;

        lblCustomerValue.Text =
            $"{details.CustomerNumber} — " +
            details.Customer;

        lblContactValue.Text =
            ValueOrDash(
                details.CustomerContactName);

        lblEmailValue.Text =
            ValueOrDash(
                details.CustomerEmail);

        lblPhoneValue.Text =
            ValueOrDash(
                details.CustomerPhone);

        lblDescriptionValue.Text =
            ValueOrDash(
                details.Description);

        lblCommodityValue.Text =
            ValueOrDash(
                details.Commodity);

        lblWeightValue.Text =
            $"{details.WeightLbs:N0} lb";

        lblPiecesValue.Text =
            details.Pieces?.ToString("N0")
            ?? "—";

        lblRevenueValue.Text =
            details.RevenueAmount?.ToString("C")
            ?? "—";

        lblTripValue.Text =
            ValueOrDash(
                details.TripNumber);

        lblTripStatusValue.Text =
            ValueOrDash(
                details.TripStatus);

        lblScheduleValue.Text =
            FormatSchedule(
                details.ScheduledPickupUtc,
                details.ScheduledDeliveryUtc);

        lblCreatedValue.Text =
            FormatLocalDate(
                details.CreatedAtUtc);

        lblUpdatedValue.Text =
            FormatLocalDate(
                details.UpdatedAtUtc);

        txtSpecialInstructions.Text =
            ValueOrDash(
                details.SpecialInstructions);

        // El botón solamente se habilita cuando
        // existe un viaje relacionado.
        btnOpenTrip.Enabled =
            details.TripId.HasValue;

        btnEditLoad.Enabled =
            CanEdit(details);

        lblMessage.ForeColor =
            Color.FromArgb(106, 116, 130);

        lblMessage.Text =
            $"Updated {DateTime.Now:g}";
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await LoadDetailsAsync();
    }

    /// <summary>
    /// Abre CreateLoadForm en modo edición y vuelve a consultar
    /// la carga cuando SQL Server confirma la actualización.
    /// </summary>
    private async void btnEditLoad_Click(
        object? sender,
        EventArgs e)
    {
        if (_currentDetails is null ||
            _loadCommandService is null ||
            _customerLookupService is null)
        {
            MessageBox.Show(
                "The load editing services are unavailable.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        using var editLoadForm =
            new CreateLoadForm(
                _currentDetails,
                _loadCommandService,
                _customerLookupService);

        if (editLoadForm.ShowDialog(this) !=
            DialogResult.OK)
        {
            return;
        }

        WasUpdated = true;
        await LoadDetailsAsync();
    }

    /// <summary>
    /// Abre TripDetailsForm para el viaje asociado.
    /// </summary>
    private void btnOpenTrip_Click(
        object? sender,
        EventArgs e)
    {
        if (_relatedTripId is not long tripId ||
            _tripDetailsService is null)
        {
            MessageBox.Show(
                "This load does not have an assigned trip.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        using var tripDetailsForm =
            new TripDetailsForm(
                tripId,
                _tripDetailsService);

        tripDetailsForm.ShowDialog(this);
    }

    private void btnClose_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Formatea fechas almacenadas en UTC
    /// utilizando la zona horaria de Windows.
    /// </summary>
    private static string FormatLocalDate(
        DateTime dateTime)
    {
        DateTime utc =
            DateTime.SpecifyKind(
                dateTime,
                DateTimeKind.Utc);

        return utc.ToLocalTime().ToString("g");
    }

    /// <summary>
    /// Construye el rango programado de la carga.
    /// </summary>
    private static string FormatSchedule(
        DateTime? pickupUtc,
        DateTime? deliveryUtc)
    {
        if (!pickupUtc.HasValue ||
            !deliveryUtc.HasValue)
        {
            return "Not scheduled";
        }

        return
            $"{FormatLocalDate(pickupUtc.Value)} → " +
            $"{FormatLocalDate(deliveryUtc.Value)}";
    }

    /// <summary>
    /// Evita mostrar valores vacíos en el formulario.
    /// </summary>
    private static string ValueOrDash(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? "—"
            : value.Trim();
    }

    /// <summary>
    /// Refleja en la interfaz las mismas reglas protegidas
    /// por operations.Load_Update.
    /// </summary>
    private bool CanEdit(LoadDetails details)
    {
        if (!_canManageLoads ||
            details.LoadStatusCode is not
                ("NEW" or "PLANNED"))
        {
            return false;
        }

        return !details.TripId.HasValue ||
            details.TripStatusCode is
                "PLANNED" or
                "OFFERED" or
                "ASSIGNED";
    }

    /// <summary>
    /// Deshabilita las acciones mientras se consulta
    /// la información en SQL Server.
    /// </summary>
    private void SetBusyState(bool isBusy)
    {
        btnRefresh.Enabled = !isBusy;

        btnEditLoad.Enabled =
            !isBusy &&
            _currentDetails is not null &&
            CanEdit(_currentDetails);

        btnOpenTrip.Enabled =
            !isBusy &&
            _relatedTripId.HasValue;

        UseWaitCursor = isBusy;

        if (isBusy)
        {
            lblMessage.ForeColor =
                Color.FromArgb(106, 116, 130);

            lblMessage.Text =
                "Loading load details...";
        }
    }
}
