using Microsoft.Web.WebView2.WinForms;

namespace FleetFlow.Dispatch.WinForms.Controls.Tracking;

partial class LiveTrackingControl
{
    private System.ComponentModel.IContainer? components = null;

    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;

    private Panel pnlToolbar = null!;
    private Label lblVehicleCount = null!;
    private NumericUpDown nudVehicleCount = null!;
    private Label lblTimeScale = null!;
    private NumericUpDown nudTimeScale = null!;
    private Button btnRefresh = null!;
    private Button btnStart = null!;
    private Button btnPause = null!;
    private Button btnStop = null!;
    private Button btnFitAll = null!;

    private SplitContainer splitTracking = null!;

    private Panel pnlVehicleListHeader = null!;
    private Label lblVehicles = null!;
    private Label lblVehicleSummary = null!;
    private DataGridView dgvVehicles = null!;
    private DataGridViewTextBoxColumn colUnitNumber = null!;
    private DataGridViewTextBoxColumn colTripNumber = null!;
    private DataGridViewTextBoxColumn colTrackingStatus = null!;
    private DataGridViewTextBoxColumn colSpeed = null!;

    private Panel pnlMapHeader = null!;
    private Label lblMapTitle = null!;
    private Label lblSelectedVehicle = null!;
    private WebView2 webViewMap = null!;

    private Panel pnlFooter = null!;
    private Label lblSimulationStatus = null!;
    private Label lblLastUpdate = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        pnlHeader = new Panel();
        lblTitle = new Label();
        lblSubtitle = new Label();

        pnlToolbar = new Panel();
        lblVehicleCount = new Label();
        nudVehicleCount = new NumericUpDown();
        lblTimeScale = new Label();
        nudTimeScale = new NumericUpDown();
        btnRefresh = new Button();
        btnStart = new Button();
        btnPause = new Button();
        btnStop = new Button();
        btnFitAll = new Button();

        splitTracking = new SplitContainer();

        pnlVehicleListHeader = new Panel();
        lblVehicles = new Label();
        lblVehicleSummary = new Label();
        dgvVehicles = new DataGridView();
        colUnitNumber = new DataGridViewTextBoxColumn();
        colTripNumber = new DataGridViewTextBoxColumn();
        colTrackingStatus = new DataGridViewTextBoxColumn();
        colSpeed = new DataGridViewTextBoxColumn();

        pnlMapHeader = new Panel();
        lblMapTitle = new Label();
        lblSelectedVehicle = new Label();
        webViewMap = new WebView2();

        pnlFooter = new Panel();
        lblSimulationStatus = new Label();
        lblLastUpdate = new Label();

        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudVehicleCount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudTimeScale).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitTracking).BeginInit();
        splitTracking.Panel1.SuspendLayout();
        splitTracking.Panel2.SuspendLayout();
        splitTracking.SuspendLayout();
        pnlVehicleListHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVehicles).BeginInit();
        pnlMapHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)webViewMap).BeginInit();
        pnlFooter.SuspendLayout();
        SuspendLayout();

        // pnlHeader
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1260, 92);
        pnlHeader.TabIndex = 0;

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font(
            "Segoe UI",
            22F,
            FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(28, 14);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(242, 50);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Live Tracking";

        // lblSubtitle
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Regular);
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(31, 64);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(359, 20);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text =
            "Real-time fleet positions and concurrent simulation.";

        // pnlToolbar
        pnlToolbar.BackColor = Color.FromArgb(244, 246, 249);
        pnlToolbar.Controls.Add(lblVehicleCount);
        pnlToolbar.Controls.Add(nudVehicleCount);
        pnlToolbar.Controls.Add(lblTimeScale);
        pnlToolbar.Controls.Add(nudTimeScale);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnStart);
        pnlToolbar.Controls.Add(btnPause);
        pnlToolbar.Controls.Add(btnStop);
        pnlToolbar.Controls.Add(btnFitAll);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 92);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1260, 70);
        pnlToolbar.TabIndex = 1;

        // lblVehicleCount
        lblVehicleCount.AutoSize = true;
        lblVehicleCount.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblVehicleCount.ForeColor = Color.FromArgb(45, 55, 70);
        lblVehicleCount.Location = new Point(28, 8);
        lblVehicleCount.Name = "lblVehicleCount";
        lblVehicleCount.Size = new Size(68, 20);
        lblVehicleCount.TabIndex = 0;
        lblVehicleCount.Text = "Trucks";

        // nudVehicleCount
        nudVehicleCount.Location = new Point(28, 32);
        nudVehicleCount.Maximum = new decimal(
            new int[] { 1000, 0, 0, 0 });
        nudVehicleCount.Minimum = new decimal(
            new int[] { 1, 0, 0, 0 });
        nudVehicleCount.Name = "nudVehicleCount";
        nudVehicleCount.Size = new Size(82, 27);
        nudVehicleCount.TabIndex = 1;
        nudVehicleCount.TextAlign = HorizontalAlignment.Right;
        nudVehicleCount.Value = new decimal(
            new int[] { 1, 0, 0, 0 });

        // lblTimeScale
        lblTimeScale.AutoSize = true;
        lblTimeScale.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblTimeScale.ForeColor = Color.FromArgb(45, 55, 70);
        lblTimeScale.Location = new Point(128, 8);
        lblTimeScale.Name = "lblTimeScale";
        lblTimeScale.Size = new Size(82, 20);
        lblTimeScale.TabIndex = 2;
        lblTimeScale.Text = "Time scale";

        // nudTimeScale
        nudTimeScale.DecimalPlaces = 1;
        nudTimeScale.Increment = new decimal(
            new int[] { 5, 0, 0, 65536 });
        nudTimeScale.Location = new Point(128, 32);
        nudTimeScale.Maximum = new decimal(
            new int[] { 3600, 0, 0, 0 });
        nudTimeScale.Minimum = new decimal(
            new int[] { 1, 0, 0, 65536 });
        nudTimeScale.Name = "nudTimeScale";
        nudTimeScale.Size = new Size(92, 27);
        nudTimeScale.TabIndex = 3;
        nudTimeScale.TextAlign = HorizontalAlignment.Right;
        nudTimeScale.Value = new decimal(
            new int[] { 10, 0, 0, 0 });

        // btnRefresh
        btnRefresh.BackColor = Color.FromArgb(225, 229, 235);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnRefresh.ForeColor = Color.FromArgb(45, 55, 70);
        btnRefresh.Location = new Point(250, 19);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(100, 40);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;

        // btnStart
        btnStart.BackColor = Color.FromArgb(243, 108, 33);
        btnStart.FlatAppearance.BorderSize = 0;
        btnStart.FlatStyle = FlatStyle.Flat;
        btnStart.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnStart.ForeColor = Color.White;
        btnStart.Location = new Point(360, 19);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(135, 40);
        btnStart.TabIndex = 5;
        btnStart.Text = "Start simulation";
        btnStart.UseVisualStyleBackColor = false;

        // btnPause
        btnPause.BackColor = Color.FromArgb(216, 160, 25);
        btnPause.Enabled = false;
        btnPause.FlatAppearance.BorderSize = 0;
        btnPause.FlatStyle = FlatStyle.Flat;
        btnPause.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnPause.ForeColor = Color.White;
        btnPause.Location = new Point(505, 19);
        btnPause.Name = "btnPause";
        btnPause.Size = new Size(100, 40);
        btnPause.TabIndex = 6;
        btnPause.Text = "Pause";
        btnPause.UseVisualStyleBackColor = false;

        // btnStop
        btnStop.BackColor = Color.FromArgb(180, 45, 55);
        btnStop.Enabled = false;
        btnStop.FlatAppearance.BorderSize = 0;
        btnStop.FlatStyle = FlatStyle.Flat;
        btnStop.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnStop.ForeColor = Color.White;
        btnStop.Location = new Point(615, 19);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(100, 40);
        btnStop.TabIndex = 7;
        btnStop.Text = "Stop";
        btnStop.UseVisualStyleBackColor = false;

        // btnFitAll
        btnFitAll.Anchor =
            AnchorStyles.Top | AnchorStyles.Right;
        btnFitAll.BackColor = Color.FromArgb(29, 39, 54);
        btnFitAll.FlatAppearance.BorderSize = 0;
        btnFitAll.FlatStyle = FlatStyle.Flat;
        btnFitAll.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnFitAll.ForeColor = Color.White;
        btnFitAll.Location = new Point(1122, 19);
        btnFitAll.Name = "btnFitAll";
        btnFitAll.Size = new Size(110, 40);
        btnFitAll.TabIndex = 8;
        btnFitAll.Text = "Fit all";
        btnFitAll.UseVisualStyleBackColor = false;

        // splitTracking
        splitTracking.BackColor = Color.FromArgb(220, 224, 230);
        splitTracking.Dock = DockStyle.Fill;
        splitTracking.FixedPanel = FixedPanel.Panel1;
        splitTracking.Location = new Point(0, 162);
        splitTracking.Name = "splitTracking";

        // splitTracking.Panel1
        splitTracking.Panel1.BackColor = Color.White;
        splitTracking.Panel1.Controls.Add(dgvVehicles);
        splitTracking.Panel1.Controls.Add(pnlVehicleListHeader);
        splitTracking.Panel1MinSize = 280;

        // splitTracking.Panel2
        splitTracking.Panel2.BackColor = Color.White;
        splitTracking.Panel2.Controls.Add(webViewMap);
        splitTracking.Panel2.Controls.Add(pnlMapHeader);
        splitTracking.Panel2MinSize = 500;

        splitTracking.Size = new Size(1260, 596);
        splitTracking.SplitterDistance = 330;
        splitTracking.SplitterWidth = 6;
        splitTracking.TabIndex = 2;

        // pnlVehicleListHeader
        pnlVehicleListHeader.BackColor = Color.White;
        pnlVehicleListHeader.Controls.Add(lblVehicles);
        pnlVehicleListHeader.Controls.Add(lblVehicleSummary);
        pnlVehicleListHeader.Dock = DockStyle.Top;
        pnlVehicleListHeader.Location = new Point(0, 0);
        pnlVehicleListHeader.Name = "pnlVehicleListHeader";
        pnlVehicleListHeader.Size = new Size(330, 68);
        pnlVehicleListHeader.TabIndex = 0;

        // lblVehicles
        lblVehicles.AutoSize = true;
        lblVehicles.Font = new Font(
            "Segoe UI",
            13F,
            FontStyle.Bold);
        lblVehicles.ForeColor = Color.FromArgb(29, 39, 54);
        lblVehicles.Location = new Point(18, 8);
        lblVehicles.Name = "lblVehicles";
        lblVehicles.Size = new Size(95, 30);
        lblVehicles.TabIndex = 0;
        lblVehicles.Text = "Vehicles";

        // lblVehicleSummary
        lblVehicleSummary.AutoSize = true;
        lblVehicleSummary.ForeColor = Color.FromArgb(106, 116, 130);
        lblVehicleSummary.Location = new Point(20, 40);
        lblVehicleSummary.Name = "lblVehicleSummary";
        lblVehicleSummary.Size = new Size(109, 20);
        lblVehicleSummary.TabIndex = 1;
        lblVehicleSummary.Text = "0 vehicles found";

        // dgvVehicles
        dgvVehicles.AllowUserToAddRows = false;
        dgvVehicles.AllowUserToDeleteRows = false;
        dgvVehicles.AllowUserToResizeRows = false;
        dgvVehicles.AutoGenerateColumns = false;
        dgvVehicles.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
        dgvVehicles.BackgroundColor = Color.White;
        dgvVehicles.BorderStyle = BorderStyle.None;
        dgvVehicles.ColumnHeadersHeight = 38;
        dgvVehicles.Columns.Add(colUnitNumber);
        dgvVehicles.Columns.Add(colTripNumber);
        dgvVehicles.Columns.Add(colTrackingStatus);
        dgvVehicles.Columns.Add(colSpeed);
        dgvVehicles.Dock = DockStyle.Fill;
        dgvVehicles.Location = new Point(0, 68);
        dgvVehicles.MultiSelect = false;
        dgvVehicles.Name = "dgvVehicles";
        dgvVehicles.ReadOnly = true;
        dgvVehicles.RowHeadersVisible = false;
        dgvVehicles.RowTemplate.Height = 36;
        dgvVehicles.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        dgvVehicles.Size = new Size(330, 528);
        dgvVehicles.TabIndex = 1;

        // colUnitNumber
        colUnitNumber.DataPropertyName = "UnitNumber";
        colUnitNumber.FillWeight = 85F;
        colUnitNumber.HeaderText = "UNIT";
        colUnitNumber.Name = "colUnitNumber";
        colUnitNumber.ReadOnly = true;

        // colTripNumber
        colTripNumber.DataPropertyName = "TripNumber";
        colTripNumber.FillWeight = 100F;
        colTripNumber.HeaderText = "TRIP";
        colTripNumber.Name = "colTripNumber";
        colTripNumber.ReadOnly = true;

        // colTrackingStatus
        colTrackingStatus.DataPropertyName = "TrackingStatus";
        colTrackingStatus.FillWeight = 85F;
        colTrackingStatus.HeaderText = "STATUS";
        colTrackingStatus.Name = "colTrackingStatus";
        colTrackingStatus.ReadOnly = true;

        // colSpeed
        colSpeed.DataPropertyName = "SpeedMph";
        colSpeed.FillWeight = 65F;
        colSpeed.HeaderText = "MPH";
        colSpeed.Name = "colSpeed";
        colSpeed.ReadOnly = true;

        // pnlMapHeader
        pnlMapHeader.BackColor = Color.White;
        pnlMapHeader.Controls.Add(lblMapTitle);
        pnlMapHeader.Controls.Add(lblSelectedVehicle);
        pnlMapHeader.Dock = DockStyle.Top;
        pnlMapHeader.Location = new Point(0, 0);
        pnlMapHeader.Name = "pnlMapHeader";
        pnlMapHeader.Size = new Size(924, 52);
        pnlMapHeader.TabIndex = 0;

        // lblMapTitle
        lblMapTitle.AutoSize = true;
        lblMapTitle.Font = new Font(
            "Segoe UI",
            12F,
            FontStyle.Bold);
        lblMapTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblMapTitle.Location = new Point(16, 11);
        lblMapTitle.Name = "lblMapTitle";
        lblMapTitle.Size = new Size(173, 28);
        lblMapTitle.TabIndex = 0;
        lblMapTitle.Text = "OpenStreetMap";

        // lblSelectedVehicle
        lblSelectedVehicle.Anchor =
            AnchorStyles.Top | AnchorStyles.Right;
        lblSelectedVehicle.ForeColor =
            Color.FromArgb(106, 116, 130);
        lblSelectedVehicle.Location = new Point(544, 15);
        lblSelectedVehicle.Name = "lblSelectedVehicle";
        lblSelectedVehicle.Size = new Size(360, 24);
        lblSelectedVehicle.TabIndex = 1;
        lblSelectedVehicle.Text = "No vehicle selected";
        lblSelectedVehicle.TextAlign =
            ContentAlignment.MiddleRight;

        // webViewMap
        webViewMap.AllowExternalDrop = false;
        webViewMap.CreationProperties = null;
        webViewMap.DefaultBackgroundColor = Color.White;
        webViewMap.Dock = DockStyle.Fill;
        webViewMap.Location = new Point(0, 52);
        webViewMap.Name = "webViewMap";
        webViewMap.Size = new Size(924, 544);
        webViewMap.TabIndex = 1;
        webViewMap.ZoomFactor = 1D;

        // pnlFooter
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(lblSimulationStatus);
        pnlFooter.Controls.Add(lblLastUpdate);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 758);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1260, 42);
        pnlFooter.TabIndex = 3;

        // lblSimulationStatus
        lblSimulationStatus.AutoSize = true;
        lblSimulationStatus.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblSimulationStatus.ForeColor =
            Color.FromArgb(106, 116, 130);
        lblSimulationStatus.Location = new Point(24, 11);
        lblSimulationStatus.Name = "lblSimulationStatus";
        lblSimulationStatus.Size = new Size(50, 20);
        lblSimulationStatus.TabIndex = 0;
        lblSimulationStatus.Text = "READY";

        // lblLastUpdate
        lblLastUpdate.Anchor =
            AnchorStyles.Top | AnchorStyles.Right;
        lblLastUpdate.ForeColor =
            Color.FromArgb(106, 116, 130);
        lblLastUpdate.Location = new Point(900, 10);
        lblLastUpdate.Name = "lblLastUpdate";
        lblLastUpdate.Size = new Size(332, 22);
        lblLastUpdate.TabIndex = 1;
        lblLastUpdate.Text = "Waiting for map";
        lblLastUpdate.TextAlign =
            ContentAlignment.MiddleRight;

        // LiveTrackingControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        Controls.Add(splitTracking);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlHeader);
        Controls.Add(pnlFooter);
        Name = "LiveTrackingControl";
        Size = new Size(1260, 800);

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudVehicleCount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudTimeScale).EndInit();
        splitTracking.Panel1.ResumeLayout(false);
        splitTracking.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitTracking).EndInit();
        splitTracking.ResumeLayout(false);
        pnlVehicleListHeader.ResumeLayout(false);
        pnlVehicleListHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVehicles).EndInit();
        pnlMapHeader.ResumeLayout(false);
        pnlMapHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)webViewMap).EndInit();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ResumeLayout(false);
    }
}