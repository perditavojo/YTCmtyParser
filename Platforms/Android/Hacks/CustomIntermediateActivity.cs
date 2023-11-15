using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Collections.Concurrent;
using System.Runtime.Versioning;

namespace Microsoft.Maui.ApplicationModel;

/// <summary>
/// 自定義 IntermediateActivity
/// <para>來源：https://github.com/dotnet/maui/blob/main/src/Essentials/src/Platform/IntermediateActivity.android.cs</para>
/// </summary>
[SupportedOSPlatform("Android")]
[Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, Exported = true)]
class CustomIntermediateActivity : Activity
{
    const string launchedExtra = "launched";
    const string actualIntentExtra = "actual_intent";
    const string guidExtra = "guid";
    const string requestCodeExtra = "request_code";

    static readonly ConcurrentDictionary<string, IntermediateTask> pendingTasks = new();

    bool launched;
    Intent? actualIntent;
    string? guid;
    int requestCode;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Bundle? extras = savedInstanceState ?? Intent?.Extras;

        // read the values
        launched = extras?.GetBoolean(launchedExtra, false) ?? false;
#pragma warning disable 618 // TODO: one day use the API 33+ version: https://developer.android.com/reference/android/os/Bundle#getParcelable(java.lang.String,%20java.lang.Class%3CT%3E)
#pragma warning disable CA1422 // Validate platform compatibility
#pragma warning disable CA1416 // Validate platform compatibility
        actualIntent = extras?.GetParcelable(actualIntentExtra) as Intent;
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore 618
        guid = extras?.GetString(guidExtra);
        requestCode = extras?.GetInt(requestCodeExtra, -1) ?? -1;

        if (GetIntermediateTask(guid) is IntermediateTask task)
        {
            task.OnCreate?.Invoke(actualIntent!);
        }

        // If this is the first time, lauch the real activity.
        if (!launched)
        {
            StartActivityForResult(actualIntent, requestCode);
        }
    }

    protected override void OnSaveInstanceState(Bundle outState)
    {
        // Make sure we mark this activity as launched.
        outState.PutBoolean(launchedExtra, true);

        // Save the values.
        outState.PutParcelable(actualIntentExtra, actualIntent);
        outState.PutString(guidExtra, guid);
        outState.PutInt(requestCodeExtra, requestCode);

        base.OnSaveInstanceState(outState);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        // We have a valid GUID, so handle the task.
        if (GetIntermediateTask(guid, true) is IntermediateTask task)
        {
            if (resultCode == Result.Canceled)
            {
                task.TaskCompletionSource.TrySetCanceled();
            }
            else
            {
                try
                {
                    data ??= new Intent();

                    task.OnResult?.Invoke(data);

                    task.TaskCompletionSource.TrySetResult(data);
                }
                catch (Exception ex)
                {
                    task.TaskCompletionSource.TrySetException(ex);
                }
            }
        }

        // Close the intermediate activity.
        Finish();
    }

    public static Task<Intent> StartAsync(Intent intent, int requestCode, Action<Intent>? onCreate = null, Action<Intent>? onResult = null)
    {
        // Make sure we have the activity.
        Activity? activity = ActivityStateManager.Default.GetCurrentActivity()!;

        // Create a new task.
        IntermediateTask data = new(onCreate, onResult);

        pendingTasks[data.Id] = data;

        // Create the intermediate intent, and add the real intent to it.
        Intent intermediateIntent = new(activity, typeof(CustomIntermediateActivity));

        intermediateIntent.PutExtra(actualIntentExtra, intent);
        intermediateIntent.PutExtra(guidExtra, data.Id);
        intermediateIntent.PutExtra(requestCodeExtra, requestCode);

        // Start the intermediate activity.
        activity.StartActivityForResult(intermediateIntent, requestCode);

        return data.TaskCompletionSource.Task;
    }

    static IntermediateTask? GetIntermediateTask(string? guid, bool remove = false)
    {
        if (string.IsNullOrEmpty(guid))
        {
            return null;
        }

        if (remove)
        {
            pendingTasks.TryRemove(guid, out var removedTask);

            return removedTask;
        }

        pendingTasks.TryGetValue(guid, out var task);

        return task;
    }

    class IntermediateTask(Action<Intent>? onCreate, Action<Intent>? onResult)
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public TaskCompletionSource<Intent> TaskCompletionSource { get; } = new TaskCompletionSource<Intent>();

        public Action<Intent>? OnCreate { get; } = onCreate;

        public Action<Intent>? OnResult { get; } = onResult;
    }
}