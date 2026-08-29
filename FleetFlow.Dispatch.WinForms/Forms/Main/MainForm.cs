using FleetFlow.Application.Authentication;

namespace FleetFlow.Dispatch.WinForms.Forms.Main;

public partial class MainForm : Form
{
    private readonly UserSession? _session;

    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(UserSession session)
        : this()
    {
        _session = session;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_session is not null)
        {
            Text = $"FleetFlow — {_session.User.Username}";
        }
    }
}