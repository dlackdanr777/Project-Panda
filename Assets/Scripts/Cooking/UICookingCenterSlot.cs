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
        if (item == null)
        {
            _currentItem = item;
            return;
        }

        _currentItem = item;
        _itemImage.sprite = item.Image;
        _cookButton.onClick.RemoveAllListeners();
        _cookButton.onClick.AddListener(() =>
        {
            _uiCooking.CookingSystem.StartCooking(_currentItem);
            CheckItem();
        });

        CheckItem();
    }

    private void CheckItem()
    {
        if(_currentItem.Count <= 0)
        {
            _itemImage.gameObject.SetActive(false);
            _uiCooking.HideButtonImage.SetActive(true);
            _cookButton.onClick.RemoveAllListeners();
            return;
        }

        _itemImage.gameObject.SetActive(true);
        Debug.Log("실행합니다");

        if (_uiCooking.CookingSystem.CheckRecipe(_currentItem))
        {
            Debug.Log("레시피에 존재합니다.");


            _uiCooking.HideButtonImage.SetActive(false);

            return;
        }

        Debug.Log("레시피에 존재하지 않습니다.");
        _uiCooking.HideButtonImage.SetActive(true);
    }


}
