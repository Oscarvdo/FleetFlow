using FleetFlow.Application.Abstractions.Loads;
using FleetFlow.Application.Loads;

namespace FleetFlow.Dispatch.WinForms.Controls.Loads;

/// <summary>
/// Muestra las cargas disponibles en FleetFlow y permite
/// buscar, filtrar y abrir el viaje asociado.
/// </summary>
public partial class LoadsControl : UserControl
{
    private readonly ILoadListService? _loadListService;

    // Conservamos la lista completa para aplicar filtros
    // localmente sin consultar SQL Server en cada tecla.
    private IReadOnlyList<LoadListItem> _allLoads = [];

    /// <summary>
    /// Notifica al MainForm que debe abrir el viaje indicado.
    /// </summary>
    public event Action<long>? TripOpenRequested;

    public LoadsControl()
    {
        InitializeComponent();

        ConfigureGrid();
        ConfigureStatusFilter();

        btnRefresh.Click += btnRefresh_Click;
        txtSearch.TextChanged += FilterChanged;
        cboStatus.SelectedIndexChanged += FilterChanged;
        dgvLoads.CellFormatting += dgvLoads_CellFormatting;
        dgvLoads.CellDoubleClick += dgvLoads_CellDoubleClick;
    }

    /// <summary>
    /// Constructor utilizado durante la ejecución.
    /// El servicio es proporcionado mediante inyección de dependencias.
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

        // El constructor sin parámetros es necesario para el diseñador.
        // Por eso verificamos que el servicio exista antes de consultar.
        if (_loadListService is not null)
        {
            await LoadLoadsAsync();
        }
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
                DataPropertyName = "ScheduledPickupUtc",
                HeaderText = "PICKUP",
                FillWeight = 110F,
                ReadOnly = true
            });
    }

    /// <summary>
    /// Configura los estados existentes en dbo.LoadStatuses.
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
    /// Abre el viaje asociado cuando el usuario hace
    /// doble clic sobre una carga.
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

        if (selectedLoad.TripId is not long tripId)
        {
            MessageBox.Show(
                "This load does not have an assigned trip.",
                "FleetFlow",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        TripOpenRequested?.Invoke(tripId);
    }

    /// <summary>
    /// Convierte valores técnicos en texto más fácil de leer.
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

        if (propertyName == "ScheduledPickupUtc" &&
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