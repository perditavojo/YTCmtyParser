using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// 附件資料類別
/// </summary>
public class AttachmentData
{
    /// <summary>
    /// 網址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// 是否為影片
    /// </summary>
    [JsonPropertyName("isVideo")]
    public bool IsVideo { get; set; } = false;

    /// <summary>
    /// 影片資料
    /// </summary>
    [JsonPropertyName("videoData")]
    public VideoData? VideoData { get; set; }
}