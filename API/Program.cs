using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IHostBuilder hostBuilder = CreateHostBuilder(args);
                IHost host = hostBuilder.Build();
                Log.Information("Starting host");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .WriteTo.MSSqlServer(
                            connectionString: hostingContext.Configuration.GetConnectionString("AppDbContext"),
                            tableName: "Log",
                            columnOptions: new ColumnOptions(),
                            appConfiguration: hostingContext.Configuration,
                            autoCreateSqlTable: true
                        );
                    });
                });
    }
}