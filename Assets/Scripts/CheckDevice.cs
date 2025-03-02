using DynamicFogAndMist;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Observer;
public class CheckDevice : MonoBehaviour
{
    public int devicesystemMem;
    public int graphicsMem;

    public const int deviceSystemMemNeeded = 4000;
    public const int graphicsMemNeeded = 4000;

    public DynamicFog dynamicFog;
    public void Awake()
    {
        //this
        devicesystemMem = SystemInfo.systemMemorySize;
        //Debug.Log("devicesystemMem =" + devicesystemMem);
        //or
        graphicsMem = SystemInfo.graphicsMemorySize;
        //Debug.Log("graphicsMem =" + graphicsMem);
    }

    private void Start()
    {
       
#if UNITY_ANDROID
        if (isDeviceLower)
        {
            dynamicFog.enabled = false;
            this.PostEvent(EventID.OffShadow);
            ManagerEffect.Instance.checkLowDevice = true;
        }
        else
        {
            dynamicFog.enabled = true;
        }
#endif
    }
    public bool isDeviceLower
    {
        get
        {
            return graphicsMem <= graphicsMemNeeded && devicesystemMem <= deviceSystemMemNeeded;
        }
    }
}
