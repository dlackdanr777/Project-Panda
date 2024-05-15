using System;
using UnityEngine;

[Serializable]
public class InventoryItem : Item
{
    public int Count;
    public bool AlarmCheck;
    public InventoryItem(string id, string name, string description, int count, int price, string rank, string map, Sprite image, bool alarmCheck) : base(id, name, description, price, rank, map, image)
    {
        Count = count;
        AlarmCheck = alarmCheck;
    }
}