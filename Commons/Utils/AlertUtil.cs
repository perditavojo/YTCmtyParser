using CommunityToolkit.Maui.Alerts;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

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
            await Application.Current.MainPage.DisplayAlert(
                title: title,
                message: message,
                cancel: cancel);
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
            await Application.Current.MainPage.DisplayAlert(
                title: "警告",
                message: message,
                cancel: cancel);
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
            await Application.Current.MainPage.DisplayAlert(
                title: "錯誤",
                message: message,
                cancel: cancel);
        }
    }

    /// <summary>
    /// 顯示確認警報
    /// </summary>
    /// <param name="title">字串，標題</param>
    /// <param name="message">字串，訊息</param>
    /// <param name="cancel">字串，取消</param>
    /// <returns>Task&lt;bool&gt;</returns>
    public static async Task<bool?> ShowConfirmAlert(
            string title,
            string message,
            string accept = "確認",
            string cancel = "取消")
    {
        if (Application.Current?.MainPage != null == true)
        {
            return await Application.Current.MainPage.DisplayAlert(
                title: title,
                message: message,
                accept: accept,
                cancel: cancel);
        }

        return null;
    }

    /// <summary>
    ///  顯示行為表單
    /// </summary>
    /// <param name="title">字串，標題</param>
    /// <param name="buttons">字串陣列，按鈕</param>
    /// <param name="cancel">字串，取消</param>
    /// <returns>Task&lt;string&gt;</returns>
    public static async Task<string?> ShowActionSheet(
            string title,
            string?[]? buttons,
            string cancel = "取消")
    {
        if (Application.Current?.MainPage != null == true)
        {
            return await Application.Current.MainPage.DisplayActionSheet(
                title: title,
                cancel: cancel,
                destruction: null,
                buttons: buttons);
        }

        return null;
    }
}