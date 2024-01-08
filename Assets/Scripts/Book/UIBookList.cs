using Muks.DataBind;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookList : MonoBehaviour
{
    [SerializeField] private InventoryItemField _itemField; //현재 어떤 필드의 아이템들인지
    [SerializeField] private int _typeField; //해당 필드의 몇번째 아이템들인지
    [SerializeField] private GameObject _detailView;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Sprite[] _cardImage; //봄,여름 / 가을,겨울 / 혼합

    private List<GatheringItem> _database;
    private int _current; //현재 페이지 위치

    private void Awake()
    {
        //현재 어떤 필드의 몇 번째 아이템인지 데이터 연결
        switch (_itemField)
        {
            case InventoryItemField.GatheringItem:
                if(_typeField == 0)
                {
                    _database = DatabaseManager.Instance.GetBugItemList();
                }
                else if(_typeField == 1)
                {
                    _database = DatabaseManager.Instance.GetFishItemList();
                }
                else if(_typeField == 2)
                {
                    _database = DatabaseManager.Instance.GetFruitItemList();
                }
                else
                {
                    Debug.Log("해당 데이터가 없습니다.");
                }
            break;
            case InventoryItemField.Cook:
                break;
            case InventoryItemField.Tool:
                break;
        }

        for(int i=0;i<transform.childCount;i++)
        {
            int index = i;
            Button button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(()=> OnClickDetailView(index));
        }
        _closeButton.onClick.AddListener(() => _detailView.SetActive(false));
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
                if (_database[(_current * 9) + i].Season.Equals("WSP") || _database[(_current * 9) + i].Season.Equals("WSU") || _database[(_current * 9) + i].Season.Equals("WSS")) //봄,여름,봄/여름
                {
                    child.GetComponent<Image>().sprite = _cardImage[0];
                }
                else if (_database[(_current * 9) + i].Season.Equals("WFA") || _database[(_current * 9) + i].Season.Equals("WWT") || _database[(_current * 9) + i].Season.Equals("WFW")) //가을,겨울,가을/겨울
                {
                    child.GetComponent<Image>().sprite = _cardImage[0];
                }
                else if (_database[(_current * 9) + i].Season.Equals("WAS") || _database[(_current * 9) + i].Season.Equals("WSF") || _database[(_current * 9) + i].Season.Equals("WWS")) //봄/여름/가을/겨울,여름/가을,겨울/봄
                {
                    child.GetComponent<Image>().sprite = _cardImage[0];
                }

                //활성화
                if (_database[(_current * 9) + i].IsReceived)
                {
                    child.GetChild(0).GetComponent<Image>().enabled = true;
                    child.GetChild(0).GetComponent<Image>().sprite = _database[(_current*9) + i].Image;
                    child.GetComponent<Button>().interactable = true;
                }
                else
                {
                    child.GetChild(0).GetComponent<Image>().enabled = false;
                    child.GetChild(0).GetComponent<Image>().sprite = null;
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
                _current = _database.Count/9;
            }
        }
        UpdateContents();
    }

    private void OnClickDetailView(int index)
    {
        _detailView.SetActive(true);
        GetContent(index);
    }

    private void GetContent(int index)
    {
        DataBind.SetTextValue("BookItemDetailName", _database[(_current * 9) + index].Name);
        DataBind.SetTextValue("BookItemDetailDescription", _database[(_current * 9) + index].Description);
        DataBind.SetSpriteValue("BookItemDetailImage", _database[(_current * 9) + index].Image);
    }

}
