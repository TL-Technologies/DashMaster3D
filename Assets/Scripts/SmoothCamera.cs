using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmoothCamera : MonoBehaviourSingleton<SmoothCamera>
{
    public Transform lookAt;
    public Transform player;
    public Transform bone;
    public float smoothSpeed;
    public Vector3 offSet;
    public bool camFollow;
    public GameObject fxSpeed;
    // Use this for initialization
    void Start()
    {
        //camFollow = true;
        //smoothSpeed = 0.25f;
        lookAt = player;
    }
    float y;
    // Update is called once per frame
    void LateUpdate()
    {
        if (camFollow)
        {
            Vector3 desiredPosition = lookAt.position + offSet;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;
        }
    }
}