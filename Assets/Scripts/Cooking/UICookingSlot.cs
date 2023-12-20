using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UICookingSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [SerializeField] private Image _itemImage;

    [SerializeField] private TextMeshProUGUI _amountText;

    private InventoryItem _item;
    public void Init()
    {

    }

    public void UpdateUI(InventoryItem item)
    {
        if (item == null)
        {
            _item = null;
            _itemImage.sprite = null;
            _amountText.text = null;
            _itemImage.gameObject.SetActive(false);
            _amountText.gameObject.SetActive(false);
            return;
        }

        _itemImage.gameObject.SetActive(true);
        _amountText.gameObject.SetActive(true);
        _item = item;
        _itemImage.sprite = item.Image;
        _amountText.text = item.Count.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData) //마우스 드래그가 시작 됬을 때 실행되는 함수
    {
        if (_item == null)
            return;

        UICookingDragSlot.Instance.StartDrag(_item);
        UICookingDragSlot.Instance.transform.position = eventData.position;

    }

    public void OnDrag(PointerEventData eventData)
    {

        if (_item == null)
            return;

            UICookingDragSlot.Instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UICookingDragSlot.Instance.EndDrag();
    }
}
