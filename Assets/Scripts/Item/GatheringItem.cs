using System;
using UnityEngine;

public class GatheringItem : Item
{
    public string Time;
    public string Season;

    public GatheringItem(string id, string name, string description, int price, string rank, Sprite image, string time, string season) : base(id, name, description, price, rank, image)
    {
        Time = time;
        Season = season;
    }

}
