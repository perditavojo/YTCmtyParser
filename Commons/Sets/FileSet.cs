namespace YTCmtyParser.Commons.Sets;

/// <summary>
/// 檔案組
/// </summary>
public class FileSet
{
    /// <summary>
    /// Channels.json
    /// </summary>
    public static readonly string ChannelsJson = Path.Combine(FileSystem.Current.AppDataDirectory, "Channels.json");

    /// <summary>
    /// Webhooks.json
    /// </summary>
    public static readonly string WebhooksJson = Path.Combine(FileSystem.Current.AppDataDirectory, "Webhooks.json");
}