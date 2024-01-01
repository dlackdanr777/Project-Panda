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

        for (int j = 0; j < _maxCount[(int)_currentField]; j++) //���� player�� �κ��丮�� ����� ������ ����
        {
            if (j < _lists[(int)_currentField].Count)
            {
                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(true);
                _spawnPoint[(int)_currentField].GetChild(j).GetComponent<Image>().sprite = _lists[(int)_currentField][j].Image;
                _spawnPoint[(int)_currentField].GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].Count.ToString();

            }
            else
            {
                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(false);


            }
        }
    }

    private void Awake()
    {
        //�̸� ���� => spawn ����ϸ� ������
        for (int i = 0; i < System.Enum.GetValues(typeof(InventoryItemField)).Length - 1; i++)
        {
            Inventory[] itemInventory = GameManager.Instance.Player.GetItemInventory((InventoryItemField)i);
            if(itemInventory != null)
            {
                for (int j = 0; j < itemInventory.Length; j++)
                {
                    _maxCount[i] += itemInventory[j].MaxInventoryItem;
                    if (itemInventory[j].ItemsCount > 0)
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
                for (int j = 0; j < itemInventory.Length; i++)
                {
                    if (itemInventory[j].ItemsCount > 0)
                    {
                        _lists[i].AddRange(itemInventory[j].GetInventoryList());//Player�� �ִ� �κ��丮 ���� -> ����� ������ �̺�Ʈ�� ui ��������� ��
                    }
                }
            }
        }
    }
}
