using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class LoadingScripts : MonoBehaviour
{
    [SerializeField] Image fillAmount;
    void Start()
    {
        StartCoroutine(LoadYourAsyncScene());
        PlayerprefSave.FirstOpenGame();
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        fillAmount.DOFillAmount(.4f, .4f);
        yield return new WaitForSeconds(.4f);
        fillAmount.DOFillAmount(.6f, .5f);
        yield return new WaitForSeconds(.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(.5f);
        fillAmount.fillAmount = .8f;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            fillAmount.fillAmount = 1;
            yield return null;
        }
    }
}
