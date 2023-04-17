using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// ClickTracking 類別
/// </summary>
public class ClickTracking
{
    [JsonPropertyName("clickTrackingParams")]
    public string? ClickTrackingParams { get; set; }
}