using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffPool : MonoBehaviour
{
    [SerializeField] float timeOff;
    private void OnEnable()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(timeOff);
        gameObject.SetActive(false);
    }
}
