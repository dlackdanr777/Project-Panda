using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFurnitureList : UIList<InventoryItem, FurnitureType>
{
    [SerializeField] private Transform _itemSlot;

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

        UpdateList();
        Init();

    }

    private void Start()
    {

        _itemSlot.GetComponent<DropZone>().OnUseItem += ItemSlot_OnUseItem;
        _itemSlot.GetComponent<DropZone>().OnPutInItem += ItemSlot_OnPutInItem;

        
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

    private void ItemSlot_OnUseItem(string id) //아이템 
    {
        GameManager.Instance.Player.Inventory[2].RemoveById(id);
        UpdateListSlots();
        Debug.Log("확인");
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
            Inventory list = new Inventory();

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

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

    }
}