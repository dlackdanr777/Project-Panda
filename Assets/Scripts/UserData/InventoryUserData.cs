using BackEnd;
using LitJson;
using Muks.BackEnd;
using System.Collections.Generic;
using UnityEngine;


/// <summary> ������ �ѱ� �κ��丮 ������ ������ </summary>
public class InventoryServerSaveData
{
    public string Id;
    public int Count;
}


/// <summary> ������ �κ��丮 ������ �����ϴ� Ŭ���� </summary>
public class InventoryUserData
{
    private List<InventoryServerSaveData> GatheringInventoryDataArray = new List<InventoryServerSaveData>(); //������ �κ��丮 ������
    private List<InventoryServerSaveData> CookInventoryDataArray = new List<InventoryServerSaveData>();
    private List<InventoryServerSaveData> ToolInventoryDataArray = new List<InventoryServerSaveData>();

    //Item
    private List<string> GatheringItemReceived = new List<string>();
    private List<string> CookItemReceived = new List<string>();
    public List<string> ToolItemReceived = new List<string>();


    #region SaveAndLoadInventory

    public void LoadInventoryData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            return;
        }

        else
        {
            for (int i = 0; i < json[0]["GatheringInventoryDataArray"].Count; i++)
            {
                InventoryServerSaveData item = JsonUtility.FromJson<InventoryServerSaveData>(json[0]["GatheringInventoryDataArray"][i].ToJson());
                GatheringInventoryDataArray.Add(item);
            }

            for (int i = 0, count = json[0]["CookInventoryDataArray"].Count; i < count; i++)
            {
                InventoryServerSaveData item = JsonUtility.FromJson<InventoryServerSaveData>(json[0]["CookInventoryDataArray"][i].ToJson());
                CookInventoryDataArray.Add(item);
            }

            for (int i = 0, count = json[0]["ToolInventoryDataArray"].Count; i < count; i++)
            {
                InventoryServerSaveData item = JsonUtility.FromJson<InventoryServerSaveData>(json[0]["ToolInventoryDataArray"][i].ToJson());
                ToolInventoryDataArray.Add(item);
            }

            for (int i = 0, count = json[0]["GatheringItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["GatheringItemReceived"][i].ToString();
                GatheringItemReceived.Add(item);
            }

            for (int i = 0, count = json[0]["CookItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["CookItemReceived"][i].ToString();
                CookItemReceived.Add(item);
            }

            for (int i = 0, count = json[0]["ToolItemReceived"].Count; i < count; i++)
            {
                string item = json[0]["ToolItemReceived"][i].ToString();
                ToolItemReceived.Add(item);
            }

            LoadUserInventory();
            LoadGatheringItemReceived();
            LoadFoodItemReceived();
            LoadToolItemReceived();
            Debug.Log("Inventory Load����");
        }
    }


    /// <summary>������ �κ��丮 �����͸� ���������� �Է��ϴ� �Լ�</summary>
    public void SaveInventoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Inventory";

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
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveInventoryData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertInventoryData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateInventoryData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertInventoryData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary>������ �κ��丮 �����͸� �񵿱������� �Է��ϴ� �Լ�</summary>
    public void AsyncSaveInventoryData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Inventory";

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

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveInventoryData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertInventoryData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateInventoryData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertInventoryData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0}������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        }); 
    }


    /// <summary> ���� �κ��丮 �����͸� ���������� �����ϴ� �Լ� </summary>

    private void InsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� �κ��丮 �����͸� �񵿱������� �����ϴ� �Լ� </summary>

    private void AsyncInsertInventoryData(string selectedProbabilityFileId)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���� �κ��丮 �����͸� ���������� �����ϴ� �Լ� </summary>
    private void UpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ���� �κ��丮 �����͸� �񵿱������� �����ϴ� �Լ� </summary>
    private void AsyncUpdateInventoryData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserInventory();
        SaveGatheringItemReceived();
        SaveFoodItemReceived();
        SaveToolItemReceived();
        Param param = GetInventoryParam();

        Debug.LogFormat("�κ��丮 ������ ������ ��û�մϴ�.");

        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> ������ ������ �κ��丮 �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
    private Param GetInventoryParam()
    {
        Param param = new Param();

        param.Add("GatheringInventoryDataArray", GatheringInventoryDataArray);
        param.Add("CookInventoryDataArray", CookInventoryDataArray);
        param.Add("ToolInventoryDataArray", ToolInventoryDataArray);

        param.Add("GatheringItemReceived", GatheringItemReceived);
        param.Add("CookItemReceived", CookItemReceived);
        param.Add("ToolItemReceived", ToolItemReceived);

        return param;
    }

    #endregion

    #region Inventory


    private void LoadUserInventory()
    {
        for (int i = 0; i < GatheringInventoryDataArray.Count; i++) //����� ������
        {
            InventoryServerSaveData data = GatheringInventoryDataArray[i];
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None, false);
        }

        for (int i = 0, count = CookInventoryDataArray.Count; i < count; i++)
        {
            InventoryServerSaveData data = CookInventoryDataArray[i];
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None, false);
        }

        for (int i = 0, count = ToolInventoryDataArray.Count; i < count; i++)
        {
            InventoryServerSaveData data = ToolInventoryDataArray[i];
            GameManager.Instance.Player.AddItemById(data.Id, data.Count, ItemAddEventType.None, false);
        }
    }


    private void SaveUserInventory()
    {
        Inventory[] GatheringItemInventory = GameManager.Instance.Player.GatheringItemInventory;
        Inventory[] CookItemInventory = GameManager.Instance.Player.CookItemInventory;
        Inventory[] ToolItemInventory = GameManager.Instance.Player.ToolItemInventory;

        GatheringInventoryDataArray.Clear(); //�ִ� ������ ����� �ű⿡ ����
        for (int i = 0; i < GatheringItemInventory.Length; i++)
        {
            List<InventoryItem> itemList = GatheringItemInventory[i].GetItemList();

            for (int j = 0; j < GatheringItemInventory[i].ItemsCount; j++)
            {
                InventoryServerSaveData data = new InventoryServerSaveData();
                data.Id = itemList[j].Id;
                data.Count = itemList[j].Count;
                GatheringInventoryDataArray.Add(data);
            }
        }

        int index = (int)CookItemType.CookFood;
        CookInventoryDataArray.Clear(); //�ִ� ������ ����� �ű⿡ ����
        List<InventoryItem> cookInventory = CookItemInventory[index].GetItemList();

        for (int i = 0; i < cookInventory.Count; i++)
        {
            InventoryServerSaveData data = new InventoryServerSaveData();
            data.Id = cookInventory[i].Id;
            data.Count = cookInventory[i].Count;
            CookInventoryDataArray.Add(data);
        }

        index = (int)ToolItemType.GatheringTool;
        ToolInventoryDataArray.Clear(); //�ִ� ������ ����� �ű⿡ ����
        List<InventoryItem> toolInventory = ToolItemInventory[index].GetItemList();

        for (int i = 0; i < toolInventory.Count; i++)
        {
            InventoryServerSaveData data = new InventoryServerSaveData();
            data.Id = toolInventory[i].Id;
            data.Count = 1;
            ToolInventoryDataArray.Add(data);
        }

    }
    #endregion

    #region Item

    private void SaveGatheringItemReceived()
    {
        List<InventoryItem> _items = new List<InventoryItem>();
        for (int i = 0, count = GameManager.Instance.Player.GatheringItemInventory.Length; i < count; i++)
        {
            _items.AddRange(GameManager.Instance.Player.GatheringItemInventory[i].GetItemList());
        }

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (GatheringItemReceived.Find((x) => x == _items[i].Id) != null)
            {
                continue;
            }

            GatheringItemReceived.Add(_items[i].Id);
        }
    }


    private void LoadGatheringItemReceived()
    {
        List<Item> _items = new List<Item>();
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemBugList);
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFruitList);
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFishList);

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (GatheringItemReceived.Find((x) => x == _items[i].Id) == null)
            {
                continue;
            }

            _items[i].IsReceived = true;
        }
    }


    private void SaveFoodItemReceived()
    {
        List<InventoryItem> _items = new List<InventoryItem>();
        for (int i = 0, count = GameManager.Instance.Player.CookItemInventory.Length; i < count; i++)
        {
            _items.AddRange(GameManager.Instance.Player.CookItemInventory[i].GetItemList());
        }

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (CookItemReceived.Find((x) => x == _items[i].Id) != null)
                continue;

            CookItemReceived.Add(_items[i].Id);
        }
    }


    private void LoadFoodItemReceived()
    {
        List<Item> _items = new List<Item>();
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemFoodList);

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (CookItemReceived.Find((x) => x == _items[i].Id) == null)
                continue;

            _items[i].IsReceived = true;
        }
    }


    private void SaveToolItemReceived()
    {
        List<InventoryItem> _items = new List<InventoryItem>();
        for (int i = 0, count = GameManager.Instance.Player.ToolItemInventory.Length; i < count; i++)
        {
            _items.AddRange(GameManager.Instance.Player.ToolItemInventory[i].GetItemList());
        }

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (ToolItemReceived.Find((x) => x == _items[i].Id) != null)
                continue;

            ToolItemReceived.Add(_items[i].Id);
        }
    }


    private void LoadToolItemReceived()
    {
        List<Item> _items = new List<Item>();
        _items.AddRange(DatabaseManager.Instance.ItemDatabase.ItemToolList);

        for (int i = 0, count = _items.Count; i < count; i++)
        {
            if (ToolItemReceived.Find((x) => x == _items[i].Id) == null)
                continue;

            _items[i].IsReceived = true;
        }
    }

    #endregion
}
