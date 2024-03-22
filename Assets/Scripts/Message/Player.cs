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


    [Header("Sticker")]
    public StickerList StickerInventory = new StickerList();
    public List<StickerData> StickerPosList = new List<StickerData>();

    public int Bamboo { get; private set; }
    public int MaxBamboo => 10000;

    public void Init()
    {
        MailInit();
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
   
        //DatabaseManager.Instance.UserInfo.LoadUserReceivedSticker(); //sticker inventory
        //DatabaseManager.Instance.UserInfo.LoadUserStickerData(); //sticker pos

        DataBind.SetTextValue("BambooCount", Bamboo.ToString());


    }


    public bool SpendBamboo(int amount)
    {
        if(0 <= Bamboo - amount) 
        {
            Bamboo -= amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            SaveBambooData(3);
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
            DatabaseManager.Instance.Challenges.StackedBambooCount += amount; // �������� �޼� üũ
            SaveBambooData(3);
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

        Debug.LogErrorFormat("{0} id�� ���� �������� �������� �ʽ��ϴ�.");
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

        Debug.LogErrorFormat("{0} id�� ���� �������� �������� �ʽ��ϴ�.");
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


    #region Mail

    private MessageList[] _mailLists = new MessageList[Enum.GetValues(typeof(MessageField)).Length - 1];
    private List<string> _mailRecivedList = new List<string>();

    private List<MessageData> MessageDataArray = new List<MessageData>(); //������ �޽��� ������


    public class MessageData
    {
        public string Id;
        public bool IsCheck;
        public bool IsReceived;
        public MessageData(string id, bool isCheck, bool isReceived)
        {
            Id = id;
            IsCheck = isCheck;
            IsReceived = isReceived;
        }
    }

    public enum MailType
    {
        Mail,
        Wish
    }


    public MessageList GetMailList(MailType mailType)
    {
        return _mailLists[(int)mailType];
    }


    private void MailInit()
    {
        //Message
        for (int i = 0; i < _mailLists.Length - 1; i++)
        {
            _mailLists[i] = new MessageList();
        }
    }


    public void LoadMailData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {

            for (int i = 0, count = json[0]["MessageDataArray"].Count; i < count; i++)
            {
                MessageData item = JsonUtility.FromJson<MessageData>(json[0]["MessageDataArray"][i].ToJson());
                MessageDataArray.Add(item);
            }
            LoadUserMailData();

            for (int i = 0, count = json[0]["MailRecivedList"].Count; i < count; i++)
            {
                string id = json[0]["MailRecivedList"].ToString();
                _mailRecivedList.Add(id);
            }


            Debug.Log("Mail Load����");
        }
    }


    public void SaveMailData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Mail";

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
                SaveMailData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertMailData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateMailData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertMailData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    public void InsertMailData(string selectedProbabilityFileId)
    {
        SaveUserMailData();

        Param param = GetMailParam();

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.",  selectedProbabilityFileId);

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    public void UpdateMailData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserMailData();
        SaveMailReceived();

        Param param = GetMailParam();

        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }



    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    public Param GetMailParam()
    {
        Param param = new Param();
        param.Add("MessageDataArray", MessageDataArray);
        param.Add("MailRecivedList", _mailRecivedList);
        return param;
    }


    public void SaveUserMailData()
    {
        MessageDataArray.Clear();

        for (int i = 0; i < _mailLists.Length; i++)
        {
            if (_mailLists[i] == null)
                continue;

            List<Message> mailList = _mailLists[i].GetMessageList();
            for (int j = 0; j < _mailLists[i].MessagesCount; j++)
            {
                string id = mailList[j].Id;
                bool isCheck = mailList[j].IsCheck;
                bool isReceived = mailList[j].IsReceived;

                MessageDataArray.Add(new MessageData(id, isCheck, isReceived));
            }

        }
    }


    public void LoadUserMailData()
    {
        for (int i = 0; i < MessageDataArray.Count; i++)
        {
            if (MessageDataArray[i].Id.StartsWith("ML"))
            {
                _mailLists[0].AddById(MessageDataArray[i].Id, MessageField.Mail, false);
                _mailLists[0].GetMessageList()[i].IsCheck = MessageDataArray[i].IsCheck;
                _mailLists[0].GetMessageList()[i].IsReceived = MessageDataArray[i].IsReceived;
            }
        }
    }


    public bool FindMailReceivedById(string id)
    {
        for(int i = 0, count = _mailRecivedList.Count; i < count; i++)
        {
            if (_mailRecivedList[i] == id)
                return true;
        }

        return false;
    }


    public void SaveMailReceived()
    {
        List<Message> mailList = new List<Message>();
        mailList.AddRange(_mailLists[(int)MailType.Mail].GetMessageList());
        for (int i = 0, count = mailList.Count; i < count; i++)
        {
            if (_mailRecivedList.Find((x) => x == mailList[i].Id) != null)
                continue;

            _mailRecivedList.Add(mailList[i].Id);
        }
    }


    public void AddMailReceived(string id)
    {
        UnityEngine.Debug.Log(id);

        if (_mailRecivedList.Find((x) => x == id) != null)
        {
            Debug.Log("�̹� �����ϴ� ���� �Դϴ�.");
            return;
        }
            
        _mailRecivedList.Add(id);
    }


    #endregion
}

