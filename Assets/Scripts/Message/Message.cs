using System;
using UnityEngine;

[Serializable]
public class Message
{
    public string Id;
    public string To;
    public string From;
    public string Content;
    public bool IsSend;
    public Sprite GiftImage;
    public InventoryItem Gift;
}
