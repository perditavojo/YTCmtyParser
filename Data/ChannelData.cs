using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// 頻道資料類別
/// </summary>
public class ChannelData
{
    /// <summary>
    /// 名稱
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 網址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}