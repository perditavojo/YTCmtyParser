using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// ConfigInfo 類別
/// </summary>
public class ConfigInfo
{
    [JsonPropertyName("appInstallData")]
    public string? AppInstallData { get; set; }
}