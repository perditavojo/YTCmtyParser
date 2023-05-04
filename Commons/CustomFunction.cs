using CommunityToolkit.Maui.Storage;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text.Json;
using YTApi.Commons.Models;
using YTCmtyParser.Commons.Sets;
using YTCmtyParser.Commons.Utils;

namespace YTCmtyParser.Commons;

/// <summary>
/// 自定義函式
/// </summary>
public static class CustomFunction
{
    #region 權限

    /// <summary>
    /// 檢查權限
    /// </summary>
    /// <returns>Task</returns>
    public static async Task CheckPermissions()
    {
        // 當作業系統為 Android 時才需要執行。
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            // 在 Android API Level 小於 33 前都要執行。
            if (!OperatingSystem.IsAndroidVersionAtLeast(33))
            {   // API Level 32。
                await CheckAndRequestStorageReadPermission();
            }

            // 在 Android API Level 小於 30 前都要執行。
            if (!OperatingSystem.IsAndroidVersionAtLeast(30))
            {
                // API Level 29。
                await CheckAndRequestStorageWritePermission();
            }
        }
    }

    /// <summary>
    /// 檢查並請求 StorageRead 權限
    /// </summary>
    /// <returns>Task&lt;PermissionStatus&gt;</returns>
    public static async Task<PermissionStatus> CheckAndRequestStorageReadPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

        if (status == PermissionStatus.Granted)
        {
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
        {
            await AlertUtil.ShowSnackbar("需要您允許此權限請求，用以獲得載入 JSON 檔案的能力。", async () =>
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            });
        }
        else
        {
            status = await Permissions.RequestAsync<Permissions.StorageRead>();
        }

        return status;
    }

    /// <summary>
    /// 檢查並請求 StorageWrite 權限
    /// </summary>
    /// <returns>Task&lt;PermissionStatus&gt;</returns>
    public static async Task<PermissionStatus> CheckAndRequestStorageWritePermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

        if (status == PermissionStatus.Granted)
        {
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
        {
            await AlertUtil.ShowSnackbar("需要您允許此權限請求，用以獲得匯出 JSON 檔案的能力。", async () =>
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            });
        }
        else
        {
            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        return status;
    }

    #endregion

    #region 儲存檔案

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
                await AlertUtil.ShowToast(
                    message: "檔案路徑不得為空。",
                    ct: ct);

                return;
            }

            // 判斷案是否已存在。
            if (File.Exists(filePath))
            {
                // 刪除檔案。
                File.Delete(filePath);
            }

            string fileName = Path.GetFileName(filePath);

            using FileStream fileStream = File.OpenWrite(filePath);
            using StreamWriter streamWriter = new(fileStream);

            await streamWriter.WriteAsync(content);

            await AlertUtil.ShowToast(
                message: $"檔案 {fileName} 已儲存至：{filePath}。",
                ct: ct);
        }
        catch (Exception ex)
        {
            await AlertUtil.ShowErrorAlert(ex.Message);
        }
    }

    /// <summary>
    /// 取得 FileSaverResult
    /// </summary>
    /// <param name="fileName">字串，檔案名稱</param>
    /// <param name="stream">MemoryStream</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Task&lt;FileSaverResult&gt;</returns>
    public static async Task<FileSaverResult?> GetFileSaverResult(
        string fileName,
        Stream stream,
        CancellationToken cancellationToken)
    {
        FileSaverResult? fileSaverResult = null;

        if (DeviceInfo.Platform == DevicePlatform.Android &&
            OperatingSystem.IsAndroidVersionAtLeast(30))
        {
#if ANDROID

            AndroidFileSaver androidFileSaver = new();

            fileSaverResult = await androidFileSaver.SaveAsync(
               fileName: fileName,
               stream: stream,
               cancellationToken: cancellationToken);

#endif
        }
        else
        {
            fileSaverResult = await FileSaver.Default.SaveAsync(
               fileName: fileName,
               stream: stream,
               cancellationToken: cancellationToken);
        }

        return fileSaverResult;
    }

    #endregion

    #region 統一資源標識符

    /// <summary>
    /// 設定 PostData 的資料統一資源標識符
    /// </summary>
    /// <param name="postData">PostData</param>
    /// <returns>Task&lt;string&gt;></returns>
    public static async Task<string?> SetDataUri(PostData postData)
    {
        try
        {
            if (string.IsNullOrEmpty(postData.AuthorThumbnailUrl))
            {
                return null;
            }

            byte[]? imageBytes = await DiscordUtil.GetImageBytes(postData.AuthorThumbnailUrl);

            if (imageBytes == null)
            {
                return null;
            }

            if (postData != null)
            {
                postData.AuthorThumbnailDataUri = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }

            return postData?.AuthorThumbnailDataUri;
        }
        catch (Exception ex)
        {
            await AlertUtil.ShowErrorAlert(ex.Message);
        }

        return null;
    }

    /// <summary>
    /// 設定 AttachmentData 的資料統一資源標識符
    /// </summary>
    /// <param name="attachmentData">AttachmentData</param>
    /// <returns>Task&lt;string&gt;></returns>
    public static async Task<string?> SetDataUri(AttachmentData attachmentData)
    {
        try
        {
            if (attachmentData.IsVideo)
            {
                if (string.IsNullOrEmpty(attachmentData?.VideoData?.ThumbnailUrl))
                {
                    return null;
                }

                string? imageUrl = attachmentData?.VideoData?.ThumbnailUrl;

                byte[]? imageBytes = await DiscordUtil.GetImageBytes(imageUrl);

                if (imageBytes == null)
                {
                    return null;
                }

                if (attachmentData != null)
                {
                    attachmentData.DataUri = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
                }

                return attachmentData?.DataUri;
            }
            else if (attachmentData.IsPoll)
            {
                if (attachmentData?.PollData?.ChoiceDatas == null)
                {
                    return null;
                }

                attachmentData?.PollData?.ChoiceDatas?.ForEach(async (ChoiceData choiceData) =>
                {
                    string? imageUrl = choiceData.ImageUrl;

                    byte[]? imageBytes = await DiscordUtil.GetImageBytes(imageUrl);

                    if (imageBytes != null)
                    {
                        choiceData.ImageDataUri = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
                    }
                });
            }
            else
            {
                if (string.IsNullOrEmpty(attachmentData.Url))
                {
                    return null;
                }

                string? imageUrl = attachmentData.Url;

                byte[]? imageBytes = await DiscordUtil.GetImageBytes(imageUrl);

                if (imageBytes == null)
                {
                    return null;
                }

                if (attachmentData != null)
                {
                    attachmentData.DataUri = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
                }

                return attachmentData?.DataUri;
            }
        }
        catch (Exception ex)
        {
            await AlertUtil.ShowErrorAlert(ex.Message);
        }

        return null;
    }

    #endregion

    #region 功能性

    /// <summary>
    /// 執行簡易延遲
    /// </summary>
    /// <returns>Task</returns>
    public static async Task DoEasyDelay()
    {
        // 簡易的隨機減速機制。
        int delayMs = RandomNumberGenerator.GetInt32(500, 1500);

        await Task.Delay(delayMs);
    }

    /// <summary>
    /// 取得 HttpClient
    /// </summary>
    /// <param name="httpClientFactory">IHttpClientFactory</param>
    /// <returns>HttpClient</returns>
    public static HttpClient GetHttpClient(IHttpClientFactory httpClientFactory)
    {
        return HttpClientUtil.GetHttpClient(
            httpClientFactory: httpClientFactory,
            userAgent: StringSet.UserAgent);
    }

    #endregion

    #region 網頁瀏覽器相關

    /// <summary>
    /// 取得 Cookies 字串
    /// </summary>
    /// <returns>字串</returns>
    [SupportedOSPlatform("Windows")]
    public static string GetCookies()
    {
        string cookies = string.Empty;

        bool? useCookies = PreferencesUtil.GetBooleanValue(KeySet.UseCookies);

        string tempCookies = PreferencesUtil.GetStringValue(KeySet.Cookies) ?? string.Empty;

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            string browserType = PreferencesUtil.GetStringValue(KeySet.BrowserType) ?? string.Empty;
            string profileName = PreferencesUtil.GetStringValue(KeySet.ProfileName) ?? string.Empty;

            if (useCookies == true)
            {
                if (string.IsNullOrEmpty(tempCookies))
                {
                    List<BrowserUtil.Cookie> listCookies = BrowserUtil.GetCookies(
                        browser: GetBrowserType(browserType),
                        profileName: profileName,
                        hostKey: ".youtube.com");

                    cookies = string.Join(";", listCookies.Select(n => $"{n.Name}={n.Value}"));
                }
                else
                {
                    cookies = tempCookies;
                }
            }
        }
        else
        {
            cookies = useCookies == true ? tempCookies : string.Empty;
        }

        return cookies;
    }

    /// <summary>
    /// 取得網頁瀏覽器類型
    /// </summary>
    /// <param name="value">字串，網頁瀏覽器類型</param>
    /// <returns>BrowserUtil.BrowserType</returns>
    public static BrowserUtil.BrowserType GetBrowserType(string value)
    {
        return value switch
        {
            nameof(BrowserUtil.BrowserType.Brave) => BrowserUtil.BrowserType.Brave,
            nameof(BrowserUtil.BrowserType.GoogleChrome) => BrowserUtil.BrowserType.GoogleChrome,
            nameof(BrowserUtil.BrowserType.Chromium) => BrowserUtil.BrowserType.Chromium,
            nameof(BrowserUtil.BrowserType.MicrosoftEdge) => BrowserUtil.BrowserType.MicrosoftEdge,
            nameof(BrowserUtil.BrowserType.Opera) => BrowserUtil.BrowserType.Opera,
            nameof(BrowserUtil.BrowserType.OperaGX) => BrowserUtil.BrowserType.OperaGX,
            nameof(BrowserUtil.BrowserType.Vivaldi) => BrowserUtil.BrowserType.Vivaldi,
            nameof(BrowserUtil.BrowserType.MozillaFirefox) => BrowserUtil.BrowserType.MozillaFirefox,
            _ => BrowserUtil.BrowserType.GoogleChrome
        };
    }

    #endregion

    #region Webhook 相關

    /// <summary>
    /// 取得 Webhook
    /// </summary>
    /// <returns> Task&lt;List&lt;WebhookData&gt;&gt;</returns>
    public static async Task<List<WebhookData>?> GetWebhooks()
    {
        try
        {
            if (File.Exists(FileSet.WebhooksJson))
            {
                using FileStream fileStream = File.OpenRead(FileSet.WebhooksJson);

                return await JsonSerializer
                    .DeserializeAsync<List<WebhookData>>(fileStream);
            }
        }
        catch (Exception ex)
        {
            await AlertUtil.ShowErrorAlert(ex.Message);
        }

        return null;
    }

    /// <summary>
    /// 取得 Webhook 名稱組
    /// </summary>
    /// <param name="webhooks">List&lt;WebhookData&gt;</param>
    /// <returns>Task&lt;string[]&gt;</returns>
    public static async Task<string?[]?> GetWebhookNames(List<WebhookData>? webhooks)
    {
        try
        {
            if (webhooks == null)
            {
                return null;
            }

            return webhooks.Select(n => n.Name).ToArray();
        }
        catch (Exception ex)
        {
            await AlertUtil.ShowErrorAlert(ex.Message);
        }

        return null;
    }

    /// <summary>
    /// 取得 Webhook 的網址
    /// </summary>
    /// <returns>Task&lt;string&gt;</returns>
    public static async Task<string?> GetWebhookUrl()
    {
        try
        {
            List<WebhookData>? webhooks = await GetWebhooks();

            string?[]? buttons = await GetWebhookNames(webhooks);

            if (buttons == null || buttons.Length <= 0)
            {
                await AlertUtil.ShowToast("請先至管理 Webhook 畫面新增 Webhook。");

                return null;
            }

            string? webhookName = await AlertUtil.ShowActionSheet(
                title: "請選擇要使用的 Webhook",
                buttons: buttons);

            if (string.IsNullOrEmpty(webhookName) || webhookName == "取消")
            {
                // 點擊 Alert 外面，以讓 Alert 關閉，或是點擊取消按鈕。
                return null;
            }

            string? webhookUrl = webhooks?.FirstOrDefault(n => n.Name == webhookName)?.Url;

            if (string.IsNullOrEmpty(webhookUrl))
            {
                await AlertUtil.ShowToast($"找不到 Webhook \"{webhookName}\" 的 Webhook 網址。");

                return null;
            }

            return webhookUrl;
        }
        catch (Exception ex)
        {
            await AlertUtil.ShowErrorAlert(ex.Message);
        }

        return null;
    }

    #endregion
}