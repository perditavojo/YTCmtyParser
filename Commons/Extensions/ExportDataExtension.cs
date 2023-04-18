using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Web;
using YTApi.Commons.Models;

namespace YTCmtyParser.Commons.Extensions;

/// <summary>
/// ExportData 的擴充方法
/// </summary>
public static partial class ExportDataExtension
{
    /// <summary>
    /// 轉換成 JSON 字串
    /// </summary>
    /// <returns>字串</returns>
    public static string ToJsonString(this ExportData exportData, bool unescape = false)
    {
        JsonSerializerOptions options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };

        string jsonContent = JsonSerializer.Serialize(value: exportData, options: options);

        if (unescape)
        {
            jsonContent = Regex.Unescape(jsonContent);
            jsonContent = HttpUtility.UrlDecode(jsonContent);
        }

        return jsonContent;
    }
}