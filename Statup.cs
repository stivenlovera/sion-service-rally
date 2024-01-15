using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using service_rally_diciembre_2023.Context;
using service_rally_diciembre_2023.Modules;
using service_rally_diciembre_2023.repository;
using service_rally_diciembre_2023.services;
using service_rally_diciembre_2023.Utils;

namespace service_rally_diciembre_2023
{
    public class Startup
    {
        private readonly IHostBuilder _hostBuilder;

        public Startup(IHostBuilder hostBuilder)
        {
            this._hostBuilder = hostBuilder;
        }

        public void ConfigureServices()
        {
            this._hostBuilder.ConfigureServices((hostContext, services) =>
            {
                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    Log.Warning("----------En ambiente de desarrollo");

                }
                else
                {

                }
                ServicesDataBase(services);
                ServicesRepository(services);
                ServicesModules(services);
                Services(services);
            });
        }
        public void ServicesDataBase(IServiceCollection services)
        {
            //Add services
            services.AddSingleton<DBComisionesContext>();
            services.AddSingleton<DBRallyDiciembre2023>();
            services.AddTransient<EmailHttpClient>();
            services.AddTransient<EmailRequest>();
        }

        public void Services(IServiceCollection services)
        {
            //Add services
            services.AddHostedService<ActualizacionRallyService>();
        }

        /*Modulos*/
        public void ServicesModules(IServiceCollection services)
        {
            services.AddTransient<ActualizacionRallyModulo>();
        }

        /*Repositorio*/
        public void ServicesRepository(IServiceCollection services)
        {
            services.AddTransient<MiembrosRepository>();
            services.AddTransient<VwListaVentaGralRepository>();
            services.AddTransient<MiembroAvanceRepository>();
            services.AddTransient<AvanceRepository>();
        }
    }
}