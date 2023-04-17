using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// Run 類別
/// </summary>
public class RunData
{
    /// <summary>
    /// 文字
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// 網址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// 是否為連結
    /// </summary>
    [JsonPropertyName("isLink")]
    public bool IsLink { get; set; } = false;
}