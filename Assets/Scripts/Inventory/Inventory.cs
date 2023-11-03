using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    private int _maxInventoryItem; //inventory 최대 저장 개수
    private int _maxInventoryItemCount;//한 아이템당 최대 개수

    public List<InventoryItem> Items = new List<InventoryItem>();
    public int MaxInventoryItem
    {
        get { return _maxInventoryItem; }
        private set { _maxInventoryItem = 20; }
    }
    public int MaxInventoryItemCount
    { 
        get {return _maxInventoryItemCount; } 
        private set { _maxInventoryItemCount = 10; } 
    } 
    
    public void Add(Item item)
    {
        //처음 생성
        if(Items.Count == 0)
        {
            InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Image);
            Items.Add(addItem); //새로운 인벤토리 생성

        }
        else
        {
            for(int i=0;i<Items.Count;i++)
            {
                if (Items[i].Id == item.Id) //id가 같은 아이템이 있다면
                {
                    if (Items[i].Count > MaxInventoryItemCount) //개수가 최대 개수보다 많은지 확인
                    {
                        continue;
                    }
                    else
                    {
                        Items[i].Count++;
                    }
                }
                else //없다면 새로운 아이템 생성
                {
                    InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Image);
                    Items.Add(addItem); //새로운 인벤토리 생성
                }
            }

        }
    }

    public void RemoveByIndex(int index)
    {
        if (Items[index].Count > 0) //0보다 크면 count--
        {
            Items[index].Count--;
        }
        else //0보다 작거나 같으면 아이템 삭제
        {
            Items.RemoveAt(index);
        }
    }
}
