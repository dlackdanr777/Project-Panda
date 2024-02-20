using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

namespace Cooking
{
    public class UICookInventoryCategory : MonoBehaviour
    {

        [SerializeField] private Transform _slotParent;

        [SerializeField] private GameObject _parent;

        private List<UICookInventorySlot>_inventorySlotList = new List<UICookInventorySlot>();

        private Inventory _inventory;


        public void Init(Inventory inventory, UICookInventorySlot slotPrefab)
        {
            _inventory = inventory;

            for(int i = 0, count = _inventory.MaxInventoryItem; i < count; i++)
            {
                UICookInventorySlot slot = Instantiate(slotPrefab);
                slot.transform.parent = _slotParent;
                slot.transform.localScale = Vector3.one;
                slot.Init();
                slot.UpdateUI(null);

                _inventorySlotList.Add(slot);
            }
        }


        public void UpdateUI(UnityAction<InventoryItem> onButtonClicked)
        {

            for (int i = 0, count = _inventory.ItemsCount; i < count; i++)
            {
                InventoryItem item = _inventory.FindItemByIndex(i);
                _inventorySlotList[i].UpdateUI(item, onButtonClicked);
            }

            for (int i = _inventory.ItemsCount, count = _inventory.MaxInventoryItem; i < count; i++)
            {
                _inventorySlotList[i].UpdateUI(null, onButtonClicked);
            }
        }


        public void SetActive(bool value)
        {
            _parent.SetActive(value);
        }
    }
}

