using System;
using UnityEngine;

[Serializable]
public class InventoryItem : Item
{
    public int Count;

    public InventoryItem(string id, string name, string description, int price, ItemField itemField, Sprite image) :base(id, name, description, price, itemField, image)
    {
        Count = 1;
    }
}
