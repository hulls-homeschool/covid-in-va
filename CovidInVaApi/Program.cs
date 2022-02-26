using CovidInVaApi.Services;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Open Data Portal Settings
    builder.Services.Configure<OpenDataPortalSettings>(options =>
    {
        var section = builder.Configuration.GetSection("OpenDataPortal");
        if (section.Exists())
        {
            options.AppToken = section.GetValue("AppToken", string.Empty);
            options.Url = section.GetValue("Url", string.Empty);
        }
    });

    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<IHostedService, OpenDataPortalService>();

    // NLog
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Stopped program due to exception");

    throw;
}
finally
{
    logger.Debug("Shutting down...");

    LogManager.Shutdown();
}
