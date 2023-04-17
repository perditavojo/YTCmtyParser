using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// Request 類別
/// </summary>
public class Request
{
    [JsonPropertyName("useSsl")]
    public bool UseSsl { get; set; }

    [JsonPropertyName("internalExperimentFlags")]
    public List<object>? InternalExperimentFlags { get; set; }

    [JsonPropertyName("consistencyTokenJars")]
    public List<object>? ConsistencyTokenJars { get; set; }
}