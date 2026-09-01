using FleetFlow.Application.Abstractions.Customers;
using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Customers;
using FleetFlow.Application.Loads;

namespace FleetFlow.Dispatch.WinForms.Forms.Loads;

/// <summary>
/// Permite crear una carga nueva o editar
/// una carga existente.
/// </summary>
public partial class CreateLoadForm : Form
{
    private readonly ILoadCommandService?
        _loadCommandService;

    private readonly ICustomerLookupService?
        _customerLookupService;

    /*
        Cuando es null, el formulario trabaja
        en modo creación.

        Cuando contiene una carga, trabaja
        en modo edición.
    */
    private readonly LoadDetails?
        _editingDetails;

    /// <summary>
    /// Identificador de la carga creada o actualizada.
    /// </summary>
    public long? SavedLoadId { get; private set; }

    /// <summary>
    /// Indica que una carga existente fue modificada.
    /// </summary>
    public bool WasUpdated { get; private set; }

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public CreateLoadForm()
    {
        InitializeComponent();

        ConfigureStatusDisplay();
        WireEvents();
        ConfigureFormMode();
    }

    /// <summary>
    /// Constructor utilizado para crear una carga.
    /// </summary>
    public CreateLoadForm(
        ILoadCommandService loadCommandService,
        ICustomerLookupService customerLookupService)
        : this()
    {
        _loadCommandService =
            loadCommandService;

        _customerLookupService =
            customerLookupService;
    }

    /// <summary>
    /// Constructor utilizado para editar una carga.
    /// </summary>
    public CreateLoadForm(
        LoadDetails details,
        ILoadCommandService loadCommandService,
        ICustomerLookupService customerLookupService)
        : this()
    {
        ArgumentNullException.ThrowIfNull(details);

        _editingDetails =
            details;

        _loadCommandService =
            loadCommandService;

        _customerLookupService =
            customerLookupService;

        ConfigureFormMode();
    }

    /// <summary>
    /// Indica si el formulario está editando
    /// una carga existente.
    /// </summary>
    private bool IsEditMode =>
        _editingDetails is not null;

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // El constructor vacío únicamente es utilizado
        // por Visual Studio Designer.
        if (_customerLookupService is null)
        {
            return;
        }

        await LoadCustomersAsync();

        if (_editingDetails is not null)
        {
            DisplayExistingLoad(
                _editingDetails);
        }

        ApplyFieldPermissions();
    }

    /// <summary>
    /// Conecta los eventos de los controles.
    /// </summary>
    private void WireEvents()
    {
        btnSave.Click +=
            btnSave_Click;

        btnCancel.Click +=
            btnCancel_Click;

        btnRefreshCustomers.Click +=
            btnRefreshCustomers_Click;

        chkPieces.CheckedChanged +=
            chkPieces_CheckedChanged;

        chkRevenue.CheckedChanged +=
            chkRevenue_CheckedChanged;
    }

    /// <summary>
    /// El estado se presenta únicamente como información.
    /// La edición comercial no modifica estados.
    /// </summary>
    private void ConfigureStatusDisplay()
    {
        cboStatus.Items.Clear();
        cboStatus.Items.Add("New");
        cboStatus.SelectedIndex = 0;
        cboStatus.Enabled = false;

        lblStatus.Text = "Status";
    }

    /// <summary>
    /// Cambia títulos y textos según la operación.
    /// </summary>
    private void ConfigureFormMode()
    {
        if (_editingDetails is null)
        {
            Text =
                "FleetFlow — Create Load";

            lblTitle.Text =
                "Create Load";

            lblSubtitle.Text =
                "Enter the shipment information " +
                "for the new load.";

            btnSave.Text =
                "Create Load";

            return;
        }

        Text =
            $"FleetFlow — Edit " +
            $"{_editingDetails.LoadNumber}";

        lblTitle.Text =
            "Edit Load";

        lblSubtitle.Text =
            $"Update the commercial information for " +
            $"{_editingDetails.LoadNumber}.";

        btnSave.Text =
            "Save Changes";

        cboStatus.Items.Clear();
        cboStatus.Items.Add(
            _editingDetails.LoadStatus);

        cboStatus.SelectedIndex = 0;
        cboStatus.Enabled = false;
    }

    /// <summary>
    /// Consulta los clientes que pueden aparecer
    /// dentro del ComboBox.
    /// </summary>
    private async Task LoadCustomersAsync()
    {
        if (_customerLookupService is null)
        {
            return;
        }

        SetBusyState(true);

        lblMessage.ForeColor =
            Color.FromArgb(106, 116, 130);

        lblMessage.Text =
            "Loading customers...";

        try
        {
            /*
                En edición solicitamos también inactivos
                para conservar el cliente histórico actual.
            */
            IReadOnlyList<CustomerLookupItem> results =
                await _customerLookupService.SearchAsync(
                    includeInactive: IsEditMode);

            IEnumerable<CustomerLookupItem> allowed =
                results;

            if (_editingDetails is not null)
            {
                long currentCustomerId =
                    _editingDetails.CustomerId;

                /*
                    Mostramos clientes activos y, si está
                    inactivo, únicamente el cliente actual.
                */
                allowed = results.Where(
                    customer =>
                        customer.IsActive ||
                        customer.CustomerId ==
                            currentCustomerId);
            }

            List<CustomerLookupItem> customers =
                allowed
                    .OrderBy(
                        customer =>
                            customer.CompanyName)
                    .ToList();

            cboCustomer.DataSource = null;

            cboCustomer.DisplayMember =
                nameof(
                    CustomerLookupItem.DisplayName);

            cboCustomer.ValueMember =
                nameof(
                    CustomerLookupItem.CustomerId);

            cboCustomer.DataSource =
                customers;

            if (_editingDetails is not null)
            {
                cboCustomer.SelectedValue =
                    _editingDetails.CustomerId;
            }

            if (customers.Count == 0)
            {
                lblMessage.ForeColor =
                    Color.Firebrick;

                lblMessage.Text =
                    "No customers are available.";
            }
            else
            {
                lblMessage.ForeColor =
                    Color.FromArgb(106, 116, 130);

                lblMessage.Text =
                    $"{customers.Count:N0} customers available";
            }
        }
        catch (Exception exception)
        {
            lblMessage.ForeColor =
                Color.Firebrick;

            lblMessage.Text =
                "Unable to load customers";

            MessageBox.Show(
                $"The customers could not be loaded.\n\n" +
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
    /// Transfiere una carga existente
    /// hacia los controles visuales.
    /// </summary>
    private void DisplayExistingLoad(
        LoadDetails details)
    {
        txtLoadNumber.Text =
            details.LoadNumber;

        txtDescription.Text =
            details.Description;

        txtCommodity.Text =
            details.Commodity ?? string.Empty;

        numWeight.Value =
            ClampDecimal(
                details.WeightLbs,
                numWeight.Minimum,
                numWeight.Maximum);

        chkPieces.Checked =
            details.Pieces.HasValue;

        numPieces.Value =
            details.Pieces.HasValue
                ? ClampDecimal(
                    details.Pieces.Value,
                    numPieces.Minimum,
                    numPieces.Maximum)
                : 0;

        chkRevenue.Checked =
            details.RevenueAmount.HasValue;

        numRevenue.Value =
            details.RevenueAmount.HasValue
                ? ClampDecimal(
                    details.RevenueAmount.Value,
                    numRevenue.Minimum,
                    numRevenue.Maximum)
                : 0;

        txtSpecialInstructions.Text =
            details.SpecialInstructions ??
            string.Empty;

        cboStatus.Items.Clear();
        cboStatus.Items.Add(
            details.LoadStatus);

        cboStatus.SelectedIndex = 0;

        lblMessage.ForeColor =
            Color.FromArgb(106, 116, 130);

        lblMessage.Text =
            "Review the information and save your changes.";
    }

    /// <summary>
    /// Ejecuta CreateAsync o UpdateAsync según
    /// el modo actual del formulario.
    /// </summary>
    private async void btnSave_Click(
        object? sender,
        EventArgs e)
    {
        if (_loadCommandService is null)
        {
            MessageBox.Show(
                "The load command service is unavailable.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        if (!TryGetSelectedCustomer(
                out CustomerLookupItem customer))
        {
            return;
        }

        if (!ValidateVisibleFields())
        {
            return;
        }

        SetBusyState(true);

        lblMessage.ForeColor =
            Color.FromArgb(106, 116, 130);

        lblMessage.Text =
            IsEditMode
                ? "Saving changes..."
                : "Creating load...";

        try
        {
            if (_editingDetails is null)
            {
                await CreateLoadAsync(customer);
            }
            else
            {
                await UpdateLoadAsync(
                    _editingDetails,
                    customer);
            }

            DialogResult =
                DialogResult.OK;

            Close();
        }
        catch (ArgumentException exception)
        {
            ShowSaveError(
                exception.Message,
                "Invalid Load",
                MessageBoxIcon.Warning);
        }
        catch (InvalidOperationException exception)
        {
            ShowSaveError(
                exception.Message,
                IsEditMode
                    ? "Unable to Update Load"
                    : "Unable to Create Load",
                MessageBoxIcon.Warning);
        }
        catch (Exception exception)
        {
            ShowSaveError(
                $"The load could not be saved.\n\n" +
                exception.Message,
                "FleetFlow",
                MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                SetBusyState(false);
            }
        }
    }

    /// <summary>
    /// Crea una carga nueva.
    /// </summary>
    private async Task CreateLoadAsync(
        CustomerLookupItem customer)
    {
        if (_loadCommandService is null)
        {
            return;
        }

        if (!customer.IsActive)
        {
            throw new InvalidOperationException(
                "An inactive customer cannot be used " +
                "for a new load.");
        }

        CreateLoadRequest request = new()
        {
            LoadNumber =
                txtLoadNumber.Text.Trim(),

            CustomerId =
                customer.CustomerId,

            Description =
                txtDescription.Text.Trim(),

            Commodity =
                NullIfEmpty(
                    txtCommodity.Text),

            WeightLbs =
                numWeight.Value,

            Pieces =
                GetPieces(),

            RevenueAmount =
                GetRevenue(),

            SpecialInstructions =
                NullIfEmpty(
                    txtSpecialInstructions.Text)
        };

        CreateLoadResult result =
            await _loadCommandService.CreateAsync(
                request);

        SavedLoadId =
            result.LoadId;

        MessageBox.Show(
            $"Load {request.LoadNumber} " +
            "was created successfully.",
            "Load Created",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    /// <summary>
    /// Actualiza una carga existente utilizando
    /// la RowVersion consultada originalmente.
    /// </summary>
    private async Task UpdateLoadAsync(
        LoadDetails details,
        CustomerLookupItem customer)
    {
        if (_loadCommandService is null)
        {
            return;
        }

        /*
            Un cliente inactivo solamente puede conservarse
            cuando ya pertenece a esta carga.
        */
        if (!customer.IsActive &&
            customer.CustomerId != details.CustomerId)
        {
            throw new InvalidOperationException(
                "The selected customer is inactive.");
        }

        UpdateLoadRequest request = new()
        {
            LoadId =
                details.LoadId,

            LoadNumber =
                txtLoadNumber.Text.Trim(),

            CustomerId =
                customer.CustomerId,

            Description =
                txtDescription.Text.Trim(),

            Commodity =
                NullIfEmpty(
                    txtCommodity.Text),

            WeightLbs =
                numWeight.Value,

            Pieces =
                GetPieces(),

            RevenueAmount =
                GetRevenue(),

            SpecialInstructions =
                NullIfEmpty(
                    txtSpecialInstructions.Text),

            ExpectedRowVersion =
                details.RowVersion
        };

        UpdateLoadResult result =
            await _loadCommandService.UpdateAsync(
                request);

        SavedLoadId =
            result.LoadId;

        WasUpdated = true;

        MessageBox.Show(
            $"Load {request.LoadNumber} " +
            "was updated successfully.",
            "Load Updated",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    /// <summary>
    /// Obtiene y valida el cliente seleccionado.
    /// Cuando devuelve true, customer nunca es null.
    /// </summary>
    private bool TryGetSelectedCustomer(
        out CustomerLookupItem customer)
    {
        if (cboCustomer.SelectedItem
            is CustomerLookupItem selectedCustomer)
        {
            customer = selectedCustomer;
            return true;
        }

        // El parámetro out debe recibir un valor aunque
        // el método termine devolviendo false.
        customer = null!;

        ShowValidationMessage(
            "Select a customer.",
            cboCustomer);

        return false;
    }

    /// <summary>
    /// Valida los campos visuales obligatorios.
    /// </summary>
    private bool ValidateVisibleFields()
    {
        if (string.IsNullOrWhiteSpace(
                txtLoadNumber.Text))
        {
            ShowValidationMessage(
                "Enter a load number.",
                txtLoadNumber);

            return false;
        }

        if (string.IsNullOrWhiteSpace(
                txtDescription.Text))
        {
            ShowValidationMessage(
                "Enter a load description.",
                txtDescription);

            return false;
        }

        if (numWeight.Value <= 0)
        {
            ShowValidationMessage(
                "Weight must be greater than zero.",
                numWeight);

            return false;
        }

        if (chkPieces.Checked &&
            numPieces.Value <= 0)
        {
            ShowValidationMessage(
                "Pieces must be greater than zero.",
                numPieces);

            return false;
        }

        return true;
    }

    private int? GetPieces()
    {
        return chkPieces.Checked
            ? decimal.ToInt32(
                numPieces.Value)
            : null;
    }

    private decimal? GetRevenue()
    {
        return chkRevenue.Checked
            ? numRevenue.Value
            : null;
    }

    /// <summary>
    /// Aplica restricciones de edición según
    /// el viaje relacionado.
    /// </summary>
    private void ApplyFieldPermissions()
    {
        if (_editingDetails is null)
        {
            txtLoadNumber.Enabled = true;
            cboCustomer.Enabled = true;
            btnRefreshCustomers.Enabled = true;
            btnSave.Enabled = true;
            return;
        }

        bool hasTrip =
            _editingDetails.TripId.HasValue;

        bool allowedTripState =
            !hasTrip ||
            _editingDetails.TripStatusCode is
                "PLANNED" or
                "OFFERED" or
                "ASSIGNED";

        bool allowedLoadState =
            _editingDetails.LoadStatusCode is
                "NEW" or
                "PLANNED";

        /*
            Número y cliente quedan bloqueados
            cuando ya existe un viaje.
        */
        txtLoadNumber.Enabled =
            !hasTrip;

        cboCustomer.Enabled =
            !hasTrip;

        btnRefreshCustomers.Enabled =
            !hasTrip;

        btnSave.Enabled =
            allowedTripState &&
            allowedLoadState;

        if (!allowedLoadState)
        {
            lblMessage.ForeColor =
                Color.Firebrick;

            lblMessage.Text =
                "Only new or planned loads can be edited.";
        }
        else if (!allowedTripState)
        {
            lblMessage.ForeColor =
                Color.Firebrick;

            lblMessage.Text =
                "This load cannot be edited because " +
                "its trip has already started.";
        }
    }

    /// <summary>
    /// Deshabilita temporalmente las acciones mientras
    /// se ejecuta una operación.
    /// </summary>
    private void SetBusyState(bool isBusy)
    {
        btnSave.Enabled =
            !isBusy;

        btnCancel.Enabled =
            !isBusy;

        btnRefreshCustomers.Enabled =
            !isBusy;

        txtLoadNumber.Enabled =
            !isBusy;

        cboCustomer.Enabled =
            !isBusy;

        txtDescription.Enabled =
            !isBusy;

        txtCommodity.Enabled =
            !isBusy;

        numWeight.Enabled =
            !isBusy;

        chkPieces.Enabled =
            !isBusy;

        numPieces.Enabled =
            !isBusy &&
            chkPieces.Checked;

        chkRevenue.Enabled =
            !isBusy;

        numRevenue.Enabled =
            !isBusy &&
            chkRevenue.Checked;

        txtSpecialInstructions.Enabled =
            !isBusy;

        cboStatus.Enabled = false;

        btnSave.Text =
            isBusy
                ? "Saving..."
                : IsEditMode
                    ? "Save Changes"
                    : "Create Load";

        UseWaitCursor =
            isBusy;

        if (!isBusy)
        {
            ApplyFieldPermissions();
        }
    }

    private async void btnRefreshCustomers_Click(
        object? sender,
        EventArgs e)
    {
        await LoadCustomersAsync();

        if (_editingDetails is not null)
        {
            cboCustomer.SelectedValue =
                _editingDetails.CustomerId;
        }
    }

    private void chkPieces_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        numPieces.Enabled =
            chkPieces.Checked &&
            !UseWaitCursor;

        if (!chkPieces.Checked)
        {
            numPieces.Value = 0;
        }
    }

    private void chkRevenue_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        numRevenue.Enabled =
            chkRevenue.Checked &&
            !UseWaitCursor;

        if (!chkRevenue.Checked)
        {
            numRevenue.Value = 0;
        }
    }

    private void btnCancel_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult =
            DialogResult.Cancel;

        Close();
    }

    private void ShowValidationMessage(
        string message,
        Control control)
    {
        lblMessage.ForeColor =
            Color.Firebrick;

        lblMessage.Text =
            message;

        MessageBox.Show(
            message,
            "Required Information",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        control.Focus();
    }

    private void ShowSaveError(
        string message,
        string title,
        MessageBoxIcon icon)
    {
        lblMessage.ForeColor =
            Color.Firebrick;

        lblMessage.Text =
            message;

        MessageBox.Show(
            message,
            title,
            MessageBoxButtons.OK,
            icon);
    }

    private static string? NullIfEmpty(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    /// <summary>
    /// Mantiene valores cargados dentro de los límites
    /// configurados en NumericUpDown.
    /// </summary>
    private static decimal ClampDecimal(
        decimal value,
        decimal minimum,
        decimal maximum)
    {
        if (value < minimum)
        {
            return minimum;
        }

        if (value > maximum)
        {
            return maximum;
        }

        return value;
    }
}
