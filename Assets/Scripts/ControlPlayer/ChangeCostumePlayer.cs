using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangeCostumePlayer : MonoBehaviourSingleton<ChangeCostumePlayer>
{
    [SerializeField] GameObject[] costumes;
    [SerializeField] GameObject skinDefault;
    private void Start()
    {
        ChangeCostume(PlayerprefSave.CurrentCostume);
    }
    public void ChangeCostume(int idCostume)
    {
        for (int i = 0; i < costumes.Length; i++)
        {
            if (idCostume == i)
            {
                costumes[i].SetActive(true);
            }
            else
            {
                costumes[i].SetActive(false);
            }
        }
        if (idCostume == 7 || idCostume==8)
            skinDefault.SetActive(false);
        else
            skinDefault.SetActive(true);
    }
}

