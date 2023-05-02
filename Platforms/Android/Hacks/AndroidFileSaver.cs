using System.Runtime.Versioning;
using System.Web;
using CommunityToolkit.Maui.Storage;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Webkit;
using Java.IO;
using AndroidUri = Android.Net.Uri;
using Application = Android.App.Application;

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
        FileSaveException? fileSaveException = null;

        // 用以避免在 Android API Level 為 30 或以上版本時，繼續請求 Storage 權限的情況。
        if (DeviceInfo.Platform == DevicePlatform.Android &&
            !OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageWrite>().WaitAsync(cancellationToken).ConfigureAwait(false);

            if (status is not PermissionStatus.Granted)
            {
                throw new PermissionException("Storage permission is not granted.");
            }
        }

        const string baseUrl = "content://com.android.externalstorage.documents/document/primary%3A";

        if (Android.OS.Environment.ExternalStorageDirectory is not null)
        {
            initialPath = initialPath.Replace(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, string.Empty, StringComparison.InvariantCulture);
        }

        AndroidUri? initialFolderUri = AndroidUri.Parse(baseUrl + HttpUtility.UrlEncode(initialPath));

        Intent intent = new(Intent.ActionCreateDocument);

        intent.AddCategory(Intent.CategoryOpenable);
        intent.SetType(MimeTypeMap.Singleton?.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(fileName)) ?? "*/*");
        intent.PutExtra(Intent.ExtraTitle, fileName);
        intent.PutExtra(DocumentsContract.ExtraInitialUri, initialFolderUri);

        AndroidUri? filePath = null;

        await CustomIntermediateActivity.StartAsync(intent, (int)CustomAndroidRequestCode.RequestCodeSaveFilePicker, onResult: OnResult).WaitAsync(cancellationToken).ConfigureAwait(false);

        if (filePath is null)
        {
            fileSaveException = new FileSaveException("Path doesn't exist.");

            return new FileSaverResult(FilePath: null, Exception: fileSaveException);
        }

        string returnedFilePath = await SaveDocument(filePath, stream, cancellationToken).ConfigureAwait(false);

        void OnResult(Intent resultIntent)
        {
            filePath = EnsurePhysicalPath(resultIntent.Data);
        }

        return new FileSaverResult(FilePath: returnedFilePath, Exception: fileSaveException);
    }

    public Task<FileSaverResult> SaveAsync(string fileName, Stream stream, CancellationToken cancellationToken)
    {
        return SaveAsync(GetExternalDirectory(), fileName, stream, cancellationToken);
    }

    static string GetExternalDirectory()
    {
        return Android.OS.Environment.ExternalStorageDirectory?.Path ?? "/storage/emulated/0";
    }

    static AndroidUri EnsurePhysicalPath(AndroidUri? uri)
    {
        if (uri is null)
        {
            throw new FolderPickerException("Path is not selected.");
        }

        const string uriSchemeFolder = "content";

        if (uri.Scheme is not null && uri.Scheme.Equals(uriSchemeFolder, StringComparison.OrdinalIgnoreCase))
        {
            return uri;
        }

        throw new FolderPickerException($"Unable to resolve absolute path or retrieve contents of URI '{uri}'.");
    }

    static async Task<string> SaveDocument(AndroidUri uri, Stream stream, CancellationToken cancellationToken)
    {
        ParcelFileDescriptor? parcelFileDescriptor = Application.Context.ContentResolver?.OpenFileDescriptor(uri, "w");
        FileOutputStream fileOutputStream = new(parcelFileDescriptor?.FileDescriptor);

        await using MemoryStream memoryStream = new();

        stream.Seek(0, SeekOrigin.Begin);

        await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
        await fileOutputStream.WriteAsync(memoryStream.ToArray()).WaitAsync(cancellationToken).ConfigureAwait(false);

        fileOutputStream.Close();
        parcelFileDescriptor?.Close();

        string[] split = uri.Path?.Split(':') ?? throw new FolderPickerException("Unable to resolve path.");

        return $"{Android.OS.Environment.ExternalStorageDirectory}/{split[^1]}";
    }
}