using JNogueira.Discord.Webhook.Client;
using System.Drawing;
using System.Text.Json;
using System.Web;
using YTApi.Commons;
using YTApi.Commons.Extensions;
using YTApi.Commons.Models;
using YTApi.Commons.Sets;
using YTCmtyParser.Commons.Utils;
using Color = System.Drawing.Color;

namespace YTCmtyParser.Commons;

/// <summary>
/// 自定義函式
/// </summary>
public static class CustomFunction
{
    /// <summary>
    /// 取得貼文
    /// </summary>
    /// <param name="channelID">字串，頻道的 ID</param>
    /// <param name="cookies">字串，Cookies，預設值為空白</param>
    /// <param name="getWholePosts">布林值，是否取得全部貼文，預設值為 false</param>
    /// <returns>List&lt;PostData&gt;</returns>
    public static List<PostData> GetPosts(
        string channelID,
        string cookies = "",
        bool getWholePosts = false)
    {
        List<PostData> postDatas = new();

        InitialData initialData = Function.GetInitialData(
            httpClient: HttpClientUtil.GetHttpClient(
                userAgent: Sets.StringSet.UserAgent),
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
                    List<PostData> oldertPosts = GetOlderPosts(
                        httpClient: HttpClientUtil.GetHttpClient(
                            userAgent: Sets.StringSet.UserAgent),
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
    /// 取得最新的貼文
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <returns>List&lt;PostData&gt;</returns>
    public static List<PostData> GetInitialPosts(JsonElement? jsonElement, YTConfig? ytConfig)
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

                if (itemSectionRendererContentsArray != null)
                {
                    // 設定上一個梯次貼文用的 continuation。
                    Function.SetContinuation(
                        arrayEnumerator: itemSectionRendererContentsArray,
                        ytConfig: ytConfig);

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
    /// 取得舊貼文
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <param name="cookies">字串，Cookies，預設值為空白</param>
    /// <param name="referer">字串，HTTP 參照位址，預設值為空白</param>
    /// <returns>List&lt;PostData&gt;</returns>
    public static List<PostData> GetOlderPosts(
        HttpClient httpClient,
        YTConfig? ytConfig,
        string cookies = "",
        string referer = "")
    {
        List<PostData> postDatas = new();

        JsonElement jsonElement = Function.GetJsonElement(
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

                if (continuationItemsArray != null)
                {
                    // 設定上一個梯次貼文用的 continuation。
                    Function.SetContinuation(
                        arrayEnumerator: continuationItemsArray,
                        ytConfig: ytConfig);

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

    /// <summary>
    /// 傳送至 Discord 頻道
    /// <para>※只支援文字頻道。</para>
    /// </summary>
    /// <param name="postData">字串，PostData</param>
    /// <param name="disocrdWebhookUrl">字串，Discord 的 Webhook 網址</param>
    public static async void SendToDiscordChannel(PostData postData, string disocrdWebhookUrl)
    {
        DiscordWebhookClient discordWebhookClient = new(disocrdWebhookUrl);

        string content = string.Empty;

        content += $"> 網址：<{postData.Url}>{Environment.NewLine}";
        content += $"> 誰能看到：`{(postData.IsSponsorsOnly ? "頻道會員專屬" : "所有頻道會員")}`{Environment.NewLine}";
        content += $"> 發布時間：{postData.PublishedTimeText}{Environment.NewLine}";
        content += $"> 投票次數：{postData.VoteCount}{Environment.NewLine}";
        content += Environment.NewLine;

        if (postData.ContentTexts?.Count > 0)
        {
            foreach (RunData contentText in postData.ContentTexts)
            {
                string? value = contentText.Text;

                if (contentText.IsLink)
                {
                    if (value != contentText.Url)
                    {
                        value += $" (<{HttpUtility.UrlDecode(contentText.Url)}>)";
                    }
                }

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                content += value;
            }
        }

        DiscordMessageEmbed[] embeds = Array.Empty<DiscordMessageEmbed>();

        DiscordFile[] files = Array.Empty<DiscordFile>();

        if (postData.Attachments != null && postData.Attachments.Any())
        {
            List<DiscordMessageEmbed> embedSet = new();
            List<DiscordFile> fileSet = new();

            int index = 1;

            foreach (AttachmentData? attachmentData in postData.Attachments)
            {
                if (!attachmentData.IsVideo)
                {
                    byte[]? imageBytes = await GetImageBytes(attachmentData?.Url);

                    if (imageBytes != null)
                    {
                        fileSet.Add(new DiscordFile($"image{index}.jpg", imageBytes));
                    }
                }
                else
                {
                    List<DiscordMessageEmbedField> fields = new();

                    string[]? tempArray1 = attachmentData?.VideoData?.PublishedTimeText?.Split('：');
                    string[]? tempArray2 = attachmentData?.VideoData?.ViewCountText?.Split('：');

                    string value1 = tempArray1?.Length > 1 ? tempArray1[1] : tempArray1?[0] ?? string.Empty;
                    string value2 = tempArray2?.Length > 1 ? tempArray2[1] : tempArray2?[0] ?? string.Empty;

                    fields.Add(new DiscordMessageEmbedField(
                        name: "發布時間",
                        value: value1,
                        inLine: false));
                    fields.Add(new DiscordMessageEmbedField(
                        name: "觀看次數",
                        value: value2,
                        inLine: false));
                    fields.Add(new DiscordMessageEmbedField(
                        name: "長度",
                        value: attachmentData?.VideoData?.LengthText,
                        inLine: false));

                    Color color = ColorTranslator.FromHtml("#FF0000");

                    int intColor = (color.R << 16) | (color.G << 8) | (color.B);

                    embedSet.Add(new DiscordMessageEmbed(
                        title: attachmentData?.VideoData?.Title,
                        color: intColor,
                        author: new DiscordMessageEmbedAuthor(
                            name: postData.AuthorText,
                            url: null,
                            iconUrl: postData.AuthorThumbnailUrl),
                        url: attachmentData?.VideoData?.Url,
                        description: attachmentData?.VideoData?.DescriptionSnippet,
                        fields: fields,
                        thumbnail: null,
                        image: new DiscordMessageEmbedImage(
                            url: attachmentData?.VideoData?.ThumbnailUrl),
                        footer: new DiscordMessageEmbedFooter(
                            text: attachmentData?.VideoData?.OwnerText,
                            iconUrl: null)));
                }

                index++;
            }

            embeds = embedSet.ToArray();
            files = fileSet.ToArray();
        }

        DiscordMessage discordMessage = new(
            content: content,
            username: postData.AuthorText,
            avatarUrl: postData.AuthorThumbnailUrl,
            tts: false,
            embeds: embeds);

        await discordWebhookClient.SendToDiscord(
            message: discordMessage,
            files: files);
    }

    /// <summary>
    /// 取得圖片的 byte[]
    /// </summary>
    /// <param name="url">字串，圖片的網址</param>
    /// <returns>Task&lt;byte[]&gt;</returns>
    public static async Task<byte[]?> GetImageBytes(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        using HttpClient httpClient = HttpClientUtil.GetHttpClient(
            userAgent: Sets.StringSet.UserAgent);
        using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

        return await httpResponseMessage.Content.ReadAsByteArrayAsync();
    }
}