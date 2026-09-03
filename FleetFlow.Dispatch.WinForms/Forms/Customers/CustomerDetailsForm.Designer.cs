namespace FleetFlow.Dispatch.WinForms.Forms.Customers;

partial class CustomerDetailsForm
{
    private System.ComponentModel.IContainer? components = null;

    private Panel pnlHeader = null!;
    private Label lblCustomerNumber = null!;
    private Label lblCompanyName = null!;
    private Label lblStatus = null!;
    private FlowLayoutPanel flpCustomerActions = null!;
    private Button btnEdit = null!;
    private Button btnSetActive = null!;
    private Button btnRefresh = null!;
    private Button btnClose = null!;

    private TableLayoutPanel tlpSummary = null!;
    private Panel pnlLoadsSummary = null!;
    private Panel pnlOpenLoadsSummary = null!;
    private Panel pnlRevenueSummary = null!;
    private Label lblLoadsCaption = null!;
    private Label lblLoadsValue = null!;
    private Label lblOpenLoadsCaption = null!;
    private Label lblOpenLoadsValue = null!;
    private Label lblRevenueCaption = null!;
    private Label lblRevenueValue = null!;

    private TableLayoutPanel tlpInformation = null!;
    private Panel pnlContact = null!;
    private Panel pnlEmail = null!;
    private Panel pnlPhone = null!;
    private Panel pnlCreated = null!;
    private Panel pnlUpdated = null!;
    private Label lblContactCaption = null!;
    private Label lblContactValue = null!;
    private Label lblEmailCaption = null!;
    private Label lblEmailValue = null!;
    private Label lblPhoneCaption = null!;
    private Label lblPhoneValue = null!;
    private Label lblCreatedCaption = null!;
    private Label lblCreatedValue = null!;
    private Label lblUpdatedCaption = null!;
    private Label lblUpdatedValue = null!;

    private TabControl tabCustomerDetails = null!;
    private TabPage tabLocations = null!;
    private TabPage tabRecentLoads = null!;

    private FlowLayoutPanel flpLocationActions = null!;
    private Button btnNewLocation = null!;
    private Button btnEditLocation = null!;
    private Button btnLocationStatus = null!;

    private DataGridView dgvLocations = null!;
    private DataGridViewTextBoxColumn colLocationCode = null!;
    private DataGridViewTextBoxColumn colLocationName = null!;
    private DataGridViewTextBoxColumn colLocationType = null!;
    private DataGridViewTextBoxColumn colAddress = null!;
    private DataGridViewTextBoxColumn colContactName = null!;
    private DataGridViewTextBoxColumn colBillingLocation = null!;
    private DataGridViewTextBoxColumn colLocationActive = null!;

    private DataGridView dgvLoads = null!;
    private DataGridViewTextBoxColumn colLoadNumber = null!;
    private DataGridViewTextBoxColumn colLoadDescription = null!;
    private DataGridViewTextBoxColumn colLoadStatus = null!;
    private DataGridViewTextBoxColumn colLoadRevenue = null!;
    private DataGridViewTextBoxColumn colLoadCreated = null!;

    private Panel pnlFooter = null!;
    private Label lblMessage = null!;

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

        pnlHeader = new Panel();
        lblCustomerNumber = new Label();
        lblCompanyName = new Label();
        lblStatus = new Label();
        flpCustomerActions = new FlowLayoutPanel();
        btnEdit = new Button();
        btnSetActive = new Button();
        btnRefresh = new Button();
        btnClose = new Button();

        tlpSummary = new TableLayoutPanel();
        pnlLoadsSummary = new Panel();
        pnlOpenLoadsSummary = new Panel();
        pnlRevenueSummary = new Panel();
        lblLoadsCaption = new Label();
        lblLoadsValue = new Label();
        lblOpenLoadsCaption = new Label();
        lblOpenLoadsValue = new Label();
        lblRevenueCaption = new Label();
        lblRevenueValue = new Label();

        tlpInformation = new TableLayoutPanel();
        pnlContact = new Panel();
        pnlEmail = new Panel();
        pnlPhone = new Panel();
        pnlCreated = new Panel();
        pnlUpdated = new Panel();
        lblContactCaption = new Label();
        lblContactValue = new Label();
        lblEmailCaption = new Label();
        lblEmailValue = new Label();
        lblPhoneCaption = new Label();
        lblPhoneValue = new Label();
        lblCreatedCaption = new Label();
        lblCreatedValue = new Label();
        lblUpdatedCaption = new Label();
        lblUpdatedValue = new Label();

        tabCustomerDetails = new TabControl();
        tabLocations = new TabPage();
        tabRecentLoads = new TabPage();

        flpLocationActions = new FlowLayoutPanel();
        btnNewLocation = new Button();
        btnEditLocation = new Button();
        btnLocationStatus = new Button();

        dgvLocations = new DataGridView();
        colLocationCode = new DataGridViewTextBoxColumn();
        colLocationName = new DataGridViewTextBoxColumn();
        colLocationType = new DataGridViewTextBoxColumn();
        colAddress = new DataGridViewTextBoxColumn();
        colContactName = new DataGridViewTextBoxColumn();
        colBillingLocation = new DataGridViewTextBoxColumn();
        colLocationActive = new DataGridViewTextBoxColumn();

        dgvLoads = new DataGridView();
        colLoadNumber = new DataGridViewTextBoxColumn();
        colLoadDescription = new DataGridViewTextBoxColumn();
        colLoadStatus = new DataGridViewTextBoxColumn();
        colLoadRevenue = new DataGridViewTextBoxColumn();
        colLoadCreated = new DataGridViewTextBoxColumn();

        pnlFooter = new Panel();
        lblMessage = new Label();

        pnlHeader.SuspendLayout();
        flpCustomerActions.SuspendLayout();
        tlpSummary.SuspendLayout();
        pnlLoadsSummary.SuspendLayout();
        pnlOpenLoadsSummary.SuspendLayout();
        pnlRevenueSummary.SuspendLayout();
        tlpInformation.SuspendLayout();
        pnlContact.SuspendLayout();
        pnlEmail.SuspendLayout();
        pnlPhone.SuspendLayout();
        pnlCreated.SuspendLayout();
        pnlUpdated.SuspendLayout();
        tabCustomerDetails.SuspendLayout();
        tabLocations.SuspendLayout();
        tabRecentLoads.SuspendLayout();
        flpLocationActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLocations).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLoads).BeginInit();
        pnlFooter.SuspendLayout();
        SuspendLayout();

        // pnlHeader
        pnlHeader.BackColor = Color.White;
        pnlHeader.Controls.Add(lblCustomerNumber);
        pnlHeader.Controls.Add(lblCompanyName);
        pnlHeader.Controls.Add(lblStatus);
        pnlHeader.Controls.Add(flpCustomerActions);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1120, 125);
        pnlHeader.TabIndex = 0;

        // lblCustomerNumber
        lblCustomerNumber.AutoSize = true;
        lblCustomerNumber.Font = new Font(
            "Segoe UI",
            10F,
            FontStyle.Bold);
        lblCustomerNumber.ForeColor = Color.FromArgb(243, 108, 33);
        lblCustomerNumber.Location = new Point(34, 18);
        lblCustomerNumber.Name = "lblCustomerNumber";
        lblCustomerNumber.Size = new Size(98, 23);
        lblCustomerNumber.TabIndex = 0;
        lblCustomerNumber.Text = "CUSTOMER";

        // lblCompanyName
        lblCompanyName.AutoSize = true;
        lblCompanyName.Font = new Font(
            "Segoe UI",
            22F,
            FontStyle.Bold);
        lblCompanyName.ForeColor = Color.FromArgb(29, 39, 54);
        lblCompanyName.Location = new Point(32, 42);
        lblCompanyName.Name = "lblCompanyName";
        lblCompanyName.Size = new Size(194, 50);
        lblCompanyName.TabIndex = 1;
        lblCompanyName.Text = "Customer";

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        lblStatus.ForeColor = Color.FromArgb(35, 130, 85);
        lblStatus.Location = new Point(34, 96);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(63, 20);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "STATUS";

        // flpCustomerActions
        flpCustomerActions.Anchor =
            AnchorStyles.Top | AnchorStyles.Right;
        flpCustomerActions.Controls.Add(btnEdit);
        flpCustomerActions.Controls.Add(btnSetActive);
        flpCustomerActions.Controls.Add(btnRefresh);
        flpCustomerActions.Controls.Add(btnClose);
        flpCustomerActions.FlowDirection = FlowDirection.LeftToRight;
        flpCustomerActions.Location = new Point(585, 38);
        flpCustomerActions.Name = "flpCustomerActions";
        flpCustomerActions.Size = new Size(500, 48);
        flpCustomerActions.TabIndex = 3;
        flpCustomerActions.WrapContents = false;

        // btnEdit
        btnEdit.BackColor = Color.FromArgb(243, 108, 33);
        btnEdit.FlatAppearance.BorderSize = 0;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnEdit.ForeColor = Color.White;
        btnEdit.Location = new Point(4, 4);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(125, 40);
        btnEdit.TabIndex = 0;
        btnEdit.Text = "Edit Customer";
        btnEdit.UseVisualStyleBackColor = false;

        // btnSetActive
        btnSetActive.BackColor = Color.FromArgb(225, 229, 235);
        btnSetActive.FlatAppearance.BorderSize = 0;
        btnSetActive.FlatStyle = FlatStyle.Flat;
        btnSetActive.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSetActive.ForeColor = Color.FromArgb(45, 55, 70);
        btnSetActive.Location = new Point(137, 4);
        btnSetActive.Margin = new Padding(4);
        btnSetActive.Name = "btnSetActive";
        btnSetActive.Size = new Size(105, 40);
        btnSetActive.TabIndex = 1;
        btnSetActive.Text = "Deactivate";
        btnSetActive.UseVisualStyleBackColor = false;

        // btnRefresh
        btnRefresh.BackColor = Color.FromArgb(225, 229, 235);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.FromArgb(45, 55, 70);
        btnRefresh.Location = new Point(250, 4);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(90, 40);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;

        // btnClose
        btnClose.BackColor = Color.FromArgb(29, 39, 54);
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(348, 4);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(80, 40);
        btnClose.TabIndex = 3;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = false;

        // tlpSummary
        tlpSummary.BackColor = Color.White;
        tlpSummary.ColumnCount = 3;
        tlpSummary.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 33.33F));
        tlpSummary.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 33.33F));
        tlpSummary.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 33.34F));
        tlpSummary.Controls.Add(pnlLoadsSummary, 0, 0);
        tlpSummary.Controls.Add(pnlOpenLoadsSummary, 1, 0);
        tlpSummary.Controls.Add(pnlRevenueSummary, 2, 0);
        tlpSummary.Dock = DockStyle.Top;
        tlpSummary.Location = new Point(0, 125);
        tlpSummary.Name = "tlpSummary";
        tlpSummary.Padding = new Padding(26, 14, 26, 14);
        tlpSummary.RowCount = 1;
        tlpSummary.RowStyles.Add(
            new RowStyle(SizeType.Percent, 100F));
        tlpSummary.Size = new Size(1120, 112);
        tlpSummary.TabIndex = 1;

        // pnlLoadsSummary
        pnlLoadsSummary.Controls.Add(lblLoadsCaption);
        pnlLoadsSummary.Controls.Add(lblLoadsValue);
        pnlLoadsSummary.Dock = DockStyle.Fill;
        pnlLoadsSummary.Name = "pnlLoadsSummary";
        pnlLoadsSummary.TabIndex = 0;

        // lblLoadsCaption
        lblLoadsCaption.AutoSize = true;
        lblLoadsCaption.Font = new Font(
            "Segoe UI",
            8.5F,
            FontStyle.Bold);
        lblLoadsCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblLoadsCaption.Location = new Point(8, 4);
        lblLoadsCaption.Name = "lblLoadsCaption";
        lblLoadsCaption.Size = new Size(99, 20);
        lblLoadsCaption.TabIndex = 0;
        lblLoadsCaption.Text = "TOTAL LOADS";

        // lblLoadsValue
        lblLoadsValue.AutoSize = true;
        lblLoadsValue.Font = new Font(
            "Segoe UI",
            18F,
            FontStyle.Bold);
        lblLoadsValue.ForeColor = Color.FromArgb(29, 39, 54);
        lblLoadsValue.Location = new Point(8, 30);
        lblLoadsValue.Name = "lblLoadsValue";
        lblLoadsValue.Size = new Size(35, 41);
        lblLoadsValue.TabIndex = 1;
        lblLoadsValue.Text = "0";

        // pnlOpenLoadsSummary
        pnlOpenLoadsSummary.Controls.Add(lblOpenLoadsCaption);
        pnlOpenLoadsSummary.Controls.Add(lblOpenLoadsValue);
        pnlOpenLoadsSummary.Dock = DockStyle.Fill;
        pnlOpenLoadsSummary.Name = "pnlOpenLoadsSummary";
        pnlOpenLoadsSummary.TabIndex = 1;

        // lblOpenLoadsCaption
        lblOpenLoadsCaption.AutoSize = true;
        lblOpenLoadsCaption.Font = new Font(
            "Segoe UI",
            8.5F,
            FontStyle.Bold);
        lblOpenLoadsCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblOpenLoadsCaption.Location = new Point(8, 4);
        lblOpenLoadsCaption.Name = "lblOpenLoadsCaption";
        lblOpenLoadsCaption.Size = new Size(91, 20);
        lblOpenLoadsCaption.TabIndex = 0;
        lblOpenLoadsCaption.Text = "OPEN LOADS";

        // lblOpenLoadsValue
        lblOpenLoadsValue.AutoSize = true;
        lblOpenLoadsValue.Font = new Font(
            "Segoe UI",
            18F,
            FontStyle.Bold);
        lblOpenLoadsValue.ForeColor = Color.FromArgb(29, 39, 54);
        lblOpenLoadsValue.Location = new Point(8, 30);
        lblOpenLoadsValue.Name = "lblOpenLoadsValue";
        lblOpenLoadsValue.Size = new Size(35, 41);
        lblOpenLoadsValue.TabIndex = 1;
        lblOpenLoadsValue.Text = "0";

        // pnlRevenueSummary
        pnlRevenueSummary.Controls.Add(lblRevenueCaption);
        pnlRevenueSummary.Controls.Add(lblRevenueValue);
        pnlRevenueSummary.Dock = DockStyle.Fill;
        pnlRevenueSummary.Name = "pnlRevenueSummary";
        pnlRevenueSummary.TabIndex = 2;

        // lblRevenueCaption
        lblRevenueCaption.AutoSize = true;
        lblRevenueCaption.Font = new Font(
            "Segoe UI",
            8.5F,
            FontStyle.Bold);
        lblRevenueCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblRevenueCaption.Location = new Point(8, 4);
        lblRevenueCaption.Name = "lblRevenueCaption";
        lblRevenueCaption.Size = new Size(117, 20);
        lblRevenueCaption.TabIndex = 0;
        lblRevenueCaption.Text = "TOTAL REVENUE";

        // lblRevenueValue
        lblRevenueValue.AutoSize = true;
        lblRevenueValue.Font = new Font(
            "Segoe UI",
            18F,
            FontStyle.Bold);
        lblRevenueValue.ForeColor = Color.FromArgb(29, 39, 54);
        lblRevenueValue.Location = new Point(8, 30);
        lblRevenueValue.Name = "lblRevenueValue";
        lblRevenueValue.Size = new Size(53, 41);
        lblRevenueValue.TabIndex = 1;
        lblRevenueValue.Text = "$0";

        // tlpInformation
        tlpInformation.BackColor = Color.White;
        tlpInformation.ColumnCount = 5;
        tlpInformation.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));
        tlpInformation.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));
        tlpInformation.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));
        tlpInformation.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));
        tlpInformation.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 20F));
        tlpInformation.Controls.Add(pnlContact, 0, 0);
        tlpInformation.Controls.Add(pnlEmail, 1, 0);
        tlpInformation.Controls.Add(pnlPhone, 2, 0);
        tlpInformation.Controls.Add(pnlCreated, 3, 0);
        tlpInformation.Controls.Add(pnlUpdated, 4, 0);
        tlpInformation.Dock = DockStyle.Top;
        tlpInformation.Location = new Point(0, 237);
        tlpInformation.Name = "tlpInformation";
        tlpInformation.Padding = new Padding(26, 12, 26, 12);
        tlpInformation.RowCount = 1;
        tlpInformation.RowStyles.Add(
            new RowStyle(SizeType.Percent, 100F));
        tlpInformation.Size = new Size(1120, 105);
        tlpInformation.TabIndex = 2;

        // pnlContact
        pnlContact.Controls.Add(lblContactCaption);
        pnlContact.Controls.Add(lblContactValue);
        pnlContact.Dock = DockStyle.Fill;
        pnlContact.Name = "pnlContact";

        // lblContactCaption
        lblContactCaption.AutoSize = true;
        lblContactCaption.Font = new Font(
            "Segoe UI",
            8F,
            FontStyle.Bold);
        lblContactCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblContactCaption.Location = new Point(8, 4);
        lblContactCaption.Name = "lblContactCaption";
        lblContactCaption.Text = "PRIMARY CONTACT";

        // lblContactValue
        lblContactValue.AutoSize = true;
        lblContactValue.Font = new Font("Segoe UI", 10F);
        lblContactValue.ForeColor = Color.FromArgb(45, 55, 70);
        lblContactValue.Location = new Point(8, 31);
        lblContactValue.MaximumSize = new Size(190, 45);
        lblContactValue.Name = "lblContactValue";
        lblContactValue.Text = "—";

        // pnlEmail
        pnlEmail.Controls.Add(lblEmailCaption);
        pnlEmail.Controls.Add(lblEmailValue);
        pnlEmail.Dock = DockStyle.Fill;
        pnlEmail.Name = "pnlEmail";

        // lblEmailCaption
        lblEmailCaption.AutoSize = true;
        lblEmailCaption.Font = new Font(
            "Segoe UI",
            8F,
            FontStyle.Bold);
        lblEmailCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblEmailCaption.Location = new Point(8, 4);
        lblEmailCaption.Name = "lblEmailCaption";
        lblEmailCaption.Text = "EMAIL";

        // lblEmailValue
        lblEmailValue.AutoSize = true;
        lblEmailValue.Font = new Font("Segoe UI", 10F);
        lblEmailValue.ForeColor = Color.FromArgb(45, 55, 70);
        lblEmailValue.Location = new Point(8, 31);
        lblEmailValue.MaximumSize = new Size(190, 45);
        lblEmailValue.Name = "lblEmailValue";
        lblEmailValue.Text = "—";

        // pnlPhone
        pnlPhone.Controls.Add(lblPhoneCaption);
        pnlPhone.Controls.Add(lblPhoneValue);
        pnlPhone.Dock = DockStyle.Fill;
        pnlPhone.Name = "pnlPhone";

        // lblPhoneCaption
        lblPhoneCaption.AutoSize = true;
        lblPhoneCaption.Font = new Font(
            "Segoe UI",
            8F,
            FontStyle.Bold);
        lblPhoneCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblPhoneCaption.Location = new Point(8, 4);
        lblPhoneCaption.Name = "lblPhoneCaption";
        lblPhoneCaption.Text = "PHONE";

        // lblPhoneValue
        lblPhoneValue.AutoSize = true;
        lblPhoneValue.Font = new Font("Segoe UI", 10F);
        lblPhoneValue.ForeColor = Color.FromArgb(45, 55, 70);
        lblPhoneValue.Location = new Point(8, 31);
        lblPhoneValue.MaximumSize = new Size(190, 45);
        lblPhoneValue.Name = "lblPhoneValue";
        lblPhoneValue.Text = "—";

        // pnlCreated
        pnlCreated.Controls.Add(lblCreatedCaption);
        pnlCreated.Controls.Add(lblCreatedValue);
        pnlCreated.Dock = DockStyle.Fill;
        pnlCreated.Name = "pnlCreated";

        // lblCreatedCaption
        lblCreatedCaption.AutoSize = true;
        lblCreatedCaption.Font = new Font(
            "Segoe UI",
            8F,
            FontStyle.Bold);
        lblCreatedCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblCreatedCaption.Location = new Point(8, 4);
        lblCreatedCaption.Name = "lblCreatedCaption";
        lblCreatedCaption.Text = "CREATED";

        // lblCreatedValue
        lblCreatedValue.AutoSize = true;
        lblCreatedValue.Font = new Font("Segoe UI", 10F);
        lblCreatedValue.ForeColor = Color.FromArgb(45, 55, 70);
        lblCreatedValue.Location = new Point(8, 31);
        lblCreatedValue.MaximumSize = new Size(190, 45);
        lblCreatedValue.Name = "lblCreatedValue";
        lblCreatedValue.Text = "—";

        // pnlUpdated
        pnlUpdated.Controls.Add(lblUpdatedCaption);
        pnlUpdated.Controls.Add(lblUpdatedValue);
        pnlUpdated.Dock = DockStyle.Fill;
        pnlUpdated.Name = "pnlUpdated";

        // lblUpdatedCaption
        lblUpdatedCaption.AutoSize = true;
        lblUpdatedCaption.Font = new Font(
            "Segoe UI",
            8F,
            FontStyle.Bold);
        lblUpdatedCaption.ForeColor = Color.FromArgb(106, 116, 130);
        lblUpdatedCaption.Location = new Point(8, 4);
        lblUpdatedCaption.Name = "lblUpdatedCaption";
        lblUpdatedCaption.Text = "UPDATED";

        // lblUpdatedValue
        lblUpdatedValue.AutoSize = true;
        lblUpdatedValue.Font = new Font("Segoe UI", 10F);
        lblUpdatedValue.ForeColor = Color.FromArgb(45, 55, 70);
        lblUpdatedValue.Location = new Point(8, 31);
        lblUpdatedValue.MaximumSize = new Size(190, 45);
        lblUpdatedValue.Name = "lblUpdatedValue";
        lblUpdatedValue.Text = "—";

        // tabCustomerDetails
        tabCustomerDetails.Controls.Add(tabLocations);
        tabCustomerDetails.Controls.Add(tabRecentLoads);
        tabCustomerDetails.Dock = DockStyle.Fill;
        tabCustomerDetails.Font = new Font("Segoe UI", 10F);
        tabCustomerDetails.Location = new Point(0, 342);
        tabCustomerDetails.Name = "tabCustomerDetails";
        tabCustomerDetails.SelectedIndex = 0;
        tabCustomerDetails.Size = new Size(1120, 376);
        tabCustomerDetails.TabIndex = 3;

        // tabLocations
        tabLocations.BackColor = Color.White;
        tabLocations.Controls.Add(dgvLocations);
        tabLocations.Controls.Add(flpLocationActions);
        tabLocations.Location = new Point(4, 32);
        tabLocations.Name = "tabLocations";
        tabLocations.Padding = new Padding(12);
        tabLocations.Size = new Size(1112, 340);
        tabLocations.TabIndex = 0;
        tabLocations.Text = "Locations";

        // flpLocationActions
        flpLocationActions.BackColor = Color.White;
        flpLocationActions.Controls.Add(btnNewLocation);
        flpLocationActions.Controls.Add(btnEditLocation);
        flpLocationActions.Controls.Add(btnLocationStatus);
        flpLocationActions.Dock = DockStyle.Top;
        flpLocationActions.FlowDirection = FlowDirection.LeftToRight;
        flpLocationActions.Location = new Point(12, 12);
        flpLocationActions.Name = "flpLocationActions";
        flpLocationActions.Padding = new Padding(0, 4, 0, 4);
        flpLocationActions.Size = new Size(1088, 52);
        flpLocationActions.TabIndex = 0;
        flpLocationActions.WrapContents = false;

        // btnNewLocation
        btnNewLocation.BackColor = Color.FromArgb(243, 108, 33);
        btnNewLocation.FlatAppearance.BorderSize = 0;
        btnNewLocation.FlatStyle = FlatStyle.Flat;
        btnNewLocation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnNewLocation.ForeColor = Color.White;
        btnNewLocation.Margin = new Padding(4);
        btnNewLocation.Name = "btnNewLocation";
        btnNewLocation.Size = new Size(125, 40);
        btnNewLocation.TabIndex = 0;
        btnNewLocation.Text = "+ New Location";
        btnNewLocation.UseVisualStyleBackColor = false;

        // btnEditLocation
        btnEditLocation.BackColor = Color.FromArgb(29, 39, 54);
        btnEditLocation.FlatAppearance.BorderSize = 0;
        btnEditLocation.FlatStyle = FlatStyle.Flat;
        btnEditLocation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnEditLocation.ForeColor = Color.White;
        btnEditLocation.Margin = new Padding(4);
        btnEditLocation.Name = "btnEditLocation";
        btnEditLocation.Size = new Size(115, 40);
        btnEditLocation.TabIndex = 1;
        btnEditLocation.Text = "Edit Selected";
        btnEditLocation.UseVisualStyleBackColor = false;

        // btnLocationStatus
        btnLocationStatus.BackColor = Color.FromArgb(225, 229, 235);
        btnLocationStatus.FlatAppearance.BorderSize = 0;
        btnLocationStatus.FlatStyle = FlatStyle.Flat;
        btnLocationStatus.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        btnLocationStatus.ForeColor = Color.FromArgb(45, 55, 70);
        btnLocationStatus.Margin = new Padding(4);
        btnLocationStatus.Name = "btnLocationStatus";
        btnLocationStatus.Size = new Size(105, 40);
        btnLocationStatus.TabIndex = 2;
        btnLocationStatus.Text = "Deactivate";
        btnLocationStatus.UseVisualStyleBackColor = false;

        // dgvLocations
        dgvLocations.AllowUserToAddRows = false;
        dgvLocations.AllowUserToDeleteRows = false;
        dgvLocations.AutoGenerateColumns = false;
        dgvLocations.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
        dgvLocations.BackgroundColor = Color.White;
        dgvLocations.BorderStyle = BorderStyle.None;
        dgvLocations.ColumnHeadersHeight = 38;
        dgvLocations.Columns.Add(colLocationCode);
        dgvLocations.Columns.Add(colLocationName);
        dgvLocations.Columns.Add(colLocationType);
        dgvLocations.Columns.Add(colAddress);
        dgvLocations.Columns.Add(colContactName);
        dgvLocations.Columns.Add(colBillingLocation);
        dgvLocations.Columns.Add(colLocationActive);
        dgvLocations.Dock = DockStyle.Fill;
        dgvLocations.Location = new Point(12, 64);
        dgvLocations.MultiSelect = false;
        dgvLocations.Name = "dgvLocations";
        dgvLocations.ReadOnly = true;
        dgvLocations.RowHeadersVisible = false;
        dgvLocations.RowTemplate.Height = 36;
        dgvLocations.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        dgvLocations.Size = new Size(1088, 264);
        dgvLocations.TabIndex = 1;

        // colLocationCode
        colLocationCode.DataPropertyName = "LocationCode";
        colLocationCode.FillWeight = 80F;
        colLocationCode.HeaderText = "LOCATION #";
        colLocationCode.Name = "colLocationCode";
        colLocationCode.ReadOnly = true;

        // colLocationName
        colLocationName.DataPropertyName = "LocationName";
        colLocationName.FillWeight = 125F;
        colLocationName.HeaderText = "NAME";
        colLocationName.Name = "colLocationName";
        colLocationName.ReadOnly = true;

        // colLocationType
        colLocationType.DataPropertyName = "LocationType";
        colLocationType.FillWeight = 70F;
        colLocationType.HeaderText = "TYPE";
        colLocationType.Name = "colLocationType";
        colLocationType.ReadOnly = true;

        // colAddress
        colAddress.DataPropertyName = "Address";
        colAddress.FillWeight = 180F;
        colAddress.HeaderText = "ADDRESS";
        colAddress.Name = "colAddress";
        colAddress.ReadOnly = true;

        // colContactName
        colContactName.DataPropertyName = "ContactName";
        colContactName.FillWeight = 90F;
        colContactName.HeaderText = "CONTACT";
        colContactName.Name = "colContactName";
        colContactName.ReadOnly = true;

        // colBillingLocation
        colBillingLocation.DataPropertyName = "IsBillingLocation";
        colBillingLocation.FillWeight = 55F;
        colBillingLocation.HeaderText = "BILLING";
        colBillingLocation.Name = "colBillingLocation";
        colBillingLocation.ReadOnly = true;

        // colLocationActive
        colLocationActive.DataPropertyName = "IsActive";
        colLocationActive.FillWeight = 50F;
        colLocationActive.HeaderText = "ACTIVE";
        colLocationActive.Name = "colLocationActive";
        colLocationActive.ReadOnly = true;

        // tabRecentLoads
        tabRecentLoads.BackColor = Color.White;
        tabRecentLoads.Controls.Add(dgvLoads);
        tabRecentLoads.Location = new Point(4, 32);
        tabRecentLoads.Name = "tabRecentLoads";
        tabRecentLoads.Padding = new Padding(12);
        tabRecentLoads.Size = new Size(1112, 340);
        tabRecentLoads.TabIndex = 1;
        tabRecentLoads.Text = "Recent Loads";

        // dgvLoads
        dgvLoads.AllowUserToAddRows = false;
        dgvLoads.AllowUserToDeleteRows = false;
        dgvLoads.AutoGenerateColumns = false;
        dgvLoads.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
        dgvLoads.BackgroundColor = Color.White;
        dgvLoads.BorderStyle = BorderStyle.None;
        dgvLoads.ColumnHeadersHeight = 38;
        dgvLoads.Columns.Add(colLoadNumber);
        dgvLoads.Columns.Add(colLoadDescription);
        dgvLoads.Columns.Add(colLoadStatus);
        dgvLoads.Columns.Add(colLoadRevenue);
        dgvLoads.Columns.Add(colLoadCreated);
        dgvLoads.Dock = DockStyle.Fill;
        dgvLoads.Location = new Point(12, 12);
        dgvLoads.MultiSelect = false;
        dgvLoads.Name = "dgvLoads";
        dgvLoads.ReadOnly = true;
        dgvLoads.RowHeadersVisible = false;
        dgvLoads.RowTemplate.Height = 36;
        dgvLoads.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        dgvLoads.Size = new Size(1088, 316);
        dgvLoads.TabIndex = 0;

        // colLoadNumber
        colLoadNumber.DataPropertyName = "LoadNumber";
        colLoadNumber.FillWeight = 80F;
        colLoadNumber.HeaderText = "LOAD";
        colLoadNumber.Name = "colLoadNumber";
        colLoadNumber.ReadOnly = true;

        // colLoadDescription
        colLoadDescription.DataPropertyName = "Description";
        colLoadDescription.FillWeight = 180F;
        colLoadDescription.HeaderText = "DESCRIPTION";
        colLoadDescription.Name = "colLoadDescription";
        colLoadDescription.ReadOnly = true;

        // colLoadStatus
        colLoadStatus.DataPropertyName = "LoadStatus";
        colLoadStatus.FillWeight = 80F;
        colLoadStatus.HeaderText = "STATUS";
        colLoadStatus.Name = "colLoadStatus";
        colLoadStatus.ReadOnly = true;

        // colLoadRevenue
        colLoadRevenue.DataPropertyName = "RevenueAmount";
        colLoadRevenue.FillWeight = 75F;
        colLoadRevenue.HeaderText = "REVENUE";
        colLoadRevenue.Name = "colLoadRevenue";
        colLoadRevenue.ReadOnly = true;

        // colLoadCreated
        colLoadCreated.DataPropertyName = "CreatedAtUtc";
        colLoadCreated.FillWeight = 95F;
        colLoadCreated.HeaderText = "CREATED";
        colLoadCreated.Name = "colLoadCreated";
        colLoadCreated.ReadOnly = true;

        // pnlFooter
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(lblMessage);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 718);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1120, 42);
        pnlFooter.TabIndex = 4;

        // lblMessage
        lblMessage.AutoSize = true;
        lblMessage.Font = new Font("Segoe UI", 9F);
        lblMessage.ForeColor = Color.FromArgb(106, 116, 130);
        lblMessage.Location = new Point(28, 10);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(50, 20);
        lblMessage.TabIndex = 0;
        lblMessage.Text = "Ready";

        // CustomerDetailsForm
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 249);
        CancelButton = btnClose;
        ClientSize = new Size(1120, 760);
        Controls.Add(tabCustomerDetails);
        Controls.Add(tlpInformation);
        Controls.Add(tlpSummary);
        Controls.Add(pnlHeader);
        Controls.Add(pnlFooter);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(1000, 700);
        Name = "CustomerDetailsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FleetFlow — Customer Details";

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        flpCustomerActions.ResumeLayout(false);
        tlpSummary.ResumeLayout(false);
        pnlLoadsSummary.ResumeLayout(false);
        pnlLoadsSummary.PerformLayout();
        pnlOpenLoadsSummary.ResumeLayout(false);
        pnlOpenLoadsSummary.PerformLayout();
        pnlRevenueSummary.ResumeLayout(false);
        pnlRevenueSummary.PerformLayout();
        tlpInformation.ResumeLayout(false);
        pnlContact.ResumeLayout(false);
        pnlContact.PerformLayout();
        pnlEmail.ResumeLayout(false);
        pnlEmail.PerformLayout();
        pnlPhone.ResumeLayout(false);
        pnlPhone.PerformLayout();
        pnlCreated.ResumeLayout(false);
        pnlCreated.PerformLayout();
        pnlUpdated.ResumeLayout(false);
        pnlUpdated.PerformLayout();
        tabCustomerDetails.ResumeLayout(false);
        tabLocations.ResumeLayout(false);
        tabRecentLoads.ResumeLayout(false);
        flpLocationActions.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLocations).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLoads).EndInit();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ResumeLayout(false);
    }
}