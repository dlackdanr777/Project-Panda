using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Cooking
{
    public class UIMaterialItemSlot : MonoBehaviour
    {

        [SerializeField] private Image _itemImage;

        [SerializeField] private Button _slotButton;

        private UISelect _uiSelect;

        private InventoryItem _currentItem;
        public InventoryItem CurrentItem => _currentItem;

        public void Init(UISelect uiSelect)
        {
            _uiSelect = uiSelect;
            _slotButton.onClick.AddListener(OnSlotButtonClicked);
            _itemImage.gameObject.SetActive(false);
        }


        private void OnSlotButtonClicked()
        {
            _currentItem = null;
            _itemImage.gameObject.SetActive(false);
        }


        public void CheckCurrentItem()
        {
            if (_currentItem == null)
                return;

            if (_currentItem.Count <= 0)
            {
                _currentItem = null;
                _itemImage.gameObject.SetActive(false);
            }
        }


        public void SetActiveSlot(bool value)
        {
            gameObject.SetActive(value);
        }


        public void ResetItem()
        {
            _currentItem = null;
            _itemImage.gameObject.SetActive(false);
        }


        public void ChoiceItem(InventoryItem item)
        {
            _currentItem = item;
            if (_currentItem == null)
            {
                _itemImage.gameObject.SetActive(false);
                return;
            }

            _itemImage.gameObject.SetActive(true);
            _currentItem = item;
            _itemImage.sprite = item.Image;
        }

    }
}


