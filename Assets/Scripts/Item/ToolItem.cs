using System;
using UnityEngine;

public class ToolItem : Item
{
    public int GatheringPercentage;
    public int StoryStep;

    public ToolItem(string id, string name, string description, int price, string map, Sprite image, int gatheringPercentage, int storyStep) : base(id, name, description, price, null, map, image)
    {
        GatheringPercentage = gatheringPercentage;
        StoryStep = storyStep;
    }

}
