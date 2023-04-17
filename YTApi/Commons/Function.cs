using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using YTApi.Commons.Extensions;
using YTApi.Commons.Models;
using YTApi.Commons.Sets;
using IElement = AngleSharp.Dom.IElement;

namespace YTApi.Commons;

/// <summary>
/// 函式類別
/// </summary>
public static class Function
{
    /// <summary>
    /// 從 YouTube 頻道自定義網址取得頻道 ID
    /// </summary>
    /// <param name="url">字串，YouTube 頻道自定義網址</param>
    /// <returns>字串</returns>
    public static async Task<string?> GetYtChID(string url)
    {
        string? ytChId = string.Empty;

        IConfiguration configuration = Configuration.Default.WithDefaultLoader();
        IBrowsingContext browsingContext = BrowsingContext.New(configuration);
        IDocument document = await browsingContext.OpenAsync(url);
        IElement? element = document?.Body?.Children
            .FirstOrDefault(n => n.LocalName == "meta" &&
                n.GetAttribute("property") == "og:url");

        if (element != null)
        {
            ytChId = element.GetAttribute("content");
        }

        if (!string.IsNullOrEmpty(ytChId))
        {
            ytChId = ytChId.Replace($"{StringSet.Origin}/channel/", string.Empty);
        }

        return ytChId;
    }

    /// <summary>
    /// 取得初始資料
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="channelID">字串，影片 ID</param>
    /// <param name="cookies">字串，Cookies</param>
    /// <returns>InitialData</returns>
    public static InitialData GetInitialData(
        HttpClient httpClient,
        string channelID,
        string cookies)
    {
        InitialData initialData = new();

        string url = $"{StringSet.Origin}/channel/{channelID}/community";

        initialData.Referer = url;

        HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, url);

        if (!string.IsNullOrEmpty(cookies))
        {
            httpRequestMessage.SetHeaders(cookies);
        }

        HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult();

        string htmlContent = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
        {
            YTConfig ytConfig = new();

            MatchCollection collection1 = RegexSet.InnertubeApiKey.Matches(htmlContent);

            foreach (Match match in collection1.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.APIKey = match.Groups[1].Captures[0].Value;
                }
            }

            MatchCollection collection2 = RegexSet.Continuation.Matches(htmlContent);

            foreach (Match match in collection2.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.Continuation = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection3 = RegexSet.VisitorData.Matches(htmlContent);

            foreach (Match match in collection3.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.VisitorData = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection4 = RegexSet.ClientName.Matches(htmlContent);

            foreach (Match match in collection4.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    string clientName = match.Groups[1].Captures[0].Value;

                    if (clientName == "WEB")
                    {
                        ytConfig.ClientName = clientName;

                        break;
                    }
                }
            }

            MatchCollection collection5 = RegexSet.ClientVersion.Matches(htmlContent);

            foreach (Match match in collection5.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.ClientVersion = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection6 = RegexSet.IDToken.Matches(htmlContent);

            foreach (Match match in collection6.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.ID_TOKEN = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection7 = RegexSet.SessionIndex.Matches(htmlContent);

            foreach (Match match in collection7.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.SESSION_INDEX = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection8 = RegexSet.InnertubeContextClientName.Matches(htmlContent);

            foreach (Match match in collection8.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.INNERTUBE_CONTEXT_CLIENT_NAME = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection9 = RegexSet.InnertubeContextClientVersion.Matches(htmlContent);

            foreach (Match match in collection9.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.INNERTUBE_CONTEXT_CLIENT_VERSION = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            MatchCollection collection10 = RegexSet.InnertubeClientVersion.Matches(htmlContent);

            foreach (Match match in collection10.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.INNERTUBE_CLIENT_VERSION = match.Groups[1].Captures[0].Value;

                    break;
                }
            }

            bool useDelegatedSessionID = false;

            MatchCollection collection11 = RegexSet.DatasyncID.Matches(htmlContent);

            foreach (Match match in collection11.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    string dataSyncID = match.Groups[1].Captures[0].Value;

                    string[] tempArray = dataSyncID
                        .Split("||".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    if (tempArray.Length >= 2 && !string.IsNullOrEmpty(tempArray[1]))
                    {
                        ytConfig.DATASYNC_ID = tempArray[0];
                    }
                    else
                    {
                        useDelegatedSessionID = true;
                    }

                    break;
                }
            }

            // 參考：https://github.com/xenova/chat-downloader/blob/master/chat_downloader/sites/youtube.py#L1629
            MatchCollection collection12 = RegexSet.DelegatedSessionID.Matches(htmlContent);

            foreach (Match match in collection12.Cast<Match>())
            {
                if (match.Success && match.Groups.Count >= 2)
                {
                    ytConfig.DELEGATED_SESSION_ID = match.Groups[1].Captures[0].Value;

                    if (useDelegatedSessionID)
                    {
                        ytConfig.DATASYNC_ID = ytConfig.DELEGATED_SESSION_ID;
                    }

                    break;
                }
            }

            initialData.ConfigData = ytConfig;

            HtmlParser htmlParser = new();
            IHtmlDocument htmlDocument = htmlParser.ParseDocument(htmlContent);
            IHtmlCollection<IElement> scriptElements = htmlDocument.QuerySelectorAll("script");
            IElement scriptElement = scriptElements
                .FirstOrDefault(n => n.InnerHtml.Contains("var ytInitialData ="))!;

            string json = scriptElement.InnerHtml
                .Replace("var ytInitialData = ", "");

            if (json.EndsWith(";"))
            {
                json = json[0..^1];
            }

            JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(json);

            initialData.JsonData = jsonElement;
        }
        else
        {
            string errorMessage = $"[{DateTime.Now}]（{typeof(HttpClient).Name}）：" +
                $"連線發生錯誤，錯誤碼：{httpResponseMessage.StatusCode} " +
                $"({(int)httpResponseMessage.StatusCode}){Environment.NewLine}" +
                $"接收到的內容：{Environment.NewLine}" +
                $"{htmlContent}{Environment.NewLine}";

            Console.WriteLine(errorMessage);
        }

        return initialData;
    }

    /// <summary>
    /// 設定 YTConfig 的 Continuation
    /// </summary>
    /// <param name="arrayEnumerator">JsonElement.ArrayEnumerator</param>
    /// <param name="ytConfig">YTConfig</param>
    public static void SetContinuation(
        JsonElement.ArrayEnumerator? arrayEnumerator,
        YTConfig? ytConfig)
    {
        if (ytConfig != null)
        {
            if (arrayEnumerator != null)
            {
                JsonElement? continuationItemRenderer = arrayEnumerator?.FirstOrDefault(n => n.Get("continuationItemRenderer") != null);

                ytConfig.Continuation = JsonParser.GetToken(continuationItemRenderer);
            }
            else
            {
                ytConfig.Continuation = null;
            }
        }
    }

    /// <summary>
    /// 取得 JsonElement
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <param name="cookies">字串，Cookies</param>
    /// <param name="referer">字串，HTTP 參照位址</param>
    /// <returns>JsonElement</returns>
    public static JsonElement GetJsonElement(
        HttpClient httpClient,
        YTConfig? ytConfig,
        string cookies,
        string referer)
    {
        JsonElement jsonElement = new();

        string url = $"{StringSet.Origin}/youtubei/v1/browse?key={ytConfig?.APIKey}";

        // 當 ytConfig.Continuation 為 null 或空值時，則表示已經抓取完成。
        if (!string.IsNullOrEmpty(ytConfig?.Continuation))
        {
            // 當沒有時才指定，後續不更新。
            if (string.IsNullOrEmpty(ytConfig.InitPage))
            {
                ytConfig.InitPage = referer;
            }

            string inputJsonContent = GetRequestPayload(ytConfig, httpClient.DefaultRequestHeaders.UserAgent.ToString());

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, url);

            if (!string.IsNullOrEmpty(cookies))
            {
                httpRequestMessage.SetHeaders(cookies, ytConfig);
            }

            HttpContent content = new StringContent(inputJsonContent, Encoding.UTF8, "application/jsonElement");

            httpRequestMessage.Content = content;

            HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult();

            string receivedJsonContent = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                jsonElement = JsonSerializer.Deserialize<JsonElement>(receivedJsonContent);
            }
            else
            {
                string errorMessage = $"[{DateTime.Now}]（{typeof(HttpClient).Name}）：" +
                    $"連線發生錯誤，錯誤碼：{httpResponseMessage.StatusCode} " +
                    $"({(int)httpResponseMessage.StatusCode}){Environment.NewLine}" +
                    $"接收到的內容：{Environment.NewLine}" +
                    $"{receivedJsonContent}{Environment.NewLine}";

                Debug.WriteLine(errorMessage);
            }
        }

        return jsonElement;
    }

    /// <summary>
    /// 取得要求裝載
    /// </summary>
    /// <param name="ytConfig">YTConfig</param>
    /// <param name="userAgent">字串，使用者代理字串</param>
    /// <returns>字串</returns>
    public static string GetRequestPayload(YTConfig ytConfig, string userAgent)
    {
        // 2022-05-19 尚未測試會員限定直播是否需要 "clickTracking" 參數。
        // 2022-05-29 已確認不需要 "clickTracking" 參數。
        // 參考：https://github.com/xenova/chat-downloader/blob/ff9ddb1f840fa06d0cc3976badf75c1fffebd003/chat_downloader/sites/youtube.py#L1664
        // 參考：https://github.com/abhinavxd/youtube-live-chat-downloader/blob/main/yt_chat.go

        // 內容是精簡過的。
        return JsonSerializer.Serialize(new RequestPayload()
        {
            Context = new()
            {
                Client = new()
                {
                    // 語系會影響取得的內容，強制使用 zh-TW, TW。
                    Hl = "zh-TW",
                    Gl = "TW",
                    DeviceMake = string.Empty,
                    DeviceModel = string.Empty,
                    VisitorData = ytConfig.VisitorData,
                    UserAgent = userAgent,
                    ClientName = ytConfig.ClientName,
                    ClientVersion = ytConfig.ClientVersion,
                    OsName = "Windows",
                    OsVersion = "10.0",
                    Platform = "DESKTOP",
                    ClientFormFactor = "UNKNOWN_FORM_FACTOR",
                    TimeZone = "Asia/Taipei"
                }
            },
            Continuation = ytConfig.Continuation
        });
    }
}