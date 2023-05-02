using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using YTApi.Commons;
using YTApi.Commons.Extensions;
using YTApi.Commons.Models;
using YTApi.Commons.Sets;
using YTCmtyParser.Commons.Utils;

namespace YTCmtyParser.Data;

/// <summary>
/// YouTube 社群貼文服務
/// </summary>
public class YTCmtyService
{
    /// <summary>
    /// 取得 InitialData
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="channelID">字串，YouTube 頻道的 ID</param>
    /// <param name="cookies">字串，Cookies</param>
    /// <returns>Task&lt;InitialData&gt;</returns>
    [SuppressMessage("Performance", "CA1822:將成員標記為靜態", Justification = "<暫止>")]
    public async Task<InitialData> GetInitialData(
        HttpClient httpClient,
        string channelID,
        string cookies)
    {
        return await ApiFunction.GetInitialData(
            httpClient: httpClient,
            channelID: channelID,
            cookies: cookies);
    }

    /// <summary>
    /// 取得貼文
    /// </summary>
    /// <param name="channelID">字串，頻道的 ID</param>
    /// <param name="cookies">字串，Cookies，預設值為空白</param>
    /// <param name="getWholePosts">布林值，是否取得全部貼文，預設值為 false</param>
    /// <returns>List&lt;PostData&gt;</returns>
    public async Task<List<PostData>> GetPosts(
        string channelID,
        string cookies = "",
        bool getWholePosts = false)
    {
        List<PostData> postDatas = new();

        InitialData initialData = await ApiFunction.GetInitialData(
            httpClient: HttpClientUtil.GetHttpClient(
                userAgent: Commons.Sets.StringSet.UserAgent),
            channelID: channelID,
            cookies: cookies);

        string referer = initialData.Referer ?? string.Empty;

        YTConfig? configData = initialData.ConfigData;

        JsonElement? jsonElement = initialData.JsonData;

        List<PostData> latestPosts = GetInitialPosts(
            jsonElement: jsonElement,
            ytConfig: configData);

        if (latestPosts.Any())
        {
            postDatas.AddRange(latestPosts);

            if (getWholePosts)
            {
                while (!string.IsNullOrEmpty(configData?.Continuation))
                {
                    List<PostData> oldertPosts = await GetEarlierPosts(
                        httpClient: HttpClientUtil.GetHttpClient(
                            userAgent: Commons.Sets.StringSet.UserAgent),
                        ytConfig: configData,
                        cookies: cookies,
                        referer: referer);

                    if (oldertPosts.Any())
                    {
                        postDatas.AddRange(oldertPosts);
                    }
                }
            }
        }

        return postDatas;
    }

    /// <summary>
    /// 取得初始的貼文
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <returns>List&lt;PostData&gt;</returns>
    [SuppressMessage("Performance", "CA1822:將成員標記為靜態", Justification = "<暫止>")]
    public List<PostData> GetInitialPosts(JsonElement? jsonElement, YTConfig? ytConfig)
    {
        List<PostData> postDatas = new();

        JsonElement? communityTab = JsonParser.GetCommunityTab(jsonElement: jsonElement);
        JsonElement? contents = JsonParser.GetTabContents(jsonElement: communityTab);
        JsonElement.ArrayEnumerator? contentsArray = contents?.GetArrayEnumerator();

        if (contentsArray != null)
        {
            // 理論上只會有一筆。
            foreach (JsonElement content in contentsArray)
            {
                JsonElement? itemSectionRendererContents = JsonParser.GetItemSectionRendererContents(jsonElement: content);
                JsonElement.ArrayEnumerator? itemSectionRendererContentsArray = itemSectionRendererContents?.GetArrayEnumerator();

                // 設定上一個梯次貼文用的 continuation。
                ApiFunction.SetContinuation(
                    arrayEnumerator: itemSectionRendererContentsArray,
                    ytConfig: ytConfig);

                if (itemSectionRendererContentsArray != null)
                {
                    // 取得並解析最新的貼文資料。
                    IEnumerable<JsonElement>? backstagePostThreadRenderers = itemSectionRendererContentsArray
                        ?.Where(n => n.Get("backstagePostThreadRenderer") != null);

                    if (backstagePostThreadRenderers != null)
                    {
                        foreach (JsonElement? backstagePostThreadRenderer in backstagePostThreadRenderers.Select(v => (JsonElement?)v))
                        {
                            JsonElement? backstagePostRenderer = JsonParser.GetBackstagePostRenderer(jsonElement: backstagePostThreadRenderer);

                            if (backstagePostRenderer != null)
                            {
                                string postId = JsonParser.GetPostID(jsonElement: backstagePostRenderer);

                                postDatas.Add(new PostData()
                                {
                                    PostID = postId,
                                    Url = $"{StringSet.Origin}/post/{postId}",
                                    AuthorText = JsonParser.GetAuthorText(jsonElement: backstagePostRenderer),
                                    AuthorThumbnailUrl = JsonParser.GetAuthorThumbnailUrl(jsonElement: backstagePostRenderer),
                                    ContentTexts = JsonParser.GetContentText(jsonElement: backstagePostRenderer),
                                    PublishedTimeText = JsonParser.GetPublishedTimeText(jsonElement: backstagePostRenderer),
                                    VoteCount = JsonParser.GetVoteCount(jsonElement: backstagePostRenderer, simpleText: false),
                                    Attachments = JsonParser.GetBackstageAttachment(jsonElement: backstagePostRenderer),
                                    IsSponsorsOnly = JsonParser.IsSponsorsOnly(jsonElement: backstagePostRenderer),
                                });
                            }
                        }
                    }
                }
            }
        }

        return postDatas;
    }

    /// <summary>
    /// 取得先前的貼文
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <param name="cookies">字串，Cookies，預設值為空白</param>
    /// <param name="referer">字串，HTTP 參照位址，預設值為空白</param>
    /// <returns>List&lt;PostData&gt;</returns>
    [SuppressMessage("Performance", "CA1822:將成員標記為靜態", Justification = "<暫止>")]
    public async Task<List<PostData>> GetEarlierPosts(
        HttpClient httpClient,
        YTConfig? ytConfig,
        string cookies = "",
        string referer = "")
    {
        List<PostData> postDatas = new();

        JsonElement jsonElement = await ApiFunction.GetJsonElement(
            httpClient: httpClient,
            ytConfig: ytConfig,
            cookies: cookies,
            referer: referer);

        JsonElement.ArrayEnumerator? onResponseReceivedEndpointsArray = JsonParser
            .GetOnResponseReceivedEndpointsArray(
                jsonElement: jsonElement);

        if (onResponseReceivedEndpointsArray != null)
        {
            // 理論上只會有一筆。
            foreach (JsonElement onResponseReceivedEndpoint in onResponseReceivedEndpointsArray)
            {
                JsonElement? continuationItems = JsonParser
                    .GetAppendContinuationItemsActionContinuationItems(
                        jsonElement: onResponseReceivedEndpoint);
                JsonElement.ArrayEnumerator? continuationItemsArray = continuationItems?.GetArrayEnumerator();

                // 設定上一個梯次貼文用的 continuation。
                ApiFunction.SetContinuation(
                    arrayEnumerator: continuationItemsArray,
                    ytConfig: ytConfig);

                if (continuationItemsArray != null)
                {
                    // 取得並解析貼文資料。
                    IEnumerable<JsonElement>? backstagePostThreadRenderers = continuationItemsArray
                        ?.Where(n => n.Get("backstagePostThreadRenderer") != null);

                    if (backstagePostThreadRenderers != null)
                    {
                        foreach (JsonElement? backstagePostThreadRenderer in backstagePostThreadRenderers.Select(v => (JsonElement?)v))
                        {
                            JsonElement? backstagePostRenderer = JsonParser.GetBackstagePostRenderer(jsonElement: backstagePostThreadRenderer);

                            if (backstagePostRenderer != null)
                            {
                                string postId = JsonParser.GetPostID(jsonElement: backstagePostRenderer);

                                postDatas.Add(new PostData()
                                {
                                    PostID = postId,
                                    Url = $"{StringSet.Origin}/post/{postId}",
                                    AuthorText = JsonParser.GetAuthorText(jsonElement: backstagePostRenderer),
                                    AuthorThumbnailUrl = JsonParser.GetAuthorThumbnailUrl(jsonElement: backstagePostRenderer),
                                    ContentTexts = JsonParser.GetContentText(jsonElement: backstagePostRenderer),
                                    PublishedTimeText = JsonParser.GetPublishedTimeText(jsonElement: backstagePostRenderer),
                                    VoteCount = JsonParser.GetVoteCount(jsonElement: backstagePostRenderer, simpleText: false),
                                    Attachments = JsonParser.GetBackstageAttachment(jsonElement: backstagePostRenderer),
                                    IsSponsorsOnly = JsonParser.IsSponsorsOnly(jsonElement: backstagePostRenderer)
                                });
                            }
                        }
                    }
                }
            }
        }

        return postDatas;
    }
}