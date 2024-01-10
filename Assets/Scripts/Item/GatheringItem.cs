using System;
using UnityEngine;

public class GatheringItem : Item
{
    public string Rank;
    public string Time;
    public string Season;

    public GatheringItem(string id, string name, string description, int price, string rank, string map, Sprite image, string time, string season) : base(id, name, description, price, rank, map, image)
    {
        Rank = rank;
        Time = time;
        Season = season;
    }

}
