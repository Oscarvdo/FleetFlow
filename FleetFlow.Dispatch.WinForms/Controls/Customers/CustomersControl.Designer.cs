namespace FleetFlow.Dispatch.WinForms.Controls.Customers;

partial class CustomersControl
{
    private System.ComponentModel.IContainer? components;
    private Panel pnlHeader = null!;
    private TableLayoutPanel tlpActions = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Label lblCount = null!;
    private Label lblStatus = null!;
    private TextBox txtSearch = null!;
    private CheckBox chkIncludeInactive = null!;
    private Button btnRefresh = null!;
    private Button btnEditCustomer = null!;
    private Button btnNewCustomer = null!;
    private DataGridView dgvCustomers = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        tlpActions = new TableLayoutPanel();
        lblTitle = new Label();
        lblSubtitle = new Label();
        lblCount = new Label();
        lblStatus = new Label();
        txtSearch = new TextBox();
        chkIncludeInactive = new CheckBox();
        btnRefresh = new Button();
        btnEditCustomer = new Button();
        btnNewCustomer = new Button();
        dgvCustomers = new DataGridView();
        SuspendLayout();

        pnlHeader.BackColor = Color.White;
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 220;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblCount);
        pnlHeader.Controls.Add(lblStatus);
        pnlHeader.Controls.Add(tlpActions);

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(32, 24);
        lblTitle.Text = "Customers";

        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(35, 82);
        lblSubtitle.Text = "Manage customer accounts, contacts, locations and activity.";

        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCount.ForeColor = Color.FromArgb(29, 39, 54);
        lblCount.Location = new Point(1040, 32);
        lblCount.Size = new Size(280, 28);
        lblCount.Text = "0 customers";
        lblCount.TextAlign = ContentAlignment.MiddleRight;

        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblStatus.ForeColor = Color.FromArgb(106, 116, 130);
        lblStatus.Location = new Point(1040, 70);
        lblStatus.Size = new Size(280, 28);
        lblStatus.Text = "Ready";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;

        tlpActions.ColumnCount = 5;
        tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
        tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
        tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F));
        tlpActions.Dock = DockStyle.Bottom;
        tlpActions.Height = 82;
        tlpActions.Padding = new Padding(32, 15, 20, 15);
        tlpActions.Controls.Add(txtSearch, 0, 0);
        tlpActions.Controls.Add(chkIncludeInactive, 1, 0);
        tlpActions.Controls.Add(btnRefresh, 2, 0);
        tlpActions.Controls.Add(btnEditCustomer, 3, 0);
        tlpActions.Controls.Add(btnNewCustomer, 4, 0);

        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Font = new Font("Segoe UI", 10F);
        txtSearch.Margin = new Padding(3, 3, 14, 3);
        txtSearch.PlaceholderText = "Search number, company, contact, email or phone...";

        chkIncludeInactive.AutoSize = true;
        chkIncludeInactive.Dock = DockStyle.Fill;
        chkIncludeInactive.Font = new Font("Segoe UI", 9.5F);
        chkIncludeInactive.Text = "Include inactive";

        StyleButton(btnRefresh, "Refresh", Color.FromArgb(233, 237, 242), Color.FromArgb(45, 55, 70));
        btnRefresh.Margin = new Padding(0, 1, 12, 1);
        StyleButton(btnEditCustomer, "Edit Selected", Color.FromArgb(29, 39, 54), Color.White);
        btnEditCustomer.Margin = new Padding(0, 1, 12, 1);
        StyleButton(btnNewCustomer, "+ New Customer", Color.FromArgb(243, 108, 33), Color.White);

        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.AllowUserToDeleteRows = false;
        dgvCustomers.AllowUserToResizeRows = false;
        dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCustomers.BackgroundColor = Color.FromArgb(244, 246, 249);
        dgvCustomers.BorderStyle = BorderStyle.None;
        dgvCustomers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvCustomers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvCustomers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(29, 39, 54),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Padding = new Padding(5, 0, 0, 0),
            SelectionBackColor = Color.FromArgb(29, 39, 54)
        };
        dgvCustomers.ColumnHeadersHeight = 42;
        dgvCustomers.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Color.FromArgb(45, 55, 70),
            Font = new Font("Segoe UI", 9F),
            Padding = new Padding(5, 0, 5, 0),
            SelectionBackColor = Color.FromArgb(255, 231, 218),
            SelectionForeColor = Color.FromArgb(29, 39, 54)
        };
        dgvCustomers.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(249, 250, 252)
        };
        dgvCustomers.Dock = DockStyle.Fill;
        dgvCustomers.EnableHeadersVisualStyles = false;
        dgvCustomers.MultiSelect = false;
        dgvCustomers.ReadOnly = true;
        dgvCustomers.RowHeadersVisible = false;
        dgvCustomers.RowTemplate.Height = 40;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        Controls.Add(dgvCustomers);
        Controls.Add(pnlHeader);
        Name = "CustomersControl";
        Size = new Size(1371, 960);
        ResumeLayout(false);
    }

    private static void StyleButton(Button button, string text, Color backColor, Color foreColor)
    {
        button.BackColor = backColor;
        button.Cursor = Cursors.Hand;
        button.Dock = DockStyle.Fill;
        button.FlatAppearance.BorderSize = 0;
        button.FlatStyle = FlatStyle.Flat;
        button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        button.ForeColor = foreColor;
        button.Text = text;
        button.UseVisualStyleBackColor = false;
    }
}
