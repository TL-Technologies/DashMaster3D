using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Observer;

public class AIController : MonoBehaviour
{
    [Tooltip("0->3 là enemy, 4 là boss")]
    public int IdnameEnemy;
    public bool _isLive;
    public bool _isRun;
    public float speed;
    public bool isBoss;
    float heighJump;
    Rigidbody rig;
    Animator anim;
    public Transform posCheckGround;
    public Transform posCheckJump;
    public Transform originJump;
    public Transform JumpLeft;
    public Transform JumpRight;
    public LayerMask layerGround;
    public LayerMask layerVatCan;
    float distanceJump = .3f;
    CapsuleCollider capsuleCollider;
    //Vector3 directionRD;
    bool leoTuong;
    bool wallRunLeft;
    bool wallRunRight;
    float speedLeoTuong = .8f;
    public GameObject txtText;
    public bool isWin;
    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rig = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        EventDispatcher.Instance.RegisterListener(EventID.StartAI, (param) => StartGame());
        EventDispatcher.Instance.RegisterListener(EventID.PauseAI, (param) => PauseRun());
        EventDispatcher.Instance.RegisterListener(EventID.ContinueAI, (param) => ContinueRun());
        EventDispatcher.Instance.RegisterListener(EventID.OffText, (param) => OffText());
        EventDispatcher.Instance.RegisterListener(EventID.SpeedMain, (param) => SetSpeed((float)param));
        EventDispatcher.Instance.RegisterListener(EventID.SetHeighJump, (param) => SetHeighJumpFollowMain((float)param));
        posCheckPoint = transform.position;
    }
    public void StartGame()
    {
        _isLive = true;
        _isRun = true;
        Run();
    }
    Vector3 tempDirection = Vector3.zero;
    bool changeDirection;
    IEnumerator delayDoiHuong()
    {
        changeDirection = true;
        yield return new WaitForSeconds(.4f);
        changeDirection = false;
        tempDirection = Vector3.zero;
    }
    void RayscastCheckVatCan()
    {
        RaycastHit hit;
        Vector3 direction = originJump.position + Vector3.forward;
        if (Physics.Raycast(originJump.position, direction, out hit, 2f, layerVatCan))
        {
            if (Random.Range(0, 2) == 0)
                tempDirection = Vector3.left;
            else
                tempDirection = Vector3.right;
            StartCoroutine(delayDoiHuong());
        }
    }
    void SetSpeed(float speedMain)
    {
        switch (IdnameEnemy)
        {
            case 0:
                speed = GameManager.instance.dataEnemies[PlayerprefSave.IdMap()].Enemy1;
                break;
            case 1:
                speed = GameManager.instance.dataEnemies[PlayerprefSave.IdMap()].Enemy2;
                break;
            case 2:
                speed = GameManager.instance.dataEnemies[PlayerprefSave.IdMap()].Enemy3;
                break;
            case 3:
                speed = GameManager.instance.dataEnemies[PlayerprefSave.IdMap()].Enemy4;
                break;
            case 4:
                speed = GameManager.instance.dataEnemies[PlayerprefSave.IdMap()].Boss;
                break;
        }
    }
    void SetHeighJumpFollowMain(float value)
    {
        heighJump = value;
    }
    public bool IsGrounded()
    {
        return Physics.CheckCapsule(capsuleCollider.bounds.center, new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y, capsuleCollider.bounds.center.z),
            capsuleCollider.radius / 3, layerGround);
    }
    //check truoc mat
    public bool IsFrontUp()
    {
        return Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.center.y, capsuleCollider.bounds.max.z),
            new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.center.y, capsuleCollider.bounds.max.z), capsuleCollider.radius / 3, layerGround);
    }
    //check truoc mat ben trai
    public bool IsFrontUpLeft()
    {
        return Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.min.x, capsuleCollider.bounds.max.y, capsuleCollider.bounds.max.z),
            new Vector3(capsuleCollider.bounds.min.x, capsuleCollider.bounds.max.y, capsuleCollider.bounds.max.z), capsuleCollider.radius, layerGround);
    }
    //check truoc mat ben phai
    public bool IsFrontUpRight()
    {
        return Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.max.x, capsuleCollider.bounds.max.y, capsuleCollider.bounds.max.z),
            new Vector3(capsuleCollider.bounds.max.x, capsuleCollider.bounds.max.y, capsuleCollider.bounds.max.z), capsuleCollider.radius, layerGround);
    }
    public bool IsLeft()
    {
        return Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.min.x, capsuleCollider.bounds.center.y, capsuleCollider.bounds.min.z),
            new Vector3(capsuleCollider.bounds.min.x, capsuleCollider.bounds.center.y, capsuleCollider.bounds.min.z), capsuleCollider.radius, layerGround);
    }
    public bool IsRight()
    {
        return Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.max.x, capsuleCollider.bounds.center.y, capsuleCollider.bounds.min.z),
            new Vector3(capsuleCollider.bounds.max.x, capsuleCollider.bounds.center.y, capsuleCollider.bounds.min.z), capsuleCollider.radius, layerGround);
    }
    bool suggest;
    private void Update()
    {
        if (_isLive)
        {
            if (_isRun)
            {
                transform.Translate((Vector3.forward + tempDirection) * speed * Time.deltaTime);
                if (!changeDirection)
                {
                    RayscastCheckVatCan();
                }
            }
            if (!leoTuong)
            {
                if (IsGrounded())
                {
                    if (checkFirst)
                    {
                        if (batXa)
                        {
                            anim.Play("LandRoll", -1, 0);
                            batXa = false;
                            checkFirst = false;
                            isAction = false;
                        }
                        else
                        {
                            rig.isKinematic = false;
                            _isRun = true;
                            isAction = false;
                            checkFirst = false;
                            Run();
                        }
                        wallRunLeft = false;
                        wallRunRight = false;
                        rig.drag = 0;
                        posCheckPoint = transform.position;
                    }
                    if (!isAction)
                    {
                        if (!suggest)
                        {
                            if (!isAction)
                                RaycastCheckLeft();
                            if (!isAction)
                                RaycastCheckRight();
                            if (!isAction)
                                CheckIsJump();
                        }
                    }
                }
                else
                {
                    checkFirst = true;
                    //chay tren tuong
                    if (IsLeft())
                    {
                        if (!wallRunLeft)
                        {
                            leoTuong = false;
                            rig.velocity = Vector3.zero;
                            anim.Play("WallRunLeft", -1, 0);
                            wallRunLeft = true;
                            //rig.isKinematic = true;
                            rig.drag = 10;
                        }
                        else
                        {
                            if (!checkAnimPlay("WallRunLeft"))
                                anim.Play("WallRunLeft", -1, 0);
                        }
                    }
                    else
                    {
                        if (wallRunLeft)
                        {
                            anim.Play("WallRunEndLeft", -1, 0);
                            wallRunLeft = false;
                            rig.isKinematic = false;
                            rig.drag = 0;
                            rig.AddForce(1f, 4, 0, ForceMode.Impulse);
                        }
                    }
                    if (IsRight())
                    {
                        if (!wallRunRight)
                        {
                            leoTuong = false;
                            rig.velocity = Vector3.zero;
                            anim.Play("WallRunRight", -1, 0);
                            wallRunRight = true;
                            //rig.isKinematic = true;
                            rig.drag = 10;
                        }
                        else
                        {
                            if (!checkAnimPlay("WallRunRight"))
                                anim.Play("WallRunRight", -1, 0);
                        }
                    }
                    else
                    {
                        if (wallRunRight)
                        {
                            anim.Play("WallRunEndRight", -1, 0);
                            wallRunRight = false;
                            rig.isKinematic = false;
                            rig.drag = 0;
                            rig.AddForce(-1f, 4, 0, ForceMode.Impulse);
                        }
                    }
                }
                if (IsFrontUp() /*&& !wallRunRight*/)
                {
                    rig.velocity = Vector3.zero;
                    _isRun = false;
                    leoTuong = true;
                    rig.isKinematic = true;
                    anim.Play("WallClimbing", -1, 0);
                    isAction = true;
                    wallRunLeft = false;
                    wallRunRight = false;
                    rig.drag = 0;
                }
            }
            else
            {
                transform.Translate(Vector3.up * speedLeoTuong * Time.deltaTime);
                if (!checkAnimPlay("WallClimbing"))
                {
                    anim.Play("WallClimbing", -1, 0);
                }
                if (!IsFrontUpLeft() && IsFrontUpRight())
                {
                    leoTuong = false;
                    _isRun = true;
                    rig.isKinematic = false;
                    isAction = false;
                    anim.SetInteger(AnimParameter.jump, 1);
                    rig.AddForce(-.5f, 4, 0, ForceMode.Impulse);
                }
                else
                if (!IsFrontUpRight() && IsFrontUpLeft())
                {
                    leoTuong = false;
                    _isRun = true;
                    rig.isKinematic = false;
                    isAction = false;
                    anim.SetInteger(AnimParameter.jump, 1);
                    rig.AddForce(.5f, 4, 0, ForceMode.Impulse);
                }
                else if (!IsFrontUpLeft() && !IsFrontUpRight())
                {
                    if (!checkAnimPlay("WallClimbingEnd"))
                    {
                        anim.Play("WallClimbingEnd", -1, 0);
                        leoTuong = false;
                        _isRun = false;
                        checkFirst = false;
                        transform.DOMoveY(transform.position.y + .12f, .2f);
                        transform.DOMoveZ(transform.position.z + .02f, .2f);
                        //{
                        //    isAction = false;
                        //    _isRun = true;
                        //    rig.isKinematic = false;
                        //    leoTuong = false;
                        //    checkFirst = false;
                        //});
                    }
                }

            }

        }
        if (isWin)
        {
            rig.isKinematic = true;
            _isLive = false;
            _isRun = false;
            if (!checkAnimPlay("Victory"))
            {
                anim.Play("Vitory", -1, 0);
            }
        }
    }
    bool firstClimb;
    bool checkFirst;
    public bool isAction;

    void CheckIsJump()
    {
        RaycastHit hit;
        Vector3 direction = transform.TransformDirection(posCheckJump.position - originJump.position);
        if (!Physics.Raycast(originJump.position, direction, out hit, distanceJump, layerGround))
        {
            Jump(heighJump);
        }
    }
    void RaycastCheckLeft()
    {
        RaycastHit hit;
        Vector3 direction = JumpLeft.position - originJump.position;
        if (!Physics.Raycast(originJump.position, direction, out hit, distanceJump, layerGround))
        {
            isAction = true;
            anim.SetTrigger(AnimParameter.jump);
            rig.velocity = Vector3.zero;
            rig.AddForce(0f, heighJump, -2f, ForceMode.Impulse);
        }

    }
    void RaycastCheckRight()
    {
        RaycastHit hit;
        Vector3 direction = JumpRight.position - originJump.position;
        if (!Physics.Raycast(originJump.position, direction, out hit, distanceJump, layerGround))
        {
            isAction = true;
            anim.SetTrigger(AnimParameter.jump);
            rig.velocity = Vector3.zero;
            rig.AddForce(0f, heighJump, 2f, ForceMode.Impulse);
        }

    }
    public bool checkAnimPlay(string nameAnim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(nameAnim);
    }
    #region anim player
    public void Jump(float force)
    {
        if (!isAction)
        {
            isAction = true;
            anim.SetTrigger(AnimParameter.jump);
            rig.velocity = Vector3.zero;
            rig.AddForce(0, force, 0, ForceMode.Impulse);
            _isRun = true;
            rig.isKinematic = false;
        }
    }
    public void Climb(int idType)
    {
        if (!isAction)
        {
            _isRun = false;
            isAction = true;
            anim.SetInteger(AnimParameter.wallclimb, idType);
        }
    }

    public void Run()
    {
        anim.Play("Run");
    }
    public void Slide()
    {
        //if (!isAction)
        //{
        isAction = true;
        anim.SetTrigger(AnimParameter.slide);
        //    }
    }

    public void Die()
    {
        rig.drag = 0;
        isAction = false;
        _isRun = false;
        suggest = false;
        anim.Play("Die", -1, 0);
        transform.DOMoveZ(transform.position.z - .3f, .5f);
        checkFirst = false;
        _isLive = false;
        Invoke("DelayReSpawn", 1f);
    }
    void DelayReSpawn()
    {
        _isLive = true;
        transform.position = posCheckPoint;
        isAction = false;
        _isRun = true;
        if (batXa)
        {
            batXa = false;
        }
        wallRunLeft = false;
        wallRunRight = false;
        leoTuong = false;
        anim.Play("Run");
    }
    public void Win(Vector3 target)
    {

        isWin = true;
        ResetStatusAI();
        _isRun = false;
        _isLive = false;
        rig.isKinematic = true;
        anim.SetTrigger(AnimParameter.jump);
        Vector3 newTarget = new Vector3(target.x + Random.Range(-1.0f, 1.0f), target.y, target.z + Random.Range(-0.1f, -0.5f));
        transform.DOJump(newTarget, 1, 1, 1f).SetEase(Ease.Linear).OnComplete(() => { anim.SetTrigger("win"); });
    }
    int rd = 1;
    public void NhayVuotRao()
    {
        isAction = true;
        anim.SetInteger(AnimParameter.vuotrao, rd);
        rig.velocity = Vector3.zero;
        if (rd == 1)
            rig.AddForce(new Vector3(0f, 4f, 0), ForceMode.Impulse);
        else
            rig.AddForce(new Vector3(0f, 4f, 0), ForceMode.Impulse);
        if (rd == 1)
            rd = 2;
        else
            rd = 1;
    }
    public void SuggestAI(Vector3 target)
    {
        if (!isAction)
        {
            suggest = true;
            isAction = true;
            rig.velocity = Vector3.zero;
            if (target.x < transform.position.x)
                transform.GetChild(0).DOLocalRotate(new Vector3(0, -10, 0), .3f);
            else
                transform.GetChild(0).DOLocalRotate(new Vector3(0, 10, 0), .3f);
            anim.SetTrigger(AnimParameter.nhayxa);
            transform.DOJump(target, 1f, 1, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                suggest = false;
                isAction = false;
                transform.GetChild(0).DOLocalRotate(Vector3.zero, .3f);
            });
            posCheckPoint = transform.position;
        }
    }
    public void Nhayxa()
    {
        if (!isAction)
        {
            isAction = true;
            anim.SetTrigger(AnimParameter.nhayxa);
            rig.AddForce(new Vector3(0f, transform.position.y + 4f, 0), ForceMode.Impulse);
        }
    }
    bool batXa;
    public void BatNhay()
    {
        isAction = true;
        anim.Play("JumpRoll", -1, 0);
        batXa = true;
        //anim.SetInteger(AnimParameter.jump, Random.Range(1, 3));
        rig.velocity = Vector3.zero;
        rig.AddForce(0, 8, 1, ForceMode.Impulse);
        ManagerEffect.Instance.OffMoveSmoke();
    }
    Vector3 posCheckPoint;
    public void CheckPoint(Vector3 target)
    {
        posCheckPoint = target;
    }
    void PauseRun()
    {
        _isLive = false;
    }
    void ContinueRun()
    {
        ResetStatusAI();
        _isLive = true;
    }
    void OffText()
    {
        txtText.SetActive(false);
    }
    public void ResetStatusAI()
    {
        rig.drag = 0;
        isAction = false;
        _isRun = true;
        if (!isWin)
            anim.Play("Run", -1, 0);
        checkFirst = false;
        _isLive = true;
        batXa = false;
        rig.isKinematic = false;
        leoTuong = false;
        wallRunLeft = false;
        wallRunRight = false;
        //StartCoroutine(TangSpeed());
    }
    IEnumerator TangSpeed()
    {
        speed += .4f;
        yield return new WaitForSeconds(1.5f);
        speed -= .4f;
    }
    #endregion
}
