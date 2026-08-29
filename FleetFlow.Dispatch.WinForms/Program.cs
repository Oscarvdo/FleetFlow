using FleetFlow.Dispatch.WinForms.Forms.Authentication;
using FleetFlow.Dispatch.WinForms.Forms.Main;
using FleetFlow.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FleetFlow.Dispatch.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        HostApplicationBuilder builder =
            Host.CreateApplicationBuilder(
                new HostApplicationBuilderSettings
                {
                    ContentRootPath = AppContext.BaseDirectory
                });

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: false);

        string connectionString =
            builder.Configuration["ConnectionStrings:FleetFlowDb"]
            ?? throw new InvalidOperationException(
                "Connection string 'FleetFlowDb' was not found.");

        builder.Services.AddFleetFlowInfrastructure(
            connectionString);

        builder.Services.AddTransient<LoginForm>();
       

        using IHost host = builder.Build();

        host.StartAsync()
            .GetAwaiter()
            .GetResult();

        try
        {
            using LoginForm loginForm =
                host.Services.GetRequiredService<LoginForm>();

            DialogResult loginResult =
                loginForm.ShowDialog();

            if (loginResult != DialogResult.OK ||
                loginForm.AuthenticatedSession is null)
            {
                return;
            }

            using MainForm mainForm =
     ActivatorUtilities.CreateInstance<MainForm>(
         host.Services,
         loginForm.AuthenticatedSession);

            System.Windows.Forms.Application.Run(mainForm);
        }
        finally
        {
            host.StopAsync()
                .GetAwaiter()
                .GetResult();
        }
    }
}