using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DTFx;

public record EchoInput
{
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}
