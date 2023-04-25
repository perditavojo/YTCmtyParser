using Android.App;
using Android.Runtime;

[assembly: UsesPermission(Android.Manifest.Permission.ReadMediaImages)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadMediaAudio)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadMediaVideo)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage, MaxSdkVersion = 32)]
// TODO: 2023-04-25 因為 FileSaver 的需求，故不加入 MaxSdkVersion = 29。
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]

namespace YTCmtyParser;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}