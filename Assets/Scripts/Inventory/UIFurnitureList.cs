using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFurnitureList : UIList<InventoryItem, FurnitureType>
{
    [SerializeField] private GameObject _itemDrop;

    private int _currentItemIndex;
    private Database_Ssun _dataBase;

    //Test
    public Sprite Test;
    public Sprite Test2;

    private void Awake()
    {
        _dataBase = Database_Ssun.Instance;
        
        //Test
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, "2"); //player가 아이템 하나를 얻음
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, "5"); //player가 아이템 하나를 얻음
        GameManager.Instance.Player.Inventory[2].AddById(ItemField.Furniture, "5"); //player가 아이템 하나를 얻음

        for(int i=0;i<System.Enum.GetValues(typeof(FurnitureType)).Length - 1; i++)
        {

            _maxCount[i] = GameManager.Instance.Player.Inventory[2].MaxInventoryItem;
        }

        Init();
        UpdateList();

    }

    private void Start()
    {
        for(int i = 0; i < _itemDrop.transform.childCount; i++)
        {
            _itemDrop.transform.GetChild(i).GetComponent<DragDrop>().OnUseItem += UIInventoryList_OnUseItem;

        }
    }

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

    private void UIInventoryList_OnUseItem() //아이템 
    {
        UseItem();    
    }

    private void UseItem()
    {
        GameManager.Instance.Player.Inventory[2].RemoveByIndex(_currentItemIndex);
        UpdateListSlots();
    }

    private void UpdateList()
    {
        List<InventoryItem> furnitureInventory = GameManager.Instance.Player.Inventory[2].GetInventoryList();
        Inventory list = new Inventory();
        for (int i = 0; i < System.Enum.GetValues(typeof(FurnitureType)).Length-1; i++)
        {

            for (int j = 0; j < furnitureInventory.Count; j++) //가구 종류별로 나누기
            {
                if (GetFurniture(furnitureInventory[j].Id) == (FurnitureType)i) //가구의 종류가 현재 field 와 같으면 
                {
                    list.AddById(ItemField.Furniture, furnitureInventory[j].Id);

                }

            }
            _lists[i] = list.GetInventoryList();
        }

    }

    protected override void UpdateListSlots()
    {
        UpdateList();

        for (int j = 0; j < GameManager.Instance.Player.Inventory[2].ItemsCount; j++) //현재 player의 가구 인벤토리에 저장된 아이템 갯수
        {
            if (j < _lists[(int)_currentField].Count) //현재 가구 종류의 spawn 개수
            {
                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(true);
                _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();
                _spawnPoint[(int)_currentField].GetChild(j).GetComponent<DragDrop>().OnUseItem += UIInventoryList_OnUseItem;
            }
            else
            {

                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(false);


            }
        }

    }

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        //배치아이템 data bind
        DataBind.SetSpriteValue("ArrangeItemSprite", GameManager.Instance.Player.Inventory[(int)_currentField].GetInventoryList()[_currentItemIndex].Image);

    }
}