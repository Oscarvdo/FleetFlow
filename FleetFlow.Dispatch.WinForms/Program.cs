using Microsoft.Extensions.Hosting;

using FleetFlow.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
 

namespace FleetFlow.Dispatch.WinForms
{
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

            string connectionString =
                builder.Configuration.GetConnectionString("FleetFlowDb")
                ?? throw new InvalidOperationException(
                    "Connection string 'FleetFlowDb' was not found.");

            builder.Services.AddFleetFlowInfrastructure(
                connectionString);

            builder.Services.AddTransient<Form1>();

            using IHost host = builder.Build();

            host.StartAsync()
                .GetAwaiter()
                .GetResult();

            try
            {
                Form1 initialForm =
                    host.Services.GetRequiredService<Form1>();

                System.Windows.Forms.Application.Run(initialForm);
            }
            finally
            {
                host.StopAsync()
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}