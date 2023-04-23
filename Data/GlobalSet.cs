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
        /// 網頁瀏覽器類型
        /// </summary>
        public static readonly string BrowserType = "BrowserType";

        /// <summary>
        /// 設定檔名稱
        /// </summary>
        public static readonly string ProfileName = "ProfileName";

        /// <summary>
        /// Cookies
        /// </summary>
        public static readonly string Cookies = "Cookies";

        /// <summary>
        /// 使用 Cookies
        /// </summary>
        public static readonly string UseCookies = "UseCookies";

        /// <summary>
        /// 啟用設定資料統一資源標識符
        /// </summary>
        public static readonly string EnableSetDataUri = "EnableSetDataUri";
    }

    /// <summary>
    /// 檔案
    /// </summary>
    public static class Files
    {
        /// <summary>
        /// Channels.json
        /// </summary>
        public static readonly string ChannelsJson = Path.Combine(FileSystem.Current.AppDataDirectory, "Channels.json");

        /// <summary>
        /// Webhooks.json
        /// </summary>
        public static readonly string WebhooksJson = Path.Combine(FileSystem.Current.AppDataDirectory, "Webhooks.json");
    }
}