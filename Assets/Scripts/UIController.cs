using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DynamicFogAndMist;
using DG.Tweening;

public class UIController : MonoBehaviourSingleton<UIController>
{
    public GameObject panelTutorial;
    public GameObject UiRuntime;
    public GameObject dialogUgrade;
    public GameObject panelWin;
    public GameObject txtWin, txtlose;
    public Image panelTrailer;
    public GameObject slidePowerUp;
    public GameObject vongSang;
    public Image fillPower;
    public Image fillTienTrinh;
    public Button btnPowerUp;
    public Button btnUpgradeSpeed;
    public Button btnUpgradeBooster;
    public Button btnClaim;
    public Button btnVideoX5;
    public Button btnTapContinue;
    public Button btnBestReward;
    public Text txtCoin;
    public Text txtCoinEndGame;
    public Text txtLevel;
    public Text txtLevelMin;
    public Text txtCoinUpgradeSpeed;
    public Text txtCoinUpgradeBooster;
    public Text txtThutu;
    public Text txtThuHang;
    public Text txtSpeed;
    private bool upgradeSpeedbyVideo;
    private bool upgradeBoosterbyVideo;
    public GameObject panelLoading;
    public Transform Player;
    private float totalDistance;
    private float startPosition;
    private bool checkTienTrinh;
    public GameObject canvasMain;
    public GameObject canvasNewSkin;
    [Header("PowerUp")]
    [SerializeField] GameObject effPower1;
    [SerializeField] GameObject effPower2;
    [SerializeField] GameObject iconShop;
    private void Start()
    {
        //ChangeMap(PlayerprefSave.IdMap());
        ChangeTextCoin(PlayerprefSave.Coin);
        txtLevel.text = "LEVEL " + (PlayerprefSave.IdMap() + 1).ToString() + "/100";
        txtLevelMin.text = /*"LEVEL " +*/ (PlayerprefSave.IdMap() + 1).ToString();
        SetSaveSound();
        for (int i = 0; i < DataGame.Instance.dataCharacter.characters.Length; i++)
        {
            if (DataGame.Instance.dataCharacter.characters[i].levelUnlock <= PlayerprefSave.IdMap())
            {
                if (!PlayerprefSave.CheckUnlockCostume(i) && DataGame.Instance.dataCharacter.characters[i].typeUnlock == TypeUnlock.rewardVideo)
                {
                    iconShop.SetActive(true);
                }
            }
        }
    }
    public void DelayStart()
    {
        panelTrailer.DOColor(new Color32(0, 0, 0, 0), .5f).SetEase(Ease.Linear).OnComplete(() => { panelTrailer.gameObject.SetActive(false); });
    }
    #region funticon event
    public void showUINewSkin(bool check)
    {
        canvasMain.SetActive(!check);
        canvasNewSkin.SetActive(check);
    }
    public void SetInfoTienTrinh(float posEndMap)
    {
        startPosition = Player.position.z;
        totalDistance = posEndMap - Player.position.z;
        checkTienTrinh = true;
    }
    void CheckTienTrinh()
    {
        fillTienTrinh.fillAmount = (Player.position.z - startPosition) / totalDistance;
    }
    public void SetThuHangNhanVat(int oder)
    {
        txtThuHang.text = GameManager.QuyDoi(oder);
    }
    public void SetSpeedNhanVat(float speed)
    {
        txtSpeed.text = "SPEED " + speed;
    }
    public void GameStart()
    {
        slidePowerUp.SetActive(true);
        dialogUgrade.SetActive(false);
        panelTutorial.SetActive(false);
        UiRuntime.SetActive(true);
    }
    public void ChangeTextCoin(int value)
    {
        txtCoin.text = value.ToString();
    }
    public void CheckUpgrade()
    {
        Debug.Log("GameManager.instance.dataUpgrades"+GameManager.instance.dataUpgrades.Count);
        //check upgrade speed
        if (PlayerprefSave.Coin >= GameManager.instance.dataUpgrades[PlayerprefSave.levelSpeed].Cost_Upgrade_Speed)
        {
            btnUpgradeSpeed.transform.GetChild(1).gameObject.SetActive(true);
            btnUpgradeSpeed.transform.GetChild(2).gameObject.SetActive(false);
            txtCoinUpgradeSpeed.text = GameManager.instance.dataUpgrades[PlayerprefSave.levelSpeed].Cost_Upgrade_Speed.ToString();
            upgradeSpeedbyVideo = false;
        }
        else
        {
            btnUpgradeSpeed.transform.GetChild(1).gameObject.SetActive(false);
            btnUpgradeSpeed.transform.GetChild(2).gameObject.SetActive(true);
            upgradeSpeedbyVideo = true;
        }
        //check upgrade Booster
        if (PlayerprefSave.Coin >= GameManager.instance.dataUpgrades[PlayerprefSave.levelBooster].Cost_Upgrade_Booster)
        {
            btnUpgradeBooster.transform.GetChild(1).gameObject.SetActive(true);
            btnUpgradeBooster.transform.GetChild(2).gameObject.SetActive(false);
            txtCoinUpgradeBooster.text = GameManager.instance.dataUpgrades[PlayerprefSave.levelBooster].Cost_Upgrade_Booster.ToString();
            upgradeBoosterbyVideo = false;
        }
        else
        {
            btnUpgradeBooster.transform.GetChild(1).gameObject.SetActive(false);
            btnUpgradeBooster.transform.GetChild(2).gameObject.SetActive(true);
            upgradeBoosterbyVideo = true;
        }
    }
    public void ShowWin()
    {
        panelWin.SetActive(true);
        txtWin.SetActive(true);
        txtlose.SetActive(false);
        btnPowerUp.transform.localScale = Vector3.zero;
        txtCoinEndGame.text = GameManager.instance.totalCoinInLevel.ToString();
        PlayerprefSave.keyReward++;
        if (PlayerprefSave.keyReward > 3)
        {
            PlayerprefSave.keyReward = 3;
        }
        showKey(PlayerprefSave.keyReward);
        for (int i = 1; i < DataGame.Instance.dataCharacter.characters.Length; i++)
        {
            if (PlayerprefSave.IdMap() == DataGame.Instance.dataCharacter.characters[i].levelUnlock - 1)
            {
                showUINewSkin(true);
                PlayerprefSave.idTemp = i;
            }
        }
        PlayerprefSave.SetMap++;
    }
    [Header("Key open box reward")]
    [SerializeField] GameObject btnOpenScenceBestReward;
    [SerializeField] GameObject[] objKeys;
    [SerializeField] Image imgSlide;
    [SerializeField] GameObject effectBox;
    public float[] targetFill;
    [SerializeField] Sprite spKeyOn, spKeyOff;
    public void showKey(int key)
    {

        Debug.Log("key showKey " + key);
        btnOpenScenceBestReward.GetComponent<Image>().enabled = false;
        btnOpenScenceBestReward.GetComponent<Button>().enabled = false;
        effectBox.SetActive(false);
        //set fill slide
        if (PlayerprefSave.fillKeyReward != 0)
        {
            imgSlide.fillAmount = PlayerprefSave.fillKeyReward;
        }

        imgSlide.DOFillAmount(targetFill[0], .1f);
        for (int i = 0; i < objKeys.Length; i++)
        {
            objKeys[i].GetComponent<Image>().sprite = spKeyOff;
            objKeys[i].GetComponent<Image>().SetNativeSize();
            objKeys[i].transform.GetChild(0).gameObject.SetActive(false);
            if (i < key)
            {
                objKeys[i].GetComponent<Image>().sprite = spKeyOn;
                objKeys[i].transform.GetChild(0).gameObject.SetActive(true);
                //objKeys[i].GetComponent<Image>().SetNativeSize();
                PlayerprefSave.fillKeyReward = targetFill[i];
                imgSlide.DOFillAmount(targetFill[i], .5f);
            }
        }
        if (key == 3)
        {
            imgSlide.DOFillAmount(targetFill[2], .5f).OnComplete(() =>
            {

                imgSlide.DOFillAmount(targetFill[3], .5f).OnComplete(() =>
                {
                    btnOpenScenceBestReward.GetComponent<Image>().enabled = true;
                    btnOpenScenceBestReward.GetComponent<Button>().enabled = true;
                    effectBox.SetActive(true);
                });
            });
            btnVideoX5.gameObject.SetActive(false);
            btnTapContinue.gameObject.SetActive(false);
            btnBestReward.gameObject.SetActive(true);
        }
        //switch (key)
        //{
        //    case 1:
        //        objKeys[0].GetComponent<Image>().sprite = spKeyOn;
        //        objKeys[0].transform.GetChild(0).gameObject.SetActive(true);
        //        objKeys[0].GetComponent<Image>().SetNativeSize();
        //        imgSlide.DOFillAmount(targetFill[0], .1f);
        //        PlayerprefSave.fillKeyReward = targetFill[0];
        //        break;
        //    case 2:
        //        objKeys[1].GetComponent<Image>().sprite = spKeyOn;
        //        objKeys[1].transform.GetChild(0).gameObject.SetActive(true);
        //        objKeys[1].GetComponent<Image>().SetNativeSize();
        //        imgSlide.DOFillAmount(targetFill[1], .5f);
        //        PlayerprefSave.fillKeyReward = targetFill[1];
        //        break;
        //    case 3:
        //        objKeys[2].GetComponent<Image>().sprite = spKeyOn;
        //        objKeys[2].transform.GetChild(0).gameObject.SetActive(true);
        //        objKeys[2].GetComponent<Image>().SetNativeSize();
        //        PlayerprefSave.fillKeyReward = 1;
        //        imgSlide.DOFillAmount(targetFill[2], .5f).OnComplete(() =>
        //        {

        //            imgSlide.DOFillAmount(targetFill[3], .5f).OnComplete(() =>
        //            {
        //                btnOpenScenceBestReward.GetComponent<Image>().enabled = true;
        //                btnOpenScenceBestReward.GetComponent<Button>().enabled = true;
        //                effectBox.SetActive(true);
        //            });
        //        });
        //        btnVideoX5.gameObject.SetActive(false);
        //        btnTapContinue.gameObject.SetActive(false);
        //        btnBestReward.gameObject.SetActive(true);
        //        break;
        //}
    }

    public void ShowLose(int top)
    {
        panelWin.SetActive(true);
        txtlose.SetActive(true);
        txtWin.SetActive(false);
        txtCoinEndGame.text = GameManager.instance.totalCoinInLevel.ToString();
        switch (top)
        {
            case 1:
                txtThutu.text = "1ST";
                break;
            case 2:
                txtThutu.text = "2ND";
                break;
            case 3:
                txtThutu.text = "3RD";
                break;
            case 4:
                txtThutu.text = "4TH";
                break;
            case 5:
                txtThutu.text = "5TH";
                break;
            case 6:
                txtThutu.text = "6TH";
                break;
            case 7:
                txtThutu.text = "7TH";
                break;
            case 8:
                txtThutu.text = "8TH";
                break;
            case 9:
                txtThutu.text = "9TH";
                break;
            case 10:
                txtThutu.text = "10TH";
                break;
        }
        btnPowerUp.transform.localScale = Vector3.zero;
    }
    #endregion


    #region button
    public void BtnOpenBestRewardClick()
    {
        panelLoading.SetActive(true);
        StartCoroutine(LoadYourAsyncScene(3, 0));
    }
    public void ButtonUpgradeSpeed()
    {
        if (upgradeSpeedbyVideo)
        {
            //nang cap bang video
            //neu load duoc video thi nang cap
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                PlayerprefSave.SelectTypeVideo(TypeRewardVideo.speed);
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("video reward not available");
                SoundManager.Instance.ShowNotification(canvasMain.transform);
            }
        }
        else
        {
            //nang cap bang coin
            UpgradeSpeed();
            PlayerprefSave.Coin -= GameManager.instance.dataUpgrades[PlayerprefSave.levelSpeed - 1].Cost_Upgrade_Speed;
            ChangeTextCoin(PlayerprefSave.Coin);
        }
        CheckUpgrade();
        SoundManager.Instance.PlaySoundUpgrade();
    }
    public void UpgradeSpeed()
    {
        PlayerprefSave.levelSpeed += 1;
        Debug.Log(PlayerprefSave.levelSpeed);
        PlayerprefSave.speedUpgrade += GameManager.instance.dataUpgrades[PlayerprefSave.levelSpeed].Speed_Value;
        //PlayerprefSave.UpgradeSpeed(15);
        PlayerController.Instance.SetValuePlayer(false);
        ManagerEffect.Instance.ShowFxUpgrade();
    }
    public void ButtonUpgradeBooster()
    {
        if (upgradeBoosterbyVideo)
        {
            //nang cap bang video
            //neu load duoc video thi nang cap
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                PlayerprefSave.SelectTypeVideo(TypeRewardVideo.booster);
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("video reward not available");
                SoundManager.Instance.ShowNotification(canvasMain.transform);
            }
        }
        else
        {
            //nang cap bang coin
            UpgradeBooster();
            PlayerprefSave.Coin -= GameManager.instance.dataUpgrades[PlayerprefSave.levelBooster - 1].Cost_Upgrade_Booster;
            ChangeTextCoin(PlayerprefSave.Coin);
        }
        CheckUpgrade();
        SoundManager.Instance.PlaySoundUpgrade();
    }
    public void UpgradeBooster()
    {
        PlayerprefSave.levelBooster += 1;
        PlayerprefSave.boosterUpgrade += GameManager.instance.dataUpgrades[PlayerprefSave.levelBooster].Booster_Value;
        //PlayerprefSave.UpgradeBooster(15);
        PlayerController.Instance.SetValuePlayer(false);
        ManagerEffect.Instance.ShowFxUpgrade();
    }
    public void ButtonPowerUp()
    {
        PlayerController.Instance.UsePowerUp();
        btnPowerUp.transform.localScale = Vector3.zero;

    }
    public void btnReplayClick()
    {
        panelLoading.SetActive(true);
        StartCoroutine(LoadYourAsyncScene(SceneManager.GetActiveScene().buildIndex, 1f));
        SoundManager.Instance.PlaySoundClick();
    }
    public void btnShop()
    {
        panelLoading.SetActive(true);
        StartCoroutine(LoadYourAsyncScene(2, 0));
        SoundManager.Instance.PlaySoundClick();
    }
    IEnumerator LoadYourAsyncScene(int idScenes, float timeLoad)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        yield return new WaitForSeconds(timeLoad);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(idScenes);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void btnNextmap()
    {
        if (PlayerprefSave.IdMap() < RandomMapController.Instance.listBlockLevels.Count - 1)
        {
            if (txtWin.activeInHierarchy)
            {
                //PlayerprefSave.SetMap++;
                if (IronSource.Agent.isInterstitialReady())
                {
                    IronSource.Agent.showInterstitial();
                }
                else
                {
                    Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
                }

            }
            btnReplayClick();
        }
        else
        {
            PlayerprefSave.SetMap = 0;
            btnReplayClick();
        }
    }
    public void BtnGetX5Coin()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            PlayerprefSave.SelectTypeVideo(TypeRewardVideo.x5);
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("video reward not available");
            SoundManager.Instance.ShowNotification(canvasMain.transform);
        }
    }
    public void RecievedX5Coin()
    {
        btnVideoX5.gameObject.SetActive(false);
        btnTapContinue.gameObject.SetActive(false);
        btnClaim.gameObject.SetActive(true);
        GameManager.instance.GetX5Coin();
        txtCoinEndGame.text = GameManager.instance.totalCoinInLevel.ToString();
    }
    int idCostume = 1;
    public void ChangeCostume()
    {
        ChangeCostumePlayer.Instance.ChangeCostume(idCostume);
        switch (idCostume)
        {
            case 0:
                idCostume = 1;
                break;
            case 1:
                idCostume = 2;
                break;
            case 2:
                idCostume = 3;
                break;
            case 3:
                idCostume = 0;
                break;
        }
    }
    public void ChangeMap(int idmap)
    {
        MapManager.Instance.ChangeMap(idmap);
        switch (idmap)
        {
            case 0:
                DynamicFog.instance.baselineHeight = -3.5f;
                break;
            case 1:
                DynamicFog.instance.baselineHeight = -3.5f;
                break;
            case 2:
                DynamicFog.instance.baselineHeight = -3.5f;
                break;
            case 3:
                DynamicFog.instance.baselineHeight = -7f;
                break;
            case 4:
                DynamicFog.instance.baselineHeight = -5f;
                break;
            case 5:
                DynamicFog.instance.baselineHeight = -5f;
                break;
        }
    }
    #endregion
    public void ReadyBooster()
    {
        if (!PlayerPrefs.HasKey("firstPowerUp"))
        {
            btnPowerUp.transform.localScale = Vector3.one;
            PlayerPrefs.SetInt("firstPowerUp", 1);
        }
        else
        {
            StartCoroutine(delayOff());
        }
    }
    IEnumerator delayOff()
    {
        btnPowerUp.transform.DOLocalMoveX(0, 0f);
        btnPowerUp.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(3f);
        btnPowerUp.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .3f).OnComplete(() =>
        {
            btnPowerUp.transform.DOLocalMoveX(-1000, .5f);
            btnPowerUp.transform.DOScale(Vector3.zero, .5f);
        });
    }
    bool checkDoubleClick;
    float timeDoubleClick;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlayerController.Instance.fullBooster)
                if (!checkDoubleClick)
                {
                    timeDoubleClick = Time.time;
                    checkDoubleClick = true;
                }
                else
                {
                    if (Time.time - timeDoubleClick <= 0.5f)
                    {
                        checkDoubleClick = false;
                        ButtonPowerUp();
                    }
                }
        }
        if (checkDoubleClick)
        {
            if (Time.time - timeDoubleClick > 0.4f)
                checkDoubleClick = false;
        }
        if (checkTienTrinh)
            CheckTienTrinh();
        if (fillPower.fillAmount >= 0.38 && !effPower1.activeInHierarchy)
        {
            effPower1.SetActive(true);
        }
        if (fillPower.fillAmount >= 0.71 && !effPower2.activeInHierarchy)
        {
            effPower2.SetActive(true);
        }
    }
    public void disbleffPower()
    {
        effPower1.SetActive(false);
        effPower2.SetActive(false);
    }
    [Header("Control Sound")]
    [SerializeField] private GameObject dialogSound;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;
    public void OpenSetting()
    {
        dialogSound.SetActive(true);
        SoundManager.Instance.PlaySoundClick();
    }
    public void Close()
    {
        dialogSound.SetActive(false);
        SoundManager.Instance.PlaySoundClick();
    }
    void SetSaveSound()
    {
        if (!PlayerPrefs.HasKey("soundValue"))
        {
            PlayerPrefs.SetFloat("soundValue", 1);
            PlayerPrefs.SetFloat("musicValue", 1);
        }
        SoundManager.Instance.sourceSound.volume = PlayerprefSave.SoundValue;
        SoundManager.Instance.sourceMusic.volume = PlayerprefSave.MusicValue;
        sliderSound.value = PlayerprefSave.SoundValue;
        sliderMusic.value = PlayerprefSave.MusicValue;
    }
    public void ChangeValueSound()
    {
        SoundManager.Instance.sourceSound.volume = PlayerprefSave.SoundValue;
        PlayerprefSave.SoundValue = sliderSound.value;
    }
    public void ChangeValueMusic()
    {
        SoundManager.Instance.sourceMusic.volume = PlayerprefSave.MusicValue;
        PlayerprefSave.MusicValue = sliderMusic.value;
    }
}
