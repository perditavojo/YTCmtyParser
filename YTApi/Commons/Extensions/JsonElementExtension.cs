using System.Text.Json;

namespace YTApi.Commons.Extensions;

/// <summary>
/// JsonElement 的擴充方法
/// <para>來源：https://stackoverflow.com/a/61561343 </para>
/// </summary>
public static partial class JsonElementExtension
{
    /// <summary>
    /// 取得指定名稱的 JsonElement
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="name">字串，名稱</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? Get(this JsonElement jsonElement, string name) =>
        jsonElement.ValueKind != JsonValueKind.Null &&
        jsonElement.ValueKind != JsonValueKind.Undefined &&
        jsonElement.TryGetProperty(name, out var value)
            ? value : null;

    /// <summary>
    /// 取得指定索引值的 JsonElement
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <param name="index">數值，索引值</param>
    /// <returns>JsonElement</returns>
    public static JsonElement? Get(this JsonElement jsonElement, int index)
    {
        if (jsonElement.ValueKind == JsonValueKind.Null ||
            jsonElement.ValueKind == JsonValueKind.Undefined)
        {
            return null;
        }

        var value = jsonElement.EnumerateArray().ElementAtOrDefault(index);

        return value.ValueKind != JsonValueKind.Undefined ? value : null;
    }

    /// <summary>
    /// 取得 ArrayEnumerator
    /// </summary>
    /// <param name="jsonElement">JsonElement</param>
    /// <returns>JsonElement.ArrayEnumerator</returns>
    public static JsonElement.ArrayEnumerator? GetArrayEnumerator(this JsonElement jsonElement)
    {
        return jsonElement.ValueKind == JsonValueKind.Array ?
            jsonElement.EnumerateArray() :
            null;
    }
}