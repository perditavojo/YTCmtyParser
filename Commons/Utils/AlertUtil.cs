using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace YTCmtyParser.Commons.Utils;

/// <summary>
/// 警報工具類別
/// </summary>
public class AlertUtil
{
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
    /// <param name="destruction">字串，破壞</param>
    /// <param name="cancel">字串，取消</param>
    /// <returns>Task&lt;string&gt;</returns>
    public static async Task<string?> ShowActionSheet(
            string title,
            string?[]? buttons,
            string? destruction = null,
            string cancel = "取消")
    {
        if (Application.Current?.MainPage != null == true)
        {
            return await Application.Current.MainPage.DisplayActionSheet(
                title: title,
                cancel: cancel,
                destruction: destruction,
                buttons: buttons);
        }

        return null;
    }

    /// <summary>
    /// 顯示 Toast
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="textSize">數值，文字大小，預設值為 14</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task ShowToast(
        string message,
        double textSize = 14,
        CancellationToken ct = default)
    {
        await Task.Delay(
            millisecondsDelay: 500,
            cancellationToken: ct);
        await Toast.Make(
                message: message,
                duration: ToastDuration.Short,
                textSize: textSize)
            .Show(token: ct);
    }

    /// <summary>
    /// 顯示長時間的 Toast
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="textSize">數值，文字大小，預設值為 14</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task ShowLongToast(
        string message,
        double textSize = 14,
        CancellationToken ct = default)
    {
        await Task.Delay(
            millisecondsDelay: 500,
            cancellationToken: ct);
        await Toast.Make(
                message: message,
                duration: ToastDuration.Long,
                textSize: textSize)
            .Show(token: ct);
    }

    /// <summary>
    /// 顯示 Snackbar
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="action">Action</param>
    /// <param name="actionButtonText">字串，行為按鈕的文字，預設值為 "確認"</param>
    /// <param name="duration">TimeSpan，顯示時長</param>
    /// <param name="visualOptions">SnackbarOptions，視覺選項</param>
    /// <param name="anchor">IView，錨點</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task ShowSnackbar(
        string message,
        Action? action = null,
        string actionButtonText = "確認",
        TimeSpan? duration = null,
        SnackbarOptions? visualOptions = null,
        IView? anchor = null,
        CancellationToken ct = default)
    {
        await Snackbar.Make(
                message: message,
                action: action,
                actionButtonText: actionButtonText,
                duration: duration,
                visualOptions: visualOptions,
                anchor: anchor)
            .Show(token: ct);
    }
}