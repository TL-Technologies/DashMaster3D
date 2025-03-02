using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerprefSave
{
    public static int Coin
    {
        set { PlayerPrefs.SetInt("coin", value); }
        get { return PlayerPrefs.GetInt("coin"); }
    }
    public static int CurrentCostume
    {
        set { PlayerPrefs.SetInt("costumecurrent", value); }
        get { return PlayerPrefs.GetInt("costumecurrent"); }
    }
    /// <summary>
    /// set và get id map
    /// </summary>
    public static int SetMap
    {
        set { PlayerPrefs.SetInt("mapcurrent", value); }
        get { return PlayerPrefs.GetInt("mapcurrent"); }
    }
    /// <summary>
    /// level speed
    /// </summary>
    public static int levelSpeed
    {
        set { PlayerPrefs.SetInt("levelSpeed", value); }
        get { return PlayerPrefs.GetInt("levelSpeed"); }
    }
    /// <summary>
    /// value sau khi update speed
    /// </summary>
    public static float speedUpgrade
    {
        set { PlayerPrefs.SetFloat("indexUpgradeSpeed", value); }
        get { return PlayerPrefs.GetFloat("indexUpgradeSpeed"); }
    }
    /// <summary>
    /// level booster
    /// </summary>
    public static int levelBooster
    {
        set { PlayerPrefs.SetInt("levelBooster", value); }
        get { return PlayerPrefs.GetInt("levelBooster"); }
    }
    /// <summary>
    /// value sau khi update booster
    /// </summary>
    public static float boosterUpgrade
    {
        set { PlayerPrefs.SetFloat("indexUpgradeBooster", value); }
        get { return PlayerPrefs.GetFloat("indexUpgradeBooster"); }
    }
    public static int IdMap()
    {
        if (!PlayerPrefs.HasKey("mapcurrent"))
        {
            PlayerPrefs.SetInt("mapcurrent", 0);
        }
        return PlayerPrefs.GetInt("mapcurrent");
    }
    public static int CoinUpgradeSpeed()
    {
        if (!PlayerPrefs.HasKey("CoinUpgradeSpeed"))
        {
            PlayerPrefs.SetInt("CoinUpgradeSpeed", 10);
        }
        return PlayerPrefs.GetInt("CoinUpgradeSpeed");
    }
    
    public static int CoinUpgradeBooster()
    {
        if (!PlayerPrefs.HasKey("CoinUpgradeBooster"))
        {
            PlayerPrefs.SetInt("CoinUpgradeBooster", 10);
        }
        return PlayerPrefs.GetInt("CoinUpgradeBooster");
    }
    
    public static void UnlockCostume(int id)
    {
        PlayerPrefs.SetInt("customUnlock" + id, 1);
        CurrentCostume = id;
    }
    public static bool CheckUnlockCostume(int id)
    {
        if (!PlayerPrefs.HasKey("customUnlock" + id))
            return false;
        else
            return true;
    }
    public static void FirstOpenGame()
    {
        if (!PlayerPrefs.HasKey("firstOpen"))
        {
            UnlockCostume(0);
            PlayerPrefs.SetInt("firstOpen", 1);
            PlayerPrefs.SetInt("daydaily", 0);
        }
    }
    public static TypeRewardVideo TypeRewardVideo;
    public static void SelectTypeVideo(TypeRewardVideo typeRewardVideo)
    {
        TypeRewardVideo = typeRewardVideo;
    }

    //key open reward
    public static int keyReward
    {
        set { PlayerPrefs.SetInt("keyReward", value); }
        get { return PlayerPrefs.GetInt("keyReward"); }
    }
    public static float fillKeyReward
    {
        set { PlayerPrefs.SetFloat("fillKeyReward", value); }
        get { return PlayerPrefs.GetFloat("fillKeyReward"); }
    }
    public static int keyOpenRewards;
    public static float rdAnimBox = 0f;
    public static int idTemp;
    //sound
    public static float SoundValue
    {
        set { PlayerPrefs.SetFloat("soundValue", value); }
        get { return PlayerPrefs.GetFloat("soundValue"); }
    }
    public static float MusicValue
    {
        set { PlayerPrefs.SetFloat("musicValue", value); }
        get { return PlayerPrefs.GetFloat("musicValue"); }
    }


    //daily reward
   
    public static int DayDaily
    {
        set { PlayerPrefs.SetInt("daydaily", value); }
        get { return PlayerPrefs.GetInt("daydaily"); }
    }
    public static int DayOfYear
    {
        set { PlayerPrefs.SetInt("dayofyear", value); }
        get { return PlayerPrefs.GetInt("dayofyear"); }
    }
    public static void ChangeDayRecievedGiftDaily()
    {
        DayOfYear = System.DateTime.Now.DayOfYear;
        DayDaily++;
        if (DayDaily == 5)
            DayDaily = 0;
    }
}
