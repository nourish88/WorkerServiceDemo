using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace WorkerServiceDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client= new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Dispose();
            Log.Logger.Information("The service has been stopped ....");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _client.GetAsync("https://www.bizdiyetteyiz.com/");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"The website is up. Status code {result.StatusCode}");
                }
                else
                {
                    _logger.LogError($"The website is down. Status code{result.StatusCode}");
                }
                await Task.Delay(5*1000, stoppingToken);
            }
        }
    }
}
