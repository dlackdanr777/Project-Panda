using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItem
{
    public string ID;
    public string Name;
    public int HarvestTime; // �۹� ��Ȯ�� ���� �ð�
    public int Yield;
    public int MaxYield;
    public string Description;
    public Sprite[] Image = new Sprite[3]; // 1�ܰ�, 2�ܰ�, 3�ܰ� �̹���

    public HarvestItem(string id, string name, int harvestTime, int yield, int maxYield, string description)
    {
        ID = id;
        Name = name;
        HarvestTime = harvestTime * 30; // �ð� ����: �� // ��Ÿ������ �ð� �������� ����
        Yield = yield;
        MaxYield = maxYield;
        Description = description;
    }
}
