using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using YTApi.Commons;
using YTApi.Commons.Models;
using YTCmtyParser.Commons;

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
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<暫止>")]
    public async Task<InitialData> GetInitialData(
        HttpClient httpClient,
        string channelID,
        string cookies)
    {
        return await Functions.GetInitialData(
            httpClient: httpClient,
            channelID: channelID,
            cookies: cookies);
    }

    /// <summary>
    /// 取得初始的貼文
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <returns>List&lt;PostData&gt;</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<暫止>")]
    public List<PostData> GetInitialPosts(
        JsonElement? jsonElement,
        YTConfig? ytConfig)
    {
        return CustomFunction.GetInitialPosts(
            jsonElement: jsonElement,
            ytConfig: ytConfig);
    }

    /// <summary>
    /// 取得先前的貼文
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="ytConfig">YTConfig</param>
    /// <param name="cookies">字串，Cookies</param>
    /// <param name="referer">字串，HTTP 參照位址</param>
    /// <returns>Task&lt;List&lt;PostData&gt;&gt;</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<暫止>")]
    public async Task<List<PostData>> GetEarlierPosts(
        HttpClient httpClient,
        YTConfig? ytConfig,
        string cookies,
        string referer)
    {
        return await CustomFunction.GetEarlierPosts(
            httpClient: httpClient,
            ytConfig: ytConfig,
            cookies: cookies,
            referer: referer);
    }
}