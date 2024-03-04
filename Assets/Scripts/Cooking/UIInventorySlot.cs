using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

/// <summary>인벤토리 아이템 슬롯</summary>
public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    [SerializeField] private Button _button;

    [SerializeField] private TextMeshProUGUI _amountText;

    private InventoryItem _item;

    public InventoryItem Item => _item;


    public void Init(UnityAction<InventoryItem> onButtonClicked = null)
    {
        _item = null;
        _itemImage.sprite = null;
        _amountText.text = null;
        _itemImage.gameObject.SetActive(false);
        _amountText.gameObject.SetActive(false);

        if (onButtonClicked != null)
            _button.onClick.AddListener(() => onButtonClicked(_item));
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
}


