namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

partial class CustomerLocationForm
{
    private System.ComponentModel.IContainer? components;
    private Label lblTitle = null!;
    private Panel pnlFields = null!;
    private TextBox txtLocationCode = null!;
    private ComboBox cboLocationType = null!;
    private TextBox txtLocationName = null!;
    private TextBox txtAddress1 = null!;
    private TextBox txtAddress2 = null!;
    private TextBox txtCity = null!;
    private TextBox txtState = null!;
    private TextBox txtPostalCode = null!;
    private TextBox txtLatitude = null!;
    private TextBox txtLongitude = null!;
    private TextBox txtContactName = null!;
    private TextBox txtContactPhone = null!;
    private CheckBox chkBilling = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;
    private Label lblMessage = null!;
    private ErrorProvider errorProvider = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblTitle = new Label();
        pnlFields = new Panel();
        txtLocationCode = TextField(pnlFields, "Location code *", 24, 20, 285);
        cboLocationType = ComboField(pnlFields, "Location type *", 329, 20, 285);
        txtLocationName = TextField(pnlFields, "Location name *", 24, 96, 590);
        txtAddress1 = TextField(pnlFields, "Address line 1 *", 24, 172, 590);
        txtAddress2 = TextField(pnlFields, "Address line 2", 24, 248, 590);
        txtCity = TextField(pnlFields, "City *", 24, 324, 300);
        txtState = TextField(pnlFields, "State *", 344, 324, 105);
        txtPostalCode = TextField(pnlFields, "Postal code *", 469, 324, 145);
        txtLatitude = TextField(pnlFields, "Latitude", 24, 400, 285);
        txtLongitude = TextField(pnlFields, "Longitude", 329, 400, 285);
        txtContactName = TextField(pnlFields, "Location contact", 24, 476, 285);
        txtContactPhone = TextField(pnlFields, "Contact phone", 329, 476, 285);
        chkBilling = new CheckBox
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Location = new Point(24, 558),
            Text = "Use as the customer's billing location"
        };
        pnlFields.Controls.Add(chkBilling);
        btnSave = new Button();
        btnCancel = new Button();
        lblMessage = new Label();
        errorProvider = new ErrorProvider(components);
        SuspendLayout();

        BackColor = Color.FromArgb(244, 246, 249);
        ClientSize = new Size(710, 780);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(34, 22);
        lblTitle.Text = "Location";

        pnlFields.BackColor = Color.White;
        pnlFields.Location = new Point(34, 82);
        pnlFields.Size = new Size(642, 610);

        lblMessage.ForeColor = Color.FromArgb(106, 116, 130);
        lblMessage.Location = new Point(36, 724);
        lblMessage.Size = new Size(300, 28);

        ButtonStyle(btnCancel, "Cancel", Color.FromArgb(225, 229, 235), Color.FromArgb(45, 55, 70));
        btnCancel.Location = new Point(430, 710);
        ButtonStyle(btnSave, "Save Location", Color.FromArgb(243, 108, 33), Color.White);
        btnSave.Location = new Point(548, 710);
        btnSave.Size = new Size(128, 42);

        AcceptButton = btnSave;
        CancelButton = btnCancel;
        Controls.AddRange([lblTitle, pnlFields, lblMessage, btnCancel, btnSave]);
        ResumeLayout(false);
        PerformLayout();
    }

    private static TextBox TextField(Panel panel, string caption, int left, int top, int width)
    {
        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(left, top),
            Text = caption
        });
        var control = new TextBox
        {
            Font = new Font("Segoe UI", 10F),
            Location = new Point(left, top + 26),
            Size = new Size(width, 30)
        };
        panel.Controls.Add(control);
        return control;
    }

    private static ComboBox ComboField(Panel panel, string caption, int left, int top, int width)
    {
        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(left, top),
            Text = caption
        });
        var control = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F),
            Location = new Point(left, top + 26),
            Size = new Size(width, 31)
        };
        panel.Controls.Add(control);
        return control;
    }

    private static void ButtonStyle(Button button, string text, Color back, Color fore)
    {
        button.BackColor = back;
        button.FlatAppearance.BorderSize = 0;
        button.FlatStyle = FlatStyle.Flat;
        button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        button.ForeColor = fore;
        button.Size = new Size(100, 42);
        button.Text = text;
        button.UseVisualStyleBackColor = false;
    }
}
