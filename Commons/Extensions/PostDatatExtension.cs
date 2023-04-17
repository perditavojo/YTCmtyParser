using System.Text;
using System.Web;
using YTApi.Commons.Models;

namespace YTCmtyParser.Commons.Extensions;

/// <summary>
/// PostData 的擴充方法
/// </summary>
public static partial class PostDataExtension
{
    /// <summary>
    /// 轉換成格式化的字串
    /// </summary>
    /// <returns>字串</returns>
    public static string ToFormattedString(this PostData postData)
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"誰能看到：{(postData.IsSponsorsOnly ? "頻道會員專屬" : "所有頻道會員")}");
        stringBuilder.AppendLine($"貼文 ID：{postData.PostID}");
        stringBuilder.AppendLine($"貼文網址：{postData.Url}");
        stringBuilder.AppendLine($"作者：{postData.AuthorText}");
        stringBuilder.AppendLine($"作者頭像網址：{postData.AuthorThumbnailUrl}");

        if (postData.ContentTexts?.Count > 0)
        {
            stringBuilder.AppendLine($"貼文內容：{Environment.NewLine}");

            foreach (RunData contentText in postData.ContentTexts)
            {
                string? value = contentText.Text;

                if (contentText.IsLink)
                {
                    if (value != contentText.Url)
                    {
                        value += $" ({HttpUtility.UrlDecode(contentText.Url)})";
                    }
                }

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                stringBuilder.AppendLine(value);
            }
        }

        stringBuilder.AppendLine($"發布時間：{postData.PublishedTimeText}");
        stringBuilder.AppendLine($"投票次數：{postData.VoteCount}");

        if (postData.Attachments?.Count > 0)
        {
            stringBuilder.AppendLine($"附件內容：");

            foreach (AttachmentData attachment in postData.Attachments)
            {
                if (attachment.IsVideo == true)
                {
                    VideoData? videoData = attachment.VideoData;

                    if (videoData != null)
                    {
                        stringBuilder.AppendLine($"網址：{videoData.Url}");
                        stringBuilder.AppendLine($"縮略圖網址：{videoData.ThumbnailUrl}");
                        stringBuilder.AppendLine($"標題：{videoData.Title}");
                        stringBuilder.AppendLine($"描述：{videoData.DescriptionSnippet}");
                        stringBuilder.AppendLine($"發布時間：{videoData.PublishedTimeText}");
                        stringBuilder.AppendLine($"長度：{videoData.LengthText}");
                        stringBuilder.AppendLine($"{videoData.ViewCountText}");
                        stringBuilder.AppendLine($"所有者：{videoData.OwnerText}");
                    }
                }
                else
                {
                    stringBuilder.AppendLine(attachment.Url);
                }
            }
        }

        stringBuilder.AppendLine(string.Empty);
        stringBuilder.AppendLine("=======");
        stringBuilder.AppendLine(string.Empty);

        return stringBuilder.ToString();
    }
}