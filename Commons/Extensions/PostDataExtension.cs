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
        stringBuilder.AppendLine($"貼文 ID：{postData.PostID ?? "無"}");
        stringBuilder.AppendLine($"貼文網址：{postData.Url ?? "無"}");
        stringBuilder.AppendLine($"作者：{postData.AuthorText ?? "無"}");
        stringBuilder.AppendLine($"作者頭像網址：{postData.AuthorThumbnailUrl ?? "無"}");

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

        stringBuilder.AppendLine($"發布時間：{postData.PublishedTimeText ?? "無"}");
        stringBuilder.AppendLine($"投票次數：{postData.VoteCount ?? "無"}");

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
                        stringBuilder.AppendLine($"網址：{videoData.Url ?? "無"}");
                        stringBuilder.AppendLine($"縮略圖網址：{videoData.ThumbnailUrl ?? "無"}");
                        stringBuilder.AppendLine($"標題：{videoData.Title ?? "無"}");
                        stringBuilder.AppendLine($"描述：{videoData.DescriptionSnippet ?? "無"}");
                        stringBuilder.AppendLine($"發布時間：{videoData.PublishedTimeText ?? "無"}");
                        stringBuilder.AppendLine($"長度：{videoData.LengthText ?? "無"}");
                        stringBuilder.AppendLine($"{videoData.ViewCountText ?? "無"}");
                        stringBuilder.AppendLine($"所有者：{videoData.OwnerText ?? "無"}");
                    }
                }
                else if (attachment.IsPoll == true)
                {
                    PollData? pollData = attachment.PollData;

                    if (pollData != null)
                    {
                        List<ChoiceData>? choiceDatas = pollData.ChoiceDatas;

                        if (choiceDatas?.Any() == true)
                        {
                            stringBuilder.AppendLine("投票選項：");

                            foreach (ChoiceData choiceData in choiceDatas)
                            {
                                stringBuilder.AppendLine($"- {choiceData.Text}");

                                if (!string.IsNullOrEmpty(choiceData.VotePercentage))
                                {
                                    stringBuilder.AppendLine($"  - 得票率：{choiceData.VotePercentage}");
                                }

                                if (!string.IsNullOrEmpty(choiceData.NumVotes))
                                {
                                    stringBuilder.AppendLine($"  - 得票數：{choiceData.NumVotes} 票");
                                }

                                if (!string.IsNullOrEmpty(choiceData.ImageUrl))
                                {
                                    stringBuilder.AppendLine($"圖片網址：{choiceData.ImageUrl}");
                                }
                            }

                            stringBuilder.AppendLine(string.Empty);
                            stringBuilder.AppendLine($"總投票票數：{pollData?.TotalVotes}");
                        }
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