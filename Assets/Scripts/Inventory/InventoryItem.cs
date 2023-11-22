using System;
using UnityEngine;

[Serializable]
public class InventoryItem : Item
{
    public int Count;

    public InventoryItem(string id, string name, string description, int price, Sprite image) :base(id, name, description, price, image)
    {
        Count = 1;
    }
}
