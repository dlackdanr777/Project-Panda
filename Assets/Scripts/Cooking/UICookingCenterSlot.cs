using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class UICookingCenterSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Button _cookButton;

    [SerializeField] private Image _itemImage;

    private UICooking _uiCooking;

    private InventoryItem _currentItem;

    public void Init(UICooking uiCooking)
    {
        _uiCooking = uiCooking;
        _cookButton.onClick.AddListener(OnCookButtonCilcked);
        _itemImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            _currentItem = null;
            _uiCooking.HideButtonImage.SetActive(true);
            _itemImage.gameObject.SetActive(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem item = _uiCooking.UICookingDragSlot.GetItem();

        Debug.Log("드롭함");

        if (item == null)
        {
            _currentItem = null;
            return;
        }

        Debug.Log("아이템 존재");
        _currentItem = item;
        _itemImage.sprite = item.Image;

        CheckItem();
    }

    private void OnCookButtonCilcked()
    {
        Debug.Log("버튼 눌림");
        _uiCooking.StartCooking(_uiCooking.CookingSystem.GetRecipe(_currentItem));
        CheckItem();
    }

    private void CheckItem()
    {
        if(_currentItem.Count <= 0)
        {
            _itemImage.gameObject.SetActive(false);
            _uiCooking.HideButtonImage.SetActive(true);
            return;
        }

        _itemImage.gameObject.SetActive(true);

        if (_uiCooking.CookingSystem.CheckRecipe(_currentItem))
        {
            _uiCooking.HideButtonImage.SetActive(false);

            return;
        }

        _uiCooking.HideButtonImage.SetActive(true);
    }


}
