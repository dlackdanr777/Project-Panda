using System;
using UnityEngine;

public class GatheringItem : Item
{
    public DateTime StartTime;
    public DateTime EndTime;

    public GatheringItem(string id, string name, string description, int price, string rank, Sprite image, DateTime startTime, DateTime endTime) : base(id, name, description, price, rank, image)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

}
