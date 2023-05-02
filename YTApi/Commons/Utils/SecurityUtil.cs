using System.Security.Cryptography;
using System.Text;

namespace YTApi.Commons.Utils;

/// <summary>
/// 安全工具類別
/// </summary>
public static class SecurityUtil
{
    /// <summary>
    /// 取得 SAPISIDHASH 字串
    /// </summary>
    /// <param name="sapiSid">字串，SAPISID</param>
    /// <param name="origin">字串，origin</param>
    /// <returns>字串</returns>
    public static string GetSapiSidHash(string sapiSid, string origin)
    {
        long unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

        return $"{unixTimestamp}_{GetSHA1Hash($"{unixTimestamp} {sapiSid} {origin}")}";
    }

    /// <summary>
    /// 取得 SHA-1 雜湊 
    /// </summary>
    /// <param name="value">字串，值</param>
    /// <returns>字串</returns>
    private static string GetSHA1Hash(string value)
    {
        byte[] bytes = SHA1.HashData(Encoding.UTF8.GetBytes(value));

        StringBuilder builder = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}