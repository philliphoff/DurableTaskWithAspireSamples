using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DurableTask.AzureManagedBackend;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DTFx.Tasks.Echo;
using DurableTask.Core;

namespace DTFx;

sealed class ServiceCollectionObjectCreator<T> : ObjectCreator<T>
{
    readonly IServiceProvider serviceProvider;
    readonly Type type;

    public ServiceCollectionObjectCreator(string name, string version, Type type, IServiceProvider serviceProvider)
    {
        Name = name;
        Version = version;
        
        this.type = type;
        this.serviceProvider = serviceProvider;
    }

    public override T Create()
    {
        if (serviceProvider.GetService(this.type) is T obj)
        {
            return obj;
        }

        throw new InvalidOperationException($"Unable to create {typeof(T).Name}.");
    }
}

public class Worker : IHostedService
{
    readonly TaskHubWorker worker;

    public Worker(IConfiguration configuration, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        string address = configuration.GetConnectionString("taskhub");

        AzureManagedOrchestrationServiceOptions options = AzureManagedOrchestrationServiceOptions.FromConnectionString(address);

        AzureManagedOrchestrationService service = new(options, loggerFactory);

        worker = new(service, loggerFactory) { ErrorPropagationMode = ErrorPropagationMode.UseFailureDetails };

        worker.AddTaskActivities(
            [
                new ServiceCollectionObjectCreator<TaskActivity>("EchoActivity", String.Empty, typeof(EchoActivity), serviceProvider)
            ]);

        worker.AddTaskOrchestrations(
            [
                new NameValueObjectCreator<TaskOrchestration>("EchoOrchestrator", String.Empty, typeof(EchoOrchestrator))
            ]);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await worker.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await worker.StopAsync(true);
    }
}   
