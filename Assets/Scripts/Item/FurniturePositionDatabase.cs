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
        // Dictionary�� id�� position �߰�
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
            Debug.LogWarning("Furniture �����Ͱ� �������� �ʽ��ϴ�.");
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

            Debug.Log("Furniture Load ����");
        }
    }


    public void SaveFurnitureData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Furniture";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("�ʱ�ȭ ����");
                break;

            case BackendState.Maintainance:
                Debug.LogError("���� ���� ��");
                break;

            case BackendState.Retry:
                Debug.LogWarning("���� ��õ�");
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

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertFurnitureData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetFurnitureParam();

        Debug.LogFormat("{0} ���� ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateFurnitureData(string selectedProbabilityFileId, string inDate)
    {

        Param param = GetFurnitureParam();

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
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
