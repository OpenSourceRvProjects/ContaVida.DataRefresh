using ContaVida.DataRefresh.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

        //services.AddDbContext<SourceDbContext>(options =>
        //    options.UseSqlServer(
        //        context.Configuration.GetConnectionString("SourceDatabase")));

        //services.AddDbContext<TargetDbContext>(options =>
        //    options.UseSqlServer(
        //        context.Configuration.GetConnectionString("TargetDatabase")));

        services.AddTransient<IDataRefreshService, DataRefreshService>();
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var environment = host.Services.GetRequiredService<IHostEnvironment>();
var configuration = host.Services.GetRequiredService<IConfiguration>();

bool isProduction = configuration.GetValue<bool>("isProduction");
string serverID = configuration.GetValue<string>("serverID");

logger.LogInformation("========================================");
logger.LogInformation("Mirror server data sync WebJob starting");
logger.LogInformation("Environment: {Environment}", environment.EnvironmentName);
logger.LogInformation("Production: {IsProduction}", isProduction);
logger.LogInformation("ServerID: {serverID}", serverID);
logger.LogInformation("Start Time: {Time}", DateTime.Now);
logger.LogInformation("========================================");


try
{
    using var scope = host.Services.CreateScope();

    var dataRefreshService = scope.ServiceProvider.GetRequiredService<IDataRefreshService>();
    await dataRefreshService.RunDataRefresh();

}
catch (Exception ex)
{
    logger.LogError(ex, "Mirror WebJob failed.");
    throw; // Re-throw so Azure marks the job as Failed
}
finally
{
    logger.LogInformation("Mirror WebJob finished. End Time: {Time}", DateTime.Now);
}