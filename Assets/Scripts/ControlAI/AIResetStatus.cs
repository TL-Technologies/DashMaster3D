using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIResetStatus : MonoBehaviour
{
    Animator anim;
    AIController aIController;
    private void Start()
    {
        aIController = transform.parent.GetComponent<AIController>();
        anim = GetComponent<Animator>();
    }
    void ChangeNewPosition()
    {
        transform.parent.position = new Vector3(transform.parent.position.x, transform.GetChild(1).GetChild(0).position.y, transform.GetChild(1).GetChild(0).position.z);
        aIController._isRun = true;
    }
    void EndAction()
    {
        aIController.isAction = false;
    }
    private void ResetVuotRao()
    {
        anim.SetInteger(AnimParameter.vuotrao, 0);
    }
    void ResetWallClimb()
    {
        anim.SetInteger(AnimParameter.wallclimb, 0);
    }
    void ResetLeoTuong()
    {
        transform.parent.GetComponent<Rigidbody>().isKinematic = false;
        aIController.isAction = false;
        aIController._isRun = true;
    }
}
