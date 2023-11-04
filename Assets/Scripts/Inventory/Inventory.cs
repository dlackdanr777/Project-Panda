using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    public int MaxInventoryItem { get; private set; } = 30; //inventory �ִ� ���� ����
    public int MaxInventoryItemCount { get; private set; } = 10;//�� �����۴� �ִ� ����

    public List<InventoryItem> Items = new List<InventoryItem>();
    
    public int ItemsCount => Items.Count;
    
    public List<InventoryItem> GetInventoryList()
    {
        return Items;
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
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == item.Id) //id�� ���� �������� �ִٸ�
                {
                    if (Items[i].Count == MaxInventoryItemCount) //������ �ִ� ������ ������ Ȯ��
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

            //�ִ� ������ ���� �����۸� �����Ѵٸ� ���ο� �κ��丮 ������ ����
            InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Image);
            Items.Add(addItem); //���ο� �κ��丮 ����

        }
    }

    public void RemoveByIndex(int index)
    {
        Items[index].Count--;
        if (Items[index].Count == 0)//0���� ������ ������ ����
        {
            Items.RemoveAt(index);
            if(Items.Count == 0)
            {
                Items.Clear();
            }
        }
    }
}
