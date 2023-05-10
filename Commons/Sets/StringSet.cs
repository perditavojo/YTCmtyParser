namespace YTCmtyParser.Commons.Sets;

/// <summary>
/// 字串組
/// </summary>
public static class StringSet
{
    /// <summary>
    /// 使用者代理字串
    /// </summary>
    public static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";

    /// <summary>
    /// Sec-CH-UA
    /// </summary>
    public static readonly string SecCHUA = "\"Google Chrome\";v=\"113\", \"Chromium\";v=\"113\", \"Not-A.Brand\";v=\"24\"";

    /// <summary>
    /// Sec-CH-UA-Mobile
    /// </summary>
    public static readonly string SecCHUAMobile = "?0";

    /// <summary>
    /// Sec-CH-UA-Platform
    /// </summary>
    public static readonly string SecCHUAPlatform = "Windows";

    /// <summary>
    /// Sec-Fetch-Site
    /// </summary>
    public static readonly string SecFetchSite = "same-origin";

    /// <summary>
    /// Sec-Fetch-Mode
    /// </summary>
    public static readonly string SecFetchMode = "same-origin";

    /// <summary>
    /// Sec-Fetch-User
    /// </summary>
    public static readonly string SecFetchUser = string.Empty;

    /// <summary>
    /// Sec-Fetch-Dest
    /// </summary>
    public static readonly string SecFetchDest = "empty";
}