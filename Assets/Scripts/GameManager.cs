using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Observer;
using DG.Tweening;
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public static GameManager instance;
    public Rigidbody player;
    public float time_Booster;
    public float heigh_Jump;
    public float speed_Booster;
    public float speed_Player;
    public float sensity_Move;
    public int totalCoinInLevel;
    public List<DataEnemys> dataEnemies;
    public List<DataUpgrades> dataUpgrades;
    private void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        instance = this;
    }
    public void GameStart()
    {
        player.isKinematic = false;
        PlayerController.Instance.StartGame();
        UIController.Instance.GameStart();
        this.PostEvent(EventID.StartAI);
        this.PostEvent(EventID.Spawn);
    }
    public void GameOver()
    {
        UIController.Instance.slidePowerUp.SetActive(false);
        UIController.Instance.UiRuntime.SetActive(false);
        Invoke("Delay", 3f);
        ManagerEffect.Instance.OffTrial();
    }
    void Delay()
    {
        UIController.Instance.vongSang.SetActive(true);
        ManagerEffect.Instance.top1Effect.SetActive(false);
        if (ManagerEffect.Instance.top == 1)
        {
            UIController.Instance.ShowWin();
        }
        else
        {
            UIController.Instance.ShowLose(ManagerEffect.Instance.top);
        }
        // load truoc quang cao
        IronSource.Agent.loadInterstitial();
    }
    public void EatCoin(int value)
    {
        PlayerprefSave.Coin += value;
        totalCoinInLevel += value;
        UIController.Instance.ChangeTextCoin(PlayerprefSave.Coin);
    }
    public void UseCoin(int value)
    {
        if (value >= PlayerprefSave.Coin)
        {
            PlayerprefSave.Coin -= value;
            UIController.Instance.ChangeTextCoin(PlayerprefSave.Coin);
        }
        else
        {
            Debug.Log("Khong du coin");
        }
    }
    public void GetX5Coin()
    {
        totalCoinInLevel *= 5;
        PlayerprefSave.Coin += totalCoinInLevel;
    }
    public void AddDataEnemy(int level, float ai1, float ai2, float ai3, float ai4, float boss)
    {
        DataEnemys dataEnemys = new DataEnemys();
        dataEnemys.Level = level;
        dataEnemys.Enemy1 = ai1;
        dataEnemys.Enemy2 = ai2;
        dataEnemys.Enemy3 = ai3;
        dataEnemys.Enemy4 = ai4;
        dataEnemys.Boss = boss;
        dataEnemies.Add(dataEnemys);
    }
    public void AddDataUpgrade(int level, float speed_value, int cost_Upgrade_Speed, float booster_Value, int cost_Upgrade_Booster)
    {
        DataUpgrades dataUpgrade = new DataUpgrades();
        dataUpgrade.Level_ID = level;
        dataUpgrade.Speed_Value = speed_value;
        dataUpgrade.Cost_Upgrade_Speed = cost_Upgrade_Speed;
        dataUpgrade.Booster_Value = booster_Value;
        dataUpgrade.Cost_Upgrade_Booster = cost_Upgrade_Booster;
        dataUpgrades.Add(dataUpgrade);
    }
    public static string QuyDoi(int i)
    {
        switch (i)
        {
            case 1:
                return "1ST";
            case 2:
                return "2ND";
            case 3:
                return "3RD";
            case 4:
                return "4TH";
            case 5:
                return "5TH";
            case 6:
                return "6TH";
            case 7:
                return "7TH";
            case 8:
                return "8TH";
            case 9:
                return "9TH";
            case 10:
                return "10TH";
            default:
                return "none";
        }
    }
}
[System.Serializable]
public class DataEnemys
{
    public int Level;
    public float Enemy1;
    public float Enemy2;
    public float Enemy3;
    public float Enemy4;
    public float Boss;
}
[System.Serializable]
public class DataUpgrades
{
    public int Level_ID;
    public float Speed_Value;
    public int Cost_Upgrade_Speed;
    public float Booster_Value;
    public int Cost_Upgrade_Booster;
}
