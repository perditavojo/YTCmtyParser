namespace YTCmtyParser.Data;

/// <summary>
/// 全域組
/// </summary>
public static class GlobalSet
{
    /// <summary>
    /// 鍵值
    /// </summary>
    public static class Keys
    {
        /// <summary>
        /// BrowserType
        /// </summary>
        public static readonly string BrowserType = "BrowserType";

        /// <summary>
        /// ProfileName
        /// </summary>
        public static readonly string ProfileName = "ProfileName";

        /// <summary>
        /// Cookies
        /// </summary>
        public static readonly string Cookies = "Cookies";

        /// <summary>
        /// UseCookies
        /// </summary>
        public static readonly string UseCookies = "UseCookies";
    }

    /// <summary>
    /// 檔案
    /// </summary>
    public static class Files
    {
        /// <summary>
        /// Webhooj.json
        /// </summary>
        public static readonly string WebhooksJson = Path.Combine(FileSystem.Current.AppDataDirectory, "Webhooks.json");
    }
}