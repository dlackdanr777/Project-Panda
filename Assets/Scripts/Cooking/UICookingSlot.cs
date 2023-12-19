using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UICookingSlot : MonoBehaviour
{
    
    [SerializeField] private Image _itemImage;

    [SerializeField] private TextMeshProUGUI _amountText;

    private Item _item;
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
            return;
        }
        _item = item;
        _itemImage.sprite = item.Image;
        _amountText.text = item.Count.ToString();
    }
}
