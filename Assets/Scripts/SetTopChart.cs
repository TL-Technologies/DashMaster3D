using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Observer;
public class SetTopChart : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public TextMeshProUGUI textMeshSpeed;
    float speed;
    public bool isPlayer;
    public GameObject shadow;
    int thuHang;
    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SetTextAI, (param) => SetTextSpeed());
        EventDispatcher.Instance.RegisterListener(EventID.OffShadow, (param) => OffShadow());
    }
    private void Start()
    {
     
    }
    void OffShadow()
    {
        shadow.SetActive(false);
    }
    void SetTextSpeed()
    {
        if (isPlayer)
        {
            UIController.Instance.SetSpeedNhanVat(GetComponent<PlayerController>().speed);
        }
        else
        {
            textMeshSpeed.text = "Speed: " + GetComponent<AIController>().speed;
        }
       
    }
    public void SetText(int oder)
    {
        if (thuHang != oder)
        {
            thuHang = oder;
            //if (isPlayer)
            //    textMesh.GetComponent<Animator>().Play("thuHang", -1, 0);
            if (isPlayer)
            {
                UIController.Instance.SetThuHangNhanVat(oder);
            }
            else
            {
                textMesh.text = GameManager.QuyDoi(oder);
                if (oder == 1)
                {
                    textMesh.gameObject.SetActive(false);
                    textMeshSpeed.gameObject.SetActive(false);
                }
                else
                {
                    textMesh.gameObject.SetActive(true);
                    textMeshSpeed.gameObject.SetActive(true);
                }
            }
        }
    }
    public void OffOder()
    {
        textMesh.gameObject.SetActive(false);
    }
    
}
