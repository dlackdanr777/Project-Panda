using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnSticker : MonoBehaviour, IPointerDownHandler
{
    public Transform StickerZone;
    [SerializeField] private GameObject _stickerClone;

    public void OnPointerDown(PointerEventData eventData)
    {
        _stickerClone.GetComponent<Image>().sprite = transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        Instantiate(_stickerClone, StickerZone);
    }
}
