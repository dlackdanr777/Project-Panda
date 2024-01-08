using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
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
    public MessageList[] Messages = new MessageList[System.Enum.GetValues(typeof(MessageField)).Length - 1]; //0:Mail, 1:Wish 
    //public int MaxMessageCount { get; private set; }
    //public List<bool> IsCheckMessage = new List<bool>();
    //public List<bool> IsReceiveGift = new List<bool>();

    [Header("Bamboo")]
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private GameObject _dontUseBambooPanel;
    [SerializeField] private GameObject _dontGainBambooPanel;
    public int Bamboo { get; private set; }
    public int MaxBamboo;


    private void Awake()
    {
        //MaxMessageCount = 20;
        MaxBamboo = 1000;

        //for(int i=0; i < System.Enum.GetValues(typeof(ItemField)).Length - 1; i++)
        //{
        //    Inventory[i] = new Inventory();
        //}
        //for (int i = 0; i < System.Enum.GetValues(typeof(MessageField)).Length - 1; i++)
        //{
        //    //Messages[i] = new MessageList();
        //}

        GatheringItemInventory = new Inventory[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; //0:Bug, 1:Fish, 2:Fruit
        CookItemInventory = new Inventory[System.Enum.GetValues(typeof(CookItemType)).Length - 1];
        ToolItemInventory = new Inventory[System.Enum.GetValues(typeof(ToolItemType)).Length - 1];

        //Debug.Log("Player : " + System.Enum.GetValues(typeof(GatheringItemType)).Length);
        //ItemInventory ÃÊ±âÈ­
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

        //GatheringItemInventory = DatabaseManager.Instance.UserInfo.GatheringItemInventory;
        DatabaseManager.Instance.UserInfo.LoadUserInventory();
    }

    private void Start()
    {
        DataBind.SetTextValue("BambooCount", Bamboo.ToString());

    }

    //public int CurrentNotCheckedMessage {
    //    get
    //    {
    //        int count = 0;
    //        for (int i = 0; i < IsCheckMessage.Count; i++)
    //        {
    //            if (!IsCheckMessage[i])
    //                count++;
    //        }
    //        return count;
    //    }
    //}

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
            StartCoroutine(PopupCoroutine(_dontUseBambooPanel));
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
            StartCoroutine(PopupCoroutine(_dontGainBambooPanel));
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

    private IEnumerator PopupCoroutine(GameObject panel)
    {
        _popupPanel.SetActive(true);
        panel.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        panel.SetActive(false);
        _popupPanel.SetActive(false);
    }
}