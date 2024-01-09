using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryContent : UIList<InventoryItem, InventoryItemField>
{
    private int _currentItemIndex;

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("InventoryDetailName", _lists[(int)_currentField][index].Name);
        DataBind.SetTextValue("InventoryDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetSpriteValue("InventoryDetailImage", _lists[(int)_currentField][index].Image);
    }

    protected override void UpdateListSlots()
    {
        UpdateList();

        if (_lists[(int)_currentField] != null)
        {
            for (int j = 0; j < _maxCount[(int)_currentField]; j++) //���� player�� �κ��丮�� ����� ������ ����
            {
                if (j < _lists[(int)_currentField].Count)
                {
                    Transform prefab = _spawnPoint[(int)_currentField].GetChild(j).GetChild(0);
                    prefab.gameObject.SetActive(true); //���� ���� => Getchild�� ��������
                    prefab.GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                    if(_lists[(int)_currentField][j].Count > 1) //1�̻� 
                    {
                        prefab.GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();
                    }

                }
                else
                {
                    _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).gameObject.SetActive(false);


                }
            }
        }
    }

    private void Awake()
    {
        //Test
        GameManager.Instance.Player.GatheringItemInventory[0].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Bug, "IBG03");
        GameManager.Instance.Player.GatheringItemInventory[0].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Bug, "IBG03");
        GameManager.Instance.Player.GatheringItemInventory[1].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Fish, "IFI04");
        GameManager.Instance.Player.GatheringItemInventory[2].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Fruit, "IFR05");
        GameManager.Instance.Player.GatheringItemInventory[1].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Fish, "IFI04");
        GameManager.Instance.Player.GatheringItemInventory[1].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Fish, "IFI24");

        //�̸� ���� => spawn ����ϸ� ������
        for (int i = 0; i < System.Enum.GetValues(typeof(InventoryItemField)).Length - 1; i++)
        {
            Inventory[] itemInventory = GameManager.Instance.Player.GetItemInventory((InventoryItemField)i);
            if(itemInventory != null)
            {
                _maxCount[i] = 0;
                _lists[i] = new List<InventoryItem>();

                for (int j = 0; j < itemInventory.Length; j++)
                {
                    _maxCount[i] += itemInventory[j].MaxInventoryItem;
                    if (itemInventory[j] != null)
                    {
                        _lists[i].AddRange(itemInventory[j].GetInventoryList());//Player�� �ִ� �κ��丮 ���� -> ����� ������ �̺�Ʈ�� ui ��������� ��
                    }
                }

            }
            
        }
        Init();
    }

    private void UpdateList() 
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(InventoryItemField)).Length - 1; i++)
        {
            Inventory[] itemInventory = GameManager.Instance.Player.GetItemInventory((InventoryItemField)i);
            if (itemInventory != null)
            {
                _lists[i] = new List<InventoryItem>();
                for (int j = 0; j < itemInventory.Length; j++)
                {
                    if (itemInventory[j] != null)
                    {
                        _lists[i].AddRange(itemInventory[j].GetInventoryList());//Player�� �ִ� �κ��丮 ���� -> ����� ������ �̺�Ʈ�� ui ��������� ��
                    }
                }
            }
        }
    }
}
