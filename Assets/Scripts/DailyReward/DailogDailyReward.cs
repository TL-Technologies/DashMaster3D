using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DailogDailyReward : MonoBehaviour
{
    public GameObject dialog;
    public GameObject[] arrayGift;
    public List<DataRewarddaily> dataDaily;
    int dayHaveGift;
    void Start()
    {
        dayHaveGift = PlayerprefSave.DayDaily;
        //set Data for day
        for (int i = 0; i < arrayGift.Length; i++)
        {
            arrayGift[i].GetComponent<ItemDailyReward>().isDay = i;
            arrayGift[i].GetComponent<ItemDailyReward>().txtDay.text = "Day " + (i + 1);
            arrayGift[i].GetComponent<ItemDailyReward>().icon.sprite = dataDaily[i].iconReward;
            arrayGift[i].GetComponent<ItemDailyReward>().icon.SetNativeSize();
            if (dataDaily[i].typeReward == 0)
            {
                //coin
                arrayGift[i].GetComponent<ItemDailyReward>().txtDescription.text = dataDaily[i].amountCoin.ToString();
                arrayGift[i].GetComponent<ItemDailyReward>().iconCharacter.SetActive(false);
                arrayGift[i].GetComponent<ItemDailyReward>().icon.gameObject.SetActive(true);
            }
            else
            {
                //nhan vat
                if (PlayerprefSave.CheckUnlockCostume(dataDaily[i].idCostume))
                {
                    arrayGift[i].GetComponent<ItemDailyReward>().txtDescription.text = dataDaily[i].amountCoin.ToString();
                    arrayGift[i].GetComponent<ItemDailyReward>().iconCharacter.SetActive(false);
                    arrayGift[i].GetComponent<ItemDailyReward>().icon.gameObject.SetActive(true);
                }
                else
                {
                    arrayGift[i].GetComponent<ItemDailyReward>().txtDescription.text = "Yasuo";
                    arrayGift[i].GetComponent<ItemDailyReward>().iconCharacter.SetActive(true);
                    arrayGift[i].GetComponent<ItemDailyReward>().icon.gameObject.SetActive(false);
                }
                
            }
            if (i < dayHaveGift)
            {
                arrayGift[i].GetComponent<ItemDailyReward>().iconReceived.SetActive(true);
            }
            else if (i == dayHaveGift)
                arrayGift[i].GetComponent<ItemDailyReward>().objToday.SetActive(true);
        }
        if (System.DateTime.Now.DayOfYear != PlayerprefSave.DayOfYear)
        {
            //hien dialog daily
            dialog.SetActive(true);
        }
        else
        {
            Debug.Log("da nhan qua hom nay");
            //da nhan qua roi
        }
    }
    bool checkClose;
    public void ReceivedGift()
    {
        if (!checkClose)
        {
            PlayerprefSave.ChangeDayRecievedGiftDaily();
            if (dataDaily[dayHaveGift].typeReward == 0)
            {
                //tang vang
                PlayerprefSave.Coin += dataDaily[dayHaveGift].amountCoin;
                UIController.Instance.ChangeTextCoin(PlayerprefSave.Coin);
            }
            else
            {
                if (PlayerprefSave.CheckUnlockCostume(dataDaily[dayHaveGift].idCostume))
                {
                    //tang vang
                    PlayerprefSave.Coin += dataDaily[dayHaveGift].amountCoin;
                    UIController.Instance.ChangeTextCoin(PlayerprefSave.Coin);
                }
                else
                {
                    PlayerprefSave.UnlockCostume(dataDaily[dayHaveGift].idCostume);
                    ChangeCostumePlayer.Instance.ChangeCostume(dataDaily[dayHaveGift].idCostume);
                }
                //Nhan trang phuc
            }
            UIController.Instance.CheckUpgrade();
            arrayGift[dayHaveGift].GetComponent<ItemDailyReward>().effect.SetActive(true);
            Invoke("CloseDialog", .7f);
            checkClose = true;
        }
    }
    public void CloseDialog()
    {
        dialog.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 0), .3f);
        dialog.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        dialog.transform.GetChild(0).DOScale(Vector3.zero, .3f).OnComplete(()=> {
            dialog.SetActive(false);
        });
    }
}
[System.Serializable]
public class DataRewarddaily
{
    public int Day;
    public int amountCoin;
    public Sprite iconReward;
    [Tooltip("0 laf coin, 1 laf nhan vat")]
    public int typeReward;
    public int idCostume;
}
