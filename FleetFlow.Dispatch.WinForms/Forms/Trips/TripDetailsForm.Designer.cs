namespace FleetFlow.Dispatch.WinForms.Forms.Trips
{
    partial class TripDetailsForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblTripNumber;
        private Label lblTripStatus;
        private Button btnRefresh;
        private Button btnClose;

        private Panel pnlSummary;
        private Label lblCustomerTitle;
        private Label lblCustomerValue;
        private Label lblLoadTitle;
        private Label lblLoadValue;
        private Label lblDescriptionTitle;
        private Label lblDescriptionValue;
        private Label lblScheduleTitle;
        private Label lblScheduleValue;
        private Label lblDistanceTitle;
        private Label lblDistanceValue;
        private Label lblProgressTitle;
        private Label lblProgressValue;

        private TabControl tabDetails;
        private TabPage tabStops;
        private TabPage tabHistory;
        private DataGridView dgvStops;
        private DataGridView dgvHistory;
        private Label lblMessage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTripNumber = new Label();
            lblTripStatus = new Label();
            btnRefresh = new Button();
            btnClose = new Button();

            pnlSummary = new Panel();
            lblCustomerTitle = new Label();
            lblCustomerValue = new Label();
            lblLoadTitle = new Label();
            lblLoadValue = new Label();
            lblDescriptionTitle = new Label();
            lblDescriptionValue = new Label();
            lblScheduleTitle = new Label();
            lblScheduleValue = new Label();
            lblDistanceTitle = new Label();
            lblDistanceValue = new Label();
            lblProgressTitle = new Label();
            lblProgressValue = new Label();

            tabDetails = new TabControl();
            tabStops = new TabPage();
            tabHistory = new TabPage();
            dgvStops = new DataGridView();
            dgvHistory = new DataGridView();
            lblMessage = new Label();

            pnlHeader.SuspendLayout();
            pnlSummary.SuspendLayout();
            tabDetails.SuspendLayout();
            tabStops.SuspendLayout();
            tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStops)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistory)
                .BeginInit();
            SuspendLayout();

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(29, 39, 54);
            pnlHeader.Controls.Add(lblTripNumber);
            pnlHeader.Controls.Add(lblTripStatus);
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Size = new Size(1100, 92);

            // lblTripNumber
            lblTripNumber.AutoSize = true;
            lblTripNumber.Font = new Font(
                "Segoe UI",
                22F,
                FontStyle.Bold);
            lblTripNumber.ForeColor = Color.White;
            lblTripNumber.Location = new Point(28, 14);
            lblTripNumber.Text = "Trip";

            // lblTripStatus
            lblTripStatus.AutoSize = true;
            lblTripStatus.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Bold);
            lblTripStatus.ForeColor = Color.FromArgb(243, 108, 33);
            lblTripStatus.Location = new Point(32, 58);
            lblTripStatus.Text = "Loading...";

            // btnRefresh
            btnRefresh.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(243, 108, 33);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(860, 27);
            btnRefresh.Size = new Size(100, 38);
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;

            // btnClose
            btnClose.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(51, 64, 82);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(972, 27);
            btnClose.Size = new Size(100, 38);
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;

            // pnlSummary
            pnlSummary.BackColor = Color.White;
            pnlSummary.Controls.Add(lblCustomerTitle);
            pnlSummary.Controls.Add(lblCustomerValue);
            pnlSummary.Controls.Add(lblLoadTitle);
            pnlSummary.Controls.Add(lblLoadValue);
            pnlSummary.Controls.Add(lblDescriptionTitle);
            pnlSummary.Controls.Add(lblDescriptionValue);
            pnlSummary.Controls.Add(lblScheduleTitle);
            pnlSummary.Controls.Add(lblScheduleValue);
            pnlSummary.Controls.Add(lblDistanceTitle);
            pnlSummary.Controls.Add(lblDistanceValue);
            pnlSummary.Controls.Add(lblProgressTitle);
            pnlSummary.Controls.Add(lblProgressValue);
            pnlSummary.Dock = DockStyle.Top;
            pnlSummary.Location = new Point(0, 92);
            pnlSummary.Size = new Size(1100, 165);

            ConfigureTitleLabel(
                lblCustomerTitle,
                "CUSTOMER",
                28,
                18);

            ConfigureValueLabel(
                lblCustomerValue,
                "—",
                28,
                42,
                300);

            ConfigureTitleLabel(
                lblLoadTitle,
                "LOAD",
                380,
                18);

            ConfigureValueLabel(
                lblLoadValue,
                "—",
                380,
                42,
                300);

            ConfigureTitleLabel(
                lblDescriptionTitle,
                "DESCRIPTION",
                730,
                18);

            ConfigureValueLabel(
                lblDescriptionValue,
                "—",
                730,
                42,
                330);

            ConfigureTitleLabel(
                lblScheduleTitle,
                "SCHEDULE",
                28,
                88);

            ConfigureValueLabel(
                lblScheduleValue,
                "—",
                28,
                112,
                400);

            ConfigureTitleLabel(
                lblDistanceTitle,
                "DISTANCE",
                500,
                88);

            ConfigureValueLabel(
                lblDistanceValue,
                "—",
                500,
                112,
                220);

            ConfigureTitleLabel(
                lblProgressTitle,
                "PROGRESS",
                780,
                88);

            ConfigureValueLabel(
                lblProgressValue,
                "—",
                780,
                112,
                250);

            // tabDetails
            tabDetails.Controls.Add(tabStops);
            tabDetails.Controls.Add(tabHistory);
            tabDetails.Dock = DockStyle.Fill;
            tabDetails.Location = new Point(18, 275);
            tabDetails.Name = "tabDetails";
            tabDetails.SelectedIndex = 0;

            // tabStops
            tabStops.BackColor = Color.White;
            tabStops.Controls.Add(dgvStops);
            tabStops.Text = "Stops";

            // dgvStops
            dgvStops.AllowUserToAddRows = false;
            dgvStops.AllowUserToDeleteRows = false;
            dgvStops.AutoGenerateColumns = false;
            dgvStops.BackgroundColor = Color.White;
            dgvStops.BorderStyle = BorderStyle.None;
            dgvStops.ColumnHeadersHeight = 38;
            dgvStops.Dock = DockStyle.Fill;
            dgvStops.ReadOnly = true;
            dgvStops.RowHeadersVisible = false;
            dgvStops.RowTemplate.Height = 36;
            dgvStops.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvStops.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StopSequence",
                    HeaderText = "#",
                    Width = 50
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StopTypeCode",
                    HeaderText = "Type",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StopStatusCode",
                    HeaderText = "Status",
                    Width = 110
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "LocationName",
                    HeaderText = "Location",
                    Width = 190
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "City",
                    HeaderText = "City",
                    Width = 130
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StateCode",
                    HeaderText = "State",
                    Width = 65
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ScheduledArrivalUtc",
                    HeaderText = "Scheduled",
                    Width = 150
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ActualArrivalUtc",
                    HeaderText = "Actual",
                    Width = 150
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Instructions",
                    HeaderText = "Instructions",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill
                });

            // tabHistory
            tabHistory.BackColor = Color.White;
            tabHistory.Controls.Add(dgvHistory);
            tabHistory.Text = "Status History";

            // dgvHistory
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.BackgroundColor = Color.White;
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.ColumnHeadersHeight = 38;
            dgvHistory.Dock = DockStyle.Fill;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.RowTemplate.Height = 36;
            dgvHistory.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvHistory.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ChangedAtUtc",
                    HeaderText = "Changed",
                    Width = 155
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "PreviousStatusCode",
                    HeaderText = "Previous",
                    Width = 140
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "NewStatusCode",
                    HeaderText = "New Status",
                    Width = 160
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Source",
                    HeaderText = "Source",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ChangedBy",
                    HeaderText = "Changed By",
                    Width = 160
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Notes",
                    HeaderText = "Notes",
                    AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill
                });

            // lblMessage
            lblMessage.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;
            lblMessage.ForeColor = Color.Firebrick;
            lblMessage.Location = new Point(18, 676);
            lblMessage.Size = new Size(1064, 24);
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblMessage.Visible = false;

            // TripDetailsForm
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1100, 720);
            Controls.Add(tabDetails);
            Controls.Add(lblMessage);
            Controls.Add(pnlSummary);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(950, 650);
            Name = "TripDetailsForm";
            Padding = new Padding(18, 0, 18, 18);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "FleetFlow — Trip Details";

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            tabDetails.ResumeLayout(false);
            tabStops.ResumeLayout(false);
            tabHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStops)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistory)
                .EndInit();
            ResumeLayout(false);
        }

        private static void ConfigureTitleLabel(
            Label label,
            string text,
            int x,
            int y)
        {
            label.AutoSize = true;
            label.Font = new Font(
                "Segoe UI",
                8F,
                FontStyle.Bold);
            label.ForeColor = Color.FromArgb(106, 116, 130);
            label.Location = new Point(x, y);
            label.Text = text;
        }

        private static void ConfigureValueLabel(
            Label label,
            string text,
            int x,
            int y,
            int width)
        {
            label.AutoEllipsis = true;
            label.Font = new Font(
                "Segoe UI",
                11F,
                FontStyle.Bold);
            label.ForeColor = Color.FromArgb(29, 39, 54);
            label.Location = new Point(x, y);
            label.Size = new Size(width, 28);
            label.Text = text;
        }

        #endregion
    }
}