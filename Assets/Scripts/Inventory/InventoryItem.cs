using System;
using UnityEngine;

[Serializable]
public class InventoryItem : Item
{
    public int Count;

    public InventoryItem(string id, string name, string description, Sprite image) :base(id, name, description, image)
    {
        Count = 1;
    }
}
