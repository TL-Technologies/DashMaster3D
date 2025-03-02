using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RemoteFirebase : MonoBehaviour
{
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    public string defaultDataEnemy;
    public string defaultDataUpgrade;
    protected virtual void Start()
    {
        DelayGetData();
        //Invoke("DelayRemote", .6f);
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
        defaults.Add("Remote_Speed", 1.6f);
        defaults.Add("Remote_Speed_Booster", 2f);
        defaults.Add("Remote_Heigh_Jump", 4f);
        defaults.Add("Remote_Time_Booster", 3f);
        defaults.Add("Remote_Sensity_MoveX", 1.8f);
        defaults.Add("Data_Enemy", defaultDataEnemy);
        defaults.Add("Data_Upgrade", defaultDataUpgrade);
        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        // [END set_defaults]
        DebugLog("RemoteConfig configured and ready!");
        FetchDataAsync();
        Invoke("DelayGetData", .5f);
    }
    void DelayGetData()
    {
        //Debug.Log("Remote_Speed:::::" + (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Speed").DoubleValue);
        //Debug.Log("Remote_Speed_Booster:::::" + (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Speed_Booster").DoubleValue);
        //Debug.Log("Remote_Heigh_Jump:::::" + (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Heigh_Jump").DoubleValue);
        //Debug.Log("Remote_Time_Booster:::::" + (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Time_Booster").DoubleValue);
        //Debug.Log("Remote_Sensity_MoveX:::::" + (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Sensity_MoveX").DoubleValue);
        //Debug.Log("Data_Enemy:::::" + Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Data_Enemy").StringValue);
        //Debug.Log("Data_Upgrade:::::" + Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Data_Upgrade").StringValue);

        GameManager.instance.speed_Player =/* (float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Speed").DoubleValue*/1.6f;
        GameManager.instance.speed_Booster = /*(float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Speed_Booster").DoubleValue*/2f;
        GameManager.instance.heigh_Jump = /*(float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Heigh_Jump").DoubleValue*/4f;
        GameManager.instance.time_Booster = /*(float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Time_Booster").DoubleValue*/3f;
        GameManager.instance.sensity_Move = /*(float)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Remote_Sensity_MoveX").DoubleValue*/ 1.8f;
        string configData = /*Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Data_Enemy").StringValue*/defaultDataEnemy;
        string configDataUpgrade = /*Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("Data_Upgrade").StringValue*/defaultDataUpgrade;
        //get data enemy
        SetDataEnemy testConfig = JsonUtility.FromJson<SetDataEnemy>(configData);
        for (int i = 0; i < testConfig.GetDataEnemy.Count; i++)
        {
            GameManager.instance.AddDataEnemy(testConfig.GetDataEnemy[i].Level,
                testConfig.GetDataEnemy[i].Enemy1, testConfig.GetDataEnemy[i].Enemy2, testConfig.GetDataEnemy[i].Enemy3,
                testConfig.GetDataEnemy[i].Enemy4, testConfig.GetDataEnemy[i].Boss);
        }
        //get data Upgrade
        SetDataUpgarde ConfigUpgrade = JsonUtility.FromJson<SetDataUpgarde>(configDataUpgrade);
        for (int i = 0; i < ConfigUpgrade.GetDataUpgrade.Count; i++)
        {
            GameManager.instance.AddDataUpgrade(ConfigUpgrade.GetDataUpgrade[i].Level_ID,
                ConfigUpgrade.GetDataUpgrade[i].Speed_Value, ConfigUpgrade.GetDataUpgrade[i].Cost_Upgrade_Speed, ConfigUpgrade.GetDataUpgrade[i].Booster_value,
                ConfigUpgrade.GetDataUpgrade[i].Cost_Upgrade_Booster);
        }
        PlayerController.Instance.SetValuePlayer(true);
        UIController.Instance.DelayStart();
        UIController.Instance.CheckUpgrade();
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
[System.Serializable]
public class SetDataEnemy
{
    //public string st;
    public List<DataEnemy> DataEnemy;
    public List<DataEnemy> GetDataEnemy { get { return DataEnemy; } }
}
[System.Serializable]
public class DataEnemy
{
    public int Level;
    public float Enemy1;
    public float Enemy2;
    public float Enemy3;
    public float Enemy4;
    public float Boss;
}
[System.Serializable]
public class SetDataUpgarde
{
    public List<DataUpgrade> DataUpgrade;
    public List<DataUpgrade> GetDataUpgrade { get { return DataUpgrade; } }
}
[System.Serializable]
public class DataUpgrade
{
    public int Level_ID;
    public float Speed_Value;
    public int Cost_Upgrade_Speed;
    public float Booster_value;
    public int Cost_Upgrade_Booster;
}

