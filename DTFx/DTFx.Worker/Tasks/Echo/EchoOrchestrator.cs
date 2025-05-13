using System;
using System.Threading.Tasks;
using DurableTask.Core;

namespace DTFx.Tasks.Echo;

public class EchoOrchestrator : TaskOrchestration<string, EchoInput>
{
    public override async Task<string> RunTask(OrchestrationContext context, EchoInput input)
    {
        string output = await context.ScheduleTask<string>("EchoActivity", String.Empty, input);

        output = await context.ScheduleTask<string>("EchoActivity", String.Empty, input);

        output = await context.ScheduleTask<string>("EchoActivity", String.Empty, input);

        return output;
    }
}
