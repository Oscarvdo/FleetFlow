namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

partial class CustomerDetailsForm
{
    private System.ComponentModel.IContainer? components;
    private Label lblCustomerNumber = null!;
    private Label lblCompanyName = null!;
    private Label lblStatus = null!;
    private Label lblContactValue = null!;
    private Label lblEmailValue = null!;
    private Label lblPhoneValue = null!;
    private Label lblCreatedValue = null!;
    private Label lblUpdatedValue = null!;
    private Label lblLoadsValue = null!;
    private Label lblOpenLoadsValue = null!;
    private Label lblRevenueValue = null!;
    private Label lblMessage = null!;
    private Button btnEdit = null!;
    private Button btnSetActive = null!;
    private Button btnRefresh = null!;
    private Button btnClose = null!;
    private DataGridView dgvLocations = null!;
    private DataGridView dgvLoads = null!;
    private Button btnNewLocation = null!;
    private Button btnEditLocation = null!;
    private Button btnLocationStatus = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        BackColor = Color.FromArgb(244, 246, 249);
        ClientSize = new Size(1120, 760);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterParent;

        var header = new Panel { BackColor = Color.White, Dock = DockStyle.Top, Height = 125 };
        lblCustomerNumber = MakeLabel("CUSTOMER", 34, 22, 10F, FontStyle.Bold, Color.FromArgb(243, 108, 33));
        lblCompanyName = MakeLabel("Customer", 32, 47, 22F, FontStyle.Bold, Color.FromArgb(29, 39, 54));
        lblStatus = MakeLabel("STATUS", 34, 92, 9F, FontStyle.Bold, Color.FromArgb(35, 130, 85));
        var actions = new FlowLayoutPanel
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            FlowDirection = FlowDirection.LeftToRight,
            Location = new Point(585, 38),
            Size = new Size(500, 48)
        };
        btnEdit = ActionButton("Edit Customer", Color.FromArgb(243, 108, 33), Color.White, 125);
        btnSetActive = ActionButton("Deactivate", Color.FromArgb(225, 229, 235), Color.FromArgb(45, 55, 70), 105);
        btnRefresh = ActionButton("Refresh", Color.FromArgb(225, 229, 235), Color.FromArgb(45, 55, 70), 90);
        btnClose = ActionButton("Close", Color.FromArgb(29, 39, 54), Color.White, 80);
        actions.Controls.AddRange([btnEdit, btnSetActive, btnRefresh, btnClose]);
        header.Controls.AddRange([lblCustomerNumber, lblCompanyName, lblStatus, actions]);

        var summary = new TableLayoutPanel
        {
            BackColor = Color.White,
            ColumnCount = 3,
            Dock = DockStyle.Top,
            Height = 112,
            Padding = new Padding(26, 14, 26, 14)
        };
        summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        lblLoadsValue = SummaryCard(summary, "TOTAL LOADS", 0);
        lblOpenLoadsValue = SummaryCard(summary, "OPEN LOADS", 1);
        lblRevenueValue = SummaryCard(summary, "TOTAL REVENUE", 2);

        var info = new TableLayoutPanel
        {
            BackColor = Color.White,
            ColumnCount = 5,
            Dock = DockStyle.Top,
            Height = 105,
            Padding = new Padding(26, 12, 26, 12)
        };
        for (int i = 0; i < 5; i++) info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        lblContactValue = InfoField(info, "PRIMARY CONTACT", 0);
        lblEmailValue = InfoField(info, "EMAIL", 1);
        lblPhoneValue = InfoField(info, "PHONE", 2);
        lblCreatedValue = InfoField(info, "CREATED", 3);
        lblUpdatedValue = InfoField(info, "UPDATED", 4);

        var tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F) };
        var locationsPage = new TabPage("Locations") { BackColor = Color.White, Padding = new Padding(12) };
        var loadsPage = new TabPage("Recent Loads") { BackColor = Color.White, Padding = new Padding(12) };
        dgvLocations = Grid();
        var locationActions = new FlowLayoutPanel
        {
            BackColor = Color.White,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Height = 52,
            Padding = new Padding(0, 4, 0, 4)
        };
        btnNewLocation = ActionButton("+ New Location", Color.FromArgb(243, 108, 33), Color.White, 125);
        btnEditLocation = ActionButton("Edit Selected", Color.FromArgb(29, 39, 54), Color.White, 115);
        btnLocationStatus = ActionButton("Deactivate", Color.FromArgb(225, 229, 235), Color.FromArgb(45, 55, 70), 105);
        locationActions.Controls.AddRange([btnNewLocation, btnEditLocation, btnLocationStatus]);
        dgvLocations.AutoGenerateColumns = false;
        dgvLocations.Columns.AddRange(
            GridColumn("LocationCode", "LOCATION #", 80),
            GridColumn("LocationName", "NAME", 125),
            GridColumn("LocationType", "TYPE", 70),
            GridColumn("Address", "ADDRESS", 180),
            GridColumn("ContactName", "CONTACT", 90),
            GridColumn("IsBillingLocation", "BILLING", 55),
            GridColumn("IsActive", "ACTIVE", 50));
        dgvLoads = Grid();
        dgvLoads.AutoGenerateColumns = false;
        dgvLoads.Columns.AddRange(
            GridColumn("LoadNumber", "LOAD", 80),
            GridColumn("Description", "DESCRIPTION", 180),
            GridColumn("LoadStatus", "STATUS", 80),
            GridColumn("RevenueAmount", "REVENUE", 75),
            GridColumn("CreatedAtUtc", "CREATED", 95));
        locationsPage.Controls.Add(dgvLocations);
        locationsPage.Controls.Add(locationActions);
        loadsPage.Controls.Add(dgvLoads);
        tabs.TabPages.AddRange([locationsPage, loadsPage]);

        var footer = new Panel { BackColor = Color.White, Dock = DockStyle.Bottom, Height = 42 };
        lblMessage = MakeLabel("Ready", 28, 10, 9F, FontStyle.Regular, Color.FromArgb(106, 116, 130));
        footer.Controls.Add(lblMessage);

        Controls.Add(tabs);
        Controls.Add(info);
        Controls.Add(summary);
        Controls.Add(header);
        Controls.Add(footer);
    }

    private static Label MakeLabel(string text, int left, int top, float size, FontStyle style, Color color) => new()
    {
        AutoSize = true,
        Font = new Font("Segoe UI", size, style),
        ForeColor = color,
        Location = new Point(left, top),
        Text = text
    };

    private static Button ActionButton(string text, Color back, Color fore, int width) => new()
    {
        BackColor = back,
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
        ForeColor = fore,
        Height = 40,
        Margin = new Padding(4),
        Text = text,
        UseVisualStyleBackColor = false,
        Width = width
    };

    private static Label SummaryCard(TableLayoutPanel panel, string caption, int column)
    {
        var container = new Panel { Dock = DockStyle.Fill };
        var title = MakeLabel(caption, 8, 4, 8.5F, FontStyle.Bold, Color.FromArgb(106, 116, 130));
        var value = MakeLabel("0", 8, 30, 18F, FontStyle.Bold, Color.FromArgb(29, 39, 54));
        container.Controls.AddRange([title, value]);
        panel.Controls.Add(container, column, 0);
        return value;
    }

    private static Label InfoField(TableLayoutPanel panel, string caption, int column)
    {
        var container = new Panel { Dock = DockStyle.Fill };
        var title = MakeLabel(caption, 8, 4, 8F, FontStyle.Bold, Color.FromArgb(106, 116, 130));
        var value = MakeLabel("—", 8, 31, 10F, FontStyle.Regular, Color.FromArgb(45, 55, 70));
        value.MaximumSize = new Size(190, 45);
        container.Controls.AddRange([title, value]);
        panel.Controls.Add(container, column, 0);
        return value;
    }

    private static DataGridView Grid() => new()
    {
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = Color.White,
        BorderStyle = BorderStyle.None,
        ColumnHeadersHeight = 38,
        Dock = DockStyle.Fill,
        MultiSelect = false,
        ReadOnly = true,
        RowHeadersVisible = false,
        RowTemplate = { Height = 36 },
        SelectionMode = DataGridViewSelectionMode.FullRowSelect
    };

    private static DataGridViewTextBoxColumn GridColumn(string property, string header, float weight) => new()
    {
        DataPropertyName = property,
        FillWeight = weight,
        HeaderText = header,
        ReadOnly = true
    };
}
