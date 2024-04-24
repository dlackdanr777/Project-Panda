
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>인벤토리 카테고리 클래스</summary>
public class UIInventoryCategory : MonoBehaviour
{

    [SerializeField] private Transform _slotParent;

    [SerializeField] private GameObject _parent;

    private List<UIInventorySlot> _inventorySlotList = new List<UIInventorySlot>();

    private Inventory _inventory;


    public void Init(Inventory inventory, UIInventorySlot slotPrefab, UnityAction<InventoryItem> onButtonClicked = null)
    {
        _inventory = inventory;

        for (int i = 0, count = _inventory.MaxInventoryItem; i < count; i++)
        {
            UIInventorySlot slot = Instantiate(slotPrefab);
            slot.transform.parent = _slotParent;
            slot.transform.localScale = Vector3.one;
            slot.Init(onButtonClicked);
            slot.UpdateUI(null);

            _inventorySlotList.Add(slot);
        }
    }


    public void UpdateUI()
    {
        for (int i = 0, count = _inventory.ItemsCount; i < count; i++)
        {
            InventoryItem item = _inventory.FindItemByIndex(i);
            _inventorySlotList[i].UpdateUI(item);
        }

        for (int i = _inventory.ItemsCount, count = _inventory.MaxInventoryItem; i < count; i++)
        {
            _inventorySlotList[i].UpdateUI(null);
        }
    }


    public void SetActive(bool value)
    {
        _parent.SetActive(value);
    }
}

