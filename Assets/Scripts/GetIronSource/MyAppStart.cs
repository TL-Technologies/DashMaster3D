﻿using UnityEngine;
using System.Collections;

public class MyAppStart : MonoBehaviour
{
    public static string uniqueUserId = "demoUserUnity";
    [SerializeField] private string appKeyAndroid;
    [SerializeField] private string appKeyIos;

    // Use this for initialization
    void Start()
    {
        Debug.Log("unity-script: MyAppStart Start called");

        //Dynamic config example
        IronSourceConfig.Instance.setClientSideCallbacks(true);

        string id = IronSource.Agent.getAdvertiserId();
        Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // Add Banner Events
        // IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        // IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;		
        // IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent; 
        // IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent; 
        // IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        // IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
#if UNITY_ANDROID
        IronSource.Agent.init(appKeyAndroid);
#elif UNITY_IOS
         IronSource.Agent.init(appKeyIos);
#endif

        //IronSource.Agent.init (appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);
        //IronSource.Agent.initISDemandOnly (appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);

        //Set User ID For Server To Server Integration
        //// IronSource.Agent.setUserId ("UserId");

        // Load Banner example
        //IronSource.Agent.loadBanner (IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }
    public void SetKeyIos(string keyIos)
    {
        appKeyIos = keyIos;
        IronSource.Agent.init(appKeyIos);
    }

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    //Banner Events
    //void BannerAdLoadedEvent ()
    //{
    //	Debug.Log ("unity-script: I got BannerAdLoadedEvent");
    //}

    //void BannerAdLoadFailedEvent (IronSourceError error)
    //{
    //	Debug.Log ("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode () + ", description : " + error.getDescription ());
    //}

    //void BannerAdClickedEvent ()
    //{
    //	Debug.Log ("unity-script: I got BannerAdClickedEvent");
    //}

    //void BannerAdScreenPresentedEvent ()
    //{
    //	Debug.Log ("unity-script: I got BannerAdScreenPresentedEvent");
    //}

    //void BannerAdScreenDismissedEvent ()
    //{
    //	Debug.Log ("unity-script: I got BannerAdScreenDismissedEvent");
    //}

    //void BannerAdLeftApplicationEvent ()
    //{
    //	Debug.Log ("unity-script: I got BannerAdLeftApplicationEvent");
    //}
}
