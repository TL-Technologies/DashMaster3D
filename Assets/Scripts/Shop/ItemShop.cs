using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Observer;
public class ItemShop : MonoBehaviour
{
    [Header("in scenes")]
    public Text txtLevel;
    public Image imgIconLock;
    public Image imgIcon1;
    public Image imgIcon2;
    public GameObject objIconLock;
    public GameObject objIconDefault;
    public GameObject objIconChoose;
    public GameObject objButtonVideo;
    [Header("Pass parameters")]
    public int idItem;
    public int levelUnlock;
    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CheckChooseCostume, (param)=>CheckChoose());
        EventDispatcher.Instance.RegisterListener(EventID.CheckUnlockCostume, (param)=>CheckUnlock());
    }

    public void SetInfoItem(int id, int level,Sprite iconlock,Sprite icon)
    {
        idItem = id;
        levelUnlock = level;
        imgIconLock.sprite = iconlock;
        imgIcon1.sprite = icon;
        imgIcon2.sprite = icon;
        txtLevel.text = "Level " + levelUnlock;
    }
    public void CheckLevelUnlock()
    {
        //dang lock
        if (levelUnlock > PlayerprefSave.IdMap() + 1)
        {
            objIconLock.SetActive(true);
            objIconDefault.SetActive(false);
        }
        else
        {
            objIconLock.SetActive(false);
            objIconDefault.SetActive(true);
        }
    }
    public void CheckUnlock()
    {
        objIconDefault.GetComponent<Button>().onClick.RemoveAllListeners();
        if (PlayerprefSave.CheckUnlockCostume(idItem))
        {
            objButtonVideo.SetActive(false);
            objIconDefault.GetComponent<Button>().onClick.AddListener(() => ButtonSelect());
        }
        else
        {
            objButtonVideo.SetActive(true);
            objIconDefault.GetComponent<Button>().onClick.AddListener(() => ButtonVideo());
        }
    }
    public void CheckChoose()
    {
        if (idItem == PlayerprefSave.CurrentCostume)
        {
            objIconChoose.SetActive(true);
            objIconDefault.SetActive(false);
            ChangeCostumePlayer.Instance.ChangeCostume(idItem);
            ControlShop.Instance.showName(idItem);
        }
        else
        {
            objIconChoose.SetActive(false);
            objIconDefault.SetActive(true);
        }
    }
    public void UnlockThisCostume()
    {
        PlayerprefSave.UnlockCostume(idItem);
        this.PostEvent(EventID.CheckUnlockCostume);
        this.PostEvent(EventID.CheckChooseCostume);
    }
    public void ButtonSelect()
    {
        PlayerprefSave.CurrentCostume = idItem;
        this.PostEvent(EventID.CheckChooseCostume);
    }
    public void ButtonVideo()
    {
        ControlShop.Instance.objWatchVideo = gameObject;
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            PlayerprefSave.SelectTypeVideo(TypeRewardVideo.costume);
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("video reward not available");
            ControlShop.Instance.ShowThongBao();
        }
    }
   
}
