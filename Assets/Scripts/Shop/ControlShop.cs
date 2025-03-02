using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlShop : MonoBehaviourSingleton<ControlShop>
{
    //public List<DataShop> dataShops;
    public List<GameObject> itemShop;
    public GameObject objWatchVideo;
    public GameObject panelLoad;
    public DataCharacter dataCharacter;
    int temp = 0;
    void Start()
    {
        for(int i = 0; i < dataCharacter.characters.Length; i++)
        {
            if (dataCharacter.characters[i].typeUnlock == TypeUnlock.rewardVideo)
            {
                itemShop[i-temp].GetComponent<ItemShop>().SetInfoItem(dataCharacter.characters[i].Id_Character, dataCharacter.characters[i].levelUnlock, dataCharacter.characters[i].spLock, dataCharacter.characters[i].spIcon);
                itemShop[i-temp].GetComponent<ItemShop>().CheckLevelUnlock();
                itemShop[i-temp].GetComponent<ItemShop>().CheckUnlock();
                itemShop[i-temp].GetComponent<ItemShop>().CheckChoose();
            }
            else
            {
                if (PlayerprefSave.CheckUnlockCostume(dataCharacter.characters[i].Id_Character))
                {
                    itemShop[i].GetComponent<ItemShop>().SetInfoItem(dataCharacter.characters[i].Id_Character, dataCharacter.characters[i].levelUnlock, dataCharacter.characters[i].spLock, dataCharacter.characters[i].spIcon);
                    itemShop[i].GetComponent<ItemShop>().CheckLevelUnlock();
                    itemShop[i].GetComponent<ItemShop>().CheckUnlock();
                    itemShop[i].GetComponent<ItemShop>().CheckChoose();
                }
                else
                {
                    temp = 1;
                }
            }
        }
    }
    public Text txtName;
    public GameObject effTop;

    public void showName(int id)
    {
        txtName.text = dataCharacter.characters[id].Name_Character;
    }
    public void btnReturnGameplay()
    {
        effTop.SetActive(false);
        panelLoad.SetActive(true);
        StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        yield return new WaitForSeconds(.1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
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
}
[System.Serializable]
public class DataShop
{
    public int id;
    public int levelOpen;
    public Sprite iconOpen;
    public Sprite iconLock;
}
