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
    

    #region 코스튬
    public CostumeViewModel CostumeViewModel; // 저장 필요 x
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

    // 판다 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        //LoadPandaInfoData();
/*        for (int i = 0; i < FurnitureRooms.Length; i++)
        {
            FurnitureRooms[i] = new FurnitureModel.FurnitureId();
        }*/
        
    }

/*    // 불러오기
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


    /// <summary>판다 데이터를 서버에서 불러오는 함수</summary>
    public void LoadPandaInfoData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            //이쪽에 불러올 데이터 추가
            Mbti = json[0]["Mbti"].ToString();
            Intimacy = float.Parse(json[0]["Intimacy"].ToString());
            Happiness = float.Parse(json[0]["Happiness"].ToString());

            Debug.Log("StarterPandaInfo Load성공");
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

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertPandaInfoData(string selectedProbabilityFileId)
    {

        Param param = GetPandaInfoParam();
        Debug.LogFormat("스타터 판다 데이터 삽입을 요청합니다.");
        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdatePandaInfoData(string selectedProbabilityFileId, string inDate)
    {
        Param param = GetPandaInfoParam();
        Debug.LogFormat("스타터 판다 데이터 수정을 요청합니다.");
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
      /*  // 클래스를 Json 형식으로 전환
        string json = JsonUtility.ToJson(this, true);

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(_path, json );

        Debug.Log(_path);*/
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

    // 현재 가지고 있는 가구 불러오기
    public void LoadMyFurniture()
    {
        Debug.Log("LoadMyFurniture");
        if (DatabaseManager.Instance.GetFurnitureItem() != null)
        {
            foreach (string key in DatabaseManager.Instance.GetFurnitureItem().Keys)
            {
                // 가구 인벤토리에 ID 찾기
                if (FurnitureInventoryID == null)
                {
                    Debug.Log("가구 없음");
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
            // 가구 불러온 후 저장하기
            SavePandaInfoData();

        }
        else
        {
            Debug.Log("가구 데이터베이스 null");
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

    public void AddCostume(string costumeDicID)
    {
        // 코스튬 추가 후 정렬
        CostumeInventoryID.Add(CostumeManager.Instance.CostumeDic[costumeDicID].CostumeID);
        CostumeInventoryID = CostumeInventoryID.OrderBy(id => id).ToList();

        CostumeInventory.Add(CostumeManager.Instance.CostumeDic[costumeDicID]);
        CostumeInventory = CostumeInventory.OrderBy(costume => costume.CostumeID).ToList();
        CostumeCount++;
        CostumeManager.Instance.CostumeDic[costumeDicID].IsMine = true;
    }

    public void AddFurniture(string furnitureId)
    {
        // 코스튬 추가 후 정렬
        FurnitureInventoryID.Add(furnitureId);
        FurnitureInventoryID = FurnitureInventoryID.OrderBy(id => id).ToList();

        Furniture furniture = DatabaseManager.Instance.GetFurnitureItem()[furnitureId];
        FurnitureInventory.Add(furniture);
        FurnitureInventory = FurnitureInventory.OrderBy(furniture => furniture.Id).ToList();
        FurnitureCount++;
        furniture.IsMine = true;
    }
}

