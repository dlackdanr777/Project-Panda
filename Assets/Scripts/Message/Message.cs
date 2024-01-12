using System;
using UnityEngine;

[Serializable]
public class Message
{
    public string Id;
    public string From;
    public string Content;
    public Item Gift;
    public string StoryStep;
    public string Time;
    public Sprite PaperImage;
    public bool IsCheck; //메시지 확인했는지
    public bool IsReceived; //선물 받았는지

    public Message(string id, string from, string content, Item gift, string storyStep, string time, Sprite paperImage)
    {
        Id = id;
        From = from;
        Content = content;
        Gift = gift;
        StoryStep = storyStep;
        Time = time;
        PaperImage = paperImage;
        IsCheck = false;
        IsReceived = false;
    }
}
