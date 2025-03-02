using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// Nhận key: save key, được 3 key -> show bestreward
/// 3key: out -> chơi lại-> win show bestreward
/// đã mở bestreward thì set save key về 0, không mở hết, out ra ngoài bị mất key
/// xem video show bestreward, savekey = 0
/// </summary>
[System.Serializable]
public class ListIconUnlock
{
    public int id;
    public Sprite icon;
}
public class ControlBestReward : MonoBehaviour
{
    public static ControlBestReward Instance;
    public DataCharacter dataCharacter;
    [SerializeField] Text txtCoin;
    [SerializeField] GameObject panelLoad;

    [Header("Object bestReward Top")]
    [SerializeField] GameObject obj3d;
    [SerializeField] GameObject objCoin;
    private int indexIconReward;
    public int idCostume;
    [Header("Item reward")]
    [SerializeField] List<GameObject> itemReward;
    [SerializeField] List<int> listCoin;
    [SerializeField] List<ListIconUnlock> listIconUnlocks;
    //set key
    [SerializeField] List<GameObject> keyRewards;
    [SerializeField] GameObject showKey, btnVideoKey, btnClose, btnBack, effectTop;
    //count view video
    int countViewVideo;
    int specialRewardCoin = 250;
    private void Start()
    {
        Instance = this;
        countViewVideo = 0;
        PlayerprefSave.keyOpenRewards = 3;
        for (int i = 0; i < dataCharacter.characters.Length; i++)
        {
            if (PlayerprefSave.IdMap() + 1 >= dataCharacter.characters[i].levelUnlock && !PlayerprefSave.CheckUnlockCostume(i))
            {
                ListIconUnlock listIconUnlock = new ListIconUnlock();
                listIconUnlock.id = i;
                listIconUnlock.icon = dataCharacter.characters[i].spIcon;
                listIconUnlocks.Add(listIconUnlock);
            }
        }
        updateCoin();

        showUIkey(true);

        //set info item reward
        int rd = Random.Range(0, itemReward.Count);
        for (int i = 0; i < itemReward.Count; i++)
        {
            if (rd == i) //set reward icon
            {
                if (listIconUnlocks.Count != 0) 
                {
                    // giai thuong la skin
                    indexIconReward = Random.Range(0, listIconUnlocks.Count);
                    idCostume = listIconUnlocks[indexIconReward].id;
                    itemReward[i].GetComponent<ItemReward>().setInfoItemReward(false, 0, listIconUnlocks[indexIconReward].icon);
                    obj3d.SetActive(true);
                    objCoin.SetActive(false);
                    ChangeCostumePlayer.Instance.ChangeCostume(idCostume);
                }
                else
                {
                    // het skin cho tien
                    obj3d.SetActive(false);
                    objCoin.SetActive(true);
                    itemReward[i].GetComponent<ItemReward>().setInfoItemReward(true, specialRewardCoin);
                }
               
            }
            else
            {
                int indexCoin = Random.Range(0, listCoin.Count);
                itemReward[i].GetComponent<ItemReward>().setInfoItemReward(true, listCoin[indexCoin]);
                listCoin.RemoveAt(indexCoin);
            }
        }
    }

    public void btnViewVideoKeyClick()
    {
#if UNITY_EDITOR
        OnCompliteVideo();
#elif !UNITY_EDITOR
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            PlayerprefSave.SelectTypeVideo(TypeRewardVideo.rewardKey);
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("video reward not available");
            ShowThongBao();
        }
#endif
    }

    public void btnCloseClick()
    {
        btnReturnGameplay();
    }

    #region back home
    public void btnReturnGameplay()
    {
        effectTop.SetActive(false);
        panelLoad.SetActive(true);
        StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
        yield return new WaitForSeconds(.1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    #endregion

    #region function
    public void setOpenByKey()
    {
        PlayerprefSave.keyOpenRewards--;
        for (int i = 0; i < keyRewards.Count; i++)
        {
            if (i < PlayerprefSave.keyOpenRewards)
            {
                keyRewards[i].GetComponent<Animator>().SetBool("hiden", false);
            }
            else
            {
                keyRewards[i].GetComponent<Animator>().SetBool("hiden", true);
            }
        }
        Debug.Log("PlayerprefSave.keyOpenRewards = " + PlayerprefSave.keyOpenRewards);
        if (PlayerprefSave.keyOpenRewards == 0)
        {
            showUIkey(false);
        }
    }
    public void showButton()
    {
        if (PlayerprefSave.keyOpenRewards <= 0)
        {
            if (countViewVideo < 2)
            {
                btnVideoKey.SetActive(true);
                btnBack.SetActive(true);
            }
            else
            {
                btnClose.SetActive(true);
                btnBack.SetActive(false);
            }
        }
    }
    public void updateCoin()
    {
        txtCoin.text = PlayerprefSave.Coin + "";
    }

    void setOnclickItemReward(bool check)
    {
        for (int i = 0; i < itemReward.Count; i++)
        {
            itemReward[i].GetComponent<ItemReward>().setOnclickButton(check);
        }
    }
    void showUIkey(bool check)
    {
        btnVideoKey.SetActive(false);
        btnBack.SetActive(false);
        btnClose.SetActive(false);

        StartCoroutine(delayShowKey(check));
        setOnclickItemReward(check);


        //for (int i = 0; i < keyRewards.Count; i++)
        //{
        //    keyRewards[i].GetComponent<Animator>().SetBool("hiden", false);
        //}
    }
    IEnumerator delayShowKey(bool check)
    {
        yield return new WaitForSeconds(check ? 0 : .7f);
        showKey.SetActive(check);
        if (!check)
        {
            showButton();
        }
    }
    #endregion

    #region ads
    public GameObject TextNotification;
    Coroutine notification;
    IEnumerator delayNotification()
    {
        TextNotification.SetActive(true);
        yield return new WaitForSeconds(1f);
        TextNotification.SetActive(false);
    }
    public void ShowThongBao()
    {
        if (notification != null)
            StopCoroutine(notification);

        TextNotification.SetActive(false);
        notification = StartCoroutine(delayNotification());
    }

    public void OnCompliteVideo()
    {
        PlayerprefSave.keyOpenRewards = 3;
        countViewVideo++;
        showUIkey(true);
    }

    void OnFailVideo()
    {

    }
    #endregion
}



