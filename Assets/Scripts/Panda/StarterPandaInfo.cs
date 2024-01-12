using BT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StarterPandaInfo
{
    public StarterPanda StarterPanda; // 저장 필요 x
    public string Mbti;
    public float Intimacy;
    public float Happiness;
    public bool IsExistingUser; // 나중에 userInfo에 있는 거 사용하기
    

    #region 코스튬
    public CostumeViewModel CostumeViewModel; // 저장 필요 x
    public string WearingHeadCostumeID = "";
    public int CostumeCount = -1;
    //public bool[] IsMine;
    public List<string> CostumeInventoryID;
    public List<CostumeData> CostumeInventory;
    #endregion

    public FurnitureViewModel FurnitureViewModel;


    // 판다 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        //LoadPandaInfoData();
    }

    // 불러오기
    private void LoadPandaInfoData()
    {
        Debug.Log(_path);

        // 저장된 게임이 없다면
        if (!File.Exists(_path))
        {
            Debug.Log("판다 저장 문서가 존재하지 않습니다.");

            CreateUserInfoData();
            return;
        }

        IsExistingUser = true;
        // 저장된 게임이 있다면 저장된 파일 읽어오기
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
        // 클래스를 Json 형식으로 전환
        string json = JsonUtility.ToJson(this, true);

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(_path, json );

        Debug.Log(_path);
    }

    // 현재 가지고 있는 옷 불러오기
    public void LoadMyCostume()
    {
        if (CostumeManager.Instance.CostumeDic != null)
        {
            foreach (string key in CostumeManager.Instance.CostumeDic.Keys)
            {
                // 코스튬 인벤토리에 ID 찾으면
                if (CostumeInventoryID == null)
                {
                    Debug.Log("코스튬 없음");
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
            // 코스튬 불러온 후 저장하기
            SaveMyCostume();

            //foreach (string key in CostumeManager.Instance.CostumeDic.Keys)
            //{
            //    // 코스튬 인벤토리에 ID 찾으면
            //    if (CostumeInventoryID == null)
            //    {
            //        Debug.Log("코스튬 없음");
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
            //// 코스튬 불러온 후 저장하기
            //SaveMyCostume();
        }
        else
        {
            Debug.Log("코스튬 매니저 null");
        }
    }

    // 현재 가지고 있는 옷 저장하기
    public void SaveMyCostume()
    {
        if (CostumeManager.Instance.CostumeDic != null)
        {
            SavePandaInfoData();
        }
        else
        {
            Debug.Log("코스튬 매니저 null");
        }
    }


    public void AddCostume(string CostumeDicID)
    {
        // 코스튬 추가 후 정렬
        CostumeInventoryID.Add(CostumeManager.Instance.CostumeDic[CostumeDicID].CostumeID);
        CostumeInventoryID = CostumeInventoryID.OrderBy(id => id).ToList();

        CostumeInventory.Add(CostumeManager.Instance.CostumeDic[CostumeDicID]);
        CostumeInventory = CostumeInventory.OrderBy(costume => costume.CostumeID).ToList();
        CostumeCount++;
        CostumeManager.Instance.CostumeDic[CostumeDicID].IsMine = true;
    }
}

