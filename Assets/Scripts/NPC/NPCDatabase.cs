using BackEnd.MultiCharacter;
using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserInfo;
using Muks.BackEnd;
using System;

public enum NPCType
{
    None = -1,
    Panda
}

public class NPCDatabase 
{
    public List<NPC> NpcList = new List<NPC>();
    private List<Dictionary<string, object>> _dataNPC;

    //Image
    public ItemSpriteDatabase[] NPCSpriteArray = new ItemSpriteDatabase[System.Enum.GetValues(typeof(NPCType)).Length - 1];
    public Dictionary<string, Sprite>[] _npcSpriteDic = new Dictionary<string, Sprite>[System.Enum.GetValues(typeof(NPCType)).Length - 1];

    private string _chartID => "105699";

    public void Register()
    {
        //Image
        for (int i = 0; i < _npcSpriteDic.Length; i++)
        {
            _npcSpriteDic[i] = new Dictionary<string, Sprite>();
            for (int j = 0; j < NPCSpriteArray[i].ItemSprites.Length; j++)
            {
                _npcSpriteDic[i].Add(NPCSpriteArray[i].ItemSprites[j].Id, NPCSpriteArray[i].ItemSprites[j].Image);
            }
        }

        NPCParserByLocal();
    }


    public void LoadData()
    {
        BackendManager.Instance.GetChartData(_chartID, 10, NPCParserByServer);
    }



    #region LoadNPC
    /// <summary>서버에서 NPC 정보를 받아와 List에 넣는 함수</summary>
    public void NPCParserByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        NpcList.Clear();
        for (int i = 0; i < json.Count; i++)
        {
            NpcList.Add(new NPC(
                json[i]["NpcID"].ToString(),
                json[i]["Name"].ToString(),
                json[i]["Description"].ToString(),
                json[i]["MBTI"].ToString(),
                json[i]["Cook"].ToString(),
                json[i]["mapID"].ToString(),
                json[i]["MessagePaperID"].ToString(),
                GetItemSpriteById(json[i]["NpcID"].ToString(), NPCType.Panda)));
        }

        DatabaseManager.Instance.UserInfo.LoadNPCReceived();
        Debug.Log("NPC 받아오기 성공!");
    }


    /// <summary>리소스 폴더에서 NPC 정보를 받아와 List에 넣는 함수</summary>
    public void NPCParserByLocal()
    {
        _dataNPC = CSVReader.Read("NPC");

        for (int i = 0; i < _dataNPC.Count; i++)
        {
            NpcList.Add(new NPC(
                _dataNPC[i]["ID"].ToString(),
                _dataNPC[i]["이름"].ToString(),
                _dataNPC[i]["설명"].ToString(),
                _dataNPC[i]["MBTI"].ToString(),
                _dataNPC[i]["좋아하는 요리"].ToString(),
                _dataNPC[i]["맵 ID"].ToString(),
                _dataNPC[i]["편지지 ID"].ToString(),
                GetItemSpriteById(_dataNPC[i]["ID"].ToString(), NPCType.Panda)));
        }

        DatabaseManager.Instance.UserInfo.LoadNPCReceived();
        Debug.Log("NPC 받아오기 성공!");
    }

    #endregion


    private Sprite GetItemSpriteById(string id, NPCType type)
    {
        Sprite sprite = _npcSpriteDic[(int)type][id];
        return sprite;
    }
}
