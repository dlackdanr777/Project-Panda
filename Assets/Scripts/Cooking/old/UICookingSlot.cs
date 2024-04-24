using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>요리 재료 아이템 슬롯</summary>
public class UICookingSlot : MonoBehaviour
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
        _uiCooking.ChoiceItem(_item);
    }
}
