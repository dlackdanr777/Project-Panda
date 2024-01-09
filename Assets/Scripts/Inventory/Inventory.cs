using System;
using System.Collections.Generic;
using System.Diagnostics;

[Serializable]
public class Inventory
{
    public int MaxInventoryItem { get; private set; } = 30; //inventory 최대 저장 개수
    public int MaxInventoryItemCount { get; private set; } = 10;//한 아이템당 최대 개수

    private List<InventoryItem> _items = new List<InventoryItem>();
    
    public int ItemsCount => _items.Count;


    public List<InventoryItem> GetInventoryList()
    {
        return _items;
    }

    public void Add(Item item)
    {
        if(_items.Count > 0) 
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id.Equals(item.Id)) //id가 같은 아이템이 있다면
                {
                    if (_items[i].Count == MaxInventoryItemCount) //개수가 최대 개수와 같은지 확인
                    {
                        continue;
                    }
                    else
                    {
                        _items[i].Count++;
                        return;
                    }
                }
            }
        }

        //최대 개수를 가진 아이템만 존재한다면 새로운 인벤토리 아이템 생성
        InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Price, item.Rank, item.Map, item.Image);
        _items.Add(addItem); //새로운 인벤토리 생성
    }

    /// <summary>
    /// 플레이어의 인벤토리에 있는 아이템 id을 이용해서 add
    /// field는 종류 가 있음
    /// </summary>
    /// <param name="field"></param>
    /// field index ex) GatheringItem[] 0:bug, 1:fish, 2:fruit
    /// <param name="id"></param>
    public void AddById(InventoryItemField field, int fieldindex, string id)
    {
        List<GatheringItem> database = null;
        switch (field)
        {
            case InventoryItemField.GatheringItem:
                if (fieldindex == 0)
                {
                    database = DatabaseManager.Instance.GetBugItemList();
                }
                else if (fieldindex == 1)
                {
                    database = DatabaseManager.Instance.GetFishItemList();
                }
                else if (fieldindex == 2)
                {
                    database = DatabaseManager.Instance.GetFruitItemList();
                }
                break;
            case InventoryItemField.Cook:
                break;
            case InventoryItemField.Tool:
                break;
        }

        for (int i=0;i< database.Count; i++)
        {
            if (database[i].Id.Equals(id))
            {
                Add(database[i]);
                database[i].IsReceived = true;
            }
        }
    }

    public void Remove(Item item)
    {
        
        for (int i = 0; i < ItemsCount; i++)
        {
            if (_items[i].Id.Equals(item.Id)) //id가 같은 아이템이 있다면
            {
                _items[i].Count--;
                if (_items[i].Count == 0) //개수가 최대 개수와 같은지 확인
                {
                    _items.RemoveAt(i);
                    if(ItemsCount == 0)
                    {
                        _items.Clear();

                    }
                }
            }
        }
        
    }

    public void RemoveByIndex(int index)
    {
        _items[index].Count--;
        if (_items[index].Count == 0)//0보다 작으면 아이템 삭제
        {
            _items.RemoveAt(index);
            if (_items.Count == 0)
            {
                _items.Clear();
            }

        }
    }

    public void RemoveById(string id)
    {
        for (int i = 0; i < ItemsCount; i++)
        {
            if (_items[i].Id.Equals(id)) //id가 같은 아이템이 있다면
            {
                _items[i].Count--;
                if (_items[i].Count == 0) //0인지 확인
                {
                    _items.RemoveAt(i);
                    if (_items.Count == 0)
                    {
                        _items.Clear();

                    }
                }
            }
        }
    }
}