using System.Text.Json.Serialization;

namespace Portable.Tasks.Echo;

public record EchoInput
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}
