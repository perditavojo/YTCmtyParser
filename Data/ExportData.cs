using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// 匯出資料類別
/// </summary>
public class ExportData
{
    /// <summary>
    /// 網址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// 貼文資料
    /// </summary>
    [JsonPropertyName("postDatas")]
    public List<PostData>? PostDatas { get; set; }
}