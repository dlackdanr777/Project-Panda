using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Id;
    public string Name;
    public string Description;
    public int Price;
    public ItemField ItemField;
    public Sprite Image;
    public bool IsReceived;

    public Item(string id, string name, string description, int price, ItemField itemField, Sprite image)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        ItemField = itemField;  
        Image = image;
    }
}