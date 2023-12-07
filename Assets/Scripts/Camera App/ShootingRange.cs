using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // ¥Ÿ ¡¶»Ò ≈ø ¿‘¥œ¥Ÿ.
public class ShootingRange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image _shootingImage;
    [SerializeField] private Canvas _canvas;

    public event Action<PointerEventData> OnPointerDownHandler;
    public event Action<PointerEventData> OnPointerUpHandler;
    public event Action<PointerEventData> OnDragehandler;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragehandler?.Invoke(eventData);
    }
}
