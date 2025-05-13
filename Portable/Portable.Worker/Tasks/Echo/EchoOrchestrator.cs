using System.Threading.Tasks;
using Microsoft.DurableTask;

namespace Portable.Tasks.Echo;

[DurableTask("Echo")]
public class EchoOrchestrator : TaskOrchestrator<EchoInput, string>
{
    public override async Task<string> RunAsync(TaskOrchestrationContext context, EchoInput input)
    {
        string output = await context.CallEchoActivityAsync(input);

        output = await context.CallEchoActivityAsync(new() { Text = output });

        output = await context.CallEchoActivityAsync(new() { Text = output });

        return output;
    }
}
