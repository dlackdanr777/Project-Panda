using BackEnd;
using BackEnd.MultiCharacter;
using BT;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StarterPandaInfo
{
    public string Mbti;
    public float Intimacy;
    public float Happiness;
    

    #region �ڽ�Ƭ
    public CostumeViewModel CostumeViewModel; // ���� �ʿ� x
    public string WearingHeadCostumeID = "";
    public int CostumeCount = -1;
    //public bool[] IsMine;
    public List<string> CostumeInventoryID = new List<string>();
    public List<CostumeData> CostumeInventory = new List<CostumeData>();
    #endregion

    public FurnitureViewModel FurnitureViewModel;
    public FurnitureModel.FurnitureId[] FurnitureRooms = new FurnitureModel.FurnitureId[System.Enum.GetValues(typeof(ERoom)).Length];
    public int FurnitureCount = -1;
    public List<string> FurnitureInventoryID = new List<string>();
    public List<Furniture> FurnitureInventory = new List<Furniture>();

    // �Ǵ� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        //LoadPandaInfoData();
/*        for (int i = 0; i < FurnitureRooms.Length; i++)
        {
            FurnitureRooms[i] = new FurnitureModel.FurnitureId();
        }*/
        
    }

/*    // �ҷ�����
    private void LoadPandaInfoData()
    {
        Debug.Log(_path);

        // ����� ������ ���ٸ�
        if (!File.Exists(_path))
        {
            Debug.Log("�Ǵ� ���� ������ �������� �ʽ��ϴ�.");

            CreateUserInfoData();
            return;
        }

        StarterPandaInfo pandaInfo = new StarterPandaInfo();
        string loadJson = File.ReadAllText(_path);
        pandaInfo = JsonUtility.FromJson<StarterPandaInfo>(loadJson);

        Mbti = pandaInfo.Mbti;
        Intimacy = pandaInfo.Intimacy;
        Happiness = pandaInfo.Happiness;

        WearingHeadCostumeID = pandaInfo.WearingHeadCostumeID;
        CostumeCount = pandaInfo.CostumeCount;
        CostumeInventoryID = pandaInfo.CostumeInventoryID;
        CostumeInventory = new List<CostumeData>();

        FurnitureRooms = pandaInfo.FurnitureRooms;
        FurnitureCount = pandaInfo.FurnitureCount;
        FurnitureInventoryID = pandaInfo.FurnitureInventoryID;
        FurnitureInventory = new List<Furniture>();
    }*/


    /// <summary>�Ǵ� �����͸� �������� �ҷ����� �Լ�</summary>
    public void LoadPandaInfoData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            //���ʿ� �ҷ��� ������ �߰�
            Mbti = json[0]["Mbti"].ToString();
            Intimacy = float.Parse(json[0]["Intimacy"].ToString());
            Happiness = float.Parse(json[0]["Happiness"].ToString());

            Debug.Log("StarterPandaInfo Load����");
        }
    }

    private void CreateUserInfoData()
    {
       /* string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
        CostumeInventoryID = new List<string>();
        FurnitureInventoryID = new List<string>();
        for(int i = 0; i < FurnitureRooms.Length; i++)
        {
            FurnitureRooms[i] = new FurnitureModel.FurnitureId();
        }*/
    }


    public void SavePandaInfoData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "StarterPandaInfo";

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
                SavePandaInfoData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertPandaInfoData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdatePandaInfoData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertPandaInfoData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertPandaInfoData(string selectedProbabilityFileId)
    {

        Param param = GetPandaInfoParam();
        Debug.LogFormat("��Ÿ�� �Ǵ� ������ ������ ��û�մϴ�.");
        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdatePandaInfoData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetPandaInfoParam();
        Debug.LogFormat("��Ÿ�� �Ǵ� ������ ������ ��û�մϴ�.");
        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }



    public Param GetPandaInfoParam()
    {
        Param param = new Param();
        param.Add("Mbti", Mbti);
        param.Add("Intimacy", Intimacy);
        param.Add("Happiness", Happiness);
        return param;
    }


    public void SavePandaInfoData()
    {
      /*  // Ŭ������ Json �������� ��ȯ
        string json = JsonUtility.ToJson(this, true);

        // �̹� ����� ������ �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(_path, json );

        Debug.Log(_path);*/
    }

    // ���� ������ �ִ� �� �ҷ�����
    public void LoadMyCostume()
    {
        if (CostumeManager.Instance.CostumeDic != null)
        {
            foreach (string key in CostumeManager.Instance.CostumeDic.Keys)
            {
                // �ڽ�Ƭ �κ��丮�� ID ã����
                if (CostumeInventoryID == null)
                {
                    Debug.Log("�ڽ�Ƭ ����");
                }
                else
                {
                    if (CostumeInventoryID.Find(a => a.Equals(CostumeManager.Instance.CostumeDic[key].CostumeID)) != null)
                    {
                        CostumeManager.Instance.CostumeDic[key].IsMine = true;
                        CostumeInventory.Add(CostumeManager.Instance.CostumeDic[key]);
                    }
                    else
                    {
                        CostumeManager.Instance.CostumeDic[key].IsMine = false;
                    }
                }

            }
            // �ڽ�Ƭ �ҷ��� �� �����ϱ�
            SaveMyCostume();

            //foreach (string key in CostumeManager.Instance.CostumeDic.Keys)
            //{
            //    // �ڽ�Ƭ �κ��丮�� ID ã����
            //    if (CostumeInventoryID == null)
            //    {
            //        Debug.Log("�ڽ�Ƭ ����");
            //    }
            //    else
            //    {
            //        List<string> filterCostumeInventoryID = CostumeInventoryID
            //            .Where(id => id == CostumeManager.Instance.CostumeDic[key].CostumeID)
            //            .ToList();

            //        if (CostumeInventoryID.Find(a => a.Equals(CostumeManager.Instance.CostumeDic[key].CostumeID)) != null)
            //        {
            //            CostumeManager.Instance.CostumeDic[key].IsMine = true;
            //            CostumeInventory.Add(CostumeManager.Instance.CostumeDic[key]);
            //            Debug.Log("Add Costume: " + CostumeManager.Instance.CostumeDic[key].CostumeName);
            //        }
            //        else
            //        {
            //            CostumeManager.Instance.CostumeDic[key].IsMine = false;
            //        }
            //    }

            //}
            //// �ڽ�Ƭ �ҷ��� �� �����ϱ�
            //SaveMyCostume();
        }
        else
        {
            Debug.Log("�ڽ�Ƭ �Ŵ��� null");
        }
    }

    // ���� ������ �ִ� ���� �ҷ�����
    public void LoadMyFurniture()
    {
        Debug.Log("LoadMyFurniture");
        if (DatabaseManager.Instance.GetFurnitureItem() != null)
        {
            foreach (string key in DatabaseManager.Instance.GetFurnitureItem().Keys)
            {
                // ���� �κ��丮�� ID ã��
                if (FurnitureInventoryID == null)
                {
                    Debug.Log("���� ����");
                }
                else
                {
                    if (FurnitureInventoryID.Find(id => id.Equals(DatabaseManager.Instance.GetFurnitureItem()[key].Id)) != null)
                    {
                        DatabaseManager.Instance.GetFurnitureItem()[key].IsMine = true;
                        FurnitureInventory.Add(DatabaseManager.Instance.GetFurnitureItem()[key]);
                    }
                    else
                    {
                        DatabaseManager.Instance.GetFurnitureItem()[key].IsMine = false;
                    }
                }

            }
            // ���� �ҷ��� �� �����ϱ�
            SavePandaInfoData();

        }
        else
        {
            Debug.Log("���� �����ͺ��̽� null");
        }
    }

    // ���� ������ �ִ� �� �����ϱ�
    public void SaveMyCostume()
    {
        if (CostumeManager.Instance.CostumeDic != null)
        {
            SavePandaInfoData();
        }
        else
        {
            Debug.Log("�ڽ�Ƭ �Ŵ��� null");
        }
    }

    public void AddCostume(string costumeDicID)
    {
        // �ڽ�Ƭ �߰� �� ����
        CostumeInventoryID.Add(CostumeManager.Instance.CostumeDic[costumeDicID].CostumeID);
        CostumeInventoryID = CostumeInventoryID.OrderBy(id => id).ToList();

        CostumeInventory.Add(CostumeManager.Instance.CostumeDic[costumeDicID]);
        CostumeInventory = CostumeInventory.OrderBy(costume => costume.CostumeID).ToList();
        CostumeCount++;
        CostumeManager.Instance.CostumeDic[costumeDicID].IsMine = true;
    }

    public void AddFurniture(string furnitureId)
    {
        // �ڽ�Ƭ �߰� �� ����
        FurnitureInventoryID.Add(furnitureId);
        FurnitureInventoryID = FurnitureInventoryID.OrderBy(id => id).ToList();

        Furniture furniture = DatabaseManager.Instance.GetFurnitureItem()[furnitureId];
        FurnitureInventory.Add(furniture);
        FurnitureInventory = FurnitureInventory.OrderBy(furniture => furniture.Id).ToList();
        FurnitureCount++;
        furniture.IsMine = true;
    }
}

