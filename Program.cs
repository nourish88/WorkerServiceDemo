using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace WorkerServiceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Users\aktas\source\repos\WorkerServiceDemoApp\WorkerServiceDemo\Logs\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Startimg up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception e)
            {
                Log.Fatal(e, "There was a problem starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();//Ensures that if there is any message in the buffer. 
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                }).UseSerilog();
    }
}
