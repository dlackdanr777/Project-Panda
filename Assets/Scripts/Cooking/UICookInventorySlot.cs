using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>요리 재료 아이템 슬롯</summary>

namespace Cooking
{
    public class UICookInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;

        [SerializeField] private Button _button;

        [SerializeField] private TextMeshProUGUI _amountText;

        private InventoryItem _item;

        public InventoryItem Item => _item;


        public void Init()
        {
            _item = null;
            _itemImage.sprite = null;
            _amountText.text = null;
            _itemImage.gameObject.SetActive(false);
            _amountText.gameObject.SetActive(false);
        }


        public void UpdateUI(InventoryItem item, UnityAction<InventoryItem> onButtonClicked = null)
        {
            _button.onClick.RemoveAllListeners();

            if(onButtonClicked != null )
                _button.onClick.AddListener(() => onButtonClicked(item));

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
    }
}

