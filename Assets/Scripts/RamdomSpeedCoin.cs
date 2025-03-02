using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomSpeedCoin : MonoBehaviour
{
    private void OnBecameVisible()
    {
        GetComponent<Animator>().enabled = true;
    }
    private void OnBecameInvisible()
    {
        GetComponent<Animator>().enabled = false;
    }
    void Start()
    {
        GetComponent<Animator>().speed = Random.Range(0.75f, 1.25f);
    }
}
