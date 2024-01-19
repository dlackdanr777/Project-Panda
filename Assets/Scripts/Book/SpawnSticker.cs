using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnSticker : MonoBehaviour, IPointerDownHandler
{
    public Transform StickerZone;
    public GameObject StickerClone;

    public void OnPointerDown(PointerEventData eventData)
    {
        StickerClone.GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        StickerClone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        Instantiate(StickerClone, StickerZone);
    }
}
