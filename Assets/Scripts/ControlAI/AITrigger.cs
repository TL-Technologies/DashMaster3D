using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    private AIController aIController;
    private void Start()
    {
        aIController = GetComponent<AIController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Die"))
        {
            aIController.Die();
        }
        if (collision.gameObject.CompareTag("Die2"))
        {
            aIController.Die();
            aIController.transform.position = new Vector3(aIController.transform.position.x, aIController.transform.position.y - 2, aIController.transform.position.z);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "vatcan":
                if (aIController.checkAnimPlay("Jump"))
                {
                    aIController.Die();
                }
                break;
            case "slide":
                aIController.Slide();
                break;
            case "climb":
                aIController.Climb(other.gameObject.GetComponent<TypeWallClimb>().typeClimb);
                break;
            case "nhayxa":
                aIController.Nhayxa();
                break;
            case "NhayVuotRao":
                aIController.NhayVuotRao();
                break;
            case "Win":
                gameObject.GetComponent<SetTopChart>().OffOder();
                ManagerEffect.Instance.SetOnTop(gameObject, false);
               aIController.Win(other.transform.GetChild(0).position);
                break;
            case "checkPoint":
                aIController.CheckPoint(other.transform.position);
                break;
            case "suggestAI":
                aIController.SuggestAI(other.transform.GetChild(Random.Range(0,other.transform.childCount)).position);
                break;
            case "coin":
                //playerController.EatItemPower(0.5f);
                break;
            case "BatNhay":
                aIController.BatNhay();
                break;
            case "jump":
                aIController.Jump(5);
                break;
        }
    }
}
