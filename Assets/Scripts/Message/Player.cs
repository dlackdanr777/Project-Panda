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
    public StickerList StickerInventory;
    public List<StickerData> StickerPosList;

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

        //Sticker
        StickerInventory = new StickerList();
        StickerPosList = new List<StickerData>();

        //가장 처음 그냥 주는 스티커 3개
        ItemSprite[] stickerImages = DatabaseManager.Instance.GetStickerImage().ItemSprites;
        for (int i = 0; i < stickerImages.Length; i++)
        {
            StickerInventory.AddById(stickerImages[i].Id, stickerImages[i].Image);
        }
   
        DatabaseManager.Instance.UserInfo.LoadUserInventory();
        DatabaseManager.Instance.UserInfo.LoadUserMailData();
        DatabaseManager.Instance.UserInfo.LoadUserReceivedSticker(); //sticker inventory
        DatabaseManager.Instance.UserInfo.LoadUserStickerData(); //sticker pos

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
}

