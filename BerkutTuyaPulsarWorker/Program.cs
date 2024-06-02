using System.Net;
using System.Net.WebSockets;
using BerkutTuyaPulsarWorker;
using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Managers;
using BerkutTuyaPulsarWorker.Options;

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

builder.Services.Configure<WebSocketOptions>(builder.Configuration.GetSection(nameof(WebSocketOptions)));
builder.Services.AddSingleton<ITuyaWebSocket, TuyaWebSocketManager>();

var host = builder.Build();
host.Run();
