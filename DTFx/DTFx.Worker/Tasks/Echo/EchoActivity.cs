using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DurableTask.Core;

namespace DTFx.Tasks.Echo;

public class EchoActivity(IHttpClientFactory clientFactory) : AsyncTaskActivity<EchoInput, string>
{
    protected override async Task<string> ExecuteAsync(TaskContext context, EchoInput input)
    {
        HttpClient client = clientFactory.CreateClient("Echo");

        var result = await client.PostAsync("/echo", JsonContent.Create(new EchoInput { Text = input.Text }));

        var output = await result.Content.ReadFromJsonAsync<EchoInput>();

        return output?.Text ?? throw new InvalidOperationException("Invalid response from echo service!");
    }
}
