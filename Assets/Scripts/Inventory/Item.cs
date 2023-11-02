using System;
using UnityEngine;

[Serializable]
public class Item
{
    public int Id;
    public string Name;
    public string Description;
    public Sprite Image;

    public Item(int id, string name, string description, Sprite image)
    {
        Id = id;
        Name = name;
        Description = description;
        Image = image;
    }
}
