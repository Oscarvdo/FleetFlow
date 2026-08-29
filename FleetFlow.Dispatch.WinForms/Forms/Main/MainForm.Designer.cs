namespace FleetFlow.Dispatch.WinForms.Forms.Main
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlSidebar;
        private Panel pnlBrand;
        private Label lblBrand;
        private Label lblBrandSubtitle;
        private FlowLayoutPanel flpNavigation;
        private Button btnDashboard;
        private Button btnDispatch;
        private Button btnTrips;
        private Button btnLoads;
        private Button btnCustomers;
        private Button btnFleet;
        private Button btnTracking;
        private Button btnReports;
        private Button btnAdministration;
        private Panel pnlUser;
        private Label lblUserName;
        private Label lblUserRole;
        private Button btnLogout;
        private Panel pnlTopBar;
        private Label lblPageTitle;
        private Label lblEnvironment;
        private Panel pnlContentHost;
        private Label lblWelcome;
        private Label lblWelcomeSubtitle;

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
            pnlSidebar = new Panel();
            pnlBrand = new Panel();
            lblBrand = new Label();
            lblBrandSubtitle = new Label();
            flpNavigation = new FlowLayoutPanel();
            btnDashboard = CreateNavigationButton("Dashboard");
            btnDispatch = CreateNavigationButton("Dispatch Board");
            btnTrips = CreateNavigationButton("Trips");
            btnLoads = CreateNavigationButton("Loads");
            btnCustomers = CreateNavigationButton("Customers");
            btnFleet = CreateNavigationButton("Fleet");
            btnTracking = CreateNavigationButton("Live Tracking");
            btnReports = CreateNavigationButton("Reports");
            btnAdministration = CreateNavigationButton("Administration");
            pnlUser = new Panel();
            lblUserName = new Label();
            lblUserRole = new Label();
            btnLogout = new Button();
            pnlTopBar = new Panel();
            lblPageTitle = new Label();
            lblEnvironment = new Label();
            pnlContentHost = new Panel();
            lblWelcome = new Label();
            lblWelcomeSubtitle = new Label();

            pnlSidebar.SuspendLayout();
            pnlBrand.SuspendLayout();
            flpNavigation.SuspendLayout();
            pnlUser.SuspendLayout();
            pnlTopBar.SuspendLayout();
            pnlContentHost.SuspendLayout();
            SuspendLayout();

            // pnlSidebar
            pnlSidebar.BackColor = Color.FromArgb(29, 39, 54);
            pnlSidebar.Controls.Add(flpNavigation);
            pnlSidebar.Controls.Add(pnlUser);
            pnlSidebar.Controls.Add(pnlBrand);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(250, 750);
            pnlSidebar.TabIndex = 0;

            // pnlBrand
            pnlBrand.Controls.Add(lblBrand);
            pnlBrand.Controls.Add(lblBrandSubtitle);
            pnlBrand.Dock = DockStyle.Top;
            pnlBrand.Name = "pnlBrand";
            pnlBrand.Size = new Size(250, 110);

            // lblBrand
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(20, 20);
            lblBrand.Name = "lblBrand";
            lblBrand.Text = "FleetFlow";

            // lblBrandSubtitle
            lblBrandSubtitle.AutoSize = true;
            lblBrandSubtitle.ForeColor = Color.FromArgb(180, 190, 204);
            lblBrandSubtitle.Location = new Point(23, 68);
            lblBrandSubtitle.Name = "lblBrandSubtitle";
            lblBrandSubtitle.Text = "DISPATCH OPERATIONS";

            // flpNavigation
            flpNavigation.AutoScroll = true;
            flpNavigation.Controls.Add(btnDashboard);
            flpNavigation.Controls.Add(btnDispatch);
            flpNavigation.Controls.Add(btnTrips);
            flpNavigation.Controls.Add(btnLoads);
            flpNavigation.Controls.Add(btnCustomers);
            flpNavigation.Controls.Add(btnFleet);
            flpNavigation.Controls.Add(btnTracking);
            flpNavigation.Controls.Add(btnReports);
            flpNavigation.Controls.Add(btnAdministration);
            flpNavigation.Dock = DockStyle.Fill;
            flpNavigation.FlowDirection = FlowDirection.TopDown;
            flpNavigation.Location = new Point(0, 110);
            flpNavigation.Name = "flpNavigation";
            flpNavigation.Padding = new Padding(10, 12, 10, 12);
            flpNavigation.Size = new Size(250, 520);
            flpNavigation.WrapContents = false;

            // pnlUser
            pnlUser.BackColor = Color.FromArgb(23, 32, 45);
            pnlUser.Controls.Add(lblUserName);
            pnlUser.Controls.Add(lblUserRole);
            pnlUser.Controls.Add(btnLogout);
            pnlUser.Dock = DockStyle.Bottom;
            pnlUser.Name = "pnlUser";
            pnlUser.Size = new Size(250, 120);

            // lblUserName
            lblUserName.AutoEllipsis = true;
            lblUserName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUserName.ForeColor = Color.White;
            lblUserName.Location = new Point(18, 14);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(214, 22);
            lblUserName.Text = "User";

            // lblUserRole
            lblUserRole.AutoEllipsis = true;
            lblUserRole.ForeColor = Color.FromArgb(180, 190, 204);
            lblUserRole.Location = new Point(18, 39);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(214, 21);
            lblUserRole.Text = "Role";

            // btnLogout
            btnLogout.BackColor = Color.FromArgb(42, 55, 73);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(18, 72);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(214, 34);
            btnLogout.Text = "Sign Out";
            btnLogout.UseVisualStyleBackColor = false;

            // pnlTopBar
            pnlTopBar.BackColor = Color.White;
            pnlTopBar.Controls.Add(lblPageTitle);
            pnlTopBar.Controls.Add(lblEnvironment);
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Name = "pnlTopBar";
            pnlTopBar.Size = new Size(950, 72);

            // lblPageTitle
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(29, 39, 54);
            lblPageTitle.Location = new Point(28, 18);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Text = "Dashboard";

            // lblEnvironment
            lblEnvironment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEnvironment.ForeColor = Color.FromArgb(106, 116, 130);
            lblEnvironment.Location = new Point(680, 25);
            lblEnvironment.Name = "lblEnvironment";
            lblEnvironment.Size = new Size(240, 23);
            lblEnvironment.Text = "LOCAL DEVELOPMENT";
            lblEnvironment.TextAlign = ContentAlignment.MiddleRight;

            // pnlContentHost
            pnlContentHost.BackColor = Color.FromArgb(244, 246, 249);
            pnlContentHost.Controls.Add(lblWelcome);
            pnlContentHost.Controls.Add(lblWelcomeSubtitle);
            pnlContentHost.Dock = DockStyle.Fill;
            pnlContentHost.Name = "pnlContentHost";
            pnlContentHost.Padding = new Padding(30);
            pnlContentHost.Size = new Size(950, 678);

            // lblWelcome
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(29, 39, 54);
            lblWelcome.Location = new Point(34, 40);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Text = "Welcome to FleetFlow";

            // lblWelcomeSubtitle
            lblWelcomeSubtitle.AutoSize = true;
            lblWelcomeSubtitle.Font = new Font("Segoe UI", 11F);
            lblWelcomeSubtitle.ForeColor = Color.FromArgb(92, 103, 118);
            lblWelcomeSubtitle.Location = new Point(38, 94);
            lblWelcomeSubtitle.Name = "lblWelcomeSubtitle";
            lblWelcomeSubtitle.Text =
                "Select an operations module from the navigation menu.";

            // MainForm
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1200, 750);
            Controls.Add(pnlContentHost);
            Controls.Add(pnlTopBar);
            Controls.Add(pnlSidebar);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1050, 680);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FleetFlow";
            WindowState = FormWindowState.Maximized;

            pnlSidebar.ResumeLayout(false);
            pnlBrand.ResumeLayout(false);
            pnlBrand.PerformLayout();
            flpNavigation.ResumeLayout(false);
            pnlUser.ResumeLayout(false);
            pnlTopBar.ResumeLayout(false);
            pnlTopBar.PerformLayout();
            pnlContentHost.ResumeLayout(false);
            pnlContentHost.PerformLayout();
            ResumeLayout(false);
        }

        private static Button CreateNavigationButton(string text)
        {
            return new Button
            {
                BackColor = Color.FromArgb(29, 39, 54),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(220, 226, 234),
                Margin = new Padding(0, 0, 0, 4),
                Size = new Size(214, 43),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false
            };
        }

        #endregion
    }
}