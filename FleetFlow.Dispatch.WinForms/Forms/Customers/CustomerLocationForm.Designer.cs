namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

partial class CustomerLocationForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label lblTitle = null!;
    private Panel pnlFields = null!;

    private Label lblLocationCode = null!;
    private TextBox txtLocationCode = null!;
    private Label lblLocationType = null!;
    private ComboBox cboLocationType = null!;

    private Label lblLocationName = null!;
    private TextBox txtLocationName = null!;

    private Label lblAddress1 = null!;
    private TextBox txtAddress1 = null!;
    private Label lblAddress2 = null!;
    private TextBox txtAddress2 = null!;

    private Label lblCity = null!;
    private TextBox txtCity = null!;
    private Label lblState = null!;
    private TextBox txtState = null!;
    private Label lblPostalCode = null!;
    private TextBox txtPostalCode = null!;

    private Label lblLatitude = null!;
    private TextBox txtLatitude = null!;
    private Label lblLongitude = null!;
    private TextBox txtLongitude = null!;

    private Label lblContactName = null!;
    private TextBox txtContactName = null!;
    private Label lblContactPhone = null!;
    private TextBox txtContactPhone = null!;

    private CheckBox chkBilling = null!;
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
        pnlFields = new Panel();
        lblLocationCode = new Label();
        txtLocationCode = new TextBox();
        lblLocationType = new Label();
        cboLocationType = new ComboBox();
        lblLocationName = new Label();
        txtLocationName = new TextBox();
        lblAddress1 = new Label();
        txtAddress1 = new TextBox();
        lblAddress2 = new Label();
        txtAddress2 = new TextBox();
        lblCity = new Label();
        txtCity = new TextBox();
        lblState = new Label();
        txtState = new TextBox();
        lblPostalCode = new Label();
        txtPostalCode = new TextBox();
        lblLatitude = new Label();
        txtLatitude = new TextBox();
        lblLongitude = new Label();
        txtLongitude = new TextBox();
        lblContactName = new Label();
        txtContactName = new TextBox();
        lblContactPhone = new Label();
        txtContactPhone = new TextBox();
        chkBilling = new CheckBox();
        lblMessage = new Label();
        btnCancel = new Button();
        btnSave = new Button();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(34, 22);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(172, 50);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Location";
        // 
        // pnlFields
        // 
        pnlFields.BackColor = Color.White;
        pnlFields.Controls.Add(lblLocationCode);
        pnlFields.Controls.Add(txtLocationCode);
        pnlFields.Controls.Add(lblLocationType);
        pnlFields.Controls.Add(cboLocationType);
        pnlFields.Controls.Add(lblLocationName);
        pnlFields.Controls.Add(txtLocationName);
        pnlFields.Controls.Add(lblAddress1);
        pnlFields.Controls.Add(txtAddress1);
        pnlFields.Controls.Add(lblAddress2);
        pnlFields.Controls.Add(txtAddress2);
        pnlFields.Controls.Add(lblCity);
        pnlFields.Controls.Add(txtCity);
        pnlFields.Controls.Add(lblState);
        pnlFields.Controls.Add(txtState);
        pnlFields.Controls.Add(lblPostalCode);
        pnlFields.Controls.Add(txtPostalCode);
        pnlFields.Controls.Add(lblLatitude);
        pnlFields.Controls.Add(txtLatitude);
        pnlFields.Controls.Add(lblLongitude);
        pnlFields.Controls.Add(txtLongitude);
        pnlFields.Controls.Add(lblContactName);
        pnlFields.Controls.Add(txtContactName);
        pnlFields.Controls.Add(lblContactPhone);
        pnlFields.Controls.Add(txtContactPhone);
        pnlFields.Controls.Add(chkBilling);
        pnlFields.Location = new Point(34, 82);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(642, 610);
        pnlFields.TabIndex = 1;
        // 
        // lblLocationCode
        // 
        lblLocationCode.AutoSize = true;
        lblLocationCode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLocationCode.ForeColor = Color.FromArgb(45, 55, 70);
        lblLocationCode.Location = new Point(24, 20);
        lblLocationCode.Name = "lblLocationCode";
        lblLocationCode.Size = new Size(117, 20);
        lblLocationCode.TabIndex = 0;
        lblLocationCode.Text = "Location code *";
        // 
        // txtLocationCode
        // 
        txtLocationCode.CharacterCasing = CharacterCasing.Upper;
        txtLocationCode.Font = new Font("Segoe UI", 10F);
        txtLocationCode.Location = new Point(24, 46);
        txtLocationCode.MaxLength = 30;
        txtLocationCode.Name = "txtLocationCode";
        txtLocationCode.Size = new Size(285, 30);
        txtLocationCode.TabIndex = 1;
        // 
        // lblLocationType
        // 
        lblLocationType.AutoSize = true;
        lblLocationType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLocationType.ForeColor = Color.FromArgb(45, 55, 70);
        lblLocationType.Location = new Point(329, 20);
        lblLocationType.Name = "lblLocationType";
        lblLocationType.Size = new Size(115, 20);
        lblLocationType.TabIndex = 2;
        lblLocationType.Text = "Location type *";
        // 
        // cboLocationType
        // 
        cboLocationType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocationType.Font = new Font("Segoe UI", 10F);
        cboLocationType.FormattingEnabled = true;
        cboLocationType.Location = new Point(329, 46);
        cboLocationType.Name = "cboLocationType";
        cboLocationType.Size = new Size(285, 31);
        cboLocationType.TabIndex = 3;
        // 
        // lblLocationName
        // 
        lblLocationName.AutoSize = true;
        lblLocationName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLocationName.ForeColor = Color.FromArgb(45, 55, 70);
        lblLocationName.Location = new Point(24, 96);
        lblLocationName.Name = "lblLocationName";
        lblLocationName.Size = new Size(123, 20);
        lblLocationName.TabIndex = 4;
        lblLocationName.Text = "Location name *";
        // 
        // txtLocationName
        // 
        txtLocationName.Font = new Font("Segoe UI", 10F);
        txtLocationName.Location = new Point(24, 122);
        txtLocationName.MaxLength = 150;
        txtLocationName.Name = "txtLocationName";
        txtLocationName.Size = new Size(590, 30);
        txtLocationName.TabIndex = 5;
        // 
        // lblAddress1
        // 
        lblAddress1.AutoSize = true;
        lblAddress1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAddress1.ForeColor = Color.FromArgb(45, 55, 70);
        lblAddress1.Location = new Point(24, 172);
        lblAddress1.Name = "lblAddress1";
        lblAddress1.Size = new Size(119, 20);
        lblAddress1.TabIndex = 6;
        lblAddress1.Text = "Address line 1 *";
        // 
        // txtAddress1
        // 
        txtAddress1.Font = new Font("Segoe UI", 10F);
        txtAddress1.Location = new Point(24, 198);
        txtAddress1.MaxLength = 200;
        txtAddress1.Name = "txtAddress1";
        txtAddress1.Size = new Size(590, 30);
        txtAddress1.TabIndex = 7;
        // 
        // lblAddress2
        // 
        lblAddress2.AutoSize = true;
        lblAddress2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAddress2.ForeColor = Color.FromArgb(45, 55, 70);
        lblAddress2.Location = new Point(24, 248);
        lblAddress2.Name = "lblAddress2";
        lblAddress2.Size = new Size(108, 20);
        lblAddress2.TabIndex = 8;
        lblAddress2.Text = "Address line 2";
        // 
        // txtAddress2
        // 
        txtAddress2.Font = new Font("Segoe UI", 10F);
        txtAddress2.Location = new Point(24, 274);
        txtAddress2.MaxLength = 200;
        txtAddress2.Name = "txtAddress2";
        txtAddress2.Size = new Size(590, 30);
        txtAddress2.TabIndex = 9;
        // 
        // lblCity
        // 
        lblCity.AutoSize = true;
        lblCity.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCity.ForeColor = Color.FromArgb(45, 55, 70);
        lblCity.Location = new Point(24, 324);
        lblCity.Name = "lblCity";
        lblCity.Size = new Size(47, 20);
        lblCity.TabIndex = 10;
        lblCity.Text = "City *";
        // 
        // txtCity
        // 
        txtCity.Font = new Font("Segoe UI", 10F);
        txtCity.Location = new Point(24, 350);
        txtCity.MaxLength = 100;
        txtCity.Name = "txtCity";
        txtCity.Size = new Size(300, 30);
        txtCity.TabIndex = 11;
        // 
        // lblState
        // 
        lblState.AutoSize = true;
        lblState.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblState.ForeColor = Color.FromArgb(45, 55, 70);
        lblState.Location = new Point(344, 324);
        lblState.Name = "lblState";
        lblState.Size = new Size(56, 20);
        lblState.TabIndex = 12;
        lblState.Text = "State *";
        // 
        // txtState
        // 
        txtState.CharacterCasing = CharacterCasing.Upper;
        txtState.Font = new Font("Segoe UI", 10F);
        txtState.Location = new Point(344, 350);
        txtState.MaxLength = 2;
        txtState.Name = "txtState";
        txtState.Size = new Size(105, 30);
        txtState.TabIndex = 13;
        // 
        // lblPostalCode
        // 
        lblPostalCode.AutoSize = true;
        lblPostalCode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPostalCode.ForeColor = Color.FromArgb(45, 55, 70);
        lblPostalCode.Location = new Point(469, 324);
        lblPostalCode.Name = "lblPostalCode";
        lblPostalCode.Size = new Size(100, 20);
        lblPostalCode.TabIndex = 14;
        lblPostalCode.Text = "Postal code *";
        // 
        // txtPostalCode
        // 
        txtPostalCode.Font = new Font("Segoe UI", 10F);
        txtPostalCode.Location = new Point(469, 350);
        txtPostalCode.MaxLength = 20;
        txtPostalCode.Name = "txtPostalCode";
        txtPostalCode.Size = new Size(145, 30);
        txtPostalCode.TabIndex = 15;
        // 
        // lblLatitude
        // 
        lblLatitude.AutoSize = true;
        lblLatitude.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLatitude.ForeColor = Color.FromArgb(45, 55, 70);
        lblLatitude.Location = new Point(24, 400);
        lblLatitude.Name = "lblLatitude";
        lblLatitude.Size = new Size(67, 20);
        lblLatitude.TabIndex = 16;
        lblLatitude.Text = "Latitude";
        // 
        // txtLatitude
        // 
        txtLatitude.Font = new Font("Segoe UI", 10F);
        txtLatitude.Location = new Point(24, 426);
        txtLatitude.Name = "txtLatitude";
        txtLatitude.Size = new Size(285, 30);
        txtLatitude.TabIndex = 17;
        // 
        // lblLongitude
        // 
        lblLongitude.AutoSize = true;
        lblLongitude.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLongitude.ForeColor = Color.FromArgb(45, 55, 70);
        lblLongitude.Location = new Point(329, 400);
        lblLongitude.Name = "lblLongitude";
        lblLongitude.Size = new Size(80, 20);
        lblLongitude.TabIndex = 18;
        lblLongitude.Text = "Longitude";
        // 
        // txtLongitude
        // 
        txtLongitude.Font = new Font("Segoe UI", 10F);
        txtLongitude.Location = new Point(329, 426);
        txtLongitude.Name = "txtLongitude";
        txtLongitude.Size = new Size(285, 30);
        txtLongitude.TabIndex = 19;
        // 
        // lblContactName
        // 
        lblContactName.AutoSize = true;
        lblContactName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblContactName.ForeColor = Color.FromArgb(45, 55, 70);
        lblContactName.Location = new Point(24, 476);
        lblContactName.Name = "lblContactName";
        lblContactName.Size = new Size(125, 20);
        lblContactName.TabIndex = 20;
        lblContactName.Text = "Location contact";
        // 
        // txtContactName
        // 
        txtContactName.Font = new Font("Segoe UI", 10F);
        txtContactName.Location = new Point(24, 502);
        txtContactName.MaxLength = 150;
        txtContactName.Name = "txtContactName";
        txtContactName.Size = new Size(285, 30);
        txtContactName.TabIndex = 21;
        // 
        // lblContactPhone
        // 
        lblContactPhone.AutoSize = true;
        lblContactPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblContactPhone.ForeColor = Color.FromArgb(45, 55, 70);
        lblContactPhone.Location = new Point(329, 476);
        lblContactPhone.Name = "lblContactPhone";
        lblContactPhone.Size = new Size(111, 20);
        lblContactPhone.TabIndex = 22;
        lblContactPhone.Text = "Contact phone";
        // 
        // txtContactPhone
        // 
        txtContactPhone.Font = new Font("Segoe UI", 10F);
        txtContactPhone.Location = new Point(329, 502);
        txtContactPhone.MaxLength = 40;
        txtContactPhone.Name = "txtContactPhone";
        txtContactPhone.Size = new Size(285, 30);
        txtContactPhone.TabIndex = 23;
        // 
        // chkBilling
        // 
        chkBilling.AutoSize = true;
        chkBilling.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        chkBilling.Location = new Point(24, 558);
        chkBilling.Name = "chkBilling";
        chkBilling.Size = new Size(316, 25);
        chkBilling.TabIndex = 24;
        chkBilling.Text = "Use as the customer's billing location";
        chkBilling.UseVisualStyleBackColor = true;
        // 
        // lblMessage
        // 
        lblMessage.ForeColor = Color.FromArgb(106, 116, 130);
        lblMessage.Location = new Point(36, 724);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(370, 28);
        lblMessage.TabIndex = 2;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.FromArgb(225, 229, 235);
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnCancel.ForeColor = Color.FromArgb(45, 55, 70);
        btnCancel.Location = new Point(430, 710);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 42);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(243, 108, 33);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(548, 710);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 42);
        btnSave.TabIndex = 4;
        btnSave.Text = "Save Location";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // CustomerLocationForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        CancelButton = btnCancel;
        ClientSize = new Size(710, 780);
        Controls.Add(lblTitle);
        Controls.Add(pnlFields);
        Controls.Add(lblMessage);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CustomerLocationForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FleetFlow — Customer Location";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
