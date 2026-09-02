namespace FleetFlow.Dispatch.WinForms.Controls.Fleet;

partial class FleetControl
{
    private Panel pnlHeader = null!;
    private Label lblStatus = null!;
    private TextBox txtSearch = null!;
    private CheckBox chkIncludeInactive = null!;
    private Button btnRefresh = null!;
    private Button btnNewVehicle = null!;
    private Button btnEditVehicle = null!;
    private Label lblVehicles = null!;
    private Label lblVehicleCaption = null!;
    private Label lblTrailers = null!;
    private Label lblDrivers = null!;
    private Label lblMaintenance = null!;
    private DataGridView dgvVehicles = null!;
    private DataGridView dgvTrailers = null!;
    private DataGridView dgvDrivers = null!;

    private void InitializeComponent()
    {
        pnlHeader = new Panel { BackColor = Color.White, Dock = DockStyle.Top, Height = 235 };
        var title = Label("Fleet", 32, 22, 22F, FontStyle.Bold, Color.FromArgb(29, 39, 54));
        var subtitle = Label("Fleet resources, operational availability, and active assignments.", 35, 80, 10F, FontStyle.Regular, Color.FromArgb(106, 116, 130));
        lblStatus = Label("Ready", 1040, 30, 9F, FontStyle.Regular, Color.FromArgb(106, 116, 130));
        lblStatus.AutoSize = false; lblStatus.Size = new Size(280, 26); lblStatus.TextAlign = ContentAlignment.MiddleRight;
        btnEditVehicle = new Button { Text="Edit Selected", Location=new Point(1030,72), Size=new Size(125,34), FlatStyle=FlatStyle.Flat, BackColor=Color.FromArgb(29,39,54), ForeColor=Color.White };
        btnNewVehicle = new Button { Text="+ New Vehicle", Location=new Point(1162,72), Size=new Size(145,34), FlatStyle=FlatStyle.Flat, BackColor=Color.FromArgb(243,108,33), ForeColor=Color.White };
        var cards = new TableLayoutPanel { Location = new Point(32, 113), Size = new Size(930, 75), ColumnCount = 4 };
        for (var i = 0; i < 4; i++) cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        lblVehicles = Card(cards, "VEHICLES", 0); lblVehicleCaption = Label("Available vehicles", 10, 47, 8F, FontStyle.Regular, Color.FromArgb(106,116,130));
        ((Panel)cards.GetControlFromPosition(0, 0)!).Controls.Add(lblVehicleCaption);
        lblTrailers = Card(cards, "TRAILERS", 1); lblDrivers = Card(cards, "DRIVERS", 2); lblMaintenance = Card(cards, "IN MAINTENANCE", 3);
        var actions = new TableLayoutPanel { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom, ColumnCount = 3, Location = new Point(32, 193), Size = new Size(1000, 32) };
        actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170)); actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        txtSearch = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Search unit, driver, type, status or active trip..." };
        chkIncludeInactive = new CheckBox { Dock = DockStyle.Fill, Text = "Include inactive" };
        btnRefresh = new Button { Dock = DockStyle.Fill, FlatStyle = FlatStyle.Flat, Text = "Refresh" };
        actions.Controls.Add(txtSearch,0,0); actions.Controls.Add(chkIncludeInactive,1,0); actions.Controls.Add(btnRefresh,2,0);
        pnlHeader.Controls.AddRange([title, subtitle, lblStatus, btnEditVehicle, btnNewVehicle, cards, actions]);
        var tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F) };
        var vehiclePage = Page("Vehicles", out dgvVehicles); var trailerPage = Page("Trailers", out dgvTrailers); var driverPage = Page("Drivers", out dgvDrivers);
        tabs.TabPages.AddRange([vehiclePage,trailerPage,driverPage]);
        BackColor = Color.FromArgb(244,246,249); Controls.Add(tabs); Controls.Add(pnlHeader); Name="FleetControl"; Size=new Size(1371,960);
    }

    private static TabPage Page(string text, out DataGridView grid)
    {
        var page = new TabPage(text) { BackColor = Color.White, Padding = new Padding(12) };
        grid = new DataGridView { AllowUserToAddRows=false, AllowUserToDeleteRows=false, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor=Color.White, BorderStyle=BorderStyle.None, Dock=DockStyle.Fill, MultiSelect=false, ReadOnly=true, RowHeadersVisible=false, SelectionMode=DataGridViewSelectionMode.FullRowSelect };
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor=Color.FromArgb(29,39,54), ForeColor=Color.White, Font=new Font("Segoe UI",9F,FontStyle.Bold), SelectionBackColor=Color.FromArgb(29,39,54) }; grid.EnableHeadersVisualStyles=false; grid.ColumnHeadersHeight=40; grid.RowTemplate.Height=38;
        page.Controls.Add(grid); return page;
    }
    private static Label Label(string text,int left,int top,float size,FontStyle style,Color color) => new() { AutoSize=true, Text=text, Location=new Point(left,top), Font=new Font("Segoe UI",size,style), ForeColor=color };
    private static Label Card(TableLayoutPanel cards,string caption,int column) { var p=new Panel { Dock=DockStyle.Fill, BackColor=Color.FromArgb(249,250,252) }; p.Controls.Add(Label(caption,10,9,8F,FontStyle.Bold,Color.FromArgb(106,116,130))); var value=Label("0",10,28,17F,FontStyle.Bold,Color.FromArgb(29,39,54)); p.Controls.Add(value); cards.Controls.Add(p,column,0); return value; }
}
