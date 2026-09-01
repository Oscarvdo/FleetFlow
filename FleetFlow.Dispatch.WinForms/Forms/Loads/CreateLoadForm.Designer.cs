namespace FleetFlow.Dispatch.WinForms.Forms.Loads;

partial class CreateLoadForm
{
    private System.ComponentModel.IContainer? components;

    private Panel pnlHeader;
    private Label lblTitle;
    private Label lblSubtitle;

    private Panel pnlBody;
    private TableLayoutPanel tlpForm;

    private Label lblLoadNumber;
    private TextBox txtLoadNumber;

    private Label lblCustomer;
    private Panel pnlCustomer;
    private ComboBox cboCustomer;
    private Button btnRefreshCustomers;

    private Label lblDescription;
    private TextBox txtDescription;

    private Label lblCommodity;
    private TextBox txtCommodity;

    private Label lblWeight;
    private NumericUpDown numWeight;

    private Label lblPieces;
    private Panel pnlPieces;
    private CheckBox chkPieces;
    private NumericUpDown numPieces;

    private Label lblRevenue;
    private Panel pnlRevenue;
    private CheckBox chkRevenue;
    private NumericUpDown numRevenue;

    private Label lblStatus;
    private ComboBox cboStatus;

    private Label lblSpecialInstructions;
    private TextBox txtSpecialInstructions;

    private Panel pnlFooter;
    private Label lblMessage;
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
        pnlHeader = new Panel();
        lblTitle = new Label();
        lblSubtitle = new Label();
        pnlBody = new Panel();
        tlpForm = new TableLayoutPanel();
        lblLoadNumber = new Label();
        txtLoadNumber = new TextBox();
        lblCustomer = new Label();
        pnlCustomer = new Panel();
        cboCustomer = new ComboBox();
        btnRefreshCustomers = new Button();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblCommodity = new Label();
        txtCommodity = new TextBox();
        lblWeight = new Label();
        numWeight = new NumericUpDown();
        lblPieces = new Label();
        pnlPieces = new Panel();
        numPieces = new NumericUpDown();
        chkPieces = new CheckBox();
        lblRevenue = new Label();
        pnlRevenue = new Panel();
        numRevenue = new NumericUpDown();
        chkRevenue = new CheckBox();
        lblStatus = new Label();
        cboStatus = new ComboBox();
        lblSpecialInstructions = new Label();
        txtSpecialInstructions = new TextBox();
        pnlFooter = new Panel();
        lblMessage = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        pnlHeader.SuspendLayout();
        pnlBody.SuspendLayout();
        tlpForm.SuspendLayout();
        pnlCustomer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numWeight).BeginInit();
        pnlPieces.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPieces).BeginInit();
        pnlRevenue.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numRevenue).BeginInit();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(32, 22, 32, 18);
        pnlHeader.Size = new Size(850, 108);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(29, 39, 54);
        lblTitle.Location = new Point(28, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(208, 46);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Create Load";
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(106, 116, 130);
        lblSubtitle.Location = new Point(32, 67);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(384, 23);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Enter the shipment information for the new load.";
        // 
        // pnlBody
        // 
        pnlBody.AutoScroll = true;
        pnlBody.BackColor = Color.FromArgb(244, 246, 249);
        pnlBody.Controls.Add(tlpForm);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Location = new Point(0, 108);
        pnlBody.Name = "pnlBody";
        pnlBody.Padding = new Padding(32, 24, 32, 24);
        pnlBody.Size = new Size(850, 592);
        pnlBody.TabIndex = 1;
        // 
        // tlpForm
        // 
        tlpForm.AutoSize = true;
        tlpForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpForm.BackColor = Color.White;
        tlpForm.ColumnCount = 2;
        tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185F));
        tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpForm.Controls.Add(lblLoadNumber, 0, 0);
        tlpForm.Controls.Add(txtLoadNumber, 1, 0);
        tlpForm.Controls.Add(lblCustomer, 0, 1);
        tlpForm.Controls.Add(pnlCustomer, 1, 1);
        tlpForm.Controls.Add(lblDescription, 0, 2);
        tlpForm.Controls.Add(txtDescription, 1, 2);
        tlpForm.Controls.Add(lblCommodity, 0, 3);
        tlpForm.Controls.Add(txtCommodity, 1, 3);
        tlpForm.Controls.Add(lblWeight, 0, 4);
        tlpForm.Controls.Add(numWeight, 1, 4);
        tlpForm.Controls.Add(lblPieces, 0, 5);
        tlpForm.Controls.Add(pnlPieces, 1, 5);
        tlpForm.Controls.Add(lblRevenue, 0, 6);
        tlpForm.Controls.Add(pnlRevenue, 1, 6);
        tlpForm.Controls.Add(lblStatus, 0, 7);
        tlpForm.Controls.Add(cboStatus, 1, 7);
        tlpForm.Controls.Add(lblSpecialInstructions, 0, 8);
        tlpForm.Controls.Add(txtSpecialInstructions, 1, 8);
        tlpForm.Dock = DockStyle.Top;
        tlpForm.Location = new Point(32, 24);
        tlpForm.Name = "tlpForm";
        tlpForm.Padding = new Padding(24, 18, 24, 20);
        tlpForm.RowCount = 9;
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.RowStyles.Add(new RowStyle());
        tlpForm.Size = new Size(786, 525);
        tlpForm.TabIndex = 0;
        // 
        // lblLoadNumber
        // 
        lblLoadNumber.AutoSize = true;
        lblLoadNumber.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblLoadNumber.ForeColor = Color.FromArgb(45, 55, 70);
        lblLoadNumber.Location = new Point(27, 25);
        lblLoadNumber.Margin = new Padding(3, 7, 12, 12);
        lblLoadNumber.Name = "lblLoadNumber";
        lblLoadNumber.Size = new Size(122, 21);
        lblLoadNumber.TabIndex = 0;
        lblLoadNumber.Text = "Load number *";
        // 
        // txtLoadNumber
        // 
        txtLoadNumber.CharacterCasing = CharacterCasing.Upper;
        txtLoadNumber.Dock = DockStyle.Fill;
        txtLoadNumber.Font = new Font("Segoe UI", 10F);
        txtLoadNumber.Location = new Point(212, 21);
        txtLoadNumber.Margin = new Padding(3, 3, 3, 12);
        txtLoadNumber.MaxLength = 30;
        txtLoadNumber.Name = "txtLoadNumber";
        txtLoadNumber.PlaceholderText = "Example: LD-2026-0002";
        txtLoadNumber.Size = new Size(547, 30);
        txtLoadNumber.TabIndex = 0;
        // 
        // lblCustomer
        // 
        lblCustomer.AutoSize = true;
        lblCustomer.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblCustomer.ForeColor = Color.FromArgb(45, 55, 70);
        lblCustomer.Location = new Point(27, 70);
        lblCustomer.Margin = new Padding(3, 7, 12, 12);
        lblCustomer.Name = "lblCustomer";
        lblCustomer.Size = new Size(94, 21);
        lblCustomer.TabIndex = 1;
        lblCustomer.Text = "Customer *";
        // 
        // pnlCustomer
        // 
        pnlCustomer.Controls.Add(cboCustomer);
        pnlCustomer.Controls.Add(btnRefreshCustomers);
        pnlCustomer.Dock = DockStyle.Fill;
        pnlCustomer.Location = new Point(212, 66);
        pnlCustomer.Margin = new Padding(3, 3, 3, 12);
        pnlCustomer.Name = "pnlCustomer";
        pnlCustomer.Size = new Size(547, 34);
        pnlCustomer.TabIndex = 1;
        // 
        // cboCustomer
        // 
        cboCustomer.Dock = DockStyle.Fill;
        cboCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCustomer.Font = new Font("Segoe UI", 10F);
        cboCustomer.FormattingEnabled = true;
        cboCustomer.Location = new Point(0, 0);
        cboCustomer.Name = "cboCustomer";
        cboCustomer.Size = new Size(443, 31);
        cboCustomer.TabIndex = 0;
        // 
        // btnRefreshCustomers
        // 
        btnRefreshCustomers.BackColor = Color.FromArgb(233, 237, 242);
        btnRefreshCustomers.Cursor = Cursors.Hand;
        btnRefreshCustomers.Dock = DockStyle.Right;
        btnRefreshCustomers.FlatAppearance.BorderColor = Color.FromArgb(205, 211, 220);
        btnRefreshCustomers.FlatStyle = FlatStyle.Flat;
        btnRefreshCustomers.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnRefreshCustomers.ForeColor = Color.FromArgb(45, 55, 70);
        btnRefreshCustomers.Location = new Point(443, 0);
        btnRefreshCustomers.Name = "btnRefreshCustomers";
        btnRefreshCustomers.Size = new Size(104, 34);
        btnRefreshCustomers.TabIndex = 1;
        btnRefreshCustomers.Text = "Refresh";
        btnRefreshCustomers.UseVisualStyleBackColor = false;
        // 
        // lblDescription
        // 
        lblDescription.AutoSize = true;
        lblDescription.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblDescription.ForeColor = Color.FromArgb(45, 55, 70);
        lblDescription.Location = new Point(27, 119);
        lblDescription.Margin = new Padding(3, 7, 12, 12);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(109, 21);
        lblDescription.TabIndex = 2;
        lblDescription.Text = "Description *";
        // 
        // txtDescription
        // 
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Font = new Font("Segoe UI", 10F);
        txtDescription.Location = new Point(212, 115);
        txtDescription.Margin = new Padding(3, 3, 3, 12);
        txtDescription.MaxLength = 300;
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.PlaceholderText = "Describe the shipment";
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(547, 62);
        txtDescription.TabIndex = 2;
        // 
        // lblCommodity
        // 
        lblCommodity.AutoSize = true;
        lblCommodity.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblCommodity.ForeColor = Color.FromArgb(45, 55, 70);
        lblCommodity.Location = new Point(27, 196);
        lblCommodity.Margin = new Padding(3, 7, 12, 12);
        lblCommodity.Name = "lblCommodity";
        lblCommodity.Size = new Size(100, 21);
        lblCommodity.TabIndex = 3;
        lblCommodity.Text = "Commodity";
        // 
        // txtCommodity
        // 
        txtCommodity.Dock = DockStyle.Fill;
        txtCommodity.Font = new Font("Segoe UI", 10F);
        txtCommodity.Location = new Point(212, 192);
        txtCommodity.Margin = new Padding(3, 3, 3, 12);
        txtCommodity.MaxLength = 100;
        txtCommodity.Name = "txtCommodity";
        txtCommodity.PlaceholderText = "Example: Dry packaged food";
        txtCommodity.Size = new Size(547, 30);
        txtCommodity.TabIndex = 3;
        // 
        // lblWeight
        // 
        lblWeight.AutoSize = true;
        lblWeight.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblWeight.ForeColor = Color.FromArgb(45, 55, 70);
        lblWeight.Location = new Point(27, 241);
        lblWeight.Margin = new Padding(3, 7, 12, 12);
        lblWeight.Name = "lblWeight";
        lblWeight.Size = new Size(108, 21);
        lblWeight.TabIndex = 4;
        lblWeight.Text = "Weight (lb) *";
        // 
        // numWeight
        // 
        numWeight.DecimalPlaces = 2;
        numWeight.Font = new Font("Segoe UI", 10F);
        numWeight.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        numWeight.Location = new Point(212, 237);
        numWeight.Margin = new Padding(3, 3, 3, 12);
        numWeight.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numWeight.Name = "numWeight";
        numWeight.Size = new Size(220, 30);
        numWeight.TabIndex = 4;
        numWeight.TextAlign = HorizontalAlignment.Right;
        numWeight.ThousandsSeparator = true;
        // 
        // lblPieces
        // 
        lblPieces.AutoSize = true;
        lblPieces.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPieces.ForeColor = Color.FromArgb(45, 55, 70);
        lblPieces.Location = new Point(27, 286);
        lblPieces.Margin = new Padding(3, 7, 12, 12);
        lblPieces.Name = "lblPieces";
        lblPieces.Size = new Size(58, 21);
        lblPieces.TabIndex = 5;
        lblPieces.Text = "Pieces";
        // 
        // pnlPieces
        // 
        pnlPieces.Controls.Add(numPieces);
        pnlPieces.Controls.Add(chkPieces);
        pnlPieces.Dock = DockStyle.Fill;
        pnlPieces.Location = new Point(212, 282);
        pnlPieces.Margin = new Padding(3, 3, 3, 12);
        pnlPieces.Name = "pnlPieces";
        pnlPieces.Size = new Size(547, 34);
        pnlPieces.TabIndex = 5;
        // 
        // numPieces
        // 
        numPieces.Enabled = false;
        numPieces.Font = new Font("Segoe UI", 10F);
        numPieces.Location = new Point(112, 1);
        numPieces.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numPieces.Name = "numPieces";
        numPieces.Size = new Size(180, 30);
        numPieces.TabIndex = 1;
        numPieces.TextAlign = HorizontalAlignment.Right;
        numPieces.ThousandsSeparator = true;
        // 
        // chkPieces
        // 
        chkPieces.AutoSize = true;
        chkPieces.Font = new Font("Segoe UI", 9.5F);
        chkPieces.Location = new Point(0, 5);
        chkPieces.Name = "chkPieces";
        chkPieces.Size = new Size(82, 25);
        chkPieces.TabIndex = 0;
        chkPieces.Text = "Specify";
        chkPieces.UseVisualStyleBackColor = true;
        // 
        // lblRevenue
        // 
        lblRevenue.AutoSize = true;
        lblRevenue.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblRevenue.ForeColor = Color.FromArgb(45, 55, 70);
        lblRevenue.Location = new Point(27, 335);
        lblRevenue.Margin = new Padding(3, 7, 12, 12);
        lblRevenue.Name = "lblRevenue";
        lblRevenue.Size = new Size(76, 21);
        lblRevenue.TabIndex = 6;
        lblRevenue.Text = "Revenue";
        // 
        // pnlRevenue
        // 
        pnlRevenue.Controls.Add(numRevenue);
        pnlRevenue.Controls.Add(chkRevenue);
        pnlRevenue.Dock = DockStyle.Fill;
        pnlRevenue.Location = new Point(212, 331);
        pnlRevenue.Margin = new Padding(3, 3, 3, 12);
        pnlRevenue.Name = "pnlRevenue";
        pnlRevenue.Size = new Size(547, 34);
        pnlRevenue.TabIndex = 6;
        // 
        // numRevenue
        // 
        numRevenue.DecimalPlaces = 2;
        numRevenue.Enabled = false;
        numRevenue.Font = new Font("Segoe UI", 10F);
        numRevenue.Increment = new decimal(new int[] { 50, 0, 0, 0 });
        numRevenue.Location = new Point(112, 1);
        numRevenue.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numRevenue.Name = "numRevenue";
        numRevenue.Size = new Size(180, 30);
        numRevenue.TabIndex = 1;
        numRevenue.TextAlign = HorizontalAlignment.Right;
        numRevenue.ThousandsSeparator = true;
        // 
        // chkRevenue
        // 
        chkRevenue.AutoSize = true;
        chkRevenue.Font = new Font("Segoe UI", 9.5F);
        chkRevenue.Location = new Point(0, 5);
        chkRevenue.Name = "chkRevenue";
        chkRevenue.Size = new Size(82, 25);
        chkRevenue.TabIndex = 0;
        chkRevenue.Text = "Specify";
        chkRevenue.UseVisualStyleBackColor = true;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblStatus.ForeColor = Color.FromArgb(45, 55, 70);
        lblStatus.Location = new Point(27, 384);
        lblStatus.Margin = new Padding(3, 7, 12, 12);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(115, 21);
        lblStatus.TabIndex = 7;
        lblStatus.Text = "Initial status *";
        // 
        // cboStatus
        // 
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.Font = new Font("Segoe UI", 10F);
        cboStatus.FormattingEnabled = true;
        cboStatus.Location = new Point(212, 380);
        cboStatus.Margin = new Padding(3, 3, 3, 12);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(260, 31);
        cboStatus.TabIndex = 7;
        // 
        // lblSpecialInstructions
        // 
        lblSpecialInstructions.AutoSize = true;
        lblSpecialInstructions.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblSpecialInstructions.ForeColor = Color.FromArgb(45, 55, 70);
        lblSpecialInstructions.Location = new Point(27, 430);
        lblSpecialInstructions.Margin = new Padding(3, 7, 12, 12);
        lblSpecialInstructions.Name = "lblSpecialInstructions";
        lblSpecialInstructions.Size = new Size(159, 21);
        lblSpecialInstructions.TabIndex = 8;
        lblSpecialInstructions.Text = "Special instructions";
        // 
        // txtSpecialInstructions
        // 
        txtSpecialInstructions.Dock = DockStyle.Fill;
        txtSpecialInstructions.Font = new Font("Segoe UI", 10F);
        txtSpecialInstructions.Location = new Point(212, 426);
        txtSpecialInstructions.Margin = new Padding(3, 3, 3, 4);
        txtSpecialInstructions.MaxLength = 1000;
        txtSpecialInstructions.Multiline = true;
        txtSpecialInstructions.Name = "txtSpecialInstructions";
        txtSpecialInstructions.PlaceholderText = "Delivery requirements, handling notes, or other instructions";
        txtSpecialInstructions.ScrollBars = ScrollBars.Vertical;
        txtSpecialInstructions.Size = new Size(547, 75);
        txtSpecialInstructions.TabIndex = 8;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(lblMessage);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 700);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(32, 18, 32, 18);
        pnlFooter.Size = new Size(850, 82);
        pnlFooter.TabIndex = 2;
        // 
        // lblMessage
        // 
        lblMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblMessage.Font = new Font("Segoe UI", 9F);
        lblMessage.ForeColor = Color.FromArgb(106, 116, 130);
        lblMessage.Location = new Point(32, 27);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(470, 24);
        lblMessage.TabIndex = 0;
        lblMessage.Text = "Complete the required information.";
        lblMessage.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.BackColor = Color.FromArgb(243, 108, 33);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(668, 20);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 42);
        btnSave.TabIndex = 2;
        btnSave.Text = "Create Load";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.BackColor = Color.FromArgb(233, 237, 242);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(205, 211, 220);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCancel.ForeColor = Color.FromArgb(45, 55, 70);
        btnCancel.Location = new Point(540, 20);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(112, 42);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = false;
        // 
        // CreateLoadForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        CancelButton = btnCancel;
        ClientSize = new Size(850, 782);
        Controls.Add(pnlBody);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(750, 700);
        Name = "CreateLoadForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FleetFlow — Create Load";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlBody.ResumeLayout(false);
        pnlBody.PerformLayout();
        tlpForm.ResumeLayout(false);
        tlpForm.PerformLayout();
        pnlCustomer.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numWeight).EndInit();
        pnlPieces.ResumeLayout(false);
        pnlPieces.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPieces).EndInit();
        pnlRevenue.ResumeLayout(false);
        pnlRevenue.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numRevenue).EndInit();
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }
}