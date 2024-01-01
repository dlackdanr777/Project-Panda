using System;
using UnityEngine;

[Serializable]
public class InventoryItem : Item
{
    public int Count;

    public InventoryItem(string id, string name, string description, int price, string rank, Sprite image) : base(id, name, description, price, rank, image)
    {
        Count = 1;
    }
}