using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cronos;
using service_rally_diciembre_2023.Modules;

namespace service_rally_diciembre_2023.services
{
    public class ActualizacionRallyService : BackgroundService
    {
        private const string schedule = "*/2 * * * *"; 
        private readonly ILogger<ActualizacionRallyService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CronExpression cron;
        public ActualizacionRallyService(
            ILogger<ActualizacionRallyService> logger,
            IServiceProvider serviceProvider
            )
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
            this.cron = CronExpression.Parse(schedule);
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("INICIANDO SERVICIO...");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("SERVICIO INTERRUMPIDO...");
            await base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("INICIO DE SERVICIO : {time}", DateTimeOffset.Now);
                var utcNow = DateTime.UtcNow;
                var nextUtc = cron.GetNextOccurrence(utcNow);
                await Task.Delay(nextUtc.Value - utcNow, stoppingToken);
                try
                {
                    using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        await scope.ServiceProvider.GetRequiredService<ActualizacionRallyModulo>().ActualizacionRally();
                    }
                }
                catch (System.Exception ex)
                {
                    this._logger.LogCritical($"Error generado {ex.Message}");
                }
            }
        }
    }
}

