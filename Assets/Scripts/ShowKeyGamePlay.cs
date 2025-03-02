using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowKeyGamePlay : MonoBehaviour
{
    [SerializeField] List<GameObject> keyRewards;
    [SerializeField] Sprite spKeyOn, spKeyOff;
    void Start()
    {
        for (int i = 0; i < keyRewards.Count; i++)
        {
            if (i < PlayerprefSave.keyReward)
            {
                keyRewards[i].GetComponent<Image>().sprite = spKeyOn;
            }
            else
            {
                keyRewards[i].GetComponent<Image>().sprite = spKeyOff;
            }
        }
    }

}
