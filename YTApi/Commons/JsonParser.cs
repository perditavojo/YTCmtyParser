using Microsoft.Maui.Controls.Handlers.Compatibility;
using System.Text.Json;
using YTApi.Commons.Extensions;
using YTApi.Commons.Models;
using YTApi.Commons.Sets;

namespace YTApi.Commons;

/// <summary>
/// JSON 解析器類別
/// </summary>
public static class JsonParser
{
    /// <summary>
    /// 取得 Tabs
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? GetTabs(JsonElement? jsonElement)
    {
        return jsonElement?.Get("contents")
            ?.Get("twoColumnBrowseResultsRenderer")
            ?.Get("tabs");
    }

    /// <summary>
    /// 取得社群的 tab
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? GetCommunityTab(JsonElement? jsonElement)
    {
        JsonElement? tabs = GetTabs(jsonElement);

        if (tabs != null && tabs.HasValue && tabs.Value.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement tab in tabs.Value.EnumerateArray())
            {
                JsonElement? url = tab.Get("tabRenderer")
                    ?.Get("endpoint")
                    ?.Get("commandMetadata")
                    ?.Get("webCommandMetadata")
                    ?.Get("url");

                if (url != null && url.HasValue && url.Value.GetRawText().Contains("/community"))
                {
                    return tab;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 取得 tab 的 contents
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? GetTabContents(JsonElement? jsonElement)
    {
        return jsonElement?.Get("tabRenderer")
            ?.Get("content")
            ?.Get("sectionListRenderer")
            ?.Get("contents");
    }

    /// <summary>
    /// 取得 itemSectionRenderer 的 contents
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? GetItemSectionRendererContents(JsonElement? jsonElement)
    {
        return jsonElement?.Get("itemSectionRenderer")
            ?.Get("contents");
    }

    /// <summary>
    /// 取得 onResponseReceivedEndpoints 的陣列
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement.ArrayEnumerator</returns>
    public static JsonElement.ArrayEnumerator? GetOnResponseReceivedEndpointsArray(JsonElement? jsonElement)
    {
        return jsonElement?.Get("onResponseReceivedEndpoints")
                ?.GetArrayEnumerator();
    }

    /// <summary>
    /// 取得 appendContinuationItemsAction 的 continuationItems
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? GetAppendContinuationItemsActionContinuationItems(JsonElement? jsonElement)
    {
        return jsonElement?.Get("appendContinuationItemsAction")
            ?.Get("continuationItems");
    }

    /// <summary>
    /// 取得 backstagePostRenderer
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? GetBackstagePostRenderer(JsonElement? jsonElement)
    {
        return jsonElement?.Get("backstagePostThreadRenderer")
            ?.Get("post")
            ?.Get("backstagePostRenderer");
    }

    /// <summary>
    /// 判斷 backstagePostRenderer 是否為頻道會員專屬
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>布林值</returns>
    public static bool IsSponsorsOnly(JsonElement? jsonElement)
    {
        JsonElement? sponsorsOnlyBadge = jsonElement?.Get("sponsorsOnlyBadge");

        return sponsorsOnlyBadge != null;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 postId
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetPostID(JsonElement? jsonElement)
    {
        JsonElement? postId = jsonElement?.Get("postId");

        if (postId != null && postId.HasValue)
        {
            return postId.Value.GetString() ?? string.Empty;
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 authorText 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetAuthorText(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("authorText")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(jsonElement: run);

                if (runData != null)
                {
                    return runData.Text ?? string.Empty;
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 authorThumbnail 的網址字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetAuthorThumbnailUrl(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? thumbnails = jsonElement?.Get("authorThumbnail")
            ?.Get("thumbnails")
            ?.GetArrayEnumerator();

        if (thumbnails != null)
        {
            // 32*32, 48*48, 76*76
            return GetThumbnailUrl(arrayEnumerator: thumbnails, width: 76);
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 contentText
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static List<RunData> GetContentText(JsonElement? jsonElement)
    {
        List<RunData> runDatas = new();

        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("contentText")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(jsonElement: run);

                if (runData != null)
                {
                    runDatas.Add(runData);
                }
            }
        }

        return runDatas;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 publishedTimeText 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetPublishedTimeText(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("publishedTimeText")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(jsonElement: run);

                if (runData != null)
                {
                    return runData.Text ?? string.Empty;
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 voteCount 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="simpleText">布林值，是否取得 simpleText 的文字內容，預設值為 false</param>
    /// <returns>字串</returns>
    public static string GetVoteCount(JsonElement? jsonElement, bool simpleText = false)
    {
        JsonElement? voteCount = jsonElement?.Get("voteCount");
        JsonElement? element = simpleText ?
            voteCount?.Get("simpleText") :
            voteCount?.Get("accessibility")
                ?.Get("accessibilityData")
                ?.Get("label");

        return element?.GetString() ?? string.Empty;
    }

    /// <summary>
    /// 取得 backstagePostRenderer 的 backstageAttachment
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>List&lt;Attachment&gt;</returns>
    public static List<AttachmentData> GetBackstageAttachment(JsonElement? jsonElement)
    {
        List<AttachmentData> attachmentDatas = new();

        JsonElement? backstageAttachment = jsonElement?.Get("backstageAttachment");
        JsonElement? postMultiImageRenderer = backstageAttachment?.Get("postMultiImageRenderer");
        JsonElement? backstageImageRenderer = backstageAttachment?.Get("backstageImageRenderer");
        JsonElement? videoRenderer = backstageAttachment?.Get("videoRenderer");
        JsonElement? pollRenderer = backstageAttachment?.Get("pollRenderer");

        // 有多張圖片附件。
        if (postMultiImageRenderer != null)
        {
            JsonElement.ArrayEnumerator? images = postMultiImageRenderer?.Get("images")
                ?.GetArrayEnumerator();

            if (images != null)
            {
                foreach (JsonElement image in images)
                {
                    string url = GetBackstageImageRendererThumbnailUrl(jsonElement: image);

                    if (!string.IsNullOrEmpty(url))
                    {
                        attachmentDatas.Add(new AttachmentData()
                        {
                            Url = url
                        });
                    }
                }
            }
        }

        // 有單一圖片附件。
        if (backstageImageRenderer != null)
        {
            string url = GetBackstageImageRendererThumbnailUrl(jsonElement: backstageAttachment);

            if (!string.IsNullOrEmpty(url))
            {
                attachmentDatas.Add(new AttachmentData()
                {
                    Url = url
                });
            }
        }

        // 有影片附件。
        if (videoRenderer != null)
        {
            VideoData? videoData = GetVideoData(jsonElement: videoRenderer);

            if (videoData != null)
            {
                attachmentDatas.Add(new AttachmentData()
                {
                    Url = videoData.ThumbnailUrl,
                    IsVideo = true,
                    VideoData = videoData
                });
            }
        }

        // 有投票附件。
        if (pollRenderer != null)
        {
            PollData? pollData = GetPollData(jsonElement: pollRenderer);

            if (pollData != null)
            {
                attachmentDatas.Add(new AttachmentData()
                {
                    IsPoll = true,
                    PollData = pollData
                });
            }
        }

        return attachmentDatas;
    }

    /// <summary>
    /// 取得 backstageAttachment 的 videoRenderer
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>VideoData</returns>
    public static VideoData? GetVideoData(JsonElement? jsonElement)
    {
        string? url = GetVideoRendererVideoUrl(jsonElement: jsonElement);

        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        return new VideoData()
        {
            ID = GetVideoRendererVideoID(jsonElement: jsonElement),
            Url = url,
            ThumbnailUrl = GetVideoRendererThumbnailUrl(jsonElement: jsonElement),
            Title = GetVideoRendererTitle(jsonElement: jsonElement),
            DescriptionSnippet = GetVideoRendererDescriptionSnippet(jsonElement: jsonElement),
            PublishedTimeText = GetVideoRendererPublishedTimeText(jsonElement: jsonElement),
            LengthText = GetVideoRendererLengthText(jsonElement: jsonElement, simpleText: false),
            ViewCountText = GetVideoRendererViewCountText(jsonElement: jsonElement),
            OwnerText = GetVideoRendererOwnerText(jsonElement: jsonElement)
        };
    }

    /// <summary>
    /// 取得 videoRenderer 的 videoId 字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererVideoID(JsonElement? jsonElement)
    {
        return jsonElement?.Get("videoId")?.GetString();
    }

    /// <summary>
    /// 取得 videoRenderer 的影片網址字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererVideoUrl(JsonElement? jsonElement)
    {
        string? url = jsonElement?.Get("navigationEndpoint")
            ?.Get("commandMetadata")
            ?.Get("webCommandMetadata")
            ?.Get("url")
            ?.GetString();

        if (string.IsNullOrEmpty(url))
        {
            return string.Empty;
        }

        return $"{StringSet.Origin}{url}";
    }

    /// <summary>
    /// 取得 videoRenderer 的 thumbnail 的網址字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererThumbnailUrl(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? thumbnails = jsonElement?.Get("thumbnail")
            ?.Get("thumbnails")
            ?.GetArrayEnumerator();

        return GetThumbnailUrl(arrayEnumerator: thumbnails, width: 0);
    }

    /// <summary>
    /// 取得 videoRenderer 的 title 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererTitle(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("title")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            // 理論上只會有一筆。
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(jsonElement: run);

                if (runData != null)
                {
                    return runData.Text;
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 videoRenderer 的 descriptionSnippet 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererDescriptionSnippet(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("descriptionSnippet")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            // 理論上只會有一筆。
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(jsonElement: run);

                if (runData != null)
                {
                    return runData.Text;
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 videoRenderer 的 publishedTimeText 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererPublishedTimeText(JsonElement? jsonElement)
    {
        return jsonElement?.Get("publishedTimeText")?.Get("simpleText")?.GetString();
    }

    /// <summary>
    /// 取得 videoRenderer 的 lengthText 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="simpleText">布林值，是否取得 simpleText 的文字內容，預設值為 false</param>
    /// <returns>字串</returns>
    public static string GetVideoRendererLengthText(JsonElement? jsonElement, bool simpleText = false)
    {
        JsonElement? lengthText = jsonElement?.Get("lengthText");
        JsonElement? element = simpleText ?
            lengthText?.Get("simpleText") :
            lengthText?.Get("accessibility")
                ?.Get("accessibilityData")
                ?.Get("label");

        return element?.GetString() ?? string.Empty;
    }

    /// <summary>
    /// 取得 videoRenderer 的 viewCountText 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererViewCountText(JsonElement? jsonElement)
    {
        return jsonElement?.Get("viewCountText")?.Get("simpleText")?.GetString();
    }

    /// <summary>
    /// 取得 videoRenderer 的 ownerText 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetVideoRendererOwnerText(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("ownerText")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            // 理論上只會有一筆。
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(run);

                if (runData != null)
                {
                    return runData.Text;
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得 backstageAttachment 的 pollRenderer
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>VideoData</returns>
    public static PollData? GetPollData(JsonElement? jsonElement)
    {
        return new PollData()
        {
            ChoiceDatas = GetPollRendererChoices(jsonElement: jsonElement),
            TotalVotes = GetPollRendererTotalVotes(jsonElement: jsonElement)
        };
    }

    /// <summary>
    /// 取得 pollRenderer 的 totalVotes 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetPollRendererTotalVotes(JsonElement? jsonElement)
    {
        return jsonElement?.Get("totalVotes")?.Get("simpleText")?.GetString();
    }

    /// <summary>
    /// 取得 pollRenderer 的 choices
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>List&lt;ChoiceData&gt;</returns>
    public static List<ChoiceData>? GetPollRendererChoices(JsonElement? jsonElement)
    {
        List<ChoiceData> choiceDatas = new();

        JsonElement.ArrayEnumerator? choices = jsonElement?.Get("choices")?.GetArrayEnumerator();

        if (choices != null)
        {
            foreach (JsonElement choice in choices)
            {
                string? text = GetChoicesText(jsonElement: choice);

                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                choiceDatas.Add(new ChoiceData()
                {
                    Text = text,
                    ImageUrl = GetChoicesImage(jsonElement: choice),
                    VotePercentage = GetChoicesVotePercentageIfNotSelected(jsonElement: choice)
                });
            }
        }

        return choiceDatas.Any() ? choiceDatas : null;
    }

    /// <summary>
    /// 取得 pollRenderer 的 choices 的每個項目的 text 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetChoicesText(JsonElement? jsonElement)
    {
        string? runText = string.Empty;

        JsonElement.ArrayEnumerator? runs = jsonElement?.Get("text")
            ?.Get("runs")
            ?.GetArrayEnumerator();

        if (runs != null)
        {
            // 理論上只會有一筆。
            foreach (JsonElement run in runs)
            {
                RunData? runData = GetRun(run);

                if (runData != null)
                {
                    runText += $"{runData.Text} ";
                }
            }
        }

        return string.IsNullOrEmpty(runText) ? null : runText.TrimEnd();
    }

    /// <summary>
    /// 取得 pollRenderer 的 choices 的每個項目的 votePercentageIfNotSelected 的字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetChoicesVotePercentageIfNotSelected(JsonElement? jsonElement)
    {
        return jsonElement?.Get("votePercentageIfNotSelected")?.Get("simpleText")?.GetString();
    }

    /// <summary>
    /// 取得 pollRenderer 的 choices 的每個項目的 image 的 thumbnail 的網址字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string? GetChoicesImage(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? thumbnails = jsonElement?.Get("image")
            ?.Get("thumbnails")
            ?.GetArrayEnumerator();

        return GetThumbnailUrl(arrayEnumerator: thumbnails, width: 0);
    }

    /// <summary>
    /// 取得 run 的內容
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>RunData</returns>
    public static RunData? GetRun(JsonElement? jsonElement)
    {
        JsonElement? text = jsonElement?.Get("text");

        string value = text?.GetString() ?? string.Empty;

        // 針對被縮略的網址進行替換。
        if (value.StartsWith("http") && value.EndsWith("..."))
        {
            value = GetUrl(jsonElement);
        }

        return text == null ?
            null :
            new RunData()
            {
                Text = value,
                Url = GetUrl(jsonElement),
                IsLink = IsLink(jsonElement)
            };
    }

    /// <summary>
    /// 取得 run 的網址字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetUrl(JsonElement? jsonElement)
    {
        JsonElement? url = jsonElement?.Get("navigationEndpoint")
            ?.Get("commandMetadata")
            ?.Get("webCommandMetadata")
            ?.Get("url");

        if (url != null)
        {
            string? value = url.Value.GetString();

            if (!string.IsNullOrEmpty(value) && value.StartsWith("/"))
            {
                value = $"{StringSet.Origin}{value}";
            }

            return value ?? string.Empty;
        }

        return string.Empty;
    }

    /// <summary>
    /// 判斷 run 是否為網址
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>布林值</returns>
    public static bool IsLink(JsonElement? jsonElement)
    {
        JsonElement? navigationEndpoint = jsonElement?.Get("navigationEndpoint");

        return navigationEndpoint != null;
    }

    /// <summary>
    /// 取得 backstageImageRenderer 的 thumbnail 的網址字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetBackstageImageRendererThumbnailUrl(JsonElement? jsonElement)
    {
        JsonElement.ArrayEnumerator? thumbnails = jsonElement?.Get("backstageImageRenderer")
            ?.Get("image")
            ?.Get("thumbnails")
            ?.GetArrayEnumerator();

        return GetThumbnailUrl(arrayEnumerator: thumbnails, width: 0);
    }

    /// <summary>
    /// 取得 thumbnail 的網址字串
    /// </summary>
    /// <param name="arrayEnumerator">JsonElement.ArrayEnumerator</param>
    /// <param name="width">數值，寬度，預設值為 0</param>
    /// <returns>字串</returns>
    public static string GetThumbnailUrl(
        JsonElement.ArrayEnumerator? arrayEnumerator,
        int width = 0)
    {
        string value = string.Empty;

        // 當為 width 為 0 時，自動取最後一個項目，通常為最大張圖。
        JsonElement? thumbnail = width == 0 || width == -1 ?
            arrayEnumerator?.LastOrDefault() :
            arrayEnumerator?.FirstOrDefault(n => n.Get("width") != null &&
                n.Get("width")?.GetInt32() == width);

        JsonElement? url = thumbnail?.Get("url");

        if (url != null)
        {
            value = url.Value.GetString() ?? string.Empty;
        }

        if (value.StartsWith("//"))
        {
            value = $"https:{value}";
        }

        // 用以取得完整未裁切的圖片。
        if (value.Contains("-c-fcrop64="))
        {
            string[] tempArray = value.Split("-c-fcrop64=");

            value = tempArray[0];
        }

        // 將 "=s***" 替換成 "=s0" 可以取得原圖。
        if (width == -1)
        {
            if (value.Contains("=s"))
            {
                string[] tempArray = value.Split("=s");

                value = tempArray[0] + "=s0";
            }
        }

        return value;
    }

    /// <summary>
    /// 取得 continuationItemRenderer 的 token 字串
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>字串</returns>
    public static string GetToken(JsonElement? jsonElement)
    {
        JsonElement? token = jsonElement?.Get("continuationItemRenderer")
            ?.Get("continuationEndpoint")
            ?.Get("continuationCommand")
            ?.Get("token");

        if (token != null && token.HasValue)
        {
            return token.Value.GetString() ?? string.Empty;
        }

        return string.Empty;
    }
}