namespace FleetFlow.Dispatch.WinForms.Controls.Trips;

partial class TripsControl
{
    private System.ComponentModel.IContainer? components = null;

    private Panel pnlHeader;
    private Label lblTitle;
    private Label lblSubtitle;
    private TextBox txtSearch;
    private ComboBox cboStatus;
    private Button btnRefresh;
    private Label lblCount;
    private Label lblStatus;
    private DataGridView dgvTrips;

    private DataGridViewTextBoxColumn colTripNumber;
    private DataGridViewTextBoxColumn colStatus;
    private DataGridViewTextBoxColumn colLoadNumber;
    private DataGridViewTextBoxColumn colCustomer;
    private DataGridViewTextBoxColumn colPickup;
    private DataGridViewTextBoxColumn colDelivery;
    private DataGridViewTextBoxColumn colStops;
    private DataGridViewTextBoxColumn colProgress;
    private DataGridViewTextBoxColumn colDistance;

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
        lblTitle = new Label();
        lblSubtitle = new Label();
        txtSearch = new TextBox();
        cboStatus = new ComboBox();
        btnRefresh = new Button();
        lblCount = new Label();
        lblStatus = new Label();
        dgvTrips = new DataGridView();
        pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTrips).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(txtSearch);
        pnlHeader.Controls.Add(cboStatus);
        pnlHeader.Controls.Add(btnRefresh);
        pnlHeader.Controls.Add(lblCount);
        pnlHeader.Controls.Add(lblStatus);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(3, 4, 3, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(32, 27, 32, 19);
        pnlHeader.Size = new Size(1371, 193);
        pnlHeader.TabIndex = 1;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(32, 21);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(106, 50);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Trips";
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(35, 79);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(471, 23);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Search and review all scheduled, active, and completed trips.";
        // 
        // txtSearch
        // 
        txtSearch.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Left |
            AnchorStyles.Right;
        txtSearch.Font = new Font("Segoe UI", 10F);
        txtSearch.Location = new Point(31, 96);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText =
            "Search trip, load, customer...";
        txtSearch.Size = new Size(700, 30);
        // 
        // cboStatus
        // 
        cboStatus.Anchor =
      AnchorStyles.Top |
      AnchorStyles.Right;
        cboStatus.DropDownStyle =
            ComboBoxStyle.DropDownList;
        cboStatus.Font = new Font("Segoe UI", 10F);
        cboStatus.Location = new Point(745, 95);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(190, 31);
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor =
      AnchorStyles.Top |
      AnchorStyles.Right;
        btnRefresh.BackColor =
            Color.FromArgb(243, 108, 33);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font(
            "Segoe UI",
            10F,
            FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(949, 94);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(110, 33);
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;

        // 
        // lblCount
        // 
        lblCount.Anchor =
       AnchorStyles.Top |
       AnchorStyles.Right;
        lblCount.Font = new Font(
            "Segoe UI",
            10F,
            FontStyle.Bold);
        lblCount.ForeColor =
            Color.FromArgb(29, 39, 54);
        lblCount.Location = new Point(920, 22);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(120, 25);
        lblCount.Text = "0 trips";
        lblCount.TextAlign =
            ContentAlignment.MiddleRight;

        // 
        // lblStatus
        // 
        lblStatus.Anchor =
     AnchorStyles.Top |
     AnchorStyles.Right;
        lblStatus.Font = new Font("Segoe UI", 9F);
        lblStatus.ForeColor =
            Color.FromArgb(106, 116, 130);
        lblStatus.Location = new Point(948, 56);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(220, 22);
        lblStatus.Text = "Ready";
        lblStatus.TextAlign =
            ContentAlignment.MiddleRight;
        // 
        // dgvTrips
        // 
        dgvTrips.AllowUserToAddRows = false;
        dgvTrips.AllowUserToDeleteRows = false;
        dgvTrips.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 252);
        dgvTrips.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvTrips.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvTrips.BackgroundColor = Color.FromArgb(244, 246, 249);
        dgvTrips.BorderStyle = BorderStyle.None;
        dgvTrips.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvTrips.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(29, 39, 54);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.Padding = new Padding(5, 0, 0, 0);
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(29, 39, 54);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvTrips.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvTrips.ColumnHeadersHeight = 42;
        dgvTrips.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(45, 55, 70);
        dataGridViewCellStyle3.Padding = new Padding(5, 0, 5, 0);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 231, 218);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(29, 39, 54);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvTrips.DefaultCellStyle = dataGridViewCellStyle3;
        dgvTrips.Dock = DockStyle.Fill;
        dgvTrips.EnableHeadersVisualStyles = false;
        dgvTrips.GridColor = Color.FromArgb(225, 229, 235);
        dgvTrips.Location = new Point(0, 193);
        dgvTrips.Margin = new Padding(3, 4, 3, 4);
        dgvTrips.MultiSelect = false;
        dgvTrips.Name = "dgvTrips";
        dgvTrips.ReadOnly = true;
        dgvTrips.RowHeadersVisible = false;
        dgvTrips.RowHeadersWidth = 51;
        dgvTrips.RowTemplate.Height = 40;
        dgvTrips.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTrips.Size = new Size(1371, 767);
        dgvTrips.TabIndex = 0;
        // 
        // TripsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        Controls.Add(dgvTrips);
        Controls.Add(pnlHeader);
        Margin = new Padding(3, 4, 3, 4);
        Name = "TripsControl";
        Size = new Size(1371, 960);
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTrips).EndInit();
        ResumeLayout(false);
    }
}