using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string Id;
    public string Name;
    public string Description;
    public string Rank;
    public int Price;
    public string Map;
    public Sprite Image;
    public bool IsReceived;

    public Item(string id, string name, string description, int price, string rank, string map, Sprite image)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Rank = rank;
        Map = map;
        Image = image;
    }
}