using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDatabase 
{
    public List<NPC> NpcList = new List<NPC>();
    private List<Dictionary<string, object>> _dataNPC;

    public void Register()
    {
        _dataNPC = CSVReader.Read("NPC");

        for(int i=0;i<_dataNPC.Count;i++)
        {
            NpcList.Add(new NPC(
                _dataNPC[i]["ID"].ToString(),
                _dataNPC[i]["이름"].ToString(),
                _dataNPC[i]["설명"].ToString(),
                _dataNPC[i]["MBTI"].ToString(),
                _dataNPC[i]["좋아하는 요리"].ToString(),
                _dataNPC[i]["맵 ID"].ToString(),
                _dataNPC[i]["편지지 ID"].ToString()));
        }
    }
}
