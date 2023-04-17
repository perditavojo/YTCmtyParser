using System.Text.RegularExpressions;

namespace YTApi.Commons.Sets;

/// <summary>
/// 正規表達式組
/// </summary>
public static class RegexSet
{
    /// <summary>
    /// v
    /// </summary>
    public static readonly Regex VideoID = new("v=(.+)");

    /// <summary>
    /// INNERTUBE_API_KEY
    /// </summary>
    public static readonly Regex InnertubeApiKey = new("INNERTUBE_API_KEY\":\"(.+?)\",");

    /// <summary>
    /// continuation
    /// </summary>
    public static readonly Regex Continuation = new("continuation\":\"(.+?)\",");

    /// <summary>
    /// visitorData
    /// </summary>
    public static readonly Regex VisitorData = new("visitorData\":\"(.+?)\",");

    /// <summary>
    /// clientName
    /// </summary>
    public static readonly Regex ClientName = new("clientName\":\"(.+?)\",");

    /// <summary>
    /// clientVersion
    /// </summary>
    public static readonly Regex ClientVersion = new("clientVersion\":\"(.+?)\",");

    /// <summary>
    /// ID_TOKEN
    /// </summary>
    public static readonly Regex IDToken = new("ID_TOKEN\"(.+?)\",");

    /// <summary>
    /// SESSION_INDEX
    /// </summary>
    public static readonly Regex SessionIndex = new("SESSION_INDEX\":\"(.*?)\"");

    /// <summary>
    /// INNERTUBE_CONTEXT_CLIENT_NAME
    /// </summary>
    public static readonly Regex InnertubeContextClientName = new("INNERTUBE_CONTEXT_CLIENT_NAME\":(.*?),");

    /// <summary>
    /// INNERTUBE_CONTEXT_CLIENT_VERSION
    /// </summary>
    public static readonly Regex InnertubeContextClientVersion = new("INNERTUBE_CONTEXT_CLIENT_VERSION\":\"(.*?)\"");

    /// <summary>
    /// INNERTUBE_CLIENT_VERSION
    /// </summary>
    public static readonly Regex InnertubeClientVersion = new("INNERTUBE_CLIENT_VERSION\":\"(.*?)\"");

    /// <summary>
    /// DATASYNC_ID
    /// </summary>
    public static readonly Regex DatasyncID = new("DATASYNC_ID\":\"(.*?)\"");

    /// <summary>
    /// DELEGATED_SESSION_ID
    /// </summary>
    public static readonly Regex DelegatedSessionID = new("DELEGATED_SESSION_ID\":\"(.*?)\"");
}