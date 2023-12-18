using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFurnitureList : UIList<InventoryItem, FurnitureType>
{
    [SerializeField] private Transform _itemSlot;
    [SerializeField] private GameObject _itempDropZone;

    private int _currentItemIndex;
    private ItemDatabase _dataBase;

    //Test
    public Sprite Test;
    public Sprite Test2;

    private void Awake()
    {
        _dataBase = DatabaseManager.Instance.ItemDatabase;

        //Test
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, "F2"); //player가 아이템 하나를 얻음
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, "F5"); //player가 아이템 하나를 얻음
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, "F5"); //player가 아이템 하나를 얻음

        for(int i=0;i<System.Enum.GetValues(typeof(FurnitureType)).Length - 1; i++)
        {

            _maxCount[i] = GameManager.Instance.Player.Inventory[2].MaxInventoryItem;
        }

        UpdateList();
        Init();

        DropZone.OnUseItem += ItemSlot_OnUseItem;
        DropZone.OnPutInItem += ItemSlot_OnPutInItem;

    }
    private void OnDestroy()
    {
        DropZone.OnUseItem -= ItemSlot_OnUseItem;
        DropZone.OnPutInItem -= ItemSlot_OnPutInItem;
    }

    //private void Start()
    //{

    //    _itemSlot.GetComponent<DropZone>().OnUseItem += ItemSlot_OnUseItem;
    //    _itemSlot.GetComponent<DropZone>().OnPutInItem += ItemSlot_OnPutInItem;

        
    //}
    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    //_itempDropZone.SetActive(true);
        
    //}

    //protected override void OnDisable()
    //{
    //    //_itempDropZone.SetActive(false);
    //}
    private FurnitureType GetFurniture(string id)
    {
        for(int i=0;i<_dataBase.FurnitureTypeList.Length;i++)
        {
            if (id.Equals(_dataBase.ItemList[2][i].Id)){ //이름이 같은지 검사 후 FurnitureType 반환
                return _dataBase.FurnitureTypeList[i];
            }
        }
        return FurnitureType.None;
    }

    private void ItemSlot_OnUseItem(string id) //아이템 
    {
        Debug.Log("OnUseItem");
        GameManager.Instance.Player.Inventory[2].RemoveById(id);
        UpdateListSlots();
    }

    private void ItemSlot_OnPutInItem(string id)
    {
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, id);
        UpdateListSlots();
    }

    //가구 인벤토리 받아와서 가구 종류별로 저장
    //인벤토리의 count 받아오지 않음
    private void UpdateList() 
    {
        List<InventoryItem> furnitureInventory = GameManager.Instance.Player.Inventory[2].GetInventoryList();
        for (int i = 0; i < System.Enum.GetValues(typeof(FurnitureType)).Length-1; i++)
        {
            List<InventoryItem> inventory = new List<InventoryItem>();

            for (int j = 0; j < furnitureInventory.Count; j++) //가구 종류별로 나누기
            {
                if (GetFurniture(furnitureInventory[j].Id) == (FurnitureType)i) //가구의 종류가 현재 field 와 같으면 
                {
                    inventory.Add(furnitureInventory[j]);

                }

            }
            _lists[i] = inventory;
        }

    }

    protected override void UpdateListSlots()
    {
        UpdateList();

        if (_currentField != FurnitureType.None)
        {
            for (int j = 0; j < _maxCount[(int)_currentField]; j++)
            {
                if (j < _lists[(int)_currentField].Count) //현재 가구 종류의 spawn 개수
                {
                    _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(true);
                    _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                    _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();
                    _spawnPoint[(int)_currentField].GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Id.ToString();
                }
                else
                {
                    _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(false);
                }
            }
        }        
    }

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

    }
}