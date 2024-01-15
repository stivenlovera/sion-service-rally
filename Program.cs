using System.Reflection;
using Serilog;
using service_rally_diciembre_2023;
//System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

//loguen en entorno produccion /Bin
Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

//serilog configuracion
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Debug)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/Log-.log", Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u15}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args).UseWindowsService();
host.UseSerilog(Log.Logger);

var startup = new Startup(host);
startup.ConfigureServices();

var app = host.Build();

try
{
    Log.Information("SERVICIO DE RALLY DICIEMBRE 2023");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return;
}
