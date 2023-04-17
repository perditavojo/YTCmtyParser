using System.Text.Json.Serialization;

namespace YTApi.Commons.Models;

/// <summary>
/// Client 類別
/// </summary>
public class Client
{
    [JsonPropertyName("hl")]
    public string? Hl { get; set; }

    [JsonPropertyName("gl")]
    public string? Gl { get; set; }

    [JsonPropertyName("remoteHost")]
    public string? RemoteHost { get; set; }

    [JsonPropertyName("deviceMake")]
    public string? DeviceMake { get; set; }

    [JsonPropertyName("deviceModel")]
    public string? DeviceModel { get; set; }

    [JsonPropertyName("visitorData")]
    public string? VisitorData { get; set; }

    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; set; }

    [JsonPropertyName("clientName")]
    public string? ClientName { get; set; }

    [JsonPropertyName("clientVersion")]
    public string? ClientVersion { get; set; }

    [JsonPropertyName("osName")]
    public string? OsName { get; set; }

    [JsonPropertyName("osVersion")]
    public string? OsVersion { get; set; }

    [JsonPropertyName("originalUrl")]
    public string? OriginalUrl { get; set; }

    [JsonPropertyName("platform")]
    public string? Platform { get; set; }

    [JsonPropertyName("clientFormFactor")]
    public string? ClientFormFactor { get; set; }

    [JsonPropertyName("configInfo")]
    public ConfigInfo? ConfigInfo { get; set; }

    [JsonPropertyName("timeZone")]
    public string? TimeZone { get; set; }

    [JsonPropertyName("browserName")]
    public string? BrowserName { get; set; }

    [JsonPropertyName("browserVersion")]
    public string? BrowserVersion { get; set; }

    [JsonPropertyName("acceptHeader")]
    public string? AcceptHeader { get; set; }

    [JsonPropertyName("deviceExperimentId")]
    public string? DeviceExperimentId { get; set; }

    [JsonPropertyName("screenWidthPoints")]
    public int ScreenWidthPoints { get; set; }

    [JsonPropertyName("screenHeightPoints")]
    public int ScreenHeightPoints { get; set; }

    [JsonPropertyName("screenPixelDensity")]
    public int ScreenPixelDensity { get; set; }

    [JsonPropertyName("screenDensityFloat")]
    public int ScreenDensityFloat { get; set; }

    [JsonPropertyName("utcOffsetMinutes")]
    public int UtcOffsetMinutes { get; set; }

    [JsonPropertyName("userInterfaceTheme")]
    public string? UserInterfaceTheme { get; set; }

    [JsonPropertyName("connectionType")]
    public string? ConnectionType { get; set; }

    [JsonPropertyName("memoryTotalKbytes")]
    public string? MemoryTotalKbytes { get; set; }

    [JsonPropertyName("mainAppWebInfo")]
    public MainAppWebInfo? MainAppWebInfo { get; set; }
}