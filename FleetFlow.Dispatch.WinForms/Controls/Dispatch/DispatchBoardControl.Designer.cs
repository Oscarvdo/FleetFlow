namespace FleetFlow.Dispatch.WinForms.Controls.Dispatch
{
    partial class DispatchBoardControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblDescription;
        private TextBox txtSearch;
        private Button btnRefresh;
        private DataGridView dgvTrips;
        private Panel pnlStatus;
        private Label lblCount;
        private Label lblStatus;

        private DataGridViewTextBoxColumn colTripNumber;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colLoad;
        private DataGridViewTextBoxColumn colCustomer;
        private DataGridViewTextBoxColumn colDriver;
        private DataGridViewTextBoxColumn colVehicle;
        private DataGridViewTextBoxColumn colTrailer;
        private DataGridViewTextBoxColumn colPickup;
        private DataGridViewTextBoxColumn colDelivery;
        private DataGridViewTextBoxColumn colScheduledPickup;
        private DataGridViewTextBoxColumn colScheduledDelivery;
        private DataGridViewTextBoxColumn colProgress;
        private DataGridViewTextBoxColumn colFuel;

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
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblDescription = new Label();
            txtSearch = new TextBox();
            btnRefresh = new Button();
            dgvTrips = new DataGridView();
            pnlStatus = new Panel();
            lblCount = new Label();
            lblStatus = new Label();

            colTripNumber = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colLoad = new DataGridViewTextBoxColumn();
            colCustomer = new DataGridViewTextBoxColumn();
            colDriver = new DataGridViewTextBoxColumn();
            colVehicle = new DataGridViewTextBoxColumn();
            colTrailer = new DataGridViewTextBoxColumn();
            colPickup = new DataGridViewTextBoxColumn();
            colDelivery = new DataGridViewTextBoxColumn();
            colScheduledPickup = new DataGridViewTextBoxColumn();
            colScheduledDelivery = new DataGridViewTextBoxColumn();
            colProgress = new DataGridViewTextBoxColumn();
            colFuel = new DataGridViewTextBoxColumn();

            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrips)
                .BeginInit();
            pnlStatus.SuspendLayout();
            SuspendLayout();

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(244, 246, 249);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblDescription);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1050, 105);

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font(
                "Segoe UI",
                22F,
                FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
            lblTitle.Location = new Point(14, 8);
            lblTitle.Text = "Dispatch Board";

            // lblDescription
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = Color.FromArgb(92, 103, 118);
            lblDescription.Location = new Point(18, 57);
            lblDescription.Text =
                "Monitor active trips, assignments, schedules, and progress.";

            // txtSearch
            txtSearch.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.Location = new Point(645, 35);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText =
                "Search trip, load, customer, driver...";
            txtSearch.Size = new Size(280, 25);

            // btnRefresh
            btnRefresh.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(29, 39, 54);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(937, 29);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 38);
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;

            // dgvTrips
            dgvTrips.AllowUserToAddRows = false;
            dgvTrips.AllowUserToDeleteRows = false;
            dgvTrips.AllowUserToResizeRows = false;
            dgvTrips.AutoGenerateColumns = false;
            dgvTrips.BackgroundColor = Color.White;
            dgvTrips.BorderStyle = BorderStyle.None;
            dgvTrips.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTrips.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;
            dgvTrips.ColumnHeadersHeight = 42;
            dgvTrips.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTrips.Columns.AddRange(
                colTripNumber,
                colStatus,
                colLoad,
                colCustomer,
                colDriver,
                colVehicle,
                colTrailer,
                colPickup,
                colDelivery,
                colScheduledPickup,
                colScheduledDelivery,
                colProgress,
                colFuel);
            dgvTrips.Dock = DockStyle.Fill;
            dgvTrips.EnableHeadersVisualStyles = false;
            dgvTrips.GridColor = Color.FromArgb(230, 234, 240);
            dgvTrips.MultiSelect = false;
            dgvTrips.Name = "dgvTrips";
            dgvTrips.ReadOnly = true;
            dgvTrips.RowHeadersVisible = false;
            dgvTrips.RowTemplate.Height = 38;
            dgvTrips.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvTrips.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(29, 39, 54),
                    ForeColor = Color.White,
                    Font = new Font(
                        "Segoe UI",
                        9F,
                        FontStyle.Bold),
                    SelectionBackColor =
                        Color.FromArgb(29, 39, 54),
                    Alignment =
                        DataGridViewContentAlignment.MiddleLeft
                };

            dgvTrips.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(45, 55, 72),
                    SelectionBackColor =
                        Color.FromArgb(224, 235, 249),
                    SelectionForeColor =
                        Color.FromArgb(29, 39, 54),
                    Padding = new Padding(4, 0, 4, 0)
                };

            dgvTrips.AlternatingRowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 250, 252)
                };

            // Columns
            ConfigureColumn(
                colTripNumber,
                "Trip",
                "TripNumber",
                125);

            ConfigureColumn(
                colStatus,
                "Status",
                "TripStatus",
                140);

            ConfigureColumn(
                colLoad,
                "Load",
                "LoadNumber",
                115);

            ConfigureColumn(
                colCustomer,
                "Customer",
                "Customer",
                175);

            ConfigureColumn(
                colDriver,
                "Driver",
                "DriverName",
                165);

            ConfigureColumn(
                colVehicle,
                "Truck",
                "VehicleUnitNumber",
                90);

            ConfigureColumn(
                colTrailer,
                "Trailer",
                "TrailerUnitNumber",
                90);

            ConfigureColumn(
                colPickup,
                "Pickup",
                "PickupLocation",
                180);

            ConfigureColumn(
                colDelivery,
                "Delivery",
                "DeliveryLocation",
                180);

            ConfigureColumn(
                colScheduledPickup,
                "Pickup Time",
                "ScheduledPickupUtc",
                150);

            ConfigureColumn(
                colScheduledDelivery,
                "Delivery Time",
                "ScheduledDeliveryUtc",
                150);

            ConfigureColumn(
                colProgress,
                "Progress",
                "ProgressPercent",
                90);

            ConfigureColumn(
                colFuel,
                "Fuel",
                "FuelPercent",
                80);

            // pnlStatus
            pnlStatus.BackColor = Color.White;
            pnlStatus.Controls.Add(lblCount);
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Dock = DockStyle.Bottom;
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Size = new Size(1050, 42);

            // lblCount
            lblCount.AutoSize = true;
            lblCount.ForeColor = Color.FromArgb(75, 86, 101);
            lblCount.Location = new Point(14, 11);
            lblCount.Text = "0 active trips";

            // lblStatus
            lblStatus.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            lblStatus.ForeColor = Color.FromArgb(106, 116, 130);
            lblStatus.Location = new Point(700, 10);
            lblStatus.Size = new Size(335, 23);
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleRight;

            // DispatchBoardControl
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            Controls.Add(dgvTrips);
            Controls.Add(pnlStatus);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            Name = "DispatchBoardControl";
            Padding = new Padding(20);
            Size = new Size(1050, 650);

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrips)
                .EndInit();
            pnlStatus.ResumeLayout(false);
            pnlStatus.PerformLayout();
            ResumeLayout(false);
        }

        private static void ConfigureColumn(
            DataGridViewTextBoxColumn column,
            string headerText,
            string propertyName,
            int width)
        {
            column.DataPropertyName = propertyName;
            column.HeaderText = headerText;
            column.Name = $"col{propertyName}";
            column.ReadOnly = true;
            column.Width = width;
        }

        #endregion
    }
}