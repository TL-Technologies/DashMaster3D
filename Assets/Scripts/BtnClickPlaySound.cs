using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BtnClickPlaySound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySoundClick();
        //scale btn
        Vector3 tempBtn = gameObject.transform.localScale;
        if (tempBtn.x > 0) tempBtn = new Vector3(0.85f, 0.85f, 0.85f);
        transform.DOScale(tempBtn, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 tempBtn = gameObject.transform.localScale;
        if (tempBtn.x > 0) tempBtn = new Vector3(1f, 1f, 1f);
        transform.DOScale(tempBtn, 0.1f);
    }
}
