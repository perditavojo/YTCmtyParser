using JNogueira.Discord.Webhook.Client;
using System.Drawing;
using System.Web;
using YTApi.Commons.Models;
using Color = System.Drawing.Color;

namespace YTCmtyParser.Commons.Utils;

/// <summary>
/// Discord 工具類別
/// </summary>
public class DiscordUtil
{
    /// <summary>
    /// 傳送至 Discord
    /// <para>※只支援文字頻道。</para>
    /// </summary>
    /// <param name="postData">字串，PostData</param>
    /// <param name="disocrdWebhookUrl">字串，Discord 的 Webhook 網址</param>
    public static async Task SendToDiscord(PostData postData, string disocrdWebhookUrl)
    {
        DiscordWebhookClient discordWebhookClient = new(disocrdWebhookUrl);

        string content = string.Empty;

        content += $"> 貼文網址：<{postData.Url}>{Environment.NewLine}";
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

        DiscordMessageEmbed[]? embeds = Array.Empty<DiscordMessageEmbed>();

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

        if (embeds == Array.Empty<DiscordMessageEmbed>())
        {
            embeds = null;
        }

        DiscordMessage discordMessage = new(
            content: content,
            username: postData.AuthorText,
            avatarUrl: postData.AuthorThumbnailUrl,
            tts: false,
            embeds: embeds);

        await discordWebhookClient.SendToDiscord(
            message: discordMessage,
            files: files,
            sendMessageAsFileAttachmentOnError: true);
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