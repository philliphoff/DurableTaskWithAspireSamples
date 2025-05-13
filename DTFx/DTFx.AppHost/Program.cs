var builder = DistributedApplication.CreateBuilder(args);

var scheduler =
    builder.AddContainer("scheduler", "mcr.microsoft.com/dts/dts-emulator", "latest")
           .WithHttpEndpoint(name: "grpc", targetPort: 8080)
           .WithHttpEndpoint(name: "dashboard", targetPort: 8082);

var schedulerConnectionString = ReferenceExpression.Create($"Endpoint={scheduler.GetEndpoint("grpc")};TaskHub=default;Authentication=None");

var webApi =
    builder.AddProject<Projects.DTFx_WebApi>("webapi")
           .WithEnvironment("ConnectionStrings__taskhub", schedulerConnectionString);

builder.AddProject<Projects.DTFx_Worker>("worker")
       .WithReference(webApi)
       .WithEnvironment("ConnectionStrings__taskhub", schedulerConnectionString);

builder.Build().Run();
