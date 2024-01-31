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
        ItemSprite[] stickerImages = DatabaseManager.Instance.GetStickerImage().ItemSprites;
        for (int i = 0; i < stickerImages.Length; i++)
        {
            StickerInventory.AddById(stickerImages[i].Id, stickerImages[i].Image);
        }
   
        DatabaseManager.Instance.UserInfo.LoadUserMailData();
        //DatabaseManager.Instance.UserInfo.LoadUserReceivedSticker(); //sticker inventory
        //DatabaseManager.Instance.UserInfo.LoadUserStickerData(); //sticker pos

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
        if(Bamboo < MaxBamboo)
        {
            Bamboo += amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            DatabaseManager.Instance.Challenges.StackedBambooCount += amount; // 도전과제 달성 체크
            return true;
        }
        else
        {
            return false;
        }
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
    #endregion

}

