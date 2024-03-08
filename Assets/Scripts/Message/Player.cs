using BackEnd.MultiCharacter;
using BackEnd;
using LitJson;
using Muks.BackEnd;
using Muks.DataBind;
using System;
using System.Collections;
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

    [Header("Message")]
    public MessageList[] Messages; //0:Mail, 1:Wish

    [Header("Sticker")]
    public StickerList StickerInventory = new StickerList();
    public List<StickerData> StickerPosList = new List<StickerData>();

    public int Bamboo { get; private set; }
    public int MaxBamboo;


    public void Init()
    {
        MaxBamboo = 1000;

        //Message
        Messages = new MessageList[System.Enum.GetValues(typeof(MessageField)).Length - 1];
        for (int i = 0; i < System.Enum.GetValues(typeof(MessageField)).Length - 1; i++)
        {
            Messages[i] = new MessageList();
        }

        //Inventory
        GatheringItemInventory = new Inventory[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; //0:Bug, 1:Fish, 2:Fruit
        CookItemInventory = new Inventory[System.Enum.GetValues(typeof(CookItemType)).Length - 1];
        ToolItemInventory = new Inventory[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];

        //ItemInventory �ʱ�ȭ
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

        //���� ó�� �׳� �ִ� ��ƼĿ 3��
        ItemSprite[] stickerImages = DatabaseManager.Instance.GetStickerImages().ItemSprites;
        for (int i = 0; i < stickerImages.Length; i++)
        {
            StickerInventory.AddById(stickerImages[i].Id, stickerImages[i].Image);
        }
   
        DatabaseManager.Instance.UserInfo.LoadUserMailData();
        //DatabaseManager.Instance.UserInfo.LoadUserReceivedSticker(); //sticker inventory
        //DatabaseManager.Instance.UserInfo.LoadUserStickerData(); //sticker pos

        AddItemById("IFI01");
        AddItemById("IFI44");
        AddItemById("IFI19");

        DataBind.SetTextValue("BambooCount", Bamboo.ToString());
    }


    public bool SpendBamboo(int amount)
    {
        if(Bamboo > 0) 
        {
            Bamboo -= amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
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
            Bamboo += amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            DatabaseManager.Instance.Challenges.StackedBambooCount += amount; // �������� �޼� üũ
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool AddItemById(string id, int count = 1, ItemAddEventType type = ItemAddEventType.AddChallengesCount)
    {
        InventoryItemField field = GetField(id);

        switch (field)
        {
            case InventoryItemField.GatheringItem:
                GatheringItemInventory[GetItemType(id)].AddById(id, count, type);
                return true;

            case InventoryItemField.Cook:
                CookItemInventory[GetItemType(id)].AddById(id, count, type);
                return true;

            case InventoryItemField.Tool:
                ToolItemInventory[GetItemType(id)].AddById(id, count, type);
                return true;
        }

        Debug.LogErrorFormat("{0} id�� ���� �������� �������� �ʽ��ϴ�.");
        return false;
    }


    public bool RemoveItemById(string id, int count = 1)
    {
        InventoryItemField field = GetField(id);

        switch (field)
        {
            case InventoryItemField.GatheringItem:
                GatheringItemInventory[GetItemType(id)].RemoveItemById(id, count);
                return true;

            case InventoryItemField.Cook:
                CookItemInventory[GetItemType(id)].RemoveItemById(id, count);
                return true;

            case InventoryItemField.Tool:
                ToolItemInventory[GetItemType(id)].RemoveItemById(id, count);
                return true;
        }

        Debug.LogErrorFormat("{0} id�� ���� �������� �������� �ʽ��ϴ�.");
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
                Debug.LogErrorFormat("{0}�� �ش��ϴ� �������� �������� �ʽ��ϴ�.", startId);
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
                Debug.LogErrorFormat("{0}�� �ش��ϴ� �������� �������� �ʽ��ϴ�.", startId);
                return InventoryItemField.None;
        }
    }

    #region sidestory
    /// <summary>
    /// �������� ���� code�� ���� �κ��丮 ����� �� �� �̻��ִ��� Ȯ��
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
    /// �� ��
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>������ �ִ� ���� amount���� ũ�� true, �ƴϸ� false</returns>
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


    #region SaveAndLoadBamboo
    public void LoadBambooData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            Bamboo = int.Parse(json[0]["Bamboo"].ToString());
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            Debug.Log("Bamboo Load����");
        }
    }


    public void SaveBambooData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Bamboo";

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

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertBambooData(string selectedProbabilityFileId)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetBambooParam();

        Debug.LogFormat("��ȭ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateUserInfoData(string selectedProbabilityFileId, string inDate)
    {
        string paser = DateTime.Now.ToString();
        Param param = GetBambooParam();

        Debug.LogFormat("��ȭ ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ ��ȭ �����͸� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetBambooParam()
    {
        Param param = new Param();

        param.Add("Bamboo", Bamboo);
        return param;
    }

    #endregion
}

