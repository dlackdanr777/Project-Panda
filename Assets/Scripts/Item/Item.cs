using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Id;
    public string Name;
    public string Description;
    public int Price;
    public Sprite Image;
    public bool IsReceived;

    public Item(string id, string name, string description, int price, Sprite image)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Image = image;
    }
}
