using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class RotationCharacter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool checkRotation;
    Vector3 rotateVector = Vector3.zero;
    public Transform characterGroup;
    float x, temp;

    public void OnPointerDown(PointerEventData eventData)
    {
        checkRotation = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        checkRotation = false;
    }

    private void Update()
    {
        if (checkRotation)
        {
            if (Input.GetMouseButtonDown(0))
            {
                x = Input.mousePosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                temp = Input.mousePosition.x;

                if (x != temp)
                {
                    rotateVector = characterGroup.localRotation.eulerAngles;

                    rotateVector.y += (x - temp) * Mathf.Rad2Deg * 0.02f;
                    characterGroup.DOLocalRotate(rotateVector, 0.2f);
                    x = temp;
                }
            }
        }
    }
}
