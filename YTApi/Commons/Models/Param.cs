using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// Param 類別
/// </summary>
public class Param
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}