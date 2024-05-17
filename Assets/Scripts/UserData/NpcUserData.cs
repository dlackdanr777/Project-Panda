using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 유저의 NPC 정보를 보관하는 클래스 </summary>
public class NpcUserData
{
    public List<NPCData> NPCReceived = new List<NPCData>();

    #region Save&LoadNPC

    /// <summary> 동기적으로 서버 유저 NPC 정보를 불러옴 </summary>
    public void LoadNPCData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            NPCReceived.Clear();
            for (int i = 0, count = json[0]["NPCReceived"].Count; i < count; i++)
            {
                NPCData item = JsonUtility.FromJson<NPCData>(json[0]["NPCReceived"][i].ToJson());
                NPCReceived.Add(item);
            }
            LoadNPCReceived();
            Debug.Log("NPC Load성공");
        }
    }

    /// <summary> 동기적으로 서버 유저 NPC 정보 저장 </summary>
    public void SaveNPCData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "NPC";

        if (!Backend.IsLogin)
        {
            Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveNPCData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertNPCData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateNPCData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertNPCData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 동기적으로 서버 유저 NPC 정보 삽입 </summary>
    private void InsertNPCData(string selectedProbabilityFileId)
    {
        SaveNPCReceived();
        Param param = GetNPCParam();

        Debug.LogFormat("NPC 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 동기적으로 서버 유저 NPC 정보 수정 </summary>
    private void UpdateNPCData(string selectedProbabilityFileId, string inDate)
    {
        SaveNPCReceived();
        Param param = GetNPCParam();

        Debug.LogFormat("NPC 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 NPC 데이터를 모아 반환하는 클래스 </summary>
    private Param GetNPCParam()
    {
        Param param = new Param();
        param.Add("NPCReceived", NPCReceived);
        return param;
    }


    public void LoadNPCReceived()
    {
        List<NPC> npcList = DatabaseManager.Instance.GetNPCList();
        //NPC
        for (int i = 0; i < NPCReceived.Count; i++)
        {
            for (int j = 0, count = npcList.Count; j < count; j++)
            {
                if (NPCReceived[i].Id.Equals(npcList[j].Id))
                {
                    npcList[j].IsReceived = true;
                    npcList[j].Intimacy = NPCReceived[i].Intimacy;
                    npcList[j].SSId = NPCReceived[i].SSId;
                    npcList[j].DiaryAlarmCheck = NPCReceived[i].AlarmCheck;
                }
            }

        }
    }


    private void SaveNPCReceived()
    {
        List<NPC>[] npcDatabase = { DatabaseManager.Instance.GetNPCList() };

        NPCReceived.Clear();
        for (int i = 0; i < npcDatabase.Length; i++)
        {
            for (int j = 0; j < npcDatabase[i].Count; j++)
            {
                if (npcDatabase[i][j].IsReceived)
                {
                    NPCReceived.Add(new NPCData(npcDatabase[i][j].Id, npcDatabase[i][j].Intimacy, npcDatabase[i][j].SSId, npcDatabase[i][j].DiaryAlarmCheck));
                }
            }
        }
    }

    #endregion




}
