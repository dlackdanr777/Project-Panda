using Muks.DataBind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private InventoryItemField _itemField; //현재 어떤 필드의 아이템들인지
    [SerializeField] private int _typeField; //해당 필드의 몇번째 아이템들인지
    [SerializeField] private GameObject _detailView;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Sprite[] _cardImage; //봄,여름 / 가을,겨울 / 혼합

    private List<GatheringItem> _gatheringDatabase;
    private List<ToolItem> _toolDatabase;
    private List<NPC> _npcDatabase;
    private List<Item> _database;
    private int _current; //현재 페이지 위치

    private void Awake()
    {
        //현재 어떤 필드의 몇 번째 아이템인지 데이터 연결
        if(_bookField == BookField.Item)
        {
            switch (_itemField)
            {
                case InventoryItemField.GatheringItem:
                    if (_typeField == 0)
                    {
                        _gatheringDatabase = DatabaseManager.Instance.GetBugItemList();
                    }
                    else if (_typeField == 1)
                    {
                        _gatheringDatabase = DatabaseManager.Instance.GetFishItemList();
                    }
                    else if (_typeField == 2)
                    {
                        _gatheringDatabase = DatabaseManager.Instance.GetFruitItemList();
                    }
                    else
                    {
                        Debug.Log("해당 데이터가 없습니다.");
                    }
                    _database = GetDatabase(_gatheringDatabase);
                    break;
                case InventoryItemField.Cook:
                    break;
                case InventoryItemField.Tool:
                    if (_typeField == 0)
                    {
                        _toolDatabase = DatabaseManager.Instance.GetGatheringToolItemList();
                    }
                    else
                    {
                        Debug.Log("해당 데이터가 없습니다.");
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
                Debug.Log("해당 데이터가 없습니다.");
            }
            _database = GetDatabase(_npcDatabase);
        }
   
        for(int i=0;i<transform.childCount;i++)
        {
            int index = i;
            Button button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(()=> OnClickDetailView(index));
        }
        _closeButton.onClick.AddListener(OnClickCloseButton);
        _leftButton.onClick.AddListener(()=>OnClickPageButton(0));
        _rightButton.onClick.AddListener(()=>OnClickPageButton(1));
    }

    private void OnEnable()
    {
        _current = 0;
        UpdateContents();
    }

    private void OnDisable()
    {
        if (_detailView.activeSelf)
        {
            _detailView.SetActive(false);
        }
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
                //카드 UI 변경
                if(_itemField == InventoryItemField.GatheringItem)
                {
                    if (_gatheringDatabase[(_current * 9) + i].Season.Equals("WSP") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WSU") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WSS")) //봄,여름,봄/여름
                    {
                        child.GetComponent<Image>().sprite = _cardImage[0];
                    }
                    else if (_gatheringDatabase[(_current * 9) + i].Season.Equals("WFA") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WWT") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WFW")) //가을,겨울,가을/겨울
                    {
                        child.GetComponent<Image>().sprite = _cardImage[1];
                    }
                    else if (_gatheringDatabase[(_current * 9) + i].Season.Equals("WAS") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WSF") || _gatheringDatabase[(_current * 9) + i].Season.Equals("WWS")) //봄/여름/가을/겨울,여름/가을,겨울/봄
                    {
                        child.GetComponent<Image>().sprite = _cardImage[2];
                    }
                }

                //활성화
                if (_database[(_current * 9) + i].IsReceived)
                {
                    child.GetChild(0).GetComponent<Image>().enabled = true;
                    child.GetChild(0).GetComponent<Image>().sprite = _database[(_current*9) + i].Image;
                    child.GetChild(0).GetComponent<Image>().preserveAspect = true;
                    child.GetChild(1).GetComponent<TextMeshProUGUI>().text = _database[(_current * 9) + i].Name;
                    child.GetComponent<Button>().interactable = true;
                }
                else
                {
                    child.GetChild(0).GetComponent<Image>().enabled = false;
                    child.GetChild(0).GetComponent<Image>().sprite = null;
                    child.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    child.GetComponent<Button>().interactable = false;
                }

            }
        }
    }

    private void OnClickPageButton(int arrow) //arrow: 0 이면 왼쪽, 1이면 오른쪽
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
        GetContent(index);
        _detailView.SetActive(true);
    }

    private void OnClickCloseButton()
    {
        _detailView.SetActive(false);
        ClearContent();
    }

    private void ClearContent()
    {
        DataBind.SetTextValue("BookItemDetailName", "");
        DataBind.SetTextValue("BookItemDetailDescription", "");
        DataBind.SetSpriteValue("BookItemDetailImage", null);
    }

    private void GetContent(int index)
    {
        DataBind.SetTextValue("BookItemDetailName", _database[(_current * 9) + index].Name);
        DataBind.SetTextValue("BookItemDetailDescription", _database[(_current * 9) + index].Description);
        DataBind.SetSpriteValue("BookItemDetailImage", _database[(_current * 9) + index].Image);
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
