using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource sourceMusic;
    public AudioSource sourceSound;
    public AudioClip soundCoin;
    public AudioClip soundDie;
    public AudioClip soundDieVacham;
    public AudioClip soundNhay;
    public AudioClip soundSlide;
    public AudioClip soundBatXa;
    public AudioClip soundHaXuong;
    public AudioClip soundHoisinh;
    public AudioClip soundLonVongTiepDat;
    public AudioClip soundClick;
    public AudioClip soundUpgrade;
    bool isMusic;
    [SerializeField] GameObject notification;
    private GameObject objTemp;
    private void Awake()
    {
        PlayerprefSave.FirstOpenGame();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetInt("sound", 1);
        }
        ControlMusic();
    }
    public void ControlMusic()
    {
        if (isMusic)
        {
            //tat nhac
            isMusic = false;
            sourceMusic.volume = 0;
        }
        else
        {
            //bat nhac
            isMusic = true;
            sourceMusic.volume = 1;
        }
    }
    public void ControlSound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            //tat nhac
            PlayerPrefs.SetInt("sound", 0);
            sourceSound.volume = 0;
        }
        else
        {
            //bat nhac
            PlayerPrefs.SetInt("sound", 1);
            sourceSound.volume = 1;
        }
    }
    public void PlaySoundCoin()
    {
        sourceSound.PlayOneShot(soundCoin, 1);
    }
    public void PlaySoundDie()
    {
        sourceSound.PlayOneShot(soundDie, 1);
    }
    public void PlaySoundDieVaCham()
    {
        sourceSound.PlayOneShot(soundDieVacham, 1);
    }
    public void PlaySoundHoiSinh()
    {
        sourceSound.PlayOneShot(soundHoisinh, 1);
    }
    public void PlaySoundLonVongTiepDat()
    {
        sourceSound.PlayOneShot(soundLonVongTiepDat, 1);
    }
    public void PlaySoundNhay()
    {
        sourceSound.PlayOneShot(soundNhay, 1);
    }
    public void PlaySoundSlide()
    {
        sourceSound.PlayOneShot(soundSlide, 1);
    }
    public void PlaySoundBatXa()
    {
        sourceSound.PlayOneShot(soundBatXa, 1);
    }
    public void PlaySoundHaXuong()
    {
        sourceSound.PlayOneShot(soundHaXuong, 1);
    }
    public void PlaySoundClick()
    {
        sourceSound.PlayOneShot(soundClick, 1);
    }
    public void PlaySoundUpgrade()
    {
        sourceSound.PlayOneShot(soundUpgrade, 1);
    }
    public void ShowNotification(Transform transformParent)
    {
        if (objTemp == null)
        {
            objTemp = Instantiate(notification, transformParent);
            objTemp.transform.localPosition =new Vector3(0,0,-1000);
            StartCoroutine(DelayNotification());
        }
        else
        {
            StopAllCoroutines();
            Destroy(objTemp);
            objTemp = Instantiate(notification, transformParent);
            objTemp.transform.localPosition = new Vector3(0, 0, -1000);
            StartCoroutine(DelayNotification());
        }
    }
    IEnumerator DelayNotification()
    {
        yield return new WaitForSeconds(1f);
        Destroy(objTemp);
    }
}
