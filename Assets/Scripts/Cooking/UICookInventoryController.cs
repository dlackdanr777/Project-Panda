
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cooking
{
    public class UICookInventoryController : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private UIInventoryCategory _bugInventory;
        [SerializeField] private UIInventoryCategory _fishInventory;
        [SerializeField] private UIInventoryCategory _fruitInventory;

        [Space]
        [Header("Toggles")]
        [SerializeField] private Toggle _bugButton;
        [SerializeField] private Toggle _fishButton;
        [SerializeField] private Toggle _fruitButton;

        [Space]
        [Header("Prefabs")]   
        [SerializeField] private UIInventorySlot _slotPrefab;


        private Inventory[] _inventorys;


        public void Init(UnityAction<InventoryItem> onButtonClicked = null)
        {
            LoadingSceneManager.OnLoadSceneHandler += LoadedSceneEvent;

            _inventorys = GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem);
            _bugInventory.Init(_inventorys[(int)GatheringItemType.Bug], _slotPrefab, onButtonClicked);
            _fishInventory.Init(_inventorys[(int)GatheringItemType.Fish], _slotPrefab, onButtonClicked);
            _fruitInventory.Init(_inventorys[(int)GatheringItemType.Fruit], _slotPrefab, onButtonClicked);

            _bugButton.onValueChanged.AddListener(OnBugButtonClicked);
            _fishButton.onValueChanged.AddListener(OnFishButtonClicked);
            _fruitButton.onValueChanged.AddListener(OnFruitButtonClicked);

            _inventorys[(int)GatheringItemType.Bug].OnAddHandler += UpdateUI;
            _inventorys[(int)GatheringItemType.Bug].OnRemoveHandler += UpdateUI;
            _inventorys[(int)GatheringItemType.Fish].OnAddHandler += UpdateUI;
            _inventorys[(int)GatheringItemType.Fish].OnRemoveHandler += UpdateUI;
            _inventorys[(int)GatheringItemType.Fruit].OnAddHandler += UpdateUI;
            _inventorys[(int)GatheringItemType.Fruit].OnRemoveHandler += UpdateUI;

            UpdateUI();
        }


        public void UpdateUI()
        {
            _bugInventory.UpdateUI();
            _fishInventory.UpdateUI();
            _fruitInventory.UpdateUI();
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


        private void LoadedSceneEvent()
        {
            _inventorys[(int)GatheringItemType.Bug].OnAddHandler -= UpdateUI;
            _inventorys[(int)GatheringItemType.Bug].OnRemoveHandler -= UpdateUI;
            _inventorys[(int)GatheringItemType.Fish].OnAddHandler -= UpdateUI;
            _inventorys[(int)GatheringItemType.Fish].OnRemoveHandler -= UpdateUI;
            _inventorys[(int)GatheringItemType.Fruit].OnAddHandler -= UpdateUI;
            _inventorys[(int)GatheringItemType.Fruit].OnRemoveHandler -= UpdateUI;

            LoadingSceneManager.OnLoadSceneHandler -= LoadedSceneEvent;
        }
    }
}


