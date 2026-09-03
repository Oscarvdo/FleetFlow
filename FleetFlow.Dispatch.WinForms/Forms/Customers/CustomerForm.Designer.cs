namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

partial class CustomerForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Panel pnlFields = null!;

    private Label lblCustomerNumber = null!;
    private TextBox txtCustomerNumber = null!;
    private Label lblCompanyName = null!;
    private TextBox txtCompanyName = null!;
    private Label lblContactName = null!;
    private TextBox txtContactName = null!;
    private Label lblEmail = null!;
    private TextBox txtEmail = null!;
    private Label lblPhone = null!;
    private TextBox txtPhone = null!;

    private Label lblMessage = null!;
    private Button btnCancel = null!;
    private Button btnSave = null!;
    private ErrorProvider errorProvider = null!;

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

        lblTitle = new Label();
        lblSubtitle = new Label();
        pnlFields = new Panel();

        lblCustomerNumber = new Label();
        txtCustomerNumber = new TextBox();
        lblCompanyName = new Label();
        txtCompanyName = new TextBox();
        lblContactName = new Label();
        txtContactName = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();

        lblMessage = new Label();
        btnCancel = new Button();
        btnSave = new Button();
        errorProvider = new ErrorProvider(components);

        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font(
            "Segoe UI",
            22F,
            FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(34, 25);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(189, 50);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Customer";

        // lblSubtitle
        lblSubtitle.AutoSize = true;
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(38, 80);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(219, 20);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Customer account information.";

        // pnlFields
        pnlFields.BackColor = Color.White;
        pnlFields.Controls.Add(lblCustomerNumber);
        pnlFields.Controls.Add(txtCustomerNumber);
        pnlFields.Controls.Add(lblCompanyName);
        pnlFields.Controls.Add(txtCompanyName);
        pnlFields.Controls.Add(lblContactName);
        pnlFields.Controls.Add(txtContactName);
        pnlFields.Controls.Add(lblEmail);
        pnlFields.Controls.Add(txtEmail);
        pnlFields.Controls.Add(lblPhone);
        pnlFields.Controls.Add(txtPhone);
        pnlFields.Location = new Point(38, 120);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(624, 430);
        pnlFields.TabIndex = 2;

        // lblCustomerNumber
        lblCustomerNumber.AutoSize = true;
        lblCustomerNumber.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblCustomerNumber.ForeColor = Color.FromArgb(45, 55, 70);
        lblCustomerNumber.Location = new Point(24, 22);
        lblCustomerNumber.Name = "lblCustomerNumber";
        lblCustomerNumber.Size = new Size(151, 20);
        lblCustomerNumber.TabIndex = 0;
        lblCustomerNumber.Text = "Customer number *";

        // txtCustomerNumber
        txtCustomerNumber.CharacterCasing = CharacterCasing.Upper;
        txtCustomerNumber.Font = new Font("Segoe UI", 10F);
        txtCustomerNumber.Location = new Point(24, 49);
        txtCustomerNumber.MaxLength = 30;
        txtCustomerNumber.Name = "txtCustomerNumber";
        txtCustomerNumber.Size = new Size(576, 30);
        txtCustomerNumber.TabIndex = 1;

        // lblCompanyName
        lblCompanyName.AutoSize = true;
        lblCompanyName.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblCompanyName.ForeColor = Color.FromArgb(45, 55, 70);
        lblCompanyName.Location = new Point(24, 100);
        lblCompanyName.Name = "lblCompanyName";
        lblCompanyName.Size = new Size(129, 20);
        lblCompanyName.TabIndex = 2;
        lblCompanyName.Text = "Company name *";

        // txtCompanyName
        txtCompanyName.Font = new Font("Segoe UI", 10F);
        txtCompanyName.Location = new Point(24, 127);
        txtCompanyName.MaxLength = 200;
        txtCompanyName.Name = "txtCompanyName";
        txtCompanyName.Size = new Size(576, 30);
        txtCompanyName.TabIndex = 3;

        // lblContactName
        lblContactName.AutoSize = true;
        lblContactName.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblContactName.ForeColor = Color.FromArgb(45, 55, 70);
        lblContactName.Location = new Point(24, 178);
        lblContactName.Name = "lblContactName";
        lblContactName.Size = new Size(122, 20);
        lblContactName.TabIndex = 4;
        lblContactName.Text = "Primary contact";

        // txtContactName
        txtContactName.Font = new Font("Segoe UI", 10F);
        txtContactName.Location = new Point(24, 205);
        txtContactName.MaxLength = 150;
        txtContactName.Name = "txtContactName";
        txtContactName.Size = new Size(576, 30);
        txtContactName.TabIndex = 5;

        // lblEmail
        lblEmail.AutoSize = true;
        lblEmail.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblEmail.ForeColor = Color.FromArgb(45, 55, 70);
        lblEmail.Location = new Point(24, 256);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(47, 20);
        lblEmail.TabIndex = 6;
        lblEmail.Text = "Email";

        // txtEmail
        txtEmail.Font = new Font("Segoe UI", 10F);
        txtEmail.Location = new Point(24, 283);
        txtEmail.MaxLength = 254;
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(576, 30);
        txtEmail.TabIndex = 7;

        // lblPhone
        lblPhone.AutoSize = true;
        lblPhone.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblPhone.ForeColor = Color.FromArgb(45, 55, 70);
        lblPhone.Location = new Point(24, 334);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(52, 20);
        lblPhone.TabIndex = 8;
        lblPhone.Text = "Phone";

        // txtPhone
        txtPhone.Font = new Font("Segoe UI", 10F);
        txtPhone.Location = new Point(24, 361);
        txtPhone.MaxLength = 40;
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(576, 30);
        txtPhone.TabIndex = 9;

        // lblMessage
        lblMessage.ForeColor = Color.FromArgb(106, 116, 130);
        lblMessage.Location = new Point(38, 574);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(350, 30);
        lblMessage.TabIndex = 3;

        // btnCancel
        btnCancel.BackColor = Color.FromArgb(225, 229, 235);
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnCancel.ForeColor = Color.FromArgb(45, 55, 70);
        btnCancel.Location = new Point(412, 566);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 42);
        btnCancel.TabIndex = 4;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = false;

        // btnSave
        btnSave.BackColor = Color.FromArgb(243, 108, 33);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(524, 566);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(138, 42);
        btnSave.TabIndex = 5;
        btnSave.Text = "Save Customer";
        btnSave.UseVisualStyleBackColor = false;

        // errorProvider
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;

        // CustomerForm
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        CancelButton = btnCancel;
        ClientSize = new Size(700, 640);
        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(pnlFields);
        Controls.Add(lblMessage);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CustomerForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FleetFlow — Customer";

        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}