namespace FleetFlow.Dispatch.WinForms.Forms.Fleet;

partial class TrailerForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label lblTitle = null!;
    private Panel pnlFields = null!;
    private Label lblUnitNumber = null!;
    private Label lblVin = null!;
    private Label lblTrailerType = null!;
    private Label lblLicensePlate = null!;
    private Label lblLicenseState = null!;
    private Label lblMaxPayload = null!;
    private Label lblStatus = null!;
    private TextBox txtUnitNumber = null!;
    private TextBox txtVin = null!;
    private ComboBox cboTrailerType = null!;
    private TextBox txtLicensePlate = null!;
    private TextBox txtLicenseState = null!;
    private TextBox txtMaxPayload = null!;
    private ComboBox cboStatus = null!;
    private CheckBox chkActive = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;

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
        lblUnitNumber = new Label();
        txtUnitNumber = new TextBox();
        lblVin = new Label();
        txtVin = new TextBox();
        lblTrailerType = new Label();
        cboTrailerType = new ComboBox();
        lblLicensePlate = new Label();
        txtLicensePlate = new TextBox();
        lblLicenseState = new Label();
        txtLicenseState = new TextBox();
        lblMaxPayload = new Label();
        txtMaxPayload = new TextBox();
        lblStatus = new Label();
        cboStatus = new ComboBox();
        chkActive = new CheckBox();
        btnCancel = new Button();
        btnSave = new Button();
        pnlFields.SuspendLayout();
        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(22, 15);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(96, 41);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Trailer";

        // pnlFields
        pnlFields.BackColor = Color.White;
        pnlFields.Controls.Add(lblUnitNumber);
        pnlFields.Controls.Add(txtUnitNumber);
        pnlFields.Controls.Add(lblVin);
        pnlFields.Controls.Add(txtVin);
        pnlFields.Controls.Add(lblTrailerType);
        pnlFields.Controls.Add(cboTrailerType);
        pnlFields.Controls.Add(lblLicensePlate);
        pnlFields.Controls.Add(txtLicensePlate);
        pnlFields.Controls.Add(lblLicenseState);
        pnlFields.Controls.Add(txtLicenseState);
        pnlFields.Controls.Add(lblMaxPayload);
        pnlFields.Controls.Add(txtMaxPayload);
        pnlFields.Controls.Add(lblStatus);
        pnlFields.Controls.Add(cboStatus);
        pnlFields.Controls.Add(chkActive);
        pnlFields.Location = new Point(14, 62);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(576, 310);
        pnlFields.TabIndex = 1;

        // lblUnitNumber
        lblUnitNumber.AutoSize = true;
        lblUnitNumber.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUnitNumber.Location = new Point(18, 14);
        lblUnitNumber.Name = "lblUnitNumber";
        lblUnitNumber.Size = new Size(106, 20);
        lblUnitNumber.TabIndex = 0;
        lblUnitNumber.Text = "Unit number *";

        // txtUnitNumber
        txtUnitNumber.Location = new Point(18, 38);
        txtUnitNumber.Name = "txtUnitNumber";
        txtUnitNumber.Size = new Size(264, 27);
        txtUnitNumber.TabIndex = 1;

        // lblVin
        lblVin.AutoSize = true;
        lblVin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblVin.Location = new Point(306, 14);
        lblVin.Name = "lblVin";
        lblVin.Size = new Size(157, 20);
        lblVin.TabIndex = 2;
        lblVin.Text = "VIN (17 characters) *";

        // txtVin
        txtVin.CharacterCasing = CharacterCasing.Upper;
        txtVin.Location = new Point(306, 38);
        txtVin.MaxLength = 17;
        txtVin.Name = "txtVin";
        txtVin.Size = new Size(252, 27);
        txtVin.TabIndex = 3;

        // lblTrailerType
        lblTrailerType.AutoSize = true;
        lblTrailerType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTrailerType.Location = new Point(18, 84);
        lblTrailerType.Name = "lblTrailerType";
        lblTrailerType.Size = new Size(101, 20);
        lblTrailerType.TabIndex = 4;
        lblTrailerType.Text = "Trailer type *";

        // cboTrailerType
        cboTrailerType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrailerType.FormattingEnabled = true;
        cboTrailerType.Location = new Point(18, 108);
        cboTrailerType.Name = "cboTrailerType";
        cboTrailerType.Size = new Size(264, 28);
        cboTrailerType.TabIndex = 5;

        // lblLicensePlate
        lblLicensePlate.AutoSize = true;
        lblLicensePlate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLicensePlate.Location = new Point(306, 84);
        lblLicensePlate.Name = "lblLicensePlate";
        lblLicensePlate.Size = new Size(112, 20);
        lblLicensePlate.TabIndex = 6;
        lblLicensePlate.Text = "License plate *";

        // txtLicensePlate
        txtLicensePlate.CharacterCasing = CharacterCasing.Upper;
        txtLicensePlate.Location = new Point(306, 108);
        txtLicensePlate.Name = "txtLicensePlate";
        txtLicensePlate.Size = new Size(140, 27);
        txtLicensePlate.TabIndex = 7;

        // lblLicenseState
        lblLicenseState.AutoSize = true;
        lblLicenseState.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLicenseState.Location = new Point(470, 84);
        lblLicenseState.Name = "lblLicenseState";
        lblLicenseState.Size = new Size(92, 20);
        lblLicenseState.TabIndex = 8;
        lblLicenseState.Text = "Plate state *";

        // txtLicenseState
        txtLicenseState.CharacterCasing = CharacterCasing.Upper;
        txtLicenseState.Location = new Point(470, 108);
        txtLicenseState.MaxLength = 2;
        txtLicenseState.Name = "txtLicenseState";
        txtLicenseState.Size = new Size(88, 27);
        txtLicenseState.TabIndex = 9;

        // lblMaxPayload
        lblMaxPayload.AutoSize = true;
        lblMaxPayload.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMaxPayload.Location = new Point(18, 154);
        lblMaxPayload.Name = "lblMaxPayload";
        lblMaxPayload.Size = new Size(143, 20);
        lblMaxPayload.TabIndex = 10;
        lblMaxPayload.Text = "Max payload lbs *";

        // txtMaxPayload
        txtMaxPayload.Location = new Point(18, 178);
        txtMaxPayload.Name = "txtMaxPayload";
        txtMaxPayload.Size = new Size(264, 27);
        txtMaxPayload.TabIndex = 11;

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStatus.Location = new Point(306, 154);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(151, 20);
        lblStatus.TabIndex = 12;
        lblStatus.Text = "Operational status *";

        // cboStatus
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.FormattingEnabled = true;
        cboStatus.Location = new Point(306, 178);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(252, 28);
        cboStatus.TabIndex = 13;

        // chkActive
        chkActive.AutoSize = true;
        chkActive.Checked = true;
        chkActive.CheckState = CheckState.Checked;
        chkActive.Location = new Point(18, 232);
        chkActive.Name = "chkActive";
        chkActive.Size = new Size(116, 24);
        chkActive.TabIndex = 14;
        chkActive.Text = "Active trailer";
        chkActive.UseVisualStyleBackColor = true;

        // btnCancel
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(337, 394);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 38);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;

        // btnSave
        btnSave.BackColor = Color.FromArgb(243, 108, 33);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(446, 394);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(130, 38);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save Trailer";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;

        // TrailerForm
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        CancelButton = btnCancel;
        ClientSize = new Size(604, 455);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
        Controls.Add(pnlFields);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TrailerForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FleetFlow — Trailer";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}