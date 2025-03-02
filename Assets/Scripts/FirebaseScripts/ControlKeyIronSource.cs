using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ControlKeyIronSource : MonoBehaviour
{
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    public string defaultKeyIronsource = "b85a0d95";
    protected virtual void Start()
    {
#if UNITY_IOS
        Invoke("DelayRemote", .6f);
#endif
    }
    void DelayRemote()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    // Initialize remote config, and set the default values.
    void InitializeFirebase()
    {
        // [START set_defaults]
        System.Collections.Generic.Dictionary<string, object> defaults =
          new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("Remote_Key_Ironsource", defaultKeyIronsource);
        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        // [END set_defaults]
        DebugLog("RemoteConfig configured and ready!");
        FetchDataAsync();
        Invoke("DelayGetData", .5f);
    }
    void DelayGetData()
    {
        string keyIos = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Key_Ironsource").StringValue;
        Debug.Log("keyIos: " + keyIos);
        gameObject.GetComponent<MyAppStart>().SetKeyIos(keyIos);

    }
    // Display the currently loaded data.  If fetch has been called, this will be
    // the data fetched from the server.  Otherwise, it will be the defaults.
    // Note:  Firebase will cache this between sessions, so even if you haven't
    // called fetch yet, if it was called on a previous run of the program, you
    //  will still have data from the last time it was run.


    public void DisplayAllKeys()
    {
        DebugLog("Current Keys:");
        System.Collections.Generic.IEnumerable<string> keys =
            Firebase.RemoteConfig.FirebaseRemoteConfig.Keys;
        foreach (string key in keys)
        {
            DebugLog("    " + key);
        }
        DebugLog("GetKeysByPrefix(\"config_test_s\"):");
        keys = Firebase.RemoteConfig.FirebaseRemoteConfig.GetKeysByPrefix("config_test_s");
        foreach (string key in keys)
        {
            DebugLog("    " + key);
        }
    }

    // [START fetch_async]
    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    public Task FetchDataAsync()
    {
        DebugLog("Fetching data...");
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    //[END fetch_async]

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            DebugLog("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            DebugLog("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            DebugLog("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                DebugLog(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                       info.FetchTime));
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        DebugLog("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        DebugLog("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                DebugLog("Latest Fetch call still pending.");
                break;
        }
    }



    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        print(s);

    }
}
