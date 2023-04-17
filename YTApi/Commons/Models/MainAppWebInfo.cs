using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// MainAppWebInfo 類別
/// </summary>
public class MainAppWebInfo
{
    [JsonPropertyName("graftUrl")]
    public string? GraftUrl { get; set; }

    [JsonPropertyName("pwaInstallabilityStatus")]
    public string? PwaInstallabilityStatus { get; set; }

    [JsonPropertyName("webDisplayMode")]
    public string? WebDisplayMode { get; set; }

    [JsonPropertyName("isWebNativeShareAvailable")]
    public bool? IsWebNativeShareAvailable { get; set; }
}