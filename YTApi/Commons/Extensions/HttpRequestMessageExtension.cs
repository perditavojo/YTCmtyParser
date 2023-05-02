using YTApi.Commons.Models;
using YTApi.Commons.Sets;
using YTApi.Commons.Utils;

namespace YTApi.Commons.Extensions;

/// <summary>
/// HttpRequestMessage 的擴充方法
/// </summary>
public static partial class HttpRequestMessageExtension
{
    /// <summary>
    /// 設定 HttpRequestMessage 的標頭
    /// <para>參考：https://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage/13287224#13287224 </para>
    /// </summary>
    /// <param name="httpRequestMessage">HttpRequestMessage</param>
    /// <param name="cookies">字串，Cookies</param>
    /// <param name="ytConfig">YTConfig，預設值為 null</param>
    public static void SetHeaders(
        this HttpRequestMessage httpRequestMessage,
        string cookies,
        YTConfig? ytConfig = null)
    {
        httpRequestMessage.Headers.Add("Cookie", cookies);

        string[] cookieSet = cookies.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        string? sapiSid = cookieSet.FirstOrDefault(n => n.Contains("SAPISID"));

        if (!string.IsNullOrEmpty(sapiSid))
        {
            string[] tempArray = sapiSid.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            if (tempArray.Length == 2)
            {
                httpRequestMessage.Headers.Add(
                    "authorization",
                    $"SAPISIDHASH {SecurityUtil.GetSapiSidHash(tempArray[1], StringSet.Origin)}");
            }
        }

        if (ytConfig != null)
        {
            string xGoogAuthuser = "0", xGoogPageId = string.Empty;

            if (!string.IsNullOrEmpty(ytConfig.DATASYNC_ID))
            {
                xGoogPageId = ytConfig.DATASYNC_ID;
            }

            if (string.IsNullOrEmpty(xGoogPageId) &&
                !string.IsNullOrEmpty(ytConfig.DELEGATED_SESSION_ID))
            {
                xGoogPageId = ytConfig.DELEGATED_SESSION_ID;
            }

            if (!string.IsNullOrEmpty(xGoogPageId))
            {
                httpRequestMessage.Headers.Add("x-goog-pageid", xGoogPageId);
            }

            if (!string.IsNullOrEmpty(ytConfig.ID_TOKEN))
            {
                httpRequestMessage.Headers.Add("x-youtube-identity-token", ytConfig.ID_TOKEN);
            }

            if (!string.IsNullOrEmpty(ytConfig.SESSION_INDEX))
            {
                xGoogAuthuser = ytConfig.SESSION_INDEX;
            }

            httpRequestMessage.Headers.Add("x-goog-authuser", xGoogAuthuser);
            httpRequestMessage.Headers.Add("x-goog-visitor-id", ytConfig.VisitorData);
            httpRequestMessage.Headers.Add("x-youtube-client-name", ytConfig.INNERTUBE_CONTEXT_CLIENT_NAME);
            httpRequestMessage.Headers.Add("x-youtube-client-version", ytConfig.INNERTUBE_CLIENT_VERSION);

            if (!string.IsNullOrEmpty(ytConfig.InitPage))
            {
                httpRequestMessage.Headers.Add("referer", ytConfig.InitPage);
            }
        }

        httpRequestMessage.Headers.Add("origin", StringSet.Origin);
        httpRequestMessage.Headers.Add("x-origin", StringSet.Origin);
    }
}