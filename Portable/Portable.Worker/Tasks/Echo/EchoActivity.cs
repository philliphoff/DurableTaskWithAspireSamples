using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.DurableTask;

namespace Portable.Tasks.Echo;

[DurableTask("EchoActivity")]
public class EchoActivity(IHttpClientFactory clientFactory) : TaskActivity<EchoInput, string>
{
    public override async Task<string> RunAsync(TaskActivityContext context, EchoInput input)
    {
        HttpClient client = clientFactory.CreateClient("Echo");

        var result = await client.PostAsync("/echo", JsonContent.Create(new EchoInput { Text = input.Text }));

        var output = await result.Content.ReadFromJsonAsync<EchoInput>();

        return output?.Text ?? throw new InvalidOperationException("Invalid response from echo service!");
    }
}
