

namespace FleetFlow.Dispatch.WinForms.Forms.Loads;

partial class LoadDetailsForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel pnlHeader;
    private TableLayoutPanel tlpDetails;
    private Panel pnlInstructions;
    private Panel pnlFooter;

    private Label lblLoadNumber;
    private Label lblLoadStatus;

    private Button btnRefresh;
    private Button btnEditLoad;
    private Button btnOpenTrip;
    private Button btnClose;

    private Label lblCustomerCaption;
    private Label lblCustomerValue;
    private Label lblContactCaption;
    private Label lblContactValue;

    private Label lblEmailCaption;
    private Label lblEmailValue;
    private Label lblPhoneCaption;
    private Label lblPhoneValue;

    private Label lblDescriptionCaption;
    private Label lblDescriptionValue;
    private Label lblCommodityCaption;
    private Label lblCommodityValue;

    private Label lblWeightCaption;
    private Label lblWeightValue;
    private Label lblPiecesCaption;
    private Label lblPiecesValue;

    private Label lblRevenueCaption;
    private Label lblRevenueValue;
    private Label lblTripCaption;
    private Label lblTripValue;

    private Label lblTripStatusCaption;
    private Label lblTripStatusValue;
    private Label lblScheduleCaption;
    private Label lblScheduleValue;

    private Label lblCreatedCaption;
    private Label lblCreatedValue;
    private Label lblUpdatedCaption;
    private Label lblUpdatedValue;

    private Label lblInstructionsTitle;
    private TextBox txtSpecialInstructions;
    private Label lblMessage;

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
        tlpDetails = new TableLayoutPanel();
        pnlInstructions = new Panel();
        pnlFooter = new Panel();

        lblLoadNumber = new Label();
        lblLoadStatus = new Label();

        btnRefresh = new Button();
        btnEditLoad = new Button();
        btnOpenTrip = new Button();
        btnClose = new Button();

        lblCustomerCaption = new Label();
        lblCustomerValue = new Label();
        lblContactCaption = new Label();
        lblContactValue = new Label();

        lblEmailCaption = new Label();
        lblEmailValue = new Label();
        lblPhoneCaption = new Label();
        lblPhoneValue = new Label();

        lblDescriptionCaption = new Label();
        lblDescriptionValue = new Label();
        lblCommodityCaption = new Label();
        lblCommodityValue = new Label();

        lblWeightCaption = new Label();
        lblWeightValue = new Label();
        lblPiecesCaption = new Label();
        lblPiecesValue = new Label();

        lblRevenueCaption = new Label();
        lblRevenueValue = new Label();
        lblTripCaption = new Label();
        lblTripValue = new Label();

        lblTripStatusCaption = new Label();
        lblTripStatusValue = new Label();
        lblScheduleCaption = new Label();
        lblScheduleValue = new Label();

        lblCreatedCaption = new Label();
        lblCreatedValue = new Label();
        lblUpdatedCaption = new Label();
        lblUpdatedValue = new Label();

        lblInstructionsTitle = new Label();
        txtSpecialInstructions = new TextBox();
        lblMessage = new Label();

        pnlHeader.SuspendLayout();
        tlpDetails.SuspendLayout();
        pnlInstructions.SuspendLayout();
        pnlFooter.SuspendLayout();

        SuspendLayout();

        // pnlHeader
        // Presenta la identidad de la carga y las acciones.
        pnlHeader.BackColor =
            Color.FromArgb(29, 39, 54);
        pnlHeader.Controls.Add(lblLoadNumber);
        pnlHeader.Controls.Add(lblLoadStatus);
        pnlHeader.Controls.Add(btnEditLoad);
        pnlHeader.Controls.Add(btnRefresh);
        pnlHeader.Controls.Add(btnOpenTrip);
        pnlHeader.Controls.Add(btnClose);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 92;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding =
            new Padding(24, 18, 24, 18);

        // lblLoadNumber
        lblLoadNumber.AutoSize = true;
        lblLoadNumber.Font = new System.Drawing.Font(
            "Segoe UI",
            19F,
            FontStyle.Bold);
        lblLoadNumber.ForeColor = Color.White;
        lblLoadNumber.Location =
            new Point(24, 17);
        lblLoadNumber.Name = "lblLoadNumber";
        lblLoadNumber.Text = "LOAD";

        // lblLoadStatus
        lblLoadStatus.AutoSize = true;
        lblLoadStatus.Font = new System.Drawing.Font(
            "Segoe UI",
            10F,
            FontStyle.Bold);
        lblLoadStatus.ForeColor =
            Color.FromArgb(243, 108, 33);
        lblLoadStatus.Location =
            new Point(27, 57);
        lblLoadStatus.Name = "lblLoadStatus";
        lblLoadStatus.Text = "STATUS";

        // btnEditLoad
        btnEditLoad.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Right;
        btnEditLoad.BackColor =
            Color.FromArgb(243, 108, 33);
        btnEditLoad.Cursor = Cursors.Hand;
        btnEditLoad.FlatAppearance.BorderSize = 0;
        btnEditLoad.FlatStyle = FlatStyle.Flat;
        btnEditLoad.Font = new System.Drawing.Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnEditLoad.ForeColor = Color.White;
        btnEditLoad.Location =
            new Point(421, 29);
        btnEditLoad.Name = "btnEditLoad";
        btnEditLoad.Size =
            new Size(110, 36);
        btnEditLoad.Text = "Edit Load";
        btnEditLoad.UseVisualStyleBackColor = false;

        // btnRefresh
        btnRefresh.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Right;
        btnRefresh.BackColor =
            Color.FromArgb(55, 68, 86);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new System.Drawing.Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location =
            new Point(542, 29);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size =
            new Size(95, 36);
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;

        // btnOpenTrip
        btnOpenTrip.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Right;
        btnOpenTrip.BackColor =
            Color.FromArgb(55, 68, 86);
        btnOpenTrip.Cursor = Cursors.Hand;
        btnOpenTrip.FlatAppearance.BorderColor =
            Color.FromArgb(100, 115, 135);
        btnOpenTrip.FlatStyle = FlatStyle.Flat;
        btnOpenTrip.Font = new System.Drawing.Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnOpenTrip.ForeColor = Color.White;
        btnOpenTrip.Location =
            new Point(649, 29);
        btnOpenTrip.Name = "btnOpenTrip";
        btnOpenTrip.Size =
            new Size(112, 36);
        btnOpenTrip.Text = "Open Trip";
        btnOpenTrip.UseVisualStyleBackColor = false;

        // btnClose
        btnClose.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Right;
        btnClose.BackColor =
            Color.FromArgb(55, 68, 86);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult =
            DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor =
            Color.FromArgb(100, 115, 135);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new System.Drawing.Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location =
            new Point(772, 29);
        btnClose.Name = "btnClose";
        btnClose.Size =
            new Size(95, 36);
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = false;

        // tlpDetails
        // Dos pares de etiqueta/valor por fila.
        tlpDetails.BackColor = Color.White;
        tlpDetails.ColumnCount = 4;

        tlpDetails.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Absolute,
                125F));

        tlpDetails.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Percent,
                50F));

        tlpDetails.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Absolute,
                125F));

        tlpDetails.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Percent,
                50F));

        tlpDetails.Controls.Add(
            lblCustomerCaption, 0, 0);
        tlpDetails.Controls.Add(
            lblCustomerValue, 1, 0);
        tlpDetails.Controls.Add(
            lblContactCaption, 2, 0);
        tlpDetails.Controls.Add(
            lblContactValue, 3, 0);

        tlpDetails.Controls.Add(
            lblEmailCaption, 0, 1);
        tlpDetails.Controls.Add(
            lblEmailValue, 1, 1);
        tlpDetails.Controls.Add(
            lblPhoneCaption, 2, 1);
        tlpDetails.Controls.Add(
            lblPhoneValue, 3, 1);

        tlpDetails.Controls.Add(
            lblDescriptionCaption, 0, 2);
        tlpDetails.Controls.Add(
            lblDescriptionValue, 1, 2);
        tlpDetails.Controls.Add(
            lblCommodityCaption, 2, 2);
        tlpDetails.Controls.Add(
            lblCommodityValue, 3, 2);

        tlpDetails.Controls.Add(
            lblWeightCaption, 0, 3);
        tlpDetails.Controls.Add(
            lblWeightValue, 1, 3);
        tlpDetails.Controls.Add(
            lblPiecesCaption, 2, 3);
        tlpDetails.Controls.Add(
            lblPiecesValue, 3, 3);

        tlpDetails.Controls.Add(
            lblRevenueCaption, 0, 4);
        tlpDetails.Controls.Add(
            lblRevenueValue, 1, 4);
        tlpDetails.Controls.Add(
            lblTripCaption, 2, 4);
        tlpDetails.Controls.Add(
            lblTripValue, 3, 4);

        tlpDetails.Controls.Add(
            lblTripStatusCaption, 0, 5);
        tlpDetails.Controls.Add(
            lblTripStatusValue, 1, 5);
        tlpDetails.Controls.Add(
            lblScheduleCaption, 2, 5);
        tlpDetails.Controls.Add(
            lblScheduleValue, 3, 5);

        tlpDetails.Controls.Add(
            lblCreatedCaption, 0, 6);
        tlpDetails.Controls.Add(
            lblCreatedValue, 1, 6);
        tlpDetails.Controls.Add(
            lblUpdatedCaption, 2, 6);
        tlpDetails.Controls.Add(
            lblUpdatedValue, 3, 6);

        tlpDetails.Dock = DockStyle.Top;
        tlpDetails.Location =
            new Point(0, 92);
        tlpDetails.Name = "tlpDetails";
        tlpDetails.Padding =
            new Padding(24, 18, 24, 12);
        tlpDetails.RowCount = 7;
        tlpDetails.Size =
            new Size(900, 350);

        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));
        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));
        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));
        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));
        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));
        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));
        tlpDetails.RowStyles.Add(
            new RowStyle(SizeType.Percent, 14.28F));

        // Caption labels
        lblCustomerCaption.Text = "Customer";
        lblContactCaption.Text = "Contact";
        lblEmailCaption.Text = "Email";
        lblPhoneCaption.Text = "Phone";
        lblDescriptionCaption.Text = "Description";
        lblCommodityCaption.Text = "Commodity";
        lblWeightCaption.Text = "Weight";
        lblPiecesCaption.Text = "Pieces";
        lblRevenueCaption.Text = "Revenue";
        lblTripCaption.Text = "Related trip";
        lblTripStatusCaption.Text = "Trip status";
        lblScheduleCaption.Text = "Schedule";
        lblCreatedCaption.Text = "Created";
        lblUpdatedCaption.Text = "Updated";

        lblCustomerCaption.Dock = DockStyle.Fill;
        lblContactCaption.Dock = DockStyle.Fill;
        lblEmailCaption.Dock = DockStyle.Fill;
        lblPhoneCaption.Dock = DockStyle.Fill;
        lblDescriptionCaption.Dock = DockStyle.Fill;
        lblCommodityCaption.Dock = DockStyle.Fill;
        lblWeightCaption.Dock = DockStyle.Fill;
        lblPiecesCaption.Dock = DockStyle.Fill;
        lblRevenueCaption.Dock = DockStyle.Fill;
        lblTripCaption.Dock = DockStyle.Fill;
        lblTripStatusCaption.Dock = DockStyle.Fill;
        lblScheduleCaption.Dock = DockStyle.Fill;
        lblCreatedCaption.Dock = DockStyle.Fill;
        lblUpdatedCaption.Dock = DockStyle.Fill;

        lblCustomerCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblContactCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblEmailCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblPhoneCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblDescriptionCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblCommodityCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblWeightCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblPiecesCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblRevenueCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblTripCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblTripStatusCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblScheduleCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblCreatedCaption.TextAlign =
            ContentAlignment.MiddleLeft;
        lblUpdatedCaption.TextAlign =
            ContentAlignment.MiddleLeft;

        lblCustomerCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblContactCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblEmailCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblPhoneCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblDescriptionCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblCommodityCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblWeightCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblPiecesCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblRevenueCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblTripCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblTripStatusCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblScheduleCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblCreatedCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
        lblUpdatedCaption.Font =
            new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);

        // Value labels
        lblCustomerValue.Text = "—";
        lblContactValue.Text = "—";
        lblEmailValue.Text = "—";
        lblPhoneValue.Text = "—";
        lblDescriptionValue.Text = "—";
        lblCommodityValue.Text = "—";
        lblWeightValue.Text = "—";
        lblPiecesValue.Text = "—";
        lblRevenueValue.Text = "—";
        lblTripValue.Text = "—";
        lblTripStatusValue.Text = "—";
        lblScheduleValue.Text = "—";
        lblCreatedValue.Text = "—";
        lblUpdatedValue.Text = "—";

        lblCustomerValue.Dock = DockStyle.Fill;
        lblContactValue.Dock = DockStyle.Fill;
        lblEmailValue.Dock = DockStyle.Fill;
        lblPhoneValue.Dock = DockStyle.Fill;
        lblDescriptionValue.Dock = DockStyle.Fill;
        lblCommodityValue.Dock = DockStyle.Fill;
        lblWeightValue.Dock = DockStyle.Fill;
        lblPiecesValue.Dock = DockStyle.Fill;
        lblRevenueValue.Dock = DockStyle.Fill;
        lblTripValue.Dock = DockStyle.Fill;
        lblTripStatusValue.Dock = DockStyle.Fill;
        lblScheduleValue.Dock = DockStyle.Fill;
        lblCreatedValue.Dock = DockStyle.Fill;
        lblUpdatedValue.Dock = DockStyle.Fill;

        lblCustomerValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblContactValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblEmailValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblPhoneValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblDescriptionValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblCommodityValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblWeightValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblPiecesValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblRevenueValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblTripValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblTripStatusValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblScheduleValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblCreatedValue.TextAlign =
            ContentAlignment.MiddleLeft;
        lblUpdatedValue.TextAlign =
            ContentAlignment.MiddleLeft;

        // pnlInstructions
        pnlInstructions.BackColor =
            Color.FromArgb(244, 246, 249);
        pnlInstructions.Controls.Add(
            lblInstructionsTitle);
        pnlInstructions.Controls.Add(
            txtSpecialInstructions);
        pnlInstructions.Dock = DockStyle.Fill;
        pnlInstructions.Name = "pnlInstructions";
        pnlInstructions.Padding =
            new Padding(24, 18, 24, 18);

        // lblInstructionsTitle
        lblInstructionsTitle.AutoSize = true;
        lblInstructionsTitle.Font = new System.Drawing.Font(
            "Segoe UI",
            10F,
            FontStyle.Bold);
        lblInstructionsTitle.ForeColor =
            Color.FromArgb(29, 39, 54);
        lblInstructionsTitle.Location =
            new Point(24, 17);
        lblInstructionsTitle.Name =
            "lblInstructionsTitle";
        lblInstructionsTitle.Text =
            "Special instructions";

        // txtSpecialInstructions
        txtSpecialInstructions.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Bottom |
            AnchorStyles.Left |
            AnchorStyles.Right;
        txtSpecialInstructions.BackColor =
            Color.White;
        txtSpecialInstructions.Font =
            new System.Drawing.Font("Segoe UI", 10F);
        txtSpecialInstructions.Location =
            new Point(24, 48);
        txtSpecialInstructions.Multiline = true;
        txtSpecialInstructions.Name =
            "txtSpecialInstructions";
        txtSpecialInstructions.ReadOnly = true;
        txtSpecialInstructions.ScrollBars =
            ScrollBars.Vertical;
        txtSpecialInstructions.Size =
            new Size(852, 105);

        // pnlFooter
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(lblMessage);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Height = 38;
        pnlFooter.Name = "pnlFooter";

        // lblMessage
        lblMessage.Dock = DockStyle.Fill;
        lblMessage.Font =
            new System.Drawing.Font("Segoe UI", 9F);
        lblMessage.ForeColor =
            Color.FromArgb(106, 116, 130);
        lblMessage.Name = "lblMessage";
        lblMessage.Padding =
            new Padding(24, 0, 24, 0);
        lblMessage.Text = "Ready";
        lblMessage.TextAlign =
            ContentAlignment.MiddleLeft;

        // LoadDetailsForm
        AutoScaleDimensions =
            new SizeF(7F, 15F);
        AutoScaleMode =
            AutoScaleMode.Font;
        BackColor =
            Color.FromArgb(244, 246, 249);
        CancelButton = btnClose;
        ClientSize =
            new Size(900, 650);
        Controls.Add(pnlInstructions);
        Controls.Add(tlpDetails);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new System.Drawing.Font("Segoe UI", 10F);
        FormBorderStyle =
            FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoadDetailsForm";
        ShowIcon = false;
        StartPosition =
            FormStartPosition.CenterParent;
        Text = "FleetFlow — Load Details";

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        tlpDetails.ResumeLayout(false);
        tlpDetails.PerformLayout();
        pnlInstructions.ResumeLayout(false);
        pnlInstructions.PerformLayout();
        pnlFooter.ResumeLayout(false);

        ResumeLayout(false);
    }
}
