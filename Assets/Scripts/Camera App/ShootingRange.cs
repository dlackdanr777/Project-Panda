using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // ´Ù Á¦Èñ Å¿ ÀÔ´Ï´Ù.
public class ShootingRange : MonoBehaviour, IDragHandler 
{
    [SerializeField] private Image _shootingImage;
    [SerializeField] private Canvas _canvas;

    public void OnDrag(PointerEventData eventData) // Á¤Á÷ÇÔÀ¸·Î ½ÂºÎÇÏ´Â ²Û ÀÓÃ¢¹¬ ´ëÇ¥
    {
        if(eventData.clickCount > 0)
        {
            Vector2 mousePos = eventData.position;
            _shootingImage.rectTransform.position = DontMoveCalculator(mousePos);

        }
    }


    private Vector2 DontMoveCalculator(Vector2 clickPos) 
    {
        Vector2 pos = Camera.main.ScreenToViewportPoint(clickPos);

        if (pos.x > 1) pos.x = 1;
        if(pos.x < 0) pos.x = 0;
        if(pos.y > 1) pos.y = 1;
        if(pos.y < 0) pos.y = 0;

        return Camera.main.ViewportToScreenPoint(pos);
    }

}
