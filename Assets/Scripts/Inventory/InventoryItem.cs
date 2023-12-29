using System;
using UnityEngine;

[Serializable]
public class InventoryItem : Item
{
    public InventoryItemField ItemField;
    public int Count;

    public InventoryItem(string id, string name, string description, int price, InventoryItemField itemField, Sprite image) :base(id, name, description, price, image)
    {
        ItemField = itemField;
        Count = 1;
    }
}