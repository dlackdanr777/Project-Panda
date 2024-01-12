using BT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StarterPandaInfo
{
    public StarterPanda StarterPanda; // ���� �ʿ� x
    public string Mbti;
    public float Intimacy;
    public float Happiness;
    public bool IsExistingUser; // ���߿� userInfo�� �ִ� �� ����ϱ�
    

    #region �ڽ�Ƭ
    public CostumeViewModel CostumeViewModel; // ���� �ʿ� x
    public string WearingHeadCostumeID = "";
    public int CostumeCount = -1;
    //public bool[] IsMine;
    public List<string> CostumeInventoryID;
    public List<CostumeData> CostumeInventory;
    #endregion

    public FurnitureViewModel FurnitureViewModel;


    // �Ǵ� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        //LoadPandaInfoData();
    }

    // �ҷ�����
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

        IsExistingUser = true;
        // ����� ������ �ִٸ� ����� ���� �о����
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
    }

    private void CreateUserInfoData()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
        CostumeInventoryID = new List<string>();
    }

    public void SavePandaInfoData()
    {
        // Ŭ������ Json �������� ��ȯ
        string json = JsonUtility.ToJson(this, true);

        // �̹� ����� ������ �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(_path, json );

        Debug.Log(_path);
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


    public void AddCostume(string CostumeDicID)
    {
        // �ڽ�Ƭ �߰� �� ����
        CostumeInventoryID.Add(CostumeManager.Instance.CostumeDic[CostumeDicID].CostumeID);
        CostumeInventoryID = CostumeInventoryID.OrderBy(id => id).ToList();

        CostumeInventory.Add(CostumeManager.Instance.CostumeDic[CostumeDicID]);
        CostumeInventory = CostumeInventory.OrderBy(costume => costume.CostumeID).ToList();
        CostumeCount++;
        CostumeManager.Instance.CostumeDic[CostumeDicID].IsMine = true;
    }
}

