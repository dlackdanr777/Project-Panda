using System;
using System.Collections.Generic;

[Serializable]
public class Inventory
{
    public int MaxInventoryItem { get; private set; } = 30; //inventory 최대 저장 개수
    public int MaxInventoryItemCount { get; private set; } = 10;//한 아이템당 최대 개수

    public List<InventoryItem> Items = new List<InventoryItem>();
    
    public int ItemsCount => Items.Count;


    public List<InventoryItem> GetInventoryList()
    {
        return Items;
    }

    public void Add(Item item)
    {
        if(Items.Count > 0) 
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id.Equals(item.Id)) //id가 같은 아이템이 있다면
                {
                    if (Items[i].Count == MaxInventoryItemCount) //개수가 최대 개수와 같은지 확인
                    {
                        continue;
                    }
                    else
                    {
                        Items[i].Count++;
                        return;
                    }
                }
            }
        }

        //최대 개수를 가진 아이템만 존재한다면 새로운 인벤토리 아이템 생성
        InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Price, item.Rank, item.Image);
        Items.Add(addItem); //새로운 인벤토리 생성
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
        List<Item> database = null;
        switch (field)
        {
            case InventoryItemField.GatheringItem:
                if (fieldindex == 0)
                {
                    database = DatabaseManager.Instance.ItemDatabase.ItemBugList;
                }
                else if (fieldindex == 1)
                {
                    database = DatabaseManager.Instance.ItemDatabase.ItemFishList;
                }
                else if (fieldindex == 2)
                {
                    database = DatabaseManager.Instance.ItemDatabase.ItemFruitList;
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
            if (Items[i].Id.Equals(item.Id)) //id가 같은 아이템이 있다면
            {
                Items[i].Count--;
                if (Items[i].Count == 0) //개수가 최대 개수와 같은지 확인
                {
                    Items.RemoveAt(i);
                    if(ItemsCount == 0)
                    {
                        Items.Clear();

                    }
                }
            }
        }
        
    }

    public void RemoveByIndex(int index)
    {
        Items[index].Count--;
        if (Items[index].Count == 0)//0보다 작으면 아이템 삭제
        {
            Items.RemoveAt(index);
            if (Items.Count == 0)
            {
                Items.Clear();
            }

        }
    }

    public void RemoveById(string id)
    {
        for (int i = 0; i < ItemsCount; i++)
        {
            if (Items[i].Id.Equals(id)) //id가 같은 아이템이 있다면
            {
                Items[i].Count--;
                if (Items[i].Count == 0) //0인지 확인
                {
                    Items.RemoveAt(i);
                    if (Items.Count == 0)
                    {
                        Items.Clear();

                    }
                }
            }
        }
    }
}