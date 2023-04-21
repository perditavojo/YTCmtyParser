using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// 附件資料類別
/// </summary>
public partial class AttachmentData
{
    /// <summary>
    /// 資料統一資源標識符
    /// </summary>
    [JsonPropertyName("dataUri")]
    public string? DataUri { get; set; }
}