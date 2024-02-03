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
        for (int i = 0; i < FurnitureRooms.Length; i++)
        {
            if (FurnitureRooms[i] == null)
            {
                FurnitureRooms[i] = new FurnitureModel.FurnitureId();
            }
        }

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

        // ������ ����Ǹ� ����
        FurnitureInventoryID.Add("RN01A");
        FurnitureInventoryID.Add("RN01B");
        FurnitureInventoryID.Add("RN01C");
        FurnitureInventoryID.Add("RN01D");
        FurnitureInventoryID.Add("RN01E");
        FurnitureInventoryID.Add("RN01F");
        FurnitureInventoryID.Add("RN01G");
        FurnitureInventoryID.Add("RN01H");

        FurnitureInventoryID.Add("RN02A");
        FurnitureInventoryID.Add("RN02B");
        FurnitureInventoryID.Add("RN02C");
        FurnitureInventoryID.Add("RN02D");
        FurnitureInventoryID.Add("RN02E");
        FurnitureInventoryID.Add("RN02F");
        FurnitureInventoryID.Add("RN02G");
        FurnitureInventoryID.Add("RN02H");

        FurnitureInventoryID.Add("RN03A");
        FurnitureInventoryID.Add("RN03B");
        FurnitureInventoryID.Add("RN03C");
        FurnitureInventoryID.Add("RN03D");
        FurnitureInventoryID.Add("RN03E");
        FurnitureInventoryID.Add("RN03F");
        FurnitureInventoryID.Add("RN03G");
        FurnitureInventoryID.Add("RN03H");

        FurnitureInventoryID.Add("RN04A");
        FurnitureInventoryID.Add("RN04B");
        FurnitureInventoryID.Add("RN04C");
        FurnitureInventoryID.Add("RN04D");
        FurnitureInventoryID.Add("RN04E");
        FurnitureInventoryID.Add("RN04F");
        FurnitureInventoryID.Add("RN04G");
        FurnitureInventoryID.Add("RN04H");

        FurnitureInventoryID.Add("RN05A");
        FurnitureInventoryID.Add("RN05B");
        FurnitureInventoryID.Add("RN05C");
        FurnitureInventoryID.Add("RN05D");
        FurnitureInventoryID.Add("RN05E");
        FurnitureInventoryID.Add("RN05F");
        FurnitureInventoryID.Add("RN05G");
        FurnitureInventoryID.Add("RN05H");

        FurnitureInventoryID.Add("RN06A");
        FurnitureInventoryID.Add("RN06B");
        FurnitureInventoryID.Add("RN06C");
        FurnitureInventoryID.Add("RN06D");
        FurnitureInventoryID.Add("RN06E");
        FurnitureInventoryID.Add("RN06F");
        FurnitureInventoryID.Add("RN06G");

        FurnitureInventoryID.Add("RN07A");
        FurnitureInventoryID.Add("RN07B");
        FurnitureInventoryID.Add("RN07C");
        FurnitureInventoryID.Add("RN07D");
        FurnitureInventoryID.Add("RN07E");
        FurnitureInventoryID.Add("RN07F");
        FurnitureInventoryID.Add("RN07G");
        FurnitureInventoryID.Add("RN07H");

        FurnitureInventoryID.Add("RN08A");
        FurnitureInventoryID.Add("RN08B");
        FurnitureInventoryID.Add("RN08C");
        FurnitureInventoryID.Add("RN08D");
        FurnitureInventoryID.Add("RN08E");
        FurnitureInventoryID.Add("RN08F");
        FurnitureInventoryID.Add("RN08G");
        FurnitureInventoryID.Add("RN08H");

        FurnitureInventoryID.Add("RN09A");
        FurnitureInventoryID.Add("RN09B");
        FurnitureInventoryID.Add("RN09C");
        FurnitureInventoryID.Add("RN09D");
        FurnitureInventoryID.Add("RN09E");
        FurnitureInventoryID.Add("RN09F");
        FurnitureInventoryID.Add("RN09G");
        FurnitureInventoryID.Add("RN09H");

        FurnitureInventoryID.Add("RN10A");
        FurnitureInventoryID.Add("RN10B");
        FurnitureInventoryID.Add("RN10C");
        FurnitureInventoryID.Add("RN10D");
        FurnitureInventoryID.Add("RN10E");
        FurnitureInventoryID.Add("RN10F");
        FurnitureInventoryID.Add("RN10G");
        FurnitureInventoryID.Add("RN10H");

        FurnitureInventoryID.Add("RN11A");
        FurnitureInventoryID.Add("RN11B");
        FurnitureInventoryID.Add("RN11C");
        FurnitureInventoryID.Add("RN11D");
        FurnitureInventoryID.Add("RN11E");
        FurnitureInventoryID.Add("RN11F");
        FurnitureInventoryID.Add("RN11G");
        FurnitureInventoryID.Add("RN11H");

        FurnitureInventoryID.Add("RN12A");
        FurnitureInventoryID.Add("RN12B");
        FurnitureInventoryID.Add("RN12C");
        FurnitureInventoryID.Add("RN12D");
        FurnitureInventoryID.Add("RN12E");
        FurnitureInventoryID.Add("RN12F");
        FurnitureInventoryID.Add("RN12G");
        FurnitureInventoryID.Add("RN12H");

        FurnitureInventoryID.Add("RN13A");
        FurnitureInventoryID.Add("RN13B");
        FurnitureInventoryID.Add("RN13C");
        FurnitureInventoryID.Add("RN13D");
        FurnitureInventoryID.Add("RN13E");
        FurnitureInventoryID.Add("RN13F");
        FurnitureInventoryID.Add("RN13G");
        FurnitureInventoryID.Add("RN13H");

        FurnitureInventoryID.Add("RN14A");
        FurnitureInventoryID.Add("RN14B");
        FurnitureInventoryID.Add("RN14C");
        FurnitureInventoryID.Add("RN14D");
        FurnitureInventoryID.Add("RN14E");
        FurnitureInventoryID.Add("RN14F");
        FurnitureInventoryID.Add("RN14G");
        FurnitureInventoryID.Add("RN14H");

        FurnitureInventoryID.Add("RN15A");
        FurnitureInventoryID.Add("RN15B");
        FurnitureInventoryID.Add("RN15C");
        FurnitureInventoryID.Add("RN15D");
        FurnitureInventoryID.Add("RN15E");
        FurnitureInventoryID.Add("RN15F");
        FurnitureInventoryID.Add("RN15G");
        FurnitureInventoryID.Add("RN15H");

        FurnitureInventoryID.Add("RN16A");
        FurnitureInventoryID.Add("RN16B");
        FurnitureInventoryID.Add("RN16C");
        FurnitureInventoryID.Add("RN16D");
        FurnitureInventoryID.Add("RN16E");
        FurnitureInventoryID.Add("RN16F");
        FurnitureInventoryID.Add("RN16G");
        FurnitureInventoryID.Add("RN16H");

        FurnitureInventoryID.Add("RN17A");
        FurnitureInventoryID.Add("RN17B");
        FurnitureInventoryID.Add("RN17C");
        FurnitureInventoryID.Add("RN17D");
        FurnitureInventoryID.Add("RN17E");
        FurnitureInventoryID.Add("RN17F");
        FurnitureInventoryID.Add("RN17G");
        FurnitureInventoryID.Add("RN17H");

        FurnitureInventoryID.Add("RN18A");
        FurnitureInventoryID.Add("RN18B");
        FurnitureInventoryID.Add("RN18C");
        FurnitureInventoryID.Add("RN18D");
        FurnitureInventoryID.Add("RN18E");
        FurnitureInventoryID.Add("RN18F");
        FurnitureInventoryID.Add("RN18G");
        FurnitureInventoryID.Add("RN18H");

        FurnitureInventoryID.Add("RN19A");
        FurnitureInventoryID.Add("RN19B");
        FurnitureInventoryID.Add("RN19C");
        FurnitureInventoryID.Add("RN19D");
        FurnitureInventoryID.Add("RN19E");
        FurnitureInventoryID.Add("RN19F");
        FurnitureInventoryID.Add("RN19G");
        FurnitureInventoryID.Add("RN19H");

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
        // ���� �߰� �� ����
        FurnitureInventoryID.Add(furnitureId);
        FurnitureInventoryID = FurnitureInventoryID.OrderBy(id => id).ToList();
        DatabaseManager.Instance.Challenges.FurnitureArrangement(furnitureId);

        Furniture furniture = DatabaseManager.Instance.GetFurnitureItem()[furnitureId];
        FurnitureInventory.Add(furniture);
        FurnitureInventory = FurnitureInventory.OrderBy(furniture => furniture.Id).ToList();
        FurnitureCount++;
        furniture.IsMine = true;
    }
}

