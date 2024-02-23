using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Cooking
{
    public class UIMaterialItemSlot : MonoBehaviour
    {

        [SerializeField] private Image _itemImage;

        [SerializeField] private Button _slotButton;

        [SerializeField] private Image _selectImage;

        private InventoryItem _currentItem;
        public InventoryItem CurrentItem => _currentItem;

        public void Init(int index, UnityAction<int> onButtonClicked = null)
        {
            _slotButton.onClick.AddListener(() => onButtonClicked(index));
            _itemImage.gameObject.SetActive(false);
            _selectImage.gameObject.SetActive(false);
            DisableSelectSlot();

        }


        public void EnableSelectSlot()
        {
            _selectImage.gameObject.SetActive(true);
        }


        public void DisableSelectSlot()
        {
            _selectImage.gameObject.SetActive(false);
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
            if (_currentItem == null || _currentItem.Count <= 0)
            {
                _itemImage.gameObject.SetActive(false);
                return;
            }

            _itemImage.gameObject.SetActive(true);
            _currentItem = item;
            _itemImage.sprite = item.Image;

            CheckCurrentItem();
        }

        private void CheckCurrentItem()
        {
            if (_currentItem == null)
                return;

            if (_currentItem.Count <= 0)
            {
                _currentItem = null;
                _itemImage.gameObject.SetActive(false);
            }
        }
    }
}


