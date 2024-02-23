using Cooking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cooking
{
    public class UICookInventory : MonoBehaviour
    {
        [Header("인벤토리")]
        [SerializeField] private UICookInventoryCategory _bugInventory;

        [SerializeField] private UICookInventoryCategory _fishInventory;

        [SerializeField] private UICookInventoryCategory _fruitInventory;

        [Space]
        [Header("인벤토리 버튼")]
        [SerializeField] private Toggle _bugButton;

        [SerializeField] private Toggle _fishButton;

        [SerializeField] private Toggle _fruitButton;

        [Space]
        [Header("프리팹")]
        
        [SerializeField] private UICookInventorySlot _slotPrefab;


        private Inventory[] _inventorys;


        public void Init(UnityAction<InventoryItem> onButtonClicked)
        {
            _inventorys = GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem);
            _bugInventory.Init(_inventorys[(int)GatheringItemType.Bug], _slotPrefab);
            _fishInventory.Init(_inventorys[(int)GatheringItemType.Fish], _slotPrefab);
            _fruitInventory.Init(_inventorys[(int)GatheringItemType.Fruit], _slotPrefab);

            _bugButton.onValueChanged.AddListener(OnBugButtonClicked);
            _fishButton.onValueChanged.AddListener(OnFishButtonClicked);
            _fruitButton.onValueChanged.AddListener(OnFruitButtonClicked);

            UpdateUI(onButtonClicked);
        }


        public void UpdateUI(UnityAction<InventoryItem> onButtonClicked)
        {
            _bugInventory.UpdateUI(onButtonClicked);
            _fishInventory.UpdateUI(onButtonClicked);
            _fruitInventory.UpdateUI(onButtonClicked);
        }


        private void OnBugButtonClicked(bool isOn)
        {
            _bugInventory.SetActive(isOn);
        }


        private void OnFishButtonClicked(bool isOn)
        {
            _fishInventory.SetActive(isOn);
        }


        private void OnFruitButtonClicked(bool isOn)
        {
            _fruitInventory.SetActive(isOn);
        }
    }
}


