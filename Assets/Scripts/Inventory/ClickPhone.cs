using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPhone : MonoBehaviour, IPointerDownHandler
{
    public event Action OnRemoveSelectedItem;
    public void OnPointerDown(PointerEventData eventData)
    {
        //�ڵ��� Ŭ�� ��
        //Tween �ڵ��� �ö��
        OnRemoveSelectedItem?.Invoke() ;
    }
}
