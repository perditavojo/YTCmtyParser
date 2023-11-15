namespace CommunityToolkit.Maui.Core.Essentials;

/// <summary>
/// 自定義 AndroidRequestCode
/// <para>來源：https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.Core/Essentials/AndroidRequestCode.android.cs</para>
/// </summary>
static class CustomAndroidStorageConstants
{
    public const string PrimaryStorage = "primary";
    public const string Storage = "storage";
    public const string ExternalStorageBaseUrl = "content://com.android.externalstorage.documents/document/primary%3A";
}