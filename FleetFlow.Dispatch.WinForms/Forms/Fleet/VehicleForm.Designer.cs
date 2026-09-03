namespace FleetFlow.Dispatch.WinForms.Forms.Fleet;

partial class VehicleForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label lblTitle;
    private Panel pnlFields;

    private Label lblUnitNumber;
    private Label lblVin;
    private Label lblModelYear;
    private Label lblMake;
    private Label lblModel;
    private Label lblLicensePlate;
    private Label lblLicenseState;
    private Label lblMaxPayload;
    private Label lblOdometer;
    private Label lblStatus;

    private TextBox txtUnitNumber;
    private TextBox txtVin;
    private TextBox txtModelYear;
    private TextBox txtMake;
    private TextBox txtModel;
    private TextBox txtLicensePlate;
    private TextBox txtLicenseState;
    private TextBox txtMaxPayload;
    private TextBox txtOdometer;

    private ComboBox cboStatus;
    private Button btnSave;
    private Button btnCancel;

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
        lblTitle = new Label();
        pnlFields = new Panel();

        lblUnitNumber = new Label();
        lblVin = new Label();
        lblModelYear = new Label();
        lblMake = new Label();
        lblModel = new Label();
        lblLicensePlate = new Label();
        lblLicenseState = new Label();
        lblMaxPayload = new Label();
        lblOdometer = new Label();
        lblStatus = new Label();

        txtUnitNumber = new TextBox();
        txtVin = new TextBox();
        txtModelYear = new TextBox();
        txtMake = new TextBox();
        txtModel = new TextBox();
        txtLicensePlate = new TextBox();
        txtLicenseState = new TextBox();
        txtMaxPayload = new TextBox();
        txtOdometer = new TextBox();

        cboStatus = new ComboBox();
        btnSave = new Button();
        btnCancel = new Button();

        pnlFields.SuspendLayout();
        SuspendLayout();

        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font(
            "Segoe UI",
            22F,
            FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(
            29,
            39,
            54);
        lblTitle.Location = new Point(22, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(151, 50);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Vehicle";

        // 
        // pnlFields
        // 
        pnlFields.BackColor = Color.White;
        pnlFields.Controls.Add(lblUnitNumber);
        pnlFields.Controls.Add(txtUnitNumber);
        pnlFields.Controls.Add(lblVin);
        pnlFields.Controls.Add(txtVin);
        pnlFields.Controls.Add(lblModelYear);
        pnlFields.Controls.Add(txtModelYear);
        pnlFields.Controls.Add(lblMake);
        pnlFields.Controls.Add(txtMake);
        pnlFields.Controls.Add(lblModel);
        pnlFields.Controls.Add(txtModel);
        pnlFields.Controls.Add(lblLicensePlate);
        pnlFields.Controls.Add(txtLicensePlate);
        pnlFields.Controls.Add(lblLicenseState);
        pnlFields.Controls.Add(txtLicenseState);
        pnlFields.Controls.Add(lblMaxPayload);
        pnlFields.Controls.Add(txtMaxPayload);
        pnlFields.Controls.Add(lblOdometer);
        pnlFields.Controls.Add(txtOdometer);
        pnlFields.Controls.Add(lblStatus);
        pnlFields.Controls.Add(cboStatus);
        pnlFields.Location = new Point(16, 70);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(588, 310);
        pnlFields.TabIndex = 1;

        // 
        // lblUnitNumber
        // 
        lblUnitNumber.AutoSize = true;
        lblUnitNumber.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblUnitNumber.Location = new Point(18, 14);
        lblUnitNumber.Name = "lblUnitNumber";
        lblUnitNumber.Size = new Size(115, 20);
        lblUnitNumber.TabIndex = 0;
        lblUnitNumber.Text = "Unit number *";

        // 
        // txtUnitNumber
        // 
        txtUnitNumber.Location = new Point(18, 38);
        txtUnitNumber.Name = "txtUnitNumber";
        txtUnitNumber.Size = new Size(270, 27);
        txtUnitNumber.TabIndex = 1;

        // 
        // lblVin
        // 
        lblVin.AutoSize = true;
        lblVin.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblVin.Location = new Point(312, 14);
        lblVin.Name = "lblVin";
        lblVin.Size = new Size(45, 20);
        lblVin.TabIndex = 2;
        lblVin.Text = "VIN *";

        // 
        // txtVin
        // 
        txtVin.CharacterCasing = CharacterCasing.Upper;
        txtVin.Location = new Point(312, 38);
        txtVin.MaxLength = 17;
        txtVin.Name = "txtVin";
        txtVin.Size = new Size(258, 27);
        txtVin.TabIndex = 3;

        // 
        // lblModelYear
        // 
        lblModelYear.AutoSize = true;
        lblModelYear.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblModelYear.Location = new Point(18, 84);
        lblModelYear.Name = "lblModelYear";
        lblModelYear.Size = new Size(103, 20);
        lblModelYear.TabIndex = 4;
        lblModelYear.Text = "Model year *";

        // 
        // txtModelYear
        // 
        txtModelYear.Location = new Point(18, 108);
        txtModelYear.Name = "txtModelYear";
        txtModelYear.Size = new Size(170, 27);
        txtModelYear.TabIndex = 5;

        // 
        // lblMake
        // 
        lblMake.AutoSize = true;
        lblMake.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblMake.Location = new Point(212, 84);
        lblMake.Name = "lblMake";
        lblMake.Size = new Size(59, 20);
        lblMake.TabIndex = 6;
        lblMake.Text = "Make *";

        // 
        // txtMake
        // 
        txtMake.Location = new Point(212, 108);
        txtMake.Name = "txtMake";
        txtMake.Size = new Size(180, 27);
        txtMake.TabIndex = 7;

        // 
        // lblModel
        // 
        lblModel.AutoSize = true;
        lblModel.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblModel.Location = new Point(416, 84);
        lblModel.Name = "lblModel";
        lblModel.Size = new Size(66, 20);
        lblModel.TabIndex = 8;
        lblModel.Text = "Model *";

        // 
        // txtModel
        // 
        txtModel.Location = new Point(416, 108);
        txtModel.Name = "txtModel";
        txtModel.Size = new Size(154, 27);
        txtModel.TabIndex = 9;

        // 
        // lblLicensePlate
        // 
        lblLicensePlate.AutoSize = true;
        lblLicensePlate.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblLicensePlate.Location = new Point(18, 154);
        lblLicensePlate.Name = "lblLicensePlate";
        lblLicensePlate.Size = new Size(112, 20);
        lblLicensePlate.TabIndex = 10;
        lblLicensePlate.Text = "License plate *";

        // 
        // txtLicensePlate
        // 
        txtLicensePlate.CharacterCasing =
            CharacterCasing.Upper;
        txtLicensePlate.Location = new Point(18, 178);
        txtLicensePlate.Name = "txtLicensePlate";
        txtLicensePlate.Size = new Size(270, 27);
        txtLicensePlate.TabIndex = 11;

        // 
        // lblLicenseState
        // 
        lblLicenseState.AutoSize = true;
        lblLicenseState.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblLicenseState.Location = new Point(312, 154);
        lblLicenseState.Name = "lblLicenseState";
        lblLicenseState.Size = new Size(96, 20);
        lblLicenseState.TabIndex = 12;
        lblLicenseState.Text = "Plate state *";

        // 
        // txtLicenseState
        // 
        txtLicenseState.CharacterCasing =
            CharacterCasing.Upper;
        txtLicenseState.Location = new Point(312, 178);
        txtLicenseState.MaxLength = 2;
        txtLicenseState.Name = "txtLicenseState";
        txtLicenseState.Size = new Size(120, 27);
        txtLicenseState.TabIndex = 13;

        // 
        // lblMaxPayload
        // 
        lblMaxPayload.AutoSize = true;
        lblMaxPayload.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblMaxPayload.Location = new Point(456, 154);
        lblMaxPayload.Name = "lblMaxPayload";
        lblMaxPayload.Size = new Size(111, 20);
        lblMaxPayload.TabIndex = 14;
        lblMaxPayload.Text = "Max payload *";

        // 
        // txtMaxPayload
        // 
        txtMaxPayload.Location = new Point(456, 178);
        txtMaxPayload.Name = "txtMaxPayload";
        txtMaxPayload.Size = new Size(114, 27);
        txtMaxPayload.TabIndex = 15;

        // 
        // lblOdometer
        // 
        lblOdometer.AutoSize = true;
        lblOdometer.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblOdometer.Location = new Point(18, 224);
        lblOdometer.Name = "lblOdometer";
        lblOdometer.Size = new Size(129, 20);
        lblOdometer.TabIndex = 16;
        lblOdometer.Text = "Odometer miles *";

        // 
        // txtOdometer
        // 
        txtOdometer.Location = new Point(18, 248);
        txtOdometer.Name = "txtOdometer";
        txtOdometer.Size = new Size(270, 27);
        txtOdometer.TabIndex = 17;

        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblStatus.Location = new Point(312, 224);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(161, 20);
        lblStatus.TabIndex = 18;
        lblStatus.Text = "Operational status *";

        // 
        // cboStatus
        // 
        cboStatus.DropDownStyle =
            ComboBoxStyle.DropDownList;
        cboStatus.FormattingEnabled = true;
        cboStatus.Location = new Point(312, 248);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(258, 28);
        cboStatus.TabIndex = 19;

        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(365, 400);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 38);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;

        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(
            243,
            108,
            33);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(474, 400);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(130, 38);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save Vehicle";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;

        // 
        // VehicleForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        CancelButton = btnCancel;
        ClientSize = new Size(620, 460);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
        Controls.Add(pnlFields);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "VehicleForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FleetFlow — Vehicle";

        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}