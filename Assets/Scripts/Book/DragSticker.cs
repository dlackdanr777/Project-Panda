using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSticker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform _stickerZone;

    private Canvas _canvas;
    private GameObject _stickerClone;

    private void Start()
    {
        _canvas = transform.root.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _stickerClone = Instantiate(transform.GetChild(0).gameObject, _stickerZone);
        _stickerClone.transform.position = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _stickerClone.GetComponent<RectTransform>().anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ³¡");
    }
}
