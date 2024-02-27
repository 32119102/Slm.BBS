using ContIn.Abp.Terminal.HttpApi.Host;
using ContIn.Abp.Terminal.HttpApi.Host.RedisConfiguration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
    logger.Enrich.FromLogContext();
});

// redis数据源的配置提供程序
//builder.Host.AddRedisConfiguration(options =>
//{
//    options.ConnectionString = "127.0.0.1:6379";
//    options.ConfigKey = "ContIn:Abp:Terminal:Redis:AppSettings";
//});

builder.Services.ReplaceConfiguration(builder.Configuration);
builder.Services.AddApplication<TerminalHttpApiHostModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();
