using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DTFx;
using DTFx.Tasks.Echo;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddServiceDiscovery();

builder
    .Services
    .AddHttpClient("Echo",
        client => client.BaseAddress = new Uri("https+http://webapi"))
    .AddServiceDiscovery();

builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<EchoActivity>();

var host = builder.Build();

host.Run();
