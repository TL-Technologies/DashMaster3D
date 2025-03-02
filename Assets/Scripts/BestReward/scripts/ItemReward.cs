using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemReward : MonoBehaviour
{
    [SerializeField] bool _isOpen;
    [SerializeField] bool isRewardCoin;
    [SerializeField] int coin;
    [SerializeField] GameObject rewardCoin;
    [SerializeField] GameObject object3d;
    [SerializeField] Transform box;
    [SerializeField] GameObject effBoxOff;
    [SerializeField] GameObject effBoxOpen;
    [SerializeField] GameObject effBox;
    [SerializeField] Image imgIcon;
    [SerializeField] Text txtCoin;

    float timedelayAnim = 0;
    Coroutine coroutineAnim = null;
    private void OnEnable()
    {
        timedelayAnim = Random.Range(1f, 5f);
        while (PlayerprefSave.rdAnimBox == timedelayAnim)
        {
            timedelayAnim = Random.Range(1f, 5f);
        }
        PlayerprefSave.rdAnimBox = timedelayAnim;

        if (coroutineAnim != null)
            StopCoroutine(coroutineAnim);
        coroutineAnim = StartCoroutine(delayAnimBox(timedelayAnim));
    }
    public void OnItemClick()
    {
        if (!_isOpen)
        {
            //set default
            PlayerprefSave.keyReward = 0;
            PlayerprefSave.fillKeyReward = 0;

            effBoxOff.SetActive(false);
            effBoxOpen.SetActive(true);
            box.GetComponent<RectTransform>().localPosition = new Vector3(0,-80,-450);
            StartCoroutine(delayEffectBox());
            ControlBestReward.Instance.setOpenByKey();

            _isOpen = true;
        }
    }


    IEnumerator delayEffectBox()
    {
        yield return new WaitForSeconds(.5f);
        effBox.SetActive(true);
        yield return new WaitForSeconds(.7f);
        effBoxOpen.SetActive(false);

        //ControlBestReward.Instance.showButton();

        if (isRewardCoin)
        {
            rewardCoin.SetActive(true);
            object3d.SetActive(false);
            PlayerprefSave.Coin += coin;
            ControlBestReward.Instance.updateCoin();
        }
        else
        {
            rewardCoin.SetActive(false);
            object3d.SetActive(true);
            PlayerprefSave.UnlockCostume(ControlBestReward.Instance.idCostume);
        }
    }

    IEnumerator delayAnimBox(float timedelay)
    {
        effBoxOff.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(timedelay);
        effBoxOff.GetComponent<Animator>().enabled = true;
    }
    public void setInfoItemReward(bool _isRewardCoin, int coin, Sprite spIcon = null)
    {
        _isOpen = false;
        isRewardCoin = false;
        this.coin = coin;
        effBoxOff.SetActive(true);
        effBoxOpen.SetActive(false);
        effBox.SetActive(false);
        box.GetComponent<RectTransform>().localPosition = new Vector3(0, -80, -150);
        //Debug.Log("setInfoItemReward " + _isRewardCoin);
        this.isRewardCoin = _isRewardCoin;

        if (!_isRewardCoin)
        {
            if (spIcon != null)
                imgIcon.sprite = spIcon;
        }
        else
        {
            txtCoin.text = coin + "";
        }
    }
    public void setOnclickButton(bool check)
    {
        GetComponent<Button>().enabled = check;
    }
}
