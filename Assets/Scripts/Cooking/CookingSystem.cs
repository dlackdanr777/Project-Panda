using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cookware
{
    Jar,
    FryingPan,
    Sizeof
}


[Serializable]
public class CookingUserData
{
    [SerializeField] private int _maxStamina;
    public int MaxStamina => _maxStamina;

    [Space]
    [SerializeField] private int[] _moreAddValue;
    [SerializeField] private int _moreAddValueStamina;

    [Space]
    [SerializeField] private int[] _addValue;
    [SerializeField] private int _addValueStamina;

    [Space]
    [SerializeField] private int[] _smallAddValue;
    [SerializeField] private int _smallAddValueStamina;

    public int[] MoreAddValue => _moreAddValue;
    public int MoreAddValueStamina => _moreAddValueStamina;
    public int[] AddValue => _addValue;
    public int AddValueStamina => _addValueStamina;
    public int[] SmallAddValue => _smallAddValue;
    public int SmallAddValueStamina => _smallAddValueStamina;
}


public class CookingSystem : MonoBehaviour
{
    [SerializeField] private UICooking _uiCooking;

    [SerializeField] private CookingUserData _userData;
    public CookingUserData UserData => _userData;

    [SerializeField] private GatheringItemType _inventoryType;
    public GatheringItemType InventoryType => _inventoryType;

    private Inventory[] _inventory => GameManager.Instance.Player.GatheringItemInventory;

    private Dictionary<Tuple<string, string>, RecipeData> _recipeDataDic => DatabaseManager.Instance.RecipeDatabase.RecipeDataDic;

    private Cookware _currentCookware;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //���� �����͸� ��Ʈ��ũ���� �޾ƿ��� ���� ������ ���� ��� ���ÿ��� ������ �޾ƿ´�.
        //���� ���� ����
        if (_recipeDataDic.Count <= 0)
        {
            DatabaseManager.Instance.RecipeDatabase.LocalRecipeParse();
            Debug.Log("������ ������ ���� ���ÿ��� ������ �޾ƿɴϴ�...");
        }
    }


    /*public RecipeData GetkRecipeByItems(InventoryItem item1, InventoryItem item2)
    {
        // �������� �������� �ʴ� ���
        if (item1 == null && item2 == null)
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
            return null;
        }

        foreach (RecipeData data in _recipeDatas)
        {
            // �� �������� ��� �����ϰ�, ��� ������ ���� 2���� ���
            if (item1 != null && item2 != null && data.MaterialItemList.Count == 2)
            {
                List<KeyValuePair<string, int>> materialList = new List<KeyValuePair<string, int>>(data.MaterialItemList);
                List<InventoryItem> itemList = new List<InventoryItem> { item1, item2 };

                foreach (var materialItem in materialList.ToList())
                {
                    foreach (var inventoryItem in itemList.ToList())
                    {
                        if (materialItem.Key == inventoryItem.Id && materialItem.Value <= inventoryItem.Count)
                        {
                            // ������ �������� ��� �ش� �������� count�� 2���� ���
                            int itemCount = (item1 == item2) ? 2 : 1;

                            if (itemCount <= inventoryItem.Count)
                            {
                                materialList.Remove(materialItem);
                                itemList.Remove(inventoryItem);
                            }
                            break;
                        }
                    }
                }

                if (materialList.Count == 0 && itemList.Count == 0)
                {
                    Debug.Log("2�� ���� ���չ� ����");
                    return data;
                }
            }

            // ��� ������ ���� 1���� ���
            if ((item1 == null || item2 == null) && data.MaterialItemList.Count == 1)
            {
                foreach (var materialItem in data.MaterialItemList)
                {
                    bool isEnabled = (item1 != null && materialItem.Key == item1.Id && materialItem.Value <= item1.Count) ||
                                     (item2 != null && materialItem.Key == item2.Id && materialItem.Value <= item2.Count);

                    if (isEnabled)
                    {
                        Debug.Log("1�� ���� ���չ� ����");
                        return data;
                    }
                }
            }
        }

        return null;
    }*/

    public RecipeData GetkRecipeByItems(InventoryItem item1, InventoryItem item2)
    {
        // �������� �������� �ʴ� ���
        if (item1 == null && item2 == null)
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
            return null;
        }

        if(item1 == item2 && item1.Count < 2)
        {
            return null;
        }

        if(GetCookwareSlotNum() == 2 && (item1 == null || item2 == null))
        {
            Debug.Log("2�� ¥������ 1�� �丮 �Ұ���");
            return null;
        }

        string item1ID = item1 != null ? item1.Id : "";
        string item2ID = item2 != null ? item2.Id : "";

        Tuple<string, string> tuple1 = Tuple.Create(item1ID, item2ID);
        Tuple<string, string> tuple2 = Tuple.Create(item2ID, item1ID);


        if (_recipeDataDic.TryGetValue(tuple1, out RecipeData recipe) || _recipeDataDic.TryGetValue(tuple2, out recipe))
        {
            return recipe;
        }

        Debug.Log("�ƹ��͵� ������..");
        return null;
    }


    public bool IsEnabledCooking(InventoryItem item1, InventoryItem item2)
    {
        RecipeData recipe = GetkRecipeByItems(item1, item2);

        if (recipe == null)
            return false;

        bool isRemovedItem1 = item1 == null ? true : false;
        bool isRemovedItem2 = item2 == null ? true : false;

        Inventory inventory = _inventory[(int)_inventoryType];

        List<InventoryItem> items = inventory.GetInventoryList();

        for (int i = 0; i < items.Count; i++)
        {
            if (!isRemovedItem1)
            {
                if (item1.Id == items[i].Id)
                {
                    isRemovedItem1 = true;
                    inventory.Remove(item1);
                }

            }

            if (!isRemovedItem2)
            {
                if (item2.Id == items[i].Id)
                {
                    isRemovedItem2 = true;
                    inventory.Remove(item2);
                }
            }

            if (isRemovedItem1 && isRemovedItem2)
            {
                Debug.Log("�ΰ��� ����");
                _uiCooking.UpdateUI();
                return true;
            }
        }

        return false;
    }


/*    public bool IsEnabledCooking(RecipeData data)
    {
        List<KeyValuePair<string, int>> tmpMaterialItemList = data.MaterialItemList.ToList();

        foreach (Inventory inventory in _inventory)
        {
            List<InventoryItem> items = inventory.GetInventoryList();

            for (int itemIndex = items.Count - 1; itemIndex >= 0; itemIndex--)
            {
                InventoryItem item = items[itemIndex];

                for (int i = tmpMaterialItemList.Count - 1; i >= 0; i--)
                {
                    var materialItem = tmpMaterialItemList[i];
                    if (item.Id == materialItem.Key)
                    {
                        if (item.Count >= materialItem.Value)
                        {
                            for (int j = 0; j < materialItem.Value; j++)
                            {
                                Debug.Log("���� ����");
                                inventory.Remove(item);
                            }
                            // ���ܽ�ų �������� �ش� ����Ʈ���� ����
                            tmpMaterialItemList.RemoveAt(i);
                        }
                        else
                        {
                            Debug.Log("��ᰡ �����մϴ�.");
                        }
                    }
                }

                if (tmpMaterialItemList.Count == 0)
                {
                    _uiCooking.UpdateUI();
                    return true;
                }
            }
        }

        return false;
    }*/



    public string CheckItemGrade(RecipeData data, float fireValue)
    {

        bool checkLevel_S = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_S) <= fireValue &&
            data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_S) >= fireValue;

        bool checkLevel_A = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_A) <= fireValue &&
            data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_A) >= fireValue;

        bool checkLevel_B = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_B) <= fireValue &&
    data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_B) >= fireValue;

        if (checkLevel_S)
        {
            return "S";
        }
        else if (checkLevel_A)
        {
            return "A";
        }
        else if (checkLevel_B)
        {
            return "B";
        }
        else
        {
            return "F";
        }
    }


    public int ChangeCookware(int value)
    {
        int tmpInt = (int)_currentCookware + value;
        tmpInt = Mathf.Clamp(tmpInt, 0, (int)Cookware.Sizeof - 1);

        _currentCookware = (Cookware)tmpInt;

        return tmpInt;
    }


    public int GetCookwareSlotNum()
    {
        return (int)_currentCookware + 1;
    }
}
