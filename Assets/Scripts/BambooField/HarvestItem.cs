using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItem
{
    public int ID;
    public string Name;
    public float HarvestTime; // 작물 성장 시간
    public float Yield;
    public float MaxYield;
    public string Description;
    public Sprite[] Image = new Sprite[3]; // 1단계, 2단계, 3단계 이미지

    public HarvestItem(int iD, string name, float harvestTime, float yield, float maxYield, string description)
    {
        ID = iD;
        Name = name;
        HarvestTime = harvestTime;
        Yield = yield;
        MaxYield = maxYield;
        Description = description;
    }
}
