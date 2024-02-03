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

        // 서버에 연결되면 삭제
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
        // 가구 추가 후 정렬
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

