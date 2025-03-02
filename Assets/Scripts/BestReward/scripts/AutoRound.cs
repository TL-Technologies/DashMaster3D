using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRound : MonoBehaviour
{
    public bool isRound;

    public float speed=30;

    public Vector3 velocity = new Vector3(0, 0, -1);
    private void OnEnable()
    {
        transform.eulerAngles = Vector3.zero;
    }
    void Update()
    {
        if (isRound)
        {
            transform.Rotate(velocity * speed * Time.deltaTime);
        }
    }
}
