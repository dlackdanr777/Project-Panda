using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItem
{
    public string ID;
    public string Name;
    public int HarvestTime; // 작물 수확량 증가 시간
    public int Yield;
    public int MaxYield;
    public string Description;
    public Sprite[] Image = new Sprite[3]; // 1단계, 2단계, 3단계 이미지

    public HarvestItem(string id, string name, int harvestTime, int yield, int maxYield, string description)
    {
        ID = id;
        Name = name;
        HarvestTime = harvestTime * 30; // 시간 단위: 분 // 베타에서만 시간 절반으로 설정
        Yield = yield;
        MaxYield = maxYield;
        Description = description;
    }
}
