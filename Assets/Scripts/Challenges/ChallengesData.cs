using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengesData
{
    public string Id;
    public string Name;
    public string Description;
    public int BambooCount;
    public string Item;

    public ChallengesData(string id, string name, string description, int bambooCount, string item)
    {
        Id = id;
        Name = name;
        Description = description;
        BambooCount = bambooCount;
        Item = item;
    }
}
