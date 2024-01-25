using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[Serializable]
public class Inventory
{
    public int MaxInventoryItem { get; private set; } = 30; //inventory 최대 저장 개수
    public int MaxInventoryItemCount { get; private set; } = 10;//한 아이템당 최대 개수
    public int ItemsCount => _items.Count;

    private List<InventoryItem> _items = new List<InventoryItem>();

    public List<InventoryItem> GetInventoryList()
    {
        return _items;
    }

    private void Add(Item item, InventoryItemField field)
    {
        if(_items.Count > 0)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id.Equals(item.Id)) //id가 같은 아이템이 있다면
                {
                    if(field == InventoryItemField.Tool) //도구는 한 개만 얻을 수 있음
                    {
                        return;
                    }
                    else
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
    public void AddById(InventoryItemField field, string id)
    {
        string startId = id.Substring(0, 3);
        switch (field)
        {
            case InventoryItemField.GatheringItem:
                List<GatheringItem> gatheringDatabase = null;
                if (startId.Equals("IBG"))
                {
                    gatheringDatabase = DatabaseManager.Instance.GetBugItemList();
                }
                else if (startId.Equals("IFI"))
                {
                    gatheringDatabase = DatabaseManager.Instance.GetFishItemList();
                }
                else if (startId.Equals("IFR"))
                {
                    gatheringDatabase = DatabaseManager.Instance.GetFruitItemList();
                }
                for (int i = 0; i < gatheringDatabase.Count; i++)
                {
                    if (gatheringDatabase[i].Id.Equals(id))
                    {
                        Add(gatheringDatabase[i], field);
                        gatheringDatabase[i].IsReceived = true;
                    }
                }
                break;
            case InventoryItemField.Cook:
                break;
            case InventoryItemField.Tool:
                List<ToolItem> toolDatabase = null;
                if(startId.Equals("ITG"))
                {
                    toolDatabase = DatabaseManager.Instance.GetGatheringToolItemList();
                }
                for (int i = 0; i < toolDatabase.Count; i++)
                {
                    if (toolDatabase[i].Id.Equals(id))
                    {
                        Add(toolDatabase[i], field);
                        toolDatabase[i].IsReceived = true;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 수량만큼 인벤토리에 더함
    /// </summary>
    /// <param name="field"></param>
    /// <param name="fieldIndex"></param>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void AddById(InventoryItemField field, string id, int count)
    {
        for(int i = 0; i < count; i++)
        {
            AddById(field, id);
        }
    }

    /// <summary>
    /// Item으로 인벤토리에 더함
    /// </summary>
    /// <param name="item"></param>
    /// <param name="field"></param>
    public void AddItem(Item item, InventoryItemField field)
    {
        AddById(field, item.Id);
    }

    /// <summary>
    /// Item으로 수량만큼 인벤토리에 더함
    /// </summary>
    /// <param name="item"></param>
    /// <param name="field"></param>
    /// <param name="count"></param>
    public void AddItem(Item item, InventoryItemField field, int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddById(field, item.Id);
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

    /// <summary>
    /// id로 아이템 제거
    /// </summary>
    /// <param name="id"></param>
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

    /// <summary>
    /// id로 수량만큼 아이템 제거
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public bool RemoveById(string id, int count)
    {
        for (int i = 0; i < ItemsCount; i++)
        {
            if (_items[i].Id.Equals(id)) //id가 같은 아이템이 있다면
            {
                if (_items[i].Count >= count)
                {
                    _items[i].Count -= count;
                    if (_items[i].Count == 0) //0인지 확인
                    {
                        _items.RemoveAt(i);
                        if (_items.Count == 0)
                        {
                            _items.Clear();

                        }
                    }
                    return true;
                }
            }
        }
        return false;
    }
}