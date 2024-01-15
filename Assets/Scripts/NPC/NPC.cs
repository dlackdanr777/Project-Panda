using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Item
{
    public string Mbti;
    public string Cook;
    public string MessagePaper;

    public NPC(string id, string name, string description, string mbti, string cook, string map, string messagePaper, Sprite image) : base(id, name, description, 0, null, map, image)
    {
        Mbti = mbti;
        Cook = cook;
        MessagePaper = messagePaper;
    }
}
