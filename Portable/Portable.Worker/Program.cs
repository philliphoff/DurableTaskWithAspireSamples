using System;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Worker;
using Microsoft.DurableTask.Worker.AzureManaged;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddServiceDiscovery();

builder
    .Services
    .AddHttpClient("Echo",
        client => client.BaseAddress = new Uri("https+http://webapi"))
    .AddServiceDiscovery();

builder.Services.AddDurableTaskWorker(
    workerBuilder =>
    {
        workerBuilder.AddTasks(r => r.AddAllGeneratedTasks());
        workerBuilder.UseDurableTaskScheduler(
            builder.Configuration.GetConnectionString("taskhub") ?? throw new InvalidOperationException("Scheduler connection string not configured."),
            options =>
            {
                options.AllowInsecureCredentials = true;
            });
    });

var host = builder.Build();

host.Run();
