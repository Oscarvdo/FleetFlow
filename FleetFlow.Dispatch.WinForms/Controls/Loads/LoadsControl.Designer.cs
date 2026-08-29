namespace FleetFlow.Dispatch.WinForms.Controls.Loads;

partial class LoadsControl
{
    private System.ComponentModel.IContainer? components = null;

    private Panel pnlHeader;
    private Panel pnlSummary;
    private TableLayoutPanel tlpFilters;

    private Label lblTitle;
    private Label lblSubtitle;
    private Label lblCount;
    private Label lblStatus;

    private TextBox txtSearch;
    private ComboBox cboStatus;
    private Button btnRefresh;
    private DataGridView dgvLoads;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        pnlHeader = new Panel();
        tlpFilters = new TableLayoutPanel();
        txtSearch = new TextBox();
        cboStatus = new ComboBox();
        btnRefresh = new Button();
        pnlSummary = new Panel();
        lblCount = new Label();
        lblStatus = new Label();
        lblSubtitle = new Label();
        lblTitle = new Label();
        dgvLoads = new DataGridView();
        pnlHeader.SuspendLayout();
        tlpFilters.SuspendLayout();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLoads).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(tlpFilters);
        pnlHeader.Controls.Add(pnlSummary);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(3, 4, 3, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1371, 227);
        pnlHeader.TabIndex = 1;
        // 
        // tlpFilters
        // 
        tlpFilters.ColumnCount = 3;
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 251F));
        tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 143F));
        tlpFilters.Controls.Add(txtSearch, 0, 0);
        tlpFilters.Controls.Add(cboStatus, 1, 0);
        tlpFilters.Controls.Add(btnRefresh, 2, 0);
        tlpFilters.Dock = DockStyle.Bottom;
        tlpFilters.Location = new Point(0, 142);
        tlpFilters.Margin = new Padding(3, 4, 3, 4);
        tlpFilters.Name = "tlpFilters";
        tlpFilters.Padding = new Padding(32, 16, 32, 16);
        tlpFilters.RowCount = 1;
        tlpFilters.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpFilters.Size = new Size(1045, 85);
        tlpFilters.TabIndex = 0;
        // 
        // txtSearch
        // 
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Font = new Font("Segoe UI", 10F);
        txtSearch.Location = new Point(35, 19);
        txtSearch.Margin = new Padding(3, 3, 11, 3);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search load, customer, commodity, trip...";
        txtSearch.Size = new Size(573, 30);
        txtSearch.TabIndex = 0;
        // 
        // cboStatus
        // 
        cboStatus.Dock = DockStyle.Fill;
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.Font = new Font("Segoe UI", 10F);
        cboStatus.Location = new Point(619, 19);
        cboStatus.Margin = new Padding(0, 3, 11, 3);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(240, 31);
        cboStatus.TabIndex = 1;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(243, 108, 33);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.Dock = DockStyle.Fill;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(870, 17);
        btnRefresh.Margin = new Padding(0, 1, 0, 1);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(143, 51);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblCount);
        pnlSummary.Controls.Add(lblStatus);
        pnlSummary.Dock = DockStyle.Right;
        pnlSummary.Location = new Point(1045, 0);
        pnlSummary.Margin = new Padding(3, 4, 3, 4);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(326, 227);
        pnlSummary.TabIndex = 1;
        // 
        // lblCount
        // 
        lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCount.ForeColor = Color.FromArgb(29, 39, 54);
        lblCount.Location = new Point(17, 33);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(274, 32);
        lblCount.TabIndex = 0;
        lblCount.Text = "0 loads";
        lblCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblStatus
        // 
        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblStatus.Font = new Font("Segoe UI", 9F);
        lblStatus.ForeColor = Color.FromArgb(106, 116, 130);
        lblStatus.Location = new Point(17, 73);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(274, 32);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "Ready";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(35, 83);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(456, 23);
        lblSubtitle.TabIndex = 2;
        lblSubtitle.Text = "Search and review customer loads and their assigned trips.";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(32, 24);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(123, 50);
        lblTitle.TabIndex = 3;
        lblTitle.Text = "Loads";
        // 
        // dgvLoads
        // 
        dgvLoads.AllowUserToAddRows = false;
        dgvLoads.AllowUserToDeleteRows = false;
        dgvLoads.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 252);
        dgvLoads.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvLoads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLoads.BackgroundColor = Color.FromArgb(244, 246, 249);
        dgvLoads.BorderStyle = BorderStyle.None;
        dgvLoads.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvLoads.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(29, 39, 54);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.Padding = new Padding(5, 0, 0, 0);
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(29, 39, 54);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvLoads.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvLoads.ColumnHeadersHeight = 42;
        dgvLoads.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(45, 55, 70);
        dataGridViewCellStyle3.Padding = new Padding(5, 0, 5, 0);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 231, 218);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(29, 39, 54);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvLoads.DefaultCellStyle = dataGridViewCellStyle3;
        dgvLoads.Dock = DockStyle.Fill;
        dgvLoads.EnableHeadersVisualStyles = false;
        dgvLoads.GridColor = Color.FromArgb(225, 229, 235);
        dgvLoads.Location = new Point(0, 227);
        dgvLoads.Margin = new Padding(3, 4, 3, 4);
        dgvLoads.MultiSelect = false;
        dgvLoads.Name = "dgvLoads";
        dgvLoads.ReadOnly = true;
        dgvLoads.RowHeadersVisible = false;
        dgvLoads.RowHeadersWidth = 51;
        dgvLoads.RowTemplate.Height = 40;
        dgvLoads.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLoads.Size = new Size(1371, 733);
        dgvLoads.TabIndex = 0;
        // 
        // LoadsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        Controls.Add(dgvLoads);
        Controls.Add(pnlHeader);
        Margin = new Padding(3, 4, 3, 4);
        Name = "LoadsControl";
        Size = new Size(1371, 960);
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        tlpFilters.ResumeLayout(false);
        tlpFilters.PerformLayout();
        pnlSummary.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLoads).EndInit();
        ResumeLayout(false);
    }
}