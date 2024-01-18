using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Album 
{
    public string Id;
    public string Name;
    public string Description;
    public string StoryStep;
    public Sprite Image;
    public bool IsReceived;

    public Album(string id, string name, string description, string storyStep, Sprite image)
    {
        Id = id;
        Name = name;
        Description = description;
        StoryStep = storyStep;
        Image = image;
        IsReceived = false;
    }
}
