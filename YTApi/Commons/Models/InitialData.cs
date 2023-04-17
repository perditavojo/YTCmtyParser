using System.Text.Json;

namespace YTApi.Commons.Models;

/// <summary>
/// 初始資料類別
/// </summary>
public class InitialData
{
    /// <summary>
    /// HTTP 參照位址
    /// </summary>
    public string? Referer { get; set; }

    /// <summary>
    /// YTConfig
    /// </summary>
    public YTConfig? ConfigData { get; set; }

    /// <summary>
    /// JSON 資料
    /// </summary>
    public JsonElement? JsonData { get; set; }
}