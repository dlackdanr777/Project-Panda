using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC 
{
    public string Id;
    public string Name;
    public string Description;
    public string Mbti;
    public string Cook;
    public string Map;
    public string MessagePaper;

    public NPC(string id, string name, string description, string mbti, string cook, string map, string messagePaper)
    {
        Id = id;
        Name = name;
        Description = description;
        Mbti = mbti;
        Cook = cook;
        Map = map;
        MessagePaper = messagePaper;
    }
}
