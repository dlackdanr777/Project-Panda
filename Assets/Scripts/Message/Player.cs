using BackEnd;
using LitJson;
using Muks.BackEnd;
using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Player
{
    public int Familiarity;

    [Header("Inventory")]
    //GatheringItemInventory
    public Inventory[] GatheringItemInventory; //0:Bug, 1:Fish, 2:Fruit
    //CookInventory
    public Inventory[] CookItemInventory; 
    //Tool Inventory
    public Inventory[] ToolItemInventory; 


    [Header("Sticker")]
    public StickerList StickerInventory = new StickerList();
    public List<ServerStickerData> StickerPosList = new List<ServerStickerData>();

    public int Bamboo { get; private set; }
    public int MaxBamboo => 10000;

    public void Init()
    {
        //Inventory
        GatheringItemInventory = new Inventory[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; //0:Bug, 1:Fish, 2:Fruit
        CookItemInventory = new Inventory[System.Enum.GetValues(typeof(CookItemType)).Length - 1];
        ToolItemInventory = new Inventory[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];

        //ItemInventory 초기화
        for (int i = 0; i < GatheringItemInventory.Length; i++)
        {
            GatheringItemInventory[i] = new Inventory();
        }
        for (int i = 0; i < CookItemInventory.Length; i++)
        {
            CookItemInventory[i] = new Inventory();
        }
        for (int i = 0; i < ToolItemInventory.Length; i++)
        {
            ToolItemInventory[i] = new Inventory();
        }

        //가장 처음 그냥 주는 스티커 3개
        ItemSprite[] stickerImages = DatabaseManager.Instance.GetStickerImages().ItemSprites;
        for (int i = 0; i < stickerImages.Length; i++)
        {
            StickerInventory.AddById(stickerImages[i].Id, stickerImages[i].Image);
        }
        DataBind.SetTextValue("BambooCount", Bamboo.ToString());
        //DatabaseManager.Instance.UserInfo.LoadUserReceivedSticker(); //sticker inventory
        //DatabaseManager.Instance.UserInfo.LoadUserStickerData(); //sticker pos
    }


    public bool SpendBamboo(int amount)
    {
        if(0 <= Bamboo - amount) 
        {
            Bamboo -= amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            //SaveBambooData(3); //서버 저장용이나 임시적으로 제외
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GainBamboo(int amount)
    {
        if(Bamboo + amount <= MaxBamboo)
        {
            Bamboo = Mathf.Clamp(Bamboo + amount, 0, MaxBamboo);
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            DatabaseManager.Instance.Challenges.StackedBambooCount += amount; // 도전과제 달성 체크
            //SaveBambooData(3); //서버 저장용이나 임시적으로 제외
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool AddItemById(string id, int count = 1, ItemAddEventType type = ItemAddEventType.AddChallengesCount, bool isServerUploaded = true)
    {
        InventoryItemField field = GetField(id);

        switch (field)
        {
            case InventoryItemField.GatheringItem:
                GatheringItemInventory[GetItemType(id)].AddById(id, count, type, isServerUploaded);
                return true;

            case InventoryItemField.Cook:
                CookItemInventory[GetItemType(id)].AddById(id, count, type, isServerUploaded);
                return true;

            case InventoryItemField.Tool:
                ToolItemInventory[GetItemType(id)].AddById(id, count, type, isServerUploaded);
                return true;
        }

        Debug.LogErrorFormat("{0} id를 가진 아이템이 존재하지 않습니다.");
        return false;
    }


    public bool RemoveItemById(string id, int count = 1)
    {
        InventoryItemField field = GetField(id);
        switch (field)
        {
            case InventoryItemField.GatheringItem:
                return GatheringItemInventory[GetItemType(id)].RemoveItemById(id, count);

            case InventoryItemField.Cook:
                return CookItemInventory[GetItemType(id)].RemoveItemById(id, count);

            case InventoryItemField.Tool:
                return ToolItemInventory[GetItemType(id)].RemoveItemById(id, count);
        }

        Debug.LogErrorFormat("{0} id를 가진 아이템이 존재하지 않습니다.");
        return false;
    }

    public bool FindItemById(string id, int count = 1)
    {
        InventoryItemField field = GetField(id);
        switch (field)
        {
            case InventoryItemField.GatheringItem:
                return GatheringItemInventory[GetItemType(id)].FindItemById(id, count);

            case InventoryItemField.Cook:
                return CookItemInventory[GetItemType(id)].FindItemById(id, count);

            case InventoryItemField.Tool:
                return ToolItemInventory[GetItemType(id)].FindItemById(id, count);
        }

        Debug.LogErrorFormat("{0} id를 가진 아이템이 존재하지 않습니다.");
        return false;
    }


    public Inventory[] GetItemInventory(InventoryItemField field)
    {
        Inventory[] inventoryArray = null;
        switch (field)
        {
            case InventoryItemField.GatheringItem:
                inventoryArray = GatheringItemInventory;
                break;
            case InventoryItemField.Cook:
                inventoryArray = CookItemInventory;
                break;
            case InventoryItemField.Tool:
                inventoryArray = ToolItemInventory;
                break;
        }

        return inventoryArray;
    }

    public int GetItemType(string id)
    {
        string startId = id.Substring(0, 3);

        switch (startId)
        {
            case "IBG":
                return (int)GatheringItemType.Bug;
            case "IFI":
                return (int)GatheringItemType.Fish;
            case "IFR":
                return (int)GatheringItemType.Fruit;
            case "Coo":
                return (int)CookItemType.CookFood;
            case "ITG":
                return (int)ToolItemType.GatheringTool;

            default:
                Debug.LogErrorFormat("{0}에 해당하는 아이템이 존재하지 않습니다.", startId);
                return -1;
        }
    }


    public InventoryItemField GetField(string id)
    {
        string startId = id.Substring(0, 3);

        switch (startId)
        {
            case "IBG":
                return InventoryItemField.GatheringItem;
            case "IFI":
                return InventoryItemField.GatheringItem;
            case "IFR":
                return InventoryItemField.GatheringItem;
            case "Coo":
                return InventoryItemField.Cook;
            case "ITG":
                return InventoryItemField.Tool;

            default:
                Debug.LogErrorFormat("{0}에 해당하는 아이템이 존재하지 않습니다.", startId);
                return InventoryItemField.None;
        }
    }

    #region sidestory
    /// <summary>
    /// 조건으로 받은 code에 따른 인벤토리 스페셜 몇 개 이상있는지 확인
    /// </summary>
    /// <param name="code">IBG, IFI, IFR</param>
    /// <returns></returns>
    public bool GetGISP(string code, int count)
    {
        int index = 0;

        switch (code)
        {
            case "IBG":
                index = 0;
                break;
            case "IFI":
                index = 1;
                break;
            case "IFR":
                index = 2;
                break;
        }

        return GatheringItemInventory[index].FindSpecialItem(count);
    }

    public bool GetINM(string id)
    {
        string code = id.Substring(0, 3);
        int index = 0;

        switch (code)
        {
            case "ITG":
                index = 0;
                break;
        }

        return ToolItemInventory[index].FindItemById(id, 1);
    }

    /// <summary>
    /// 돈 비교
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>가지고 있는 돈이 amount보다 크면 true, 아니면 false</returns>
    public bool GetMONEY(int amount)
    {
        if(Bamboo >= amount)
        {
            return true;
        }
        return false;
    }

    public bool GetIVGI(string id, int count)
    {
        int index = 0;
        string code = id.Substring(0, 3);

        switch (code)
        {
            case "IBG":
                index = 0;
                break;
            case "IFI":
                index = 1;
                break;
            case "IFR":
                index = 2;
                break;
        }

        return GatheringItemInventory[index].FindItemById(id, count);
    }

    public bool GetIVCK(string id, int count)
    {
        return CookItemInventory[0].FindItemById(id, count);
    }

    public bool GetIVFU(string id)
    {
        List<Furniture> furnitureInventory = DatabaseManager.Instance.StartPandaInfo.FurnitureInventory;
        for(int i = 0; i < furnitureInventory.Count; i++)
        {
            if (furnitureInventory[i].Id.Equals(id))
            {
                return true;
            }
        }
        return false;
    }

    /* public void AddIVGI(string id, int count)
     {
         int index = 0;
         string code = id.Substring(0, 3);

         switch (code)
         {
             case "IBG":
                 index = 0;
                 break;
             case "IFI":
                 index = 1;
                 break;
             case "IFR":
                 index = 2;
                 break;
         }

         if (count < 0)
         {
             GatheringItemInventory[index].RemoveById(id, -count);
         }
         else
         {
             GatheringItemInventory[index].AddById(InventoryItemField.GatheringItem, id, count);
         }
     }*/
    #endregion


    #region Save&LoadBamboo

    /// <summary> 동기적으로 서버 유저 재화 정보 불러오기 </summary>
    public void LoadBambooData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            Bamboo = int.Parse(json[0]["Bamboo"].ToString());
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            Debug.Log("Bamboo Load성공");
        }
    }


    /// <summary> 동기적으로 서버 유저 재화 정보 저장 </summary>
    public void SaveBambooData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Bamboo";

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
                SaveBambooData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertBambooData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateUserInfoData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertBambooData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 비동기적으로 서버 유저 재화 정보 저장 </summary>
    public void AsyncSaveBambooData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Bamboo";

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

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveBambooData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertBambooData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateUserInfoData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertBambooData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        });    
    }


    /// <summary> 동기적으로 서버 유저 재화 정보 삽입 </summary>
    private void InsertBambooData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetBambooParam();

        Debug.LogFormat("재화 데이터 삽입을 요청합니다.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 동기적으로 서버 유저 재화 정보 수정 </summary>
    private void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetBambooParam();

        Debug.LogFormat("재화 데이터 수정을 요청합니다.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 비동기적으로 서버 유저 재화 정보 삽입 </summary>
    private void AsyncInsertBambooData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetBambooParam();

        Debug.LogFormat("재화 데이터 삽입을 요청합니다.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 비동기적으로 서버 유저 재화 정보 수정 </summary>
    private void AsyncUpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetBambooParam();

        Debug.LogFormat("재화 데이터 수정을 요청합니다.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 서버에 저장할 재화 데이터를 반환하는 클래스 </summary>
    private Param GetBambooParam()
    {
        Param param = new Param();

        param.Add("Bamboo", Bamboo);
        return param;
    }

    #endregion

}

