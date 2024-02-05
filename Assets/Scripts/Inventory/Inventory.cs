using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/// <summary>인벤토리 아이템 추가 함수의 타입</summary>
public enum ItemAddEventType
{
    /// <summary>이벤트 없음</summary>
    None,

    /// <summary>아이템 획득 카운트 증가</summary>
    AddChallengesCount
}

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

    private void Add(Item item, InventoryItemField field, int count)
    {
        int remainCount = count; // 남은 아이템 개수

        // 이미 존재하는 아이템인지 확인
        foreach (var existingItem in _items)
        {
            if (existingItem.Id.Equals(item.Id))
            {
                // 도구는 한 개만 얻을 수 있음
                if (field == InventoryItemField.Tool)
                {
                    return;
                }

                // 아이템 개수가 최대 개수에 도달한 경우 다음 아이템으로 이동
                if (existingItem.Count == MaxInventoryItemCount)
                {
                    continue;
                }

                // 남은 아이템 수를 현재 인벤토리에 추가
                int spaceLeft = MaxInventoryItemCount - existingItem.Count;
                int addToInventory = Math.Min(spaceLeft, remainCount);
                existingItem.Count += addToInventory;
                remainCount -= addToInventory;

                // 모든 아이템을 추가했으면 종료
                if (remainCount == 0)
                {
                    return;
                }
            }
        }


        // 남은 아이템을 인벤토리에 추가
        while (remainCount > 0)
        {
            int addToInventory = Math.Min(MaxInventoryItemCount, remainCount);
            InventoryItem newItem = new InventoryItem(item.Id, item.Name, item.Description, addToInventory, item.Price, item.Rank, item.Map, item.Image);
            _items.Add(newItem);
            remainCount -= addToInventory;
            UnityEngine.Debug.Log(remainCount);
        }

    }

    /// <summary>
    /// 플레이어의 인벤토리에 있는 아이템 id을 이용해서 add
    /// field는 종류 가 있음
    /// </summary>
    /// <param name="field"></param>
    /// field index ex) GatheringItem[] 0:bug, 1:fish, 2:fruit
    /// <param name="id"></param>
    public void AddById(InventoryItemField field, string id, int count, ItemAddEventType type = ItemAddEventType.AddChallengesCount)
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
                        Add(gatheringDatabase[i], field, count);
                        if (gatheringDatabase[i].IsReceived != true)
                        {
                            gatheringDatabase[i].IsReceived = true;

                            if(type == ItemAddEventType.AddChallengesCount)
                                DatabaseManager.Instance.Challenges.UnlockingBook(startId); // 도전과제 달성 체크
                        }
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
                        Add(toolDatabase[i], field, count);
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
    public void AddById(InventoryItemField field, string id)
    {
        AddById(field, id, 1);
    }

    /// <summary>
    /// Item으로 인벤토리에 더함
    /// </summary>
    /// <param name="item"></param>
    /// <param name="field"></param>
    public void AddItem(InventoryItemField field, Item item)
    {
        AddById(field, item.Id, 1);
    }

    /// <summary>
    /// Item으로 수량만큼 인벤토리에 더함
    /// </summary>
    /// <param name="item"></param>
    /// <param name="field"></param>
    /// <param name="count"></param>
    public void AddItem(InventoryItemField field, Item item, int count)
    {
        AddById(field, item.Id, count);
    }

    public void Remove(Item item)
    {
        RemoveById(item.Id,1);   
    }

    /// <summary>
    /// id로 수량만큼 아이템 제거
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public bool RemoveById(string id, int count)
    {
        // 해당 아이템과 id가 같은 아이템들을 개수 순으로 정렬합니다.
        var items = _items.Where(item => item.Id.Equals(id)).OrderBy(item => item.Count).ToList();

        int totalItemCount = items.Sum(item => item.Count);

        // 제거하려는 아이템의 총 개수가 인벤토리에 있는 해당 아이템의 총 개수보다 많은지 확인합니다.
        if (count > totalItemCount)
        {
            return false;
        }

        int itemCountToRemove = count;

        // 아이템을 순회하면서 개수가 적은 순서대로 제거를 시도합니다.
        foreach (var item in items)
        {
            if (item.Count >= itemCountToRemove)
            {
                item.Count -= itemCountToRemove;
                if (item.Count == 0)
                {
                    _items.Remove(item);
                }
                return true; // 아이템을 성공적으로 제거하였음을 반환
            }
            else
            {
                _items.Remove(item);
                itemCountToRemove -= item.Count;
            }
        }

        return false; // 아이템을 제거하지 못했을 때
    }

    /// <summary>
    /// 아이디로 아이템 찾기
    /// </summary>
    /// <param name="id"></param>
    /// <returns>해당 아이템이 count이상 있으면 true, 없으면 false</returns>
    public bool FindItemById(string id, int count)
    {
        int amount = 0;
        for(int i=0;i<ItemsCount; i++)
        {
            if (_items[i].Id.Equals(id))
            {
                amount += _items[i].Count;
            }
        }
        if(amount >= count)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 스페셜 아이템이 있는지 확인
    /// </summary>
    /// <returns>count만큼 있으면 true, 없으면 false</returns>
    public bool FindSpecialItem(int count)
    {
        int amount = 0;
        for (int i = 0; i < ItemsCount; i++)
        {
            if (_items[i].Rank.Equals("스페셜"))
            {
                amount++;
            }
        }
        if(count >= amount)
        {
            return true;
        }
        return false;
    }
}