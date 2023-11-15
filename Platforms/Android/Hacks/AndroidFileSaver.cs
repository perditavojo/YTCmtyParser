using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Webkit;
using AndroidUri = Android.Net.Uri;
using Application = Android.App.Application;
using CommunityToolkit.Maui.Core.Essentials;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Storage;
using Java.IO;
using System.Runtime.Versioning;
using System.Web;
using Trace = System.Diagnostics.Trace;

namespace YTCmtyParser;

/// <summary>
/// AndroidFileSaver
/// <para>來源：https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.Core/Essentials/FileSaver/FileSaverImplementation.android.cs</para>
/// </summary>
[SupportedOSPlatform("Android")]
public sealed class AndroidFileSaver : IFileSaver
{
    public async Task<FileSaverResult> SaveAsync(string initialPath, string fileName, Stream stream, CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(26) && !string.IsNullOrEmpty(initialPath))
        {
            Trace.WriteLine("Specifying an initial path is only supported on Android 26 and later.");
        }

        AndroidUri? filePath = null;

        // 用以避免在 Android API Level 為 30 或以上版本時，繼續請求 Storage 權限的情況。
        if (DeviceInfo.Platform == DevicePlatform.Android && !OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageWrite>().WaitAsync(cancellationToken).ConfigureAwait(false);

            if (status is not PermissionStatus.Granted)
            {
                throw new PermissionException("Storage permission is not granted.");
            }
        }

        if (Android.OS.Environment.ExternalStorageDirectory is not null)
        {
            initialPath = initialPath.Replace(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, string.Empty, StringComparison.InvariantCulture);
        }

        AndroidUri? initialFolderUri = AndroidUri.Parse(CustomAndroidStorageConstants.ExternalStorageBaseUrl + HttpUtility.UrlEncode(initialPath));

        Intent intent = new(Intent.ActionCreateDocument);

        intent.AddCategory(Intent.CategoryOpenable);
        intent.SetType(MimeTypeMap.Singleton?.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(fileName)) ?? "*/*");
        intent.PutExtra(Intent.ExtraTitle, fileName);

        if (OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            intent.PutExtra(DocumentsContract.ExtraInitialUri, initialFolderUri);
        }

        await CustomIntermediateActivity.StartAsync(intent, (int)CustomAndroidRequestCode.RequestCodeSaveFilePicker, onResult: OnResult).WaitAsync(cancellationToken).ConfigureAwait(false);

        if (filePath is null)
        {
            throw new FileSaveException("Path doesn't exist.");
        }

        string returnedFilePath = await SaveDocument(filePath, stream, cancellationToken).ConfigureAwait(false);

        void OnResult(Intent resultIntent)
        {
            filePath = EnsurePhysicalPath(resultIntent.Data);
        }

        return new FileSaverResult(FilePath: returnedFilePath, Exception: null);
    }

    public Task<FileSaverResult> SaveAsync(string fileName, Stream stream, CancellationToken cancellationToken)
    {
        return SaveAsync(AndroidPathExtensions.GetExternalDirectory(), fileName, stream, cancellationToken);
    }

    static AndroidUri EnsurePhysicalPath(AndroidUri? uri)
    {
        if (uri is null)
        {
            throw new FileSaveException("Path is not selected.");
        }

        const string uriSchemeFolder = "content";

        if (uri.Scheme is not null && uri.Scheme.Equals(uriSchemeFolder, StringComparison.OrdinalIgnoreCase))
        {
            return uri;
        }

        throw new FileSaveException($"Unable to resolve absolute path or retrieve contents of URI '{uri}'.");
    }

    static async Task<string> SaveDocument(AndroidUri uri, Stream stream, CancellationToken cancellationToken)
    {
        using ParcelFileDescriptor? parcelFileDescriptor = Application.Context.ContentResolver?.OpenFileDescriptor(uri, "wt");
        using FileOutputStream fileOutputStream = new(parcelFileDescriptor?.FileDescriptor);

        await using MemoryStream memoryStream = new();

        await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
        await fileOutputStream.WriteAsync(memoryStream.ToArray()).WaitAsync(cancellationToken).ConfigureAwait(false);

        return uri.ToPhysicalPath() ?? throw new FileSaveException($"Unable to resolve absolute path where the file was saved '{uri}'.");
    }
}