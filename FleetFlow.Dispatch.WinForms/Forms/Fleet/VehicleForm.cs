using FleetFlow.Application.Abstractions.Fleet;
using FleetFlow.Application.Fleet;

namespace FleetFlow.Dispatch.WinForms.Forms.Fleet;

public sealed class VehicleForm : Form
{
    private readonly IFleetCommandService _service;
    private readonly FleetOverviewVehicleItem? _vehicle;
    private readonly TextBox _unit = Field("Unit number *", 22, 78, 270);
    private readonly TextBox _vin = Field("VIN *", 318, 78, 270);
    private readonly TextBox _year = Field("Model year *", 22, 150, 170);
    private readonly TextBox _make = Field("Make *", 216, 150, 180);
    private readonly TextBox _model = Field("Model *", 420, 150, 168);
    private readonly TextBox _plate = Field("License plate *", 22, 222, 270);
    private readonly TextBox _state = Field("Plate state *", 318, 222, 120);
    private readonly TextBox _payload = Field("Max payload lbs *", 462, 222, 126);
    private readonly TextBox _odometer = Field("Odometer miles *", 22, 294, 270);
    private readonly ComboBox _status = new() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(318, 319), Size = new Size(270, 30) };

    public VehicleForm(IFleetCommandService service, FleetOverviewVehicleItem? vehicle = null)
    {
        _service = service; _vehicle = vehicle;
        Text = vehicle is null ? "FleetFlow — New Vehicle" : $"FleetFlow — Edit {vehicle.UnitNumber}";
        ClientSize = new Size(620, 455); StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; BackColor = Color.FromArgb(244,246,249);
        Controls.Add(new Label { Text = vehicle is null ? "New Vehicle" : "Edit Vehicle", Font = new Font("Segoe UI",22,FontStyle.Bold), ForeColor=Color.FromArgb(29,39,54), AutoSize=true, Location=new Point(22,18) });
        var panel=new Panel { BackColor=Color.White, Location=new Point(16,58), Size=new Size(588,315) }; Controls.Add(panel);
        foreach(Control c in new Control[]{_unit,_vin,_year,_make,_model,_plate,_state,_payload,_odometer}) panel.Controls.Add(c);
        panel.Controls.Add(new Label { Text="Operational status *", Font=new Font("Segoe UI",9,FontStyle.Bold), AutoSize=true, Location=new Point(318,294) }); panel.Controls.Add(_status);
        _status.Items.AddRange(["AVAILABLE","ASSIGNED","IN_TRANSIT","MAINTENANCE","OUT_OF_SERVICE"]); _status.SelectedItem="AVAILABLE";
        var save=new Button { Text="Save Vehicle", Location=new Point(474,395), Size=new Size(130,38), BackColor=Color.FromArgb(243,108,33), ForeColor=Color.White, FlatStyle=FlatStyle.Flat }; save.Click += Save;
        var cancel=new Button { Text="Cancel", Location=new Point(365,395), Size=new Size(100,38) }; cancel.Click += (_,_)=>Close(); Controls.AddRange([save,cancel]);
        if(vehicle is not null) { _unit.Text=vehicle.UnitNumber; _vin.Text=vehicle.Vin; _year.Text=vehicle.ModelYear.ToString(); _make.Text=vehicle.Make; _model.Text=vehicle.Model; _plate.Text=vehicle.LicensePlate; _state.Text=vehicle.LicenseState; _payload.Text=vehicle.MaxPayloadLbs.ToString(); _odometer.Text=vehicle.CurrentOdometerMiles.ToString(); _status.SelectedItem=vehicle.StatusCode; }
    }

    private async void Save(object? sender, EventArgs e)
    {
        if(!short.TryParse(_year.Text,out short year)||!decimal.TryParse(_payload.Text,out decimal payload)||!decimal.TryParse(_odometer.Text,out decimal odometer)) { MessageBox.Show("Enter valid numbers for year, payload and odometer.","FleetFlow"); return; }
        try { await _service.SaveVehicleAsync(new SaveVehicleRequest { VehicleId=_vehicle?.VehicleId, UnitNumber=_unit.Text, Vin=_vin.Text, ModelYear=year, Make=_make.Text, Model=_model.Text, LicensePlate=_plate.Text, LicenseState=_state.Text, MaxPayloadLbs=payload, CurrentOdometerMiles=odometer, StatusCode=_status.SelectedItem?.ToString()??"AVAILABLE", ExpectedRowVersion=_vehicle?.RowVersion }); DialogResult=DialogResult.OK; Close(); }
        catch(Exception ex) { MessageBox.Show($"Vehicle could not be saved.\n\n{ex.Message}","FleetFlow",MessageBoxButtons.OK,MessageBoxIcon.Error); }
    }
    private static TextBox Field(string caption,int left,int top,int width) { var t=new TextBox { Location=new Point(left,top+25), Size=new Size(width,30) }; t.Tag=new Label { Text=caption, Font=new Font("Segoe UI",9,FontStyle.Bold), Location=new Point(left,top), AutoSize=true }; return t; }
    protected override void OnLoad(EventArgs e) { base.OnLoad(e); foreach(var t in Controls.OfType<Panel>().SelectMany(p=>p.Controls.OfType<TextBox>())) if(t.Tag is Label l) ((Panel)t.Parent!).Controls.Add(l); }
}
