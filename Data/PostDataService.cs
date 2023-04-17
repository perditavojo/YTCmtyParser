using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using YTApi.Commons;
using YTApi.Commons.Models;
using YTCmtyParser.Commons;

namespace YTCmtyParser.Data;

/// <summary>
/// PostData 服務
/// </summary>
public class PostDataService
{
    /// <summary>
    /// 取得 InitialData
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="channelID">字串，YouTube 頻道的 ID</param>
    /// <param name="cookies">字串，Cookies</param>
    /// <returns>InitialData</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<暫止>")]
    public InitialData GetInitialData(
        HttpClient httpClient,
        string channelID,
        string cookies)
    {
        return Function.GetInitialData(
            httpClient: httpClient,
            channelID: channelID,
            cookies: cookies);
    }

    /// <summary>
    /// 取得初始的 PostData
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <returns>List&lt;PostData&gt;</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<暫止>")]
    public List<PostData> GetInitialPosts(JsonElement? jsonElement, YTConfig? ytConfig)
    {
        return CustomFunction.GetInitialPosts(
            jsonElement: jsonElement,
            ytConfig: ytConfig);
    }
}