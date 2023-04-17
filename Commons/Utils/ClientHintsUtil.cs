using System.Net;
using YTCmtyParser.Commons.Sets;

namespace YTCmtyParser.Commons.Utils;

/// <summary>
/// Client Hints 工具
/// </summary>
public class ClientHintsUtil
{
    /// <summary>
    /// Client Hints
    /// <para>來源 1：https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers#client_hints </para>
    /// <para>來源 2：https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers#fetch_metadata_request_headers </para>
    /// </summary>
    private static readonly Dictionary<string, string> KeyValues = new()
    {
	    //{ "Sec-CH-Prefers-Reduced-Motion", string.Empty },
	    { "Sec-CH-UA", StringSet.SecCHUA },
	    //{ "Sec-CH-UA-Arch", string.Empty },
	    //{ "Sec-CH-UA-Bitness",string.Empty },
	    // Deprecated.
	    //{ "Sec-CH-UA-Full-Version", string.Empty },
	    //{ "Sec-CH-UA-Full-Version-List", string.Empty },
	    { "Sec-CH-UA-Mobile", StringSet.SecCHUAMobile },
	    //{ "Sec-CH-UA-Model", string.Empty },
	    { "Sec-CH-UA-Platform", StringSet.SecCHUAPlatform },
	    //{ "Sec-CH-UA-Platform-Version", string.Empty },
	    { "Sec-Fetch-Site", StringSet.SecFetchSite },
        { "Sec-Fetch-Mode", StringSet.SecFetchMode },
	    //{ "Sec-Fetch-User", StringSet.SecFetchUser },
	    { "Sec-Fetch-Dest", StringSet.SecFetchDest }
    };

    /// <summary>
    /// 設定 Client Hints 標頭資訊
    /// </summary>
    /// <param name="webHeaderCollection">HttpClient</param>
    public static void SetClientHints(WebHeaderCollection webHeaderCollection)
    {
        foreach (KeyValuePair<string, string> item in KeyValues)
        {
            webHeaderCollection.Add(item.Key, item.Value);
        }
    }

    /// <summary>
    /// 設定 Client Hints 標頭資訊
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    public static void SetClientHints(HttpClient? httpClient)
    {
        foreach (KeyValuePair<string, string> item in KeyValues)
        {
            httpClient?.DefaultRequestHeaders.Add(item.Key, item.Value);
        }
    }
}