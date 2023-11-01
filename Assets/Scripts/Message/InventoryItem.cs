using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public int Id;
    public string Name;
    public string Description;
    public Sprite Image;

    public InventoryItem(int id, string name, string description, Sprite image)
    {
        Id = id;
        Name = name;
        Description = description;
        Image = image;
    }
}
