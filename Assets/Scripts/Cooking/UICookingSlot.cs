using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICookingSlot : MonoBehaviour//IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [SerializeField] private Image _itemImage;

    [SerializeField] private Button _button;

    [SerializeField] private TextMeshProUGUI _amountText;

    private UICooking _uiCooking;

    private InventoryItem _item;
    public void Init(UICooking uiCooking)
    {
        _uiCooking = uiCooking;
        _button.onClick.AddListener(OnButtonClicked);
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

    public void OnButtonClicked()
    {
        _uiCooking.UICookingCenterSlot.ChoiceItem(_item);
    }

  /*  public void OnBeginDrag(PointerEventData eventData) //마우스 드래그가 시작 됬을 때 실행되는 함수
    {
        if (_item == null)
            return;

        _uiCooking.UICookingDragSlot.StartDrag(_item);
        _uiCooking.UICookingDragSlot.transform.position = eventData.position;

    }

    public void OnDrag(PointerEventData eventData)
    {

        if (_item == null)
            return;

        _uiCooking.UICookingDragSlot.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _uiCooking.UICookingDragSlot.EndDrag();
    }*/
}
