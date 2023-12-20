using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UICookingCenterSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _itemImage;

    private InventoryItem _currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem item = UICookingDragSlot.Instance.GetItem();
        if (item == null)
            return;

        _currentItem = item;

        _itemImage.sprite = item.Image;
    }
}
