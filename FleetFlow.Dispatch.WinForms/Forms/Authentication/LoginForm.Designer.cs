namespace FleetFlow.Dispatch.WinForms.Forms.Authentication
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlAccent;
        private Panel pnlContent;
        private Label lblBrand;
        private Label lblSubtitle;
        private Label lblWelcome;
        private Label lblInstructions;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private Label lblError;
        private Label lblEnvironment;

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
            pnlAccent = new Panel();
            lblBrand = new Label();
            lblSubtitle = new Label();
            pnlContent = new Panel();
            lblWelcome = new Label();
            lblInstructions = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            btnLogin = new Button();
            lblError = new Label();
            lblEnvironment = new Label();

            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            SuspendLayout();

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(29, 39, 54);
            pnlHeader.Controls.Add(pnlAccent);
            pnlHeader.Controls.Add(lblBrand);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(480, 145);
            pnlHeader.TabIndex = 0;

            // pnlAccent
            pnlAccent.BackColor = Color.FromArgb(243, 108, 33);
            pnlAccent.Dock = DockStyle.Bottom;
            pnlAccent.Location = new Point(0, 140);
            pnlAccent.Name = "pnlAccent";
            pnlAccent.Size = new Size(480, 5);
            pnlAccent.TabIndex = 0;

            // lblBrand
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font(
                "Segoe UI",
                27F,
                FontStyle.Bold,
                GraphicsUnit.Point);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(38, 28);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(220, 48);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "FleetFlow";

            // lblSubtitle
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Regular,
                GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(195, 203, 214);
            lblSubtitle.Location = new Point(42, 86);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(246, 19);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Trucking Fleet Management System";

            // pnlContent
            pnlContent.BackColor = Color.FromArgb(246, 248, 251);
            pnlContent.Controls.Add(lblWelcome);
            pnlContent.Controls.Add(lblInstructions);
            pnlContent.Controls.Add(lblUsername);
            pnlContent.Controls.Add(txtUsername);
            pnlContent.Controls.Add(lblPassword);
            pnlContent.Controls.Add(txtPassword);
            pnlContent.Controls.Add(chkShowPassword);
            pnlContent.Controls.Add(btnLogin);
            pnlContent.Controls.Add(lblError);
            pnlContent.Controls.Add(lblEnvironment);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 145);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(480, 445);
            pnlContent.TabIndex = 1;

            // lblWelcome
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font(
                "Segoe UI",
                18F,
                FontStyle.Bold,
                GraphicsUnit.Point);
            lblWelcome.ForeColor = Color.FromArgb(29, 39, 54);
            lblWelcome.Location = new Point(42, 28);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(103, 32);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Sign in";

            // lblInstructions
            lblInstructions.AutoSize = true;
            lblInstructions.ForeColor = Color.FromArgb(93, 104, 119);
            lblInstructions.Location = new Point(45, 67);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(268, 19);
            lblInstructions.TabIndex = 1;
            lblInstructions.Text = "Enter your FleetFlow account credentials.";

            // lblUsername
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Bold,
                GraphicsUnit.Point);
            lblUsername.ForeColor = Color.FromArgb(45, 55, 72);
            lblUsername.Location = new Point(45, 112);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(76, 19);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username";

            // txtUsername
            txtUsername.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Location = new Point(45, 136);
            txtUsername.MaxLength = 80;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Enter username";
            txtUsername.Size = new Size(390, 25);
            txtUsername.TabIndex = 0;

            // lblPassword
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Bold,
                GraphicsUnit.Point);
            lblPassword.ForeColor = Color.FromArgb(45, 55, 72);
            lblPassword.Location = new Point(45, 181);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(73, 19);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password";

            // txtPassword
            txtPassword.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Location = new Point(45, 205);
            txtPassword.MaxLength = 200;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Enter password";
            txtPassword.Size = new Size(390, 25);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;

            // chkShowPassword
            chkShowPassword.AutoSize = true;
            chkShowPassword.ForeColor = Color.FromArgb(75, 86, 101);
            chkShowPassword.Location = new Point(45, 245);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(124, 23);
            chkShowPassword.TabIndex = 2;
            chkShowPassword.Text = "Show password";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;

            // btnLogin
            btnLogin.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;
            btnLogin.BackColor = Color.FromArgb(243, 108, 33);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Bold,
                GraphicsUnit.Point);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(45, 286);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(390, 44);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "SIGN IN";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;

            // lblError
            lblError.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;
            lblError.Font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Regular,
                GraphicsUnit.Point);
            lblError.ForeColor = Color.Firebrick;
            lblError.Location = new Point(45, 342);
            lblError.Name = "lblError";
            lblError.Size = new Size(390, 45);
            lblError.TabIndex = 8;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;

            // lblEnvironment
            lblEnvironment.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;
            lblEnvironment.ForeColor = Color.FromArgb(120, 129, 143);
            lblEnvironment.Location = new Point(45, 405);
            lblEnvironment.Name = "lblEnvironment";
            lblEnvironment.Size = new Size(390, 20);
            lblEnvironment.TabIndex = 9;
            lblEnvironment.Text =
                "FleetFlow Dispatch • Local development environment";
            lblEnvironment.TextAlign = ContentAlignment.MiddleCenter;

            // LoginForm
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 251);
            ClientSize = new Size(480, 590);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Regular,
                GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = true;
            Name = "LoginForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FleetFlow — Sign In";

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}