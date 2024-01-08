using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class UICookingDragSlot : MonoBehaviour
{
    private UICooking _uiCooking;

    private InventoryItem _tempItem;

    private Image _image;

    public void Init(UICooking uiCooking)
    {
        _uiCooking = uiCooking;
        _image = GetComponent<Image>();
        gameObject.SetActive(false);

    }
    private void Awake()
    {

    }


    public void StartDrag(InventoryItem item)
    {
        gameObject.SetActive(true);
        _tempItem = item;
        _image.sprite = _tempItem.Image;
    }

    public void EndDrag()
    {
        _tempItem = null;
        gameObject.SetActive(false);
    }

    /// <summary> null이면 true 아닐경우 false 반환</summary>
    public InventoryItem GetItem()
    {
        return _tempItem;
    }
}
