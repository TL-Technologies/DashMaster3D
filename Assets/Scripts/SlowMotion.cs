using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SlowMotion : MonoBehaviourSingleton<SlowMotion>
{
    public float slowMotionTimeScale = 1f;
    public bool slowMotionEnabled = false;

    [System.Serializable]
    public class AudioSourceData
    {
        public AudioSource audioSource;
        public float defaultPitch;
    }

    AudioSourceData[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        //Find all AudioSources in the Scene and save their default pitch values
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        audioSources = new AudioSourceData[audios.Length];

        for (int i = 0; i < audios.Length; i++)
        {
            AudioSourceData tmpData = new AudioSourceData();
            tmpData.audioSource = audios[i];
            tmpData.defaultPitch = audios[i].pitch;
            audioSources[i] = tmpData;
        }

       // SlowMotionEffect(slowMotionEnabled);
    }

    void SlowMotionEffect(bool enabled)
    {
        Time.timeScale = enabled ? slowMotionTimeScale : 1;
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].audioSource)
            {
                audioSources[i].audioSource.pitch = audioSources[i].defaultPitch * Time.timeScale;
            }
        }
    }
    public void Slow(float time)
    {
        slowMotionEnabled = true;
        StartCoroutine(delay(time));
    }
    IEnumerator delay(float timeSlow)
    {
        SlowMotionEffect(slowMotionEnabled);
        yield return new WaitForSeconds(timeSlow);
        SlowMotionEffect(!slowMotionEnabled);
    }   
    public void SlowNoAudio(float timeSlow,float timescale)
    {
        StartCoroutine(delaySlow(timeSlow,timescale));
    }
    IEnumerator delaySlow(float time, float timescale)
    {
        Time.timeScale = timescale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
}