using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPhone : MonoBehaviour, IPointerEnterHandler
{
    public event Action OnRemoveSelectedItem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //�ڵ��� Ŭ�� ��
        //Tween �ڵ��� �ö��
        OnRemoveSelectedItem?.Invoke() ;
    }
}
