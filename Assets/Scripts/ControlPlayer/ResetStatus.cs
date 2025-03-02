using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStatus : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void ChangeNewPosition()
    {
        transform.parent.position = new Vector3(transform.parent.position.x, transform.GetChild(1).GetChild(0).position.y, transform.GetChild(1).GetChild(0).position.z);
        PlayerController.Instance.lockMove = false;
        PlayerController.Instance._isRun = true;
        //TestCamera.Instance.lookAt = TestCamera.Instance.player;
        PlayerController.Instance.lockMove = false;
    }
    void EndAction()
    {
        PlayerController.Instance.isAction = false;
        ManagerEffect.Instance.OnMoveSmoke();
    }
    private void ResetVuotRao()
    {
        anim.SetInteger(AnimParameter.vuotrao, 0);
    }
    void ResetWallClimb()
    {
        anim.SetInteger(AnimParameter.wallclimb, 0);
    }
    void ResetJump()
    {
        anim.SetInteger(AnimParameter.jump, 0);
    }
    void ResetLeoTuong()
    {
        transform.parent.GetComponent<Rigidbody>().isKinematic = false;
        PlayerController.Instance.isAction = false;
        PlayerController.Instance._isRun = true;
        PlayerController.Instance.leoTuong = false;
        PlayerController.Instance.checkFirst = true;
        PlayerController.Instance.lockMove = false;
        //Debug.Log("hanh dong");
    }
}
