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
        //핸드폰 클릭 시
        //Tween 핸드폰 올라옴
        OnRemoveSelectedItem?.Invoke() ;
    }
}
