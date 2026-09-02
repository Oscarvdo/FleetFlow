using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Loads;

namespace FleetFlow.Dispatch.WinForms.Controls.Loads;

/// <summary>
/// Muestra las cargas disponibles en FleetFlow y permite
/// buscar, filtrar, crear y abrir cargas.
/// </summary>
public partial class LoadsControl : UserControl
{
    private readonly ILoadListService?
        _loadListService;

    // Conservamos la lista completa para aplicar filtros
    // localmente sin consultar SQL Server en cada tecla.
    private IReadOnlyList<LoadListItem> _allLoads = [];

    /// <summary>
    /// Notifica al MainForm que debe abrir
    /// la carga seleccionada.
    /// </summary>
    public event Action<long>? LoadOpenRequested;

    /// <summary>
    /// Notifica al MainForm que el usuario desea
    /// crear una carga nueva.
    /// </summary>
    public event EventHandler? LoadCreateRequested;

    /// <summary>
    /// Controla la visibilidad del botón New Load según
    /// los permisos del usuario autenticado.
    /// </summary>
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(
        System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool CanCreateLoads
    {
        get => btnNewLoad.Visible;
        set => btnNewLoad.Visible = value;
    }

    /// <summary>
    /// Constructor utilizado por Visual Studio Designer.
    /// </summary>
    public LoadsControl()
    {
        InitializeComponent();

        ConfigureGrid();
        ConfigureStatusFilter();
        WireEvents();
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// </summary>
    public LoadsControl(
        ILoadListService loadListService)
        : this()
    {
        _loadListService = loadListService;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // El constructor sin parámetros es necesario
        // para Visual Studio Designer.
        if (_loadListService is not null)
        {
            await LoadLoadsAsync();
        }
    }

    /// <summary>
    /// Conecta los eventos de los controles.
    /// </summary>
    private void WireEvents()
    {
        btnRefresh.Click += btnRefresh_Click;
        btnNewLoad.Click += btnNewLoad_Click;

        txtSearch.TextChanged += FilterChanged;

        cboStatus.SelectedIndexChanged +=
            FilterChanged;

        dgvLoads.CellFormatting +=
            dgvLoads_CellFormatting;

        dgvLoads.CellDoubleClick +=
            dgvLoads_CellDoubleClick;
    }

    /// <summary>
    /// Crea las columnas mediante código para evitar que
    /// Visual Studio Designer las elimine o regenere.
    /// </summary>
    private void ConfigureGrid()
    {
        dgvLoads.AutoGenerateColumns = false;
        dgvLoads.Columns.Clear();

        dgvLoads.Columns.AddRange(
            new DataGridViewTextBoxColumn
            {
                Name = "colLoadNumber",
                DataPropertyName = "LoadNumber",
                HeaderText = "LOAD",
                FillWeight = 90F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colLoadStatus",
                DataPropertyName = "LoadStatus",
                HeaderText = "STATUS",
                FillWeight = 90F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colCustomer",
                DataPropertyName = "Customer",
                HeaderText = "CUSTOMER",
                FillWeight = 145F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colCommodity",
                DataPropertyName = "Commodity",
                HeaderText = "COMMODITY",
                FillWeight = 95F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colWeight",
                DataPropertyName = "WeightLbs",
                HeaderText = "WEIGHT",
                FillWeight = 75F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colPieces",
                DataPropertyName = "Pieces",
                HeaderText = "PIECES",
                FillWeight = 55F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colRevenue",
                DataPropertyName = "RevenueAmount",
                HeaderText = "REVENUE",
                FillWeight = 75F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colTripNumber",
                DataPropertyName = "TripNumber",
                HeaderText = "TRIP",
                FillWeight = 90F,
                ReadOnly = true
            },
            new DataGridViewTextBoxColumn
            {
                Name = "colPickup",
                DataPropertyName =
                    "ScheduledPickupUtc",
                HeaderText = "PICKUP",
                FillWeight = 110F,
                ReadOnly = true
            });
    }

    /// <summary>
    /// Configura los estados existentes
    /// en dbo.LoadStatuses.
    /// </summary>
    private void ConfigureStatusFilter()
    {
        cboStatus.DisplayMember =
            nameof(LoadStatusFilter.DisplayName);

        cboStatus.ValueMember =
            nameof(LoadStatusFilter.StatusCode);

        cboStatus.Items.AddRange(
        [
            new LoadStatusFilter(
                null,
                "All statuses"),

            new LoadStatusFilter(
                "NEW",
                "New"),

            new LoadStatusFilter(
                "PLANNED",
                "Planned"),

            new LoadStatusFilter(
                "IN_TRANSIT",
                "In Transit"),

            new LoadStatusFilter(
                "DELIVERED",
                "Delivered"),

            new LoadStatusFilter(
                "CANCELLED",
                "Cancelled")
        ]);

        cboStatus.SelectedIndex = 0;
    }

    /// <summary>
    /// Solicita al MainForm abrir CreateLoadForm.
    /// LoadsControl no crea directamente el formulario
    /// porque MainForm administra las dependencias.
    /// </summary>
    private void btnNewLoad_Click(
        object? sender,
        EventArgs e)
    {
        LoadCreateRequested?.Invoke(
            this,
            EventArgs.Empty);
    }

    private async void btnRefresh_Click(
        object? sender,
        EventArgs e)
    {
        await LoadLoadsAsync();
    }

    private void FilterChanged(
        object? sender,
        EventArgs e)
    {
        ApplyLocalFilter();
    }

    /// <summary>
    /// Permite que MainForm actualice la lista después
    /// de crear una carga correctamente.
    /// </summary>
    public async Task RefreshLoadsAsync()
    {
        await LoadLoadsAsync();
    }

    /// <summary>
    /// Obtiene todas las cargas desde Infrastructure.
    /// </summary>
    private async Task LoadLoadsAsync()
    {
        if (_loadListService is null)
        {
            return;
        }

        SetBusyState(true);

        try
        {
            _allLoads =
                await _loadListService.GetLoadsAsync();

            ApplyLocalFilter();

            lblStatus.ForeColor =
                Color.FromArgb(106, 116, 130);

            lblStatus.Text =
                $"Updated {DateTime.Now:g}";
        }
        catch (Exception exception)
        {
            lblStatus.ForeColor = Color.Firebrick;
            lblStatus.Text = "Unable to load loads";

            MessageBox.Show(
                $"The loads could not be loaded.\n\n" +
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
    /// Filtra la lista que ya se encuentra en memoria.
    /// </summary>
    private void ApplyLocalFilter()
    {
        string searchText =
            txtSearch.Text.Trim();

        string? statusCode =
            (cboStatus.SelectedItem
                as LoadStatusFilter)
            ?.StatusCode;

        IEnumerable<LoadListItem> filtered =
            _allLoads;

        if (!string.IsNullOrWhiteSpace(statusCode))
        {
            filtered = filtered.Where(
                load =>
                    string.Equals(
                        load.LoadStatusCode,
                        statusCode,
                        StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filtered = filtered.Where(
                load =>
                    Contains(
                        load.LoadNumber,
                        searchText) ||

                    Contains(
                        load.CustomerNumber,
                        searchText) ||

                    Contains(
                        load.Customer,
                        searchText) ||

                    Contains(
                        load.Description,
                        searchText) ||

                    Contains(
                        load.Commodity,
                        searchText) ||

                    Contains(
                        load.TripNumber,
                        searchText));
        }

        List<LoadListItem> records =
            filtered.ToList();

        dgvLoads.DataSource = null;
        dgvLoads.AutoGenerateColumns = false;
        dgvLoads.DataSource = records;

        lblCount.Text =
            records.Count == 1
                ? "1 load"
                : $"{records.Count:N0} loads";
    }

    private static bool Contains(
        string? value,
        string searchText)
    {
        return value?.Contains(
            searchText,
            StringComparison.OrdinalIgnoreCase) == true;
    }

    /// <summary>
    /// Solicita abrir los detalles de la carga
    /// seleccionada mediante doble clic.
    /// </summary>
    private void dgvLoads_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
        {
            return;
        }

        if (dgvLoads.Rows[e.RowIndex].DataBoundItem
            is not LoadListItem selectedLoad)
        {
            return;
        }

        LoadOpenRequested?.Invoke(
            selectedLoad.LoadId);
    }

    /// <summary>
    /// Convierte valores técnicos en texto
    /// más fácil de leer.
    /// </summary>
    private void dgvLoads_CellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex < 0)
        {
            return;
        }

        string propertyName =
            dgvLoads.Columns[e.ColumnIndex]
                .DataPropertyName;

        if (propertyName ==
                "ScheduledPickupUtc" &&
            e.Value is DateTime pickupUtc)
        {
            DateTime utc =
                DateTime.SpecifyKind(
                    pickupUtc,
                    DateTimeKind.Utc);

            e.Value =
                utc.ToLocalTime().ToString("g");

            e.FormattingApplied = true;
            return;
        }

        if (propertyName == "WeightLbs" &&
            e.Value is decimal weight)
        {
            e.Value = $"{weight:N0} lb";
            e.FormattingApplied = true;
            return;
        }

        if (propertyName == "RevenueAmount" &&
            e.Value is decimal revenue)
        {
            e.Value = $"{revenue:C}";
            e.FormattingApplied = true;
        }
    }

    /// <summary>
    /// Deshabilita temporalmente los controles mientras
    /// se realiza una consulta.
    /// </summary>
    private void SetBusyState(bool isBusy)
    {
        btnRefresh.Enabled = !isBusy;
        btnNewLoad.Enabled =
            !isBusy &&
            CanCreateLoads;
        txtSearch.Enabled = !isBusy;
        cboStatus.Enabled = !isBusy;

        btnRefresh.Text =
            isBusy
                ? "Loading..."
                : "Refresh";

        UseWaitCursor = isBusy;
    }

    /// <summary>
    /// Representa una opción del filtro de estados.
    /// </summary>
    private sealed record LoadStatusFilter(
        string? StatusCode,
        string DisplayName);
}
