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
    public EChallengesKategorie Kategorie;
    public string Type;
    public int Count;
    public bool IsSuccess;

    public ChallengesData(string id, string name, string description, int bambooCount, string item, EChallengesKategorie kategorie, string type, int count)
    {
        Id = id;
        Name = name;
        Description = description;
        BambooCount = bambooCount;
        Item = item;
        Kategorie = kategorie;
        Type = type;
        Count = count;
    }
}
