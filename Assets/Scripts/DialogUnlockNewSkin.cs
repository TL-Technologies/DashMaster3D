using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUnlockNewSkin : MonoBehaviour
{
    public static DialogUnlockNewSkin Instance;
    [Header("DialogUnlockNewSkin")]
    [SerializeField] GameObject Dialog;
    [SerializeField] GameObject[] characters;
    
    [SerializeField] Text txtName;
    [SerializeField] Animator animCharacter;

    public int idCharacterShow;
    [SerializeField] GameObject Base_1;
    void Start()
    {
        //show character unlock
        Debug.Log("id costume: " + PlayerprefSave.idTemp);
        showCharacter(PlayerprefSave.idTemp);
        idCharacterShow = PlayerprefSave.idTemp;
    }

    #region DialogUnlockNewSkin
    public void btnCloseClick()
    {
        Dialog.SetActive(false);
        UIController.Instance.showUINewSkin(false);
    }
    public void btnViewAdsUnlockSkinClick()
    {
        OnCompliteAdsSkin();
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            PlayerprefSave.SelectTypeVideo(TypeRewardVideo.unlockskin);
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("video reward not available");
            SoundManager.Instance.ShowNotification(UIController.Instance.canvasNewSkin.transform.GetChild(0));
        }
    }

    public void OnCompliteAdsSkin()
    {
        Debug.Log("OnCompliteAdsSkin");
        PlayerprefSave.UnlockCostume(idCharacterShow);
        ChangeCostumePlayer.Instance.ChangeCostume(idCharacterShow);
        btnCloseClick();
    }
    public void OnFailAdsSkin()
    {
        Debug.Log("OnFailAdsSkin");
    }

    public void showCharacter(int id)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }

        if (id < characters.Length)
        {
            if (id == 8)
                Base_1.SetActive(false);
            else Base_1.SetActive(true);
            characters[id].SetActive(true);
            txtName.text = DataGame.Instance.dataCharacter.characters[id].Name_Character;
            //set anim idle skin
            //animCharacter.SetBool...
        }
    }
    #endregion
}
