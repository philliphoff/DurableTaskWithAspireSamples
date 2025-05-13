var builder = DistributedApplication.CreateBuilder(args);

var scheduler =
    builder.AddContainer("scheduler", "mcr.microsoft.com/dts/dts-emulator", "latest")
           .WithHttpEndpoint(name: "grpc", targetPort: 8080)
           .WithHttpEndpoint(name: "dashboard", targetPort: 8082);

var schedulerConnectionString = ReferenceExpression.Create($"Endpoint={scheduler.GetEndpoint("grpc")};Authentication=None");

builder.AddAzureFunctionsProject<Projects.DurableFunctions_FunctionsHost>("functions")
       .WithExternalHttpEndpoints()
       .WithEnvironment("DURABLE_TASK_SCHEDULER_CONNECTION_STRING", schedulerConnectionString)
       .WithEnvironment("TASKHUB_NAME", "default");

builder.Build().Run();
