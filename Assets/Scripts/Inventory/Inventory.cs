using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    private int _maxInventoryItem; //inventory �ִ� ���� ����
    private int _maxInventoryItemCount;//�� �����۴� �ִ� ����

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
        //ó�� ����
        if(Items.Count == 0)
        {
            InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Image);
            Items.Add(addItem); //���ο� �κ��丮 ����

        }
        else
        {
            for(int i=0;i<Items.Count;i++)
            {
                if (Items[i].Id == item.Id) //id�� ���� �������� �ִٸ�
                {
                    if (Items[i].Count > MaxInventoryItemCount) //������ �ִ� �������� ������ Ȯ��
                    {
                        continue;
                    }
                    else
                    {
                        Items[i].Count++;
                    }
                }
                else //���ٸ� ���ο� ������ ����
                {
                    InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Image);
                    Items.Add(addItem); //���ο� �κ��丮 ����
                }
            }

        }
    }

    public void RemoveByIndex(int index)
    {
        if (Items[index].Count > 0) //0���� ũ�� count--
        {
            Items[index].Count--;
        }
        else //0���� �۰ų� ������ ������ ����
        {
            Items.RemoveAt(index);
        }
    }
}
