using Cooking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cooking
{
    public class UICookInventory : MonoBehaviour
    {
        [SerializeField] private UICookInventoryCategory _bugInventory;

        [SerializeField] private UICookInventoryCategory _fishInventory;

        [SerializeField] private UICookInventoryCategory _fruitInventory;

        [Space]
        [SerializeField] private UICookInventorySlot _slotPrefab;


        private Inventory[] _inventorys;


        public void Init(UnityAction<InventoryItem> onButtonClicked)
        {
            _inventorys = GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem);
            _bugInventory.Init(_inventorys[(int)GatheringItemType.Bug], _slotPrefab);
            _fishInventory.Init(_inventorys[(int)GatheringItemType.Fish], _slotPrefab);
            _fruitInventory.Init(_inventorys[(int)GatheringItemType.Fruit], _slotPrefab);

            UpdateUI(onButtonClicked);
        }


        public void UpdateUI(UnityAction<InventoryItem> onButtonClicked)
        {
            _bugInventory.UpdateUI(onButtonClicked);
            _fishInventory.UpdateUI(onButtonClicked);
            _fruitInventory.UpdateUI(onButtonClicked);
        }
    }
}


