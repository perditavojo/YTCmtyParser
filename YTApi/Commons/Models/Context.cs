using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// Context 類別
/// </summary>
public class Context
{
    [JsonPropertyName("client")]
    public Client? Client { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("request")]
    public Request? Request { get; set; }

    [JsonPropertyName("clickTracking")]
    public ClickTracking? ClickTracking { get; set; }

    [JsonPropertyName("adSignalsInfo")]
    public AdSignalsInfo? AdSignalsInfo { get; set; }
}