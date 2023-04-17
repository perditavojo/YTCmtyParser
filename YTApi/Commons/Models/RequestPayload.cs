using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// RequestPayload 類別
/// </summary>
public class RequestPayload
{
    [JsonPropertyName("context")]
    public Context? Context { get; set; }

    [JsonPropertyName("continuation")]
    public string? Continuation { get; set; }
}