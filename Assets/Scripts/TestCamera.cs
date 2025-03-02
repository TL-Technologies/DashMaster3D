using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class TestCamera : MonoBehaviourSingleton<TestCamera>
{
    public Transform lookAt;
    public Transform player;
    public Transform bone;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public bool camFollow;
    public Transform pos1, pos2;
    float posZ;

    private void Start()
    {
        posZ = -0.4f;
        lookAt = player;
        noiseCam = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    //void Update()
    //{
    //    if (camFollow)
    //    {
    //        // Define a target position above and behind the target transform
    //        Vector3 targetPosition = lookAt.TransformPoint(new Vector3(0, 0.8f, posZ));

    //        // Smoothly move the camera towards that target position
    //        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    //    }
    //    //Debug.Log(posZ);
    //}
    public void camWin()
    {
        camFollow = false;
        Vector3 a = pos1.transform.position;
        Vector3 b = pos2.transform.position;
        Camera.main.transform.DORotate(new Vector3(18.6f, -180f, 0), 1f).SetEase(Ease.Linear);
        Camera.main.transform.DOMove(a, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Camera.main.transform.DOMove(b, 0.5f).SetEase(Ease.Linear);
        });
    }
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noiseCam;
    public void CameraShake()
    {
        StartCoroutine(DelayShake());
    }
    IEnumerator DelayShake()
    {
        noiseCam.m_AmplitudeGain = 2;
        noiseCam.m_FrequencyGain = 2;
        yield return new WaitForSeconds(.5f);
        noiseCam.m_AmplitudeGain = 0;
        noiseCam.m_FrequencyGain = 0;
    }
    public void CamNormal()
    {
        StartCoroutine(delayReduce());
    }
    public void CamBooster()
    {
        StartCoroutine(delayReduce());
    }
    IEnumerator delayReduce()
    {
        if (virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance== 0.7f)
        {
            while (virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance < 1.2f)
            {
                virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 1.2f;
        }
        else
        {
            while (virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance > 0.7f)
            {
                virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 0.7f;
        }
    }
    public void CameraFollow()
    {
        GetComponent<CinemachineBrain>().enabled = true;
    }
    public void DontCameraFollow()
    {
        GetComponent<CinemachineBrain>().enabled = false;
    }
    public void ChangeRotateCamera()
    {
        virtualCamera.transform.DORotate(new Vector3(5, 0, 0), .5f).SetEase(Ease.Linear);
    }
    public void ChangeDefaultRotateCamera()
    {
        virtualCamera.transform.DORotate(new Vector3(20, 0, 0), .5f).SetEase(Ease.Linear);
    }
}
