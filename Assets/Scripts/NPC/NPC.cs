using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Item
{
    public int Intimacy;
    public string Mbti;
    public string Cook;
    public string MessagePaper;
    public string SSId;

    public NPC(string id, string name, string description, string mbti, string cook, string map, string messagePaper, Sprite image) : base(id, name, description, 0, null, map, image)
    {
        Intimacy = 0;
        Mbti = mbti;
        Cook = cook;
        MessagePaper = messagePaper;
    }
}

[SerializeField]
public class NPCData
{
    public string Id;
    public int Intimacy;
    public string SSId;
    
    public NPCData(string id,  int intimacy, string ssId)
    {
        Id = id;
        Intimacy =intimacy;
        SSId = ssId;
    }
}
