using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BookField
{
    None = -1,
    Item,
    NPC
}

public class UIBookList : MonoBehaviour
{
    [SerializeField] private BookField _bookField;
    [SerializeField] private InventoryItemField _itemField; //���� � �ʵ��� �����۵�����
    [SerializeField] private int _typeField; //�ش� �ʵ��� ���° �����۵�����
    [SerializeField] private UIDetailView _detailView;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Sprite[] _cardImage; //��,���� / ����,�ܿ� / ȥ��
    
    private Action _onDetailViewClickHandler;
    private List<GatheringItem> _gatheringDatabase;
    private List<ToolItem> _toolDatabase;
    private List<NPC> _npcDatabase;
    private List<Item> _database;
    private int _current; //���� ������ ��ġ
    

    public bool AlarmCheck()
    {
        for(int i = 0, count = _database.Count; i < count; i++)
        {
            if (_database[i].DiaryAlarmCheck)
                return true;
        }

        return false;
    }



    public void Init(Action onDetailViewClicked)
    {
        _detailView.Init();

        _onDetailViewClickHandler += onDetailViewClicked;

        //���� � �ʵ��� �� ��° ���������� ������ ����
        if (_bookField == BookField.Item)
        {
            switch (_itemField)
            {
                case InventoryItemField.GatheringItem:
                    if (_typeField == 0)
                    {
                        _gatheringDatabase = DatabaseManager.Instance.ItemDatabase.ItemBugList;
                    }
                    else if (_typeField == 1)
                    {
                        _gatheringDatabase = DatabaseManager.Instance.ItemDatabase.ItemFishList;
                    }
                    else if (_typeField == 2)
                    {
                        _gatheringDatabase = DatabaseManager.Instance.ItemDatabase.ItemFruitList;
                    }
                    else
                    {
                        Debug.Log("�ش� �����Ͱ� �����ϴ�.");
                    }
                    _database = GetDatabase(_gatheringDatabase);
                    break;
                case InventoryItemField.Cook:
                    break;
                case InventoryItemField.Tool:
                    if (_typeField == 0)
                    {
                        _toolDatabase = DatabaseManager.Instance.ItemDatabase.ItemToolList;
                    }
                    else
                    {
                        Debug.Log("�ش� �����Ͱ� �����ϴ�.");
                    }
                    _database = GetDatabase(_toolDatabase);
                    break;
            }
        }
        else if(_bookField == BookField.NPC)
        {
            if (_typeField == 0)
            {
                _npcDatabase = DatabaseManager.Instance.GetNPCList();
            }
            else
            {
                Debug.Log("�ش� �����Ͱ� �����ϴ�.");
            }
            _database = GetDatabase(_npcDatabase);
        }
   
        for(int i=0;i<transform.childCount;i++)
        {
            int index = i;
            Button button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(()=> OnClickDetailView(index));
        }
        _leftButton.onClick.AddListener(()=>OnClickPageButton(0));
        _rightButton.onClick.AddListener(()=>OnClickPageButton(1));

    }

    private void OnEnable()
    {
        _current = 0;

        if (_database == null)
            return;

        UpdateContents();
    }

    private void OnDisable()
    {
        _detailView.gameObject.SetActive(false);
    }

    private void UpdateContents()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if ((_current*9) + i >= _database.Count)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                //ī�� UI ����
                if(_itemField == InventoryItemField.GatheringItem)
                {
                    if (_gatheringDatabase[(_current * 9) + i].Season.Equals("WSP") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WSU") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WSS")) //��,����,��/����
                    {
                        child.GetComponent<Image>().sprite = _cardImage[0];
                    }
                    else if (_gatheringDatabase[(_current * 9) + i].Season.Equals("WFA") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WWT") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WFW")) //����,�ܿ�,����/�ܿ�
                    {
                        child.GetComponent<Image>().sprite = _cardImage[1];
                    }
                    else if (_gatheringDatabase[(_current * 9) + i].Season.Equals("WAS") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WSF") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WWS")) //��/����/����/�ܿ�,����/����,�ܿ�/��
                    {
                        child.GetComponent<Image>().sprite = _cardImage[2];
                    }
                }

                //Ȱ��ȭ
                if (_database[(_current * 9) + i].IsReceived)
                {
                    child.GetChild(0).GetComponent<Image>().enabled = true;
                    child.GetChild(0).GetComponent<Image>().sprite = _database[(_current*9) + i].Image;
                    child.GetChild(0).GetComponent<Image>().preserveAspect = true;
                    child.GetChild(1).GetComponent<TextMeshProUGUI>().text = _database[(_current * 9) + i].Name;
                    child.GetComponent<Button>().interactable = true;

                    if(_database[(_current * 9) + i].DiaryAlarmCheck)
                        child.GetChild(2).gameObject.SetActive(true);

                    else
                        child.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    child.GetChild(0).GetComponent<Image>().enabled = false;
                    child.GetChild(0).GetComponent<Image>().sprite = null;
                    child.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    child.GetChild(2).gameObject.SetActive(false);

                    child.GetComponent<Button>().interactable = false;
                }

            }
        }
    }

    private void OnClickPageButton(int arrow) //arrow: 0 �̸� ����, 1�̸� ������
    {
        if(arrow == 0)
        {
            _current--;
            if(_current < 0)
            {
                _current = 0;
            }
        }
        else if(arrow == 1)
        {
            _current++;
            if(_current > _database.Count / 9)
            {
                _current = _database.Count / 9;
            }
            else if(_current == _database.Count / 9)
            {
                if (_database.Count % 9 == 0)
                {
                    _current = _database.Count / 9 - 1;
                }
            }
        }
        UpdateContents();
    }

    private void OnClickDetailView(int index)
    {
        _detailView.Show(_database[(_current * 9) + index]);
        _database[(_current * 9) + index].DiaryAlarmCheck = false;
        transform.GetChild(index).GetChild(2).gameObject.SetActive(false);
        _onDetailViewClickHandler?.Invoke();
    }


    private List<Item> GetDatabase(List<GatheringItem> gatheringItemList)
    {
        List<Item> itemList = new List<Item>();
        itemList.AddRange(gatheringItemList);
        return itemList;
    }
    private List<Item> GetDatabase(List<ToolItem> toolItemList)
    {
        List<Item> itemList = new List<Item>();
        itemList.AddRange(toolItemList);
        return itemList;
    }

    private List<Item> GetDatabase(List<NPC> npcList)
    {
        List<Item> itemList = new List<Item>();
        itemList.AddRange(npcList);
        return itemList;
    }
}
