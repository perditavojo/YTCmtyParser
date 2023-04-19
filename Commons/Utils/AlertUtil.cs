using CommunityToolkit.Maui.Alerts;

namespace YTCmtyParser.Commons.Utils;

/// <summary>
/// 警報工具類別
/// </summary>
public class AlertUtil
{
    /// <summary>
    /// 顯示 Toast
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task ShowToast(string message, CancellationToken ct = default)
    {
        await Toast.Make(message).Show(ct);
    }

    /// <summary>
    ///  顯示警報
    /// </summary>
    /// <param name="title">字串，標題</param>
    /// <param name="message">字串，訊息</param>
    /// <param name="cancel">字串，取消</param>
    /// <returns>Task</returns>
    public static async Task ShowAlert(string title, string message, string cancel = "確認")
    {
        if (Application.Current?.MainPage != null == true)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }

    /// <summary>
    /// 顯示警告警報
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="cancel">字串，取消</param>
    /// <returns>Task</returns>
    public static async Task ShowWarningAlert(string message, string cancel = "確認")
    {
        if (Application.Current?.MainPage != null == true)
        {
            await Application.Current.MainPage.DisplayAlert("警告", message, cancel);
        }
    }

    /// <summary>
    /// 顯示錯誤警報
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="cancel">字串，取消</param>
    /// <returns>Task</returns>
    public static async Task ShowErrorAlert(string message, string cancel = "確認")
    {
        if (Application.Current?.MainPage != null == true)
        {
            await Application.Current.MainPage.DisplayAlert("錯誤", message, cancel);
        }
    }
}