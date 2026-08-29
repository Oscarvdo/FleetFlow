namespace FleetFlow.Dispatch.WinForms.Controls.Dashboard
{
    partial class DashboardControl
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel tlpRoot;
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblUpdated;
        private Button btnRefresh;
        private TableLayoutPanel tlpMetrics;

        private Label lblActiveTripsValue;
        private Label lblAvailableDriversValue;
        private Label lblAvailableVehiclesValue;
        private Label lblPendingLoadsValue;
        private Label lblDelayedTripsValue;
        private Label lblActiveIncidentsValue;
        private Label lblTrackedVehiclesValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblDescription = new Label();
            lblUpdated = new Label();
            btnRefresh = new Button();
            tlpMetrics = new TableLayoutPanel();

            tlpRoot.SuspendLayout();
            pnlHeader.SuspendLayout();
            SuspendLayout();

            // tlpRoot
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(pnlHeader, 0, 0);
            tlpRoot.Controls.Add(tlpMetrics, 0, 1);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.RowCount = 2;
            tlpRoot.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 100F));
            tlpRoot.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(244, 246, 249);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblDescription);
            pnlHeader.Controls.Add(lblUpdated);
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.Dock = DockStyle.Fill;

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font(
                "Segoe UI",
                22F,
                FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
            lblTitle.Location = new Point(12, 8);
            lblTitle.Text = "Operations Overview";

            // lblDescription
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = Color.FromArgb(92, 103, 118);
            lblDescription.Location = new Point(16, 56);
            lblDescription.Text =
                "Current fleet, dispatch, and trip indicators.";

            // lblUpdated
            lblUpdated.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            lblUpdated.ForeColor = Color.FromArgb(106, 116, 130);
            lblUpdated.Location = new Point(570, 58);
            lblUpdated.Size = new Size(230, 24);
            lblUpdated.Text = "Not loaded";
            lblUpdated.TextAlign = ContentAlignment.MiddleRight;

            // btnRefresh
            btnRefresh.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(29, 39, 54);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(815, 20);
            btnRefresh.Size = new Size(110, 38);
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;

            // tlpMetrics
            tlpMetrics.BackColor = Color.FromArgb(244, 246, 249);
            tlpMetrics.ColumnCount = 4;

            tlpMetrics.ColumnStyles.Add(
       new ColumnStyle(SizeType.Percent, 25F));

            tlpMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tlpMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tlpMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tlpMetrics.Dock = DockStyle.Fill;
            tlpMetrics.Padding = new Padding(4);
            tlpMetrics.RowCount = 2;
            tlpMetrics.RowStyles.Add(
                new RowStyle(SizeType.Percent, 50F));
            tlpMetrics.RowStyles.Add(
                new RowStyle(SizeType.Percent, 50F));

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Active Trips",
                    Color.FromArgb(52, 120, 246),
                    out lblActiveTripsValue),
                0,
                0);

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Available Drivers",
                    Color.FromArgb(39, 174, 96),
                    out lblAvailableDriversValue),
                1,
                0);

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Available Vehicles",
                    Color.FromArgb(22, 160, 133),
                    out lblAvailableVehiclesValue),
                2,
                0);

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Pending Loads",
                    Color.FromArgb(243, 156, 18),
                    out lblPendingLoadsValue),
                3,
                0);

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Delayed Trips",
                    Color.FromArgb(230, 126, 34),
                    out lblDelayedTripsValue),
                0,
                1);

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Active Incidents",
                    Color.FromArgb(192, 57, 43),
                    out lblActiveIncidentsValue),
                1,
                1);

            tlpMetrics.Controls.Add(
                CreateMetricCard(
                    "Tracked Vehicles",
                    Color.FromArgb(142, 68, 173),
                    out lblTrackedVehiclesValue),
                2,
                1);

            // DashboardControl
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            Controls.Add(tlpRoot);
            Font = new Font("Segoe UI", 10F);
            Name = "DashboardControl";
            Padding = new Padding(20);
            Size = new Size(950, 650);

            tlpRoot.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }

        private static Panel CreateMetricCard(
            string title,
            Color accentColor,
            out Label valueLabel)
        {
            var card = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Margin = new Padding(8)
            };

            var accent = new Panel
            {
                BackColor = accentColor,
                Dock = DockStyle.Left,
                Width = 6
            };

            var titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(92, 103, 118),
                Location = new Point(24, 24),
                Text = title
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    30F,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(29, 39, 54),
                Location = new Point(22, 60),
                Text = "—"
            };

            card.Controls.Add(accent);
            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);

            return card;
        }

        #endregion
    }
}