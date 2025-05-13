using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DurableTask.AzureManagedBackend;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using DurableTask.Core;
using DTFx;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapPost("/create", async ([FromBody] EchoInput value, [FromServices] IConfiguration configuration, [FromServices] ILoggerFactory loggerFactory) =>
    {
        string address = configuration.GetConnectionString("taskhub");

        AzureManagedOrchestrationServiceOptions options = AzureManagedOrchestrationServiceOptions.FromConnectionString(address);

        AzureManagedOrchestrationService service = new(options, loggerFactory);

        TaskHubClient client = new(service);

        OrchestrationInstance instance = await client.CreateOrchestrationInstanceAsync(
            "EchoOrchestrator",
            String.Empty,
            value);

        await client.WaitForOrchestrationAsync(instance, Timeout.InfiniteTimeSpan);

        return Results.Ok();
    })
    .WithName("CreateOrchestration");

app.MapPost("/echo", ([FromBody] EchoInput value) =>
    {
        return new EchoInput { Text = $"Echoed: {value.Text}" };
    })
    .WithName("EchoText");

app.Run();
