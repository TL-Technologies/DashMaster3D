using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ManagerEffect : MonoBehaviourSingleton<ManagerEffect>
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject fxTiepDat;
    [SerializeField] GameObject fxSlide;
    [SerializeField] GameObject fxJump;
    [SerializeField] GameObject fxJump2;
    [SerializeField] GameObject moveSmoke;
    [SerializeField] GameObject moveSmokeParent;
    [SerializeField] GameObject fxPowerUp;
    [SerializeField] Transform Player;
    public GameObject top1Effect;
    [SerializeField] GameObject fxWind;
    [SerializeField] GameObject fxTrigger;
    [SerializeField] GameObject fxSongAm;
    [SerializeField] GameObject fxStar;
    [SerializeField] GameObject fxBatXa;
    [SerializeField] GameObject fxUpgrade;
    [SerializeField] GameObject fxRespwan;
    [SerializeField] GameObject fxHitVatCan;
    [SerializeField] GameObject trial1, trial2;
    public List<Transform> listCharacter;
    public List<GameObject> listCharts;
    public bool checkLowDevice;
    private void Start()
    {
        SCR_Pool.Flush();
        StartCoroutine(DelayChangeEnemy());
    }
    public void EffectTiepDat()
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxTiepDat);
        ob.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + .05f, Player.transform.position.z);
    }
    public void EffectTrigger(Vector3 pos)
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxTrigger);
        ob.transform.position = new Vector3(pos.x, pos.y + 0.1f, pos.z);
    }
    public void EffectSlide(Vector3 pos)
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxSlide);
        ob.transform.position = pos;
    }
    public void EffectJump()
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxJump);
        ob.transform.position = Player.transform.position;
    }
    public void EffectJump2()
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxJump2);
        ob.transform.position = Player.transform.position;
    }
    public void OnMoveSmoke()
    {
        moveSmoke.SetActive(true);
    }
    public void OffMoveSmoke()
    {
        moveSmoke.SetActive(false);
    }
    public void OffTrial()
    {
        trial1.SetActive(false);
        trial2.SetActive(false);
    }
    public void OnPowerUp()
    {
        anim.SetFloat("speedBooster", 1.5f);
        moveSmokeParent.SetActive(false);
        fxBatXa.SetActive(true);
        fxBatXa.GetComponent<ParticleSystem>().playbackSpeed = 1.2f;
        fxPowerUp.SetActive(true);
    }
    public void ShowFxBatXa()
    {
        StartCoroutine(delayFxBatXa());
    }
    IEnumerator delayFxBatXa()
    {
        fxBatXa.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (!PlayerController.Instance._isPowerUp)
            fxBatXa.SetActive(false);
    }
    public void ShowFxUpgrade()
    {
        fxUpgrade.SetActive(false);
        fxUpgrade.SetActive(true);
    }
    public void OffPowerUp()
    {
        anim.SetFloat("speedBooster", 1);
        moveSmokeParent.SetActive(true);
        fxBatXa.SetActive(false);
        fxBatXa.GetComponent<ParticleSystem>().playbackSpeed = 0.6f;
        fxPowerUp.SetActive(false);
    }
    public void ShowFxSongAm(Vector3 pos)
    {
        if (!checkLowDevice)
        {
            GameObject ob = SCR_Pool.GetFreeObject(fxSongAm);
            ob.transform.position = new Vector3(pos.x, pos.y + 0.1f, pos.z + .2f);
        }
    }
    public void ShowFxStar(Vector3 pos)
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxStar);
        ob.transform.position = new Vector3(pos.x, pos.y + 0.2f, pos.z + .2f);
    }
    public void SetTop1(Transform gameObject)
    {
        top1Effect.transform.SetParent(gameObject);
        top1Effect.transform.localPosition = Vector3.zero;
    }
    void CheckTop1()
    {
        //for (int i = 0; i < listCharacter.Length; i++)
        //{
        //    listZ[i] = listCharacter[i].position.z;
        //}
        //float valueMax = listZ.Max();
        //int idMax = Array.IndexOf(listZ, valueMax);
        //SetTop1(listCharacter[idMax]);
        Transform temp;
        for (int i = 0; i < listCharacter.Count; i++)
        {
            for (int j = i + 1; j < listCharacter.Count; j++)
            {
                if (listCharacter[j].position.z < listCharacter[i].position.z)
                {
                    temp = listCharacter[i];
                    listCharacter[i] = listCharacter[j];
                    listCharacter[j] = temp;
                }
            }
        }
        SetTop1(listCharacter[listCharacter.Count - 1]);
        for (int i = 0; i < listCharacter.Count; i++)
        {
            listCharacter[i].GetComponent<SetTopChart>().SetText(listCharacter.Count - i);
        }
    }
    public void AddCharacter(Transform target)
    {
        listCharacter.Add(target);
    }
    private void Update()
    {
        if (!PlayerController.Instance.isWin)
            CheckTop1();
    }
    bool _endChart;
    public int top;
    public void SetOnTop(GameObject gameObject, bool endChart)
    {
        _endChart = endChart;
        if (!_endChart)
        {
            listCharts.Add(gameObject);
        }
        else
        {
            listCharts.Add(gameObject);
            top = listCharts.Count;
        }
    }
    IEnumerator DelayChangeEnemy()
    {
        yield return new WaitForSeconds(3f);
        ChangePositionEnemy();
        StartCoroutine(DelayChangeEnemy());
    }
    void ChangePositionEnemy()
    {
        int rd = UnityEngine.Random.Range(1, 4);
        if (listCharacter[rd].position.z < Player.position.z - 2f)
        {
            listCharacter[rd].position = new Vector3(Player.position.x, Player.position.y, Player.position.z - 1f);
            listCharacter[rd].GetComponent<AIController>().ResetStatusAI();
        }
    }
    public void RespwanFX()
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxRespwan);
        ob.transform.position = Player.transform.position;
    }
    public void VatCanFX()
    {
        GameObject ob = SCR_Pool.GetFreeObject(fxHitVatCan);
        ob.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.2f, Player.transform.position.z);
    }
}