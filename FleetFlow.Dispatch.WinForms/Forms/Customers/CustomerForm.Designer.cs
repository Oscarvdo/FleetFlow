namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

partial class CustomerForm
{
    private System.ComponentModel.IContainer? components;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Panel pnlFields = null!;
    private TextBox txtCustomerNumber = null!;
    private TextBox txtCompanyName = null!;
    private TextBox txtContactName = null!;
    private TextBox txtEmail = null!;
    private TextBox txtPhone = null!;
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
        lblSubtitle = new Label();
        pnlFields = new Panel();
        txtCustomerNumber = AddField(pnlFields, "Customer number *", 22);
        txtCompanyName = AddField(pnlFields, "Company name *", 100);
        txtContactName = AddField(pnlFields, "Primary contact", 178);
        txtEmail = AddField(pnlFields, "Email", 256);
        txtPhone = AddField(pnlFields, "Phone", 334);
        btnSave = new Button();
        btnCancel = new Button();
        lblMessage = new Label();
        errorProvider = new ErrorProvider(components);
        SuspendLayout();

        BackColor = Color.FromArgb(244, 246, 249);
        ClientSize = new Size(700, 640);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(34, 25);
        lblTitle.Text = "Customer";

        lblSubtitle.AutoSize = true;
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(38, 80);
        lblSubtitle.Text = "Customer account information.";

        pnlFields.BackColor = Color.White;
        pnlFields.Location = new Point(38, 120);
        pnlFields.Size = new Size(624, 430);

        lblMessage.ForeColor = Color.FromArgb(106, 116, 130);
        lblMessage.Location = new Point(38, 574);
        lblMessage.Size = new Size(300, 30);

        StyleButton(btnCancel, "Cancel", Color.FromArgb(225, 229, 235), Color.FromArgb(45, 55, 70));
        btnCancel.Location = new Point(412, 566);
        StyleButton(btnSave, "Save Customer", Color.FromArgb(243, 108, 33), Color.White);
        btnSave.Location = new Point(524, 566);
        btnSave.Size = new Size(138, 42);

        AcceptButton = btnSave;
        CancelButton = btnCancel;
        Controls.AddRange([lblTitle, lblSubtitle, pnlFields, lblMessage, btnCancel, btnSave]);
        ResumeLayout(false);
        PerformLayout();
    }

    private static TextBox AddField(Panel panel, string caption, int top)
    {
        var label = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(24, top),
            Text = caption
        };
        var textBox = new TextBox
        {
            Font = new Font("Segoe UI", 10F),
            Location = new Point(24, top + 27),
            Size = new Size(576, 30)
        };
        panel.Controls.Add(label);
        panel.Controls.Add(textBox);
        return textBox;
    }

    private static void StyleButton(Button button, string text, Color backColor, Color foreColor)
    {
        button.BackColor = backColor;
        button.FlatAppearance.BorderSize = 0;
        button.FlatStyle = FlatStyle.Flat;
        button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        button.ForeColor = foreColor;
        button.Size = new Size(100, 42);
        button.Text = text;
        button.UseVisualStyleBackColor = false;
    }
}
