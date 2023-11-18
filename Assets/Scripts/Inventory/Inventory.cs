using System;
using System.Collections.Generic;

[Serializable]
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
        if(Items.Count > 0) 
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id.Equals(item.Id)) //id�� ���� �������� �ִٸ�
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
        }

        //�ִ� ������ ���� �����۸� �����Ѵٸ� ���ο� �κ��丮 ������ ����
        InventoryItem addItem = new InventoryItem(item.Id, item.Name, item.Description, item.Price, item.Image);
        Items.Add(addItem); //���ο� �κ��丮 ����
    }

    /// <summary>
    /// �÷��̾��� �κ��丮�� id�� �̿��ؼ� add
    /// field�� ���� Field.Toy, Field.Snack�� ����
    /// </summary>
    /// <param name="field"></param>
    /// <param name="id"></param>
    public void AddById(Field field, string id)
    {
        List<Item> database = Database_Ssun.Instance.ItemList[(int)field];
        int listCount = Database_Ssun.Instance.ItemCount[(int)field];

        for (int i=0;i< listCount; i++)
        {
            if (database[i].Id.Equals(id))
            {
                Add(database[i]);
                database[i].IsReceived = true;
            }
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
