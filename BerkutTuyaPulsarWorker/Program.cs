using BerkutTuyaPulsarWorker;
using BerkutTuyaPulsarWorker.Decorators;
using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Managers;
using BerkutTuyaPulsarWorker.Options;
using BerkutTuyaPulsarWorker.Repositories;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Configuration.AddEnvironmentVariables();

using (var provider = builder.Services.BuildServiceProvider())
{
    var hostingEnvironment = provider.GetRequiredService<IHostEnvironment>();
    if (hostingEnvironment.IsDevelopment())
        builder.Configuration.AddUserSecrets<Program>();
}

var webSocketOptions = builder.Configuration.GetSection(nameof(WebSocketOptions)).Get<WebSocketOptions>();

builder.Services.Configure<BroadcasterOptions>(builder.Configuration.GetSection(nameof(BroadcasterOptions)));

builder.Services.AddHttpClient<BroadcasterRepository>();
builder.Services.AddTransient<IBroadcasterRepository, BroadcasterRepository>();
builder.Services.Decorate<IBroadcasterRepository, BroadcasterRepositoryLoggerDecorator>();

builder.Services.Configure<WebSocketOptions>(builder.Configuration.GetSection(nameof(WebSocketOptions)));
builder.Services.AddSingleton<ITuyaWebSocket, TuyaWebSocketManager>();
builder.Services.Decorate<ITuyaWebSocket, TuyaWebSocketManagerLoggerDecorator>();
builder.Services.AddApplicationInsightsTelemetryWorkerService();

var host = builder.Build();
host.Run();
