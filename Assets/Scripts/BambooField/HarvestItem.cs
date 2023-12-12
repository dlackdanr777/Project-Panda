using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItem
{
    public int ID;
    public string Name;
    public float HarvestTime; // �۹� ���� �ð�
    public float Yield;
    public float MaxYield;
    public string Description;
    public Sprite[] Image = new Sprite[3]; // 1�ܰ�, 2�ܰ�, 3�ܰ� �̹���

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
