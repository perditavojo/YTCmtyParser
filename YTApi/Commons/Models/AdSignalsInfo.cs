using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// AdSignalsInfo 類別
/// </summary>
public class AdSignalsInfo
{
    [JsonPropertyName("params")]
    public List<Param>? Params { get; set; }
}