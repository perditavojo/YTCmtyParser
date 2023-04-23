namespace YTCmtyParser.Commons.Utils;

/// <summary>
/// Preferences 工具
/// </summary>
public class PreferencesUtil
{
    /// <summary>
    /// SharedName
    /// </summary>
    private static string? SharedName;

    /// <summary>
    /// 取得 SharedName
    /// </summary>
    /// <returns>字串</returns>
    public static string? GetSharedName()
    {
        return SharedName;
    }

    /// <summary>
    /// 設定 SharedName
    /// </summary>
    /// <param name="value">字串，值</param>
    public static void SetSharedName(string? value)
    {
        SharedName = value;
    }

    /// <summary>
    /// 是否有此鍵值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns>布林值</returns>
    public static bool HasKey(string key)
    {
        return Preferences.Default.ContainsKey(
            key: key,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得布林值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns>布林值</returns>
    public static bool GetBooleanValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: false,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得 Double 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns>double</returns>
    public static double GetDoubleValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: 0d,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得 Int 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns></returns>
    public static int GetIntValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: 0,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得 Float 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns></returns>
    public static float GetFloatValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: 0f,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得 Long 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns>long</returns>
    public static long GetLongValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: 0L,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得字串值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns>字串</returns>
    public static string GetStringValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: string.Empty,
            sharedName: SharedName);
    }

    /// <summary>
    /// 取得 DateTime 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <returns>DateTime</returns>
    public static DateTime GetDateTimeValue(string key)
    {
        return Preferences.Default.Get(
            key: key,
            defaultValue: DateTime.Now,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定布林值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">布林值</param>
    public static void SetValue(string key, bool? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? false,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定 Double 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">double</param>
    public static void SetValue(string key, double? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? 0d,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定 Int 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">int</param>
    public static void GetValue(string key, int? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? 0,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定 Float 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">float</param>
    public static void GetValue(string key, float? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? 0f,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定 Long 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">long</param>
    public static void GetValue(string key, long? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? 0L,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定字串值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">字串</param>
    public static void SetValue(string key, string? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? string.Empty,
            sharedName: SharedName);
    }

    /// <summary>
    /// 設定 DateTime 值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    /// <param name="value">DateTime</param>
    public static void SetValue(string key, DateTime? value)
    {
        Preferences.Default.Set(
            key: key,
            value: value ?? DateTime.Now,
            sharedName: SharedName);
    }

    /// <summary>
    /// 移除鍵值
    /// </summary>
    /// <param name="key">字串，鍵值</param>
    public static void RemoveKey(string key)
    {
        Preferences.Default.Remove(
            key: key,
            sharedName: SharedName);
    }

    /// <summary>
    /// 清除全部鍵值
    /// </summary>
    public static void ClearAllKeys()
    {
        Preferences.Default.Clear(sharedName: SharedName);
    }
}