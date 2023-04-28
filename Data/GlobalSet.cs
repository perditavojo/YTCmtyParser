using YTCmtyParser.Commons.Utils;

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

    /// <summary>
    /// 函式
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="filePath">字串，檔案路徑</param>
        /// <param name="content">字串，文字內容</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Task</returns>
        public static async Task SaveToFile(
            string filePath,
            string content,
            CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    await AlertUtil.ShowToast($"檔案路徑不得為空。", ct);

                    return;
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                string fileName = Path.GetFileName(filePath);

                using FileStream fileStream = File.OpenWrite(filePath);
                using StreamWriter streamWriter = new(fileStream);

                await streamWriter.WriteAsync(content);

                await AlertUtil.ShowToast($"檔案 {fileName} 已儲存至：{filePath}。");
            }
            catch (Exception ex)
            {
                await AlertUtil.ShowErrorAlert(ex.Message);
            }
        }
    }
}