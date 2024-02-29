using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIMainInventoryContoller : MonoBehaviour
{
    [Header("인벤토리")]
    [SerializeField] private UIInventoryCategory _bugInventoryCategory;

    [SerializeField] private UIInventoryCategory _fishInventoryCategory;

    [SerializeField] private UIInventoryCategory _fruitInventoryCategory;

    [SerializeField] private UIInventoryCategory _foodInventoryCategory;

    [SerializeField] private UIInventoryCategory _toolInventoryCategory;


    [Space]
    [Header("인벤토리 버튼")]
    [SerializeField] private Toggle _bugButton;

    [SerializeField] private Toggle _fishButton;

    [SerializeField] private Toggle _fruitButton;

    [SerializeField] private Toggle _foodButton;

    [SerializeField] private Toggle _toolButton;

    [Space]
    [Header("프리팹")]
    [SerializeField] private UIInventorySlot _slotPrefab;


    private Inventory[] _gatheringInventorys;
    private Inventory _foodInventory;
    private Inventory _toolInventory;

    public void Start()
    {
        Init(null);
    }


    public void Init(UnityAction<InventoryItem> onButtonClicked = null)
    {
        _gatheringInventorys = GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem);
        _foodInventory = GameManager.Instance.Player.GetItemInventory(InventoryItemField.Cook)[(int)CookItemType.CookFood];
        _toolInventory = GameManager.Instance.Player.GetItemInventory(InventoryItemField.Tool)[(int)ToolItemType.GatheringTool];

        _bugInventoryCategory.Init(_gatheringInventorys[(int)GatheringItemType.Bug], _slotPrefab, onButtonClicked);
        _fishInventoryCategory.Init(_gatheringInventorys[(int)GatheringItemType.Fish], _slotPrefab, onButtonClicked);
        _fruitInventoryCategory.Init(_gatheringInventorys[(int)GatheringItemType.Fruit], _slotPrefab, onButtonClicked);
        _foodInventoryCategory.Init(_foodInventory, _slotPrefab);
        _toolInventoryCategory.Init(_toolInventory, _slotPrefab);

        _bugButton.onValueChanged.AddListener(OnBugButtonClicked);
        _fishButton.onValueChanged.AddListener(OnFishButtonClicked);
        _fruitButton.onValueChanged.AddListener(OnFruitButtonClicked);
        _foodButton.onValueChanged.AddListener(OnFoodButtonClicked);
        _toolButton.onValueChanged.AddListener(OnToolButtonClicked);

        _gatheringInventorys[(int)GatheringItemType.Bug].OnAddHandler += UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Bug].OnRemoveHandler += UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fish].OnAddHandler += UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fish].OnRemoveHandler += UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fruit].OnAddHandler += UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fruit].OnRemoveHandler += UpdateUI;
        _foodInventory.OnAddHandler += UpdateUI;
        _foodInventory.OnRemoveHandler += UpdateUI;
        _toolInventory.OnAddHandler += UpdateUI;
        _toolInventory.OnRemoveHandler += UpdateUI;

        UpdateUI();
    }

    private void OnDestroy()
    {
        _gatheringInventorys[(int)GatheringItemType.Bug].OnAddHandler -= UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Bug].OnRemoveHandler -= UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fish].OnAddHandler -= UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fish].OnRemoveHandler -= UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fruit].OnAddHandler -= UpdateUI;
        _gatheringInventorys[(int)GatheringItemType.Fruit].OnRemoveHandler -= UpdateUI;
        _foodInventory.OnAddHandler -= UpdateUI;
        _foodInventory.OnRemoveHandler -= UpdateUI;
        _toolInventory.OnAddHandler -= UpdateUI;
        _toolInventory.OnRemoveHandler -= UpdateUI;
    }


    private void _foodInventory_OnAddHandler()
    {
        throw new NotImplementedException();
    }

    public void UpdateUI()
    {
        _bugInventoryCategory.UpdateUI();
        _fishInventoryCategory.UpdateUI();
        _fruitInventoryCategory.UpdateUI();
        _foodInventoryCategory.UpdateUI();
        _toolInventoryCategory.UpdateUI();
    }


    private void OnBugButtonClicked(bool isOn)
    {
        _bugInventoryCategory.SetActive(isOn);
    }


    private void OnFishButtonClicked(bool isOn)
    {
        _fishInventoryCategory.SetActive(isOn);
    }


    private void OnFruitButtonClicked(bool isOn)
    {
        _fruitInventoryCategory.SetActive(isOn);
    }


    private void OnFoodButtonClicked(bool isOn)
    {
        _foodInventoryCategory.SetActive(isOn);
    }


    private void OnToolButtonClicked(bool isOn)
    {
        _toolInventoryCategory.SetActive(isOn);
    }
}



