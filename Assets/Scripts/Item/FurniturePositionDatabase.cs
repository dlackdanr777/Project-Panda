using BackEnd.MultiCharacter;
using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class FurniturePositionDatabase
{
    private Dictionary<Vector3, string> _saveFurnitureList = new Dictionary<Vector3, string>();
    private string _fileName => "FurniturePosData.json";

    public void Register()
    {
        //Load();
    }
    public Dictionary<Vector3, string> GetSaveFurniutreList()
    {
        return _saveFurnitureList;
    }

    public void AddFurniturePosition(string id, Vector3 position)
    {
        // Dictionary에 id와 position 추가
        _saveFurnitureList.Add(position, id);
    }

    public void RemoveFurniturePosition(Vector3 position)
    {
        if (_saveFurnitureList.ContainsKey(position))
        {
            _saveFurnitureList.Remove(position);
        }
    }

   
    #region SaveAndLoadFurniture
    public void LoadFurnitureData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("Furniture 데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["Name"].Count; i < count; i++)
            {
                string name = json[0]["Name"][i].ToString();
                float posX = float.Parse(json[0]["PosX"][i].ToString());
                float posY = float.Parse(json[0]["PosY"][i].ToString());
                float posZ = float.Parse(json[0]["PosZ"][i].ToString());

                Vector3 pos = new Vector3(posX, posY, posZ);
                _saveFurnitureList.Add(pos, name);
            }

            Debug.Log("Furniture Load 성공");
        }
    }


    public void SaveFurnitureData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Furniture";

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
                Debug.LogError("초기화 실패");
                break;

            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;

            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
                SaveFurnitureData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertFurnitureData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateFurnitureData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertFurnitureData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertFurnitureData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetFurnitureParam();

        Debug.LogFormat("{0} 정보 데이터 삽입을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateFurnitureData(string selectedProbabilityFileId, string inDate)
    {

        Param param = GetFurnitureParam();

        Debug.LogFormat("{0} 데이터 수정을 요청합니다.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 가구 데이터를 모아 반환하는 클래스 </summary>
    public Param GetFurnitureParam()
    {
        Param param = new Param();

        List<float> posX = new List<float>();
        List<float> posY = new List<float>();
        List<float> posZ = new List<float>();
        List<string> name = new List<string>();

        foreach(var data in _saveFurnitureList)
        {
            posX.Add(data.Key.x);
            posY.Add(data.Key.y);
            posZ.Add(data.Key.z);
            name.Add(data.Value);
        }

        param.Add("PosX", posX);
        param.Add("PosY", posY);
        param.Add("PosZ", posZ);
        param.Add("Name", name);

        return param;
    }

    #endregion

}
