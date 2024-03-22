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
    public Dictionary<string, NPC> NpcDic = new Dictionary<string, NPC>();  
    private List<Dictionary<string, object>> _dataNPC;

    //Image
    public Dictionary<string, Sprite>[] _npcSpriteDic = new Dictionary<string, Sprite>[System.Enum.GetValues(typeof(NPCType)).Length - 1];

    private string _chartID => "105699";

    public void Register()
    {
        DatabaseManager database = DatabaseManager.Instance;

        for (int i = 0, count = database.NpcImages.Length; i < count; i++)
        {
            _npcSpriteDic[i] = new Dictionary<string, Sprite>();
            for (int j = 0; j < database.NpcImages[i].ItemSprites.Length; j++)
            {
                _npcSpriteDic[i].Add(database.NpcImages[i].ItemSprites[j].Id, database.NpcImages[i].ItemSprites[j].Image);
            }
        }

        NPCParserByLocal();
    }





    #region LoadNPCData

    public void LoadData()
    {
        BackendManager.Instance.GetChartData(_chartID, 10, NPCParserByServer);
    }


    /// <summary>서버에서 NPC 정보를 받아와 List에 넣는 함수</summary>
    public void NPCParserByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();
        NpcList.Clear();
        NpcDic.Clear();
        for (int i = 0; i < json.Count; i++)
        {
            string id = json[i]["NpcID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            string mbti = json[i]["MBTI"].ToString();
            string cook = json[i]["Cook"].ToString();
            string mapID = json[i]["mapID"].ToString();
            string messagePaperID = json[i]["MessagePaperID"].ToString();
            Sprite sprite = GetItemSpriteById(json[i]["NpcID"].ToString(), NPCType.Panda);

            NPC npc = new NPC(id, name, description, mbti, cook, mapID, messagePaperID, sprite);
            NpcList.Add(npc);
            NpcDic.Add(id, npc);
        }

        DatabaseManager.Instance.UserInfo.LoadNPCReceived();
        Debug.Log("NPC 받아오기 성공!");
    }


    /// <summary>리소스 폴더에서 NPC 정보를 받아와 List에 넣는 함수</summary>
    public void NPCParserByLocal()
    {
        _dataNPC = CSVReader.Read("NPC");
        NpcList.Clear();
        NpcDic.Clear();

        for (int i = 0; i < _dataNPC.Count; i++)
        {
            string id = _dataNPC[i]["ID"].ToString();
            string name = _dataNPC[i]["이름"].ToString();
            string description = _dataNPC[i]["설명"].ToString();
            string mbti = _dataNPC[i]["MBTI"].ToString();
            string cook = _dataNPC[i]["좋아하는 요리"].ToString();
            string mapID = _dataNPC[i]["맵 ID"].ToString();
            string messagePaperID = _dataNPC[i]["편지지 ID"].ToString();
            Sprite sprite = GetItemSpriteById(id, NPCType.Panda);

            NPC npc = new NPC(id, name, description, mbti, cook, mapID, messagePaperID, sprite);
            NpcList.Add(npc);
            NpcDic.Add(id, npc);
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
