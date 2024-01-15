using Muks.DataBind;
using System;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMailList : MonoBehaviour
{
    public Action NoticeHandler;

    [SerializeField] private GameObject _messageSlotPf;
    [SerializeField] private GameObject _detailView;
    [SerializeField] private GameObject _giftDetailView;
    [SerializeField] private GameObject _giftButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _giftCloseButton;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Sprite[] _checkMailImage;

    private MessageList _mailList;
    private int _currentIndex;

    private void Awake()
    {
        //Test 
        //GameManager.Instance.Player.Messages[0].AddById(DatabaseManager.Instance.GetMailList()[0].Id, MessageField.Mail); //ML01 메일 추가
        //GameManager.Instance.Player.Messages[0].AddById(DatabaseManager.Instance.GetMailList()[1].Id, MessageField.Mail); //ML01 메일 추가
        //GameManager.Instance.Player.Messages[0].AddById(DatabaseManager.Instance.GetMailList()[2].Id, MessageField.Mail); //ML01 메일 추가
        //GameManager.Instance.Player.Messages[0].AddById(DatabaseManager.Instance.GetMailList()[3].Id, MessageField.Mail); //ML01 메일 추가
        for (int i = 0; i < GameManager.Instance.Player.Messages[0].MaxMessageCount; i++) //미리 slot 생성
        {
            int index = GameManager.Instance.Player.Messages[0].MaxMessageCount - 1 - i;
            GameObject messageSlot = Instantiate(_messageSlotPf, _spawnPoint);
            messageSlot.GetComponent<Button>().onClick.AddListener(()=>OnClickMessageSlot(index));
        }
    }

    private void OnEnable()
    {
        UpdateList();
        
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(OnClickCloseButton);
        _giftCloseButton.onClick.AddListener(OnClickGiftCloseButton);
    }

    private void UpdateList()
    {
        _mailList = GameManager.Instance.Player.Messages[0]; //메시지리스트 받아옴
        Debug.Log("메일: " + _mailList.MessagesCount);
        for (int i = 0; i < _spawnPoint.childCount; i++)
        {
            if (i < _mailList.MessagesCount)
            {
                _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).gameObject.SetActive(true); //활성화
                if (!_mailList.GetMessageList()[i].IsCheck)
                {
                    _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).GetChild(0).GetComponent<Image>().sprite = _checkMailImage[0];
                    _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).GetChild(0).GetChild(0).gameObject.SetActive(true); //느낌표
                }
                else
                {
                    _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).GetChild(0).GetComponent<Image>().sprite = _checkMailImage[1];
                    _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).GetChild(0).GetChild(0).gameObject.SetActive(false); //느낌표

                }
                _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).GetChild(1).GetComponent<TextMeshProUGUI>().text = GetNPCName(_mailList.GetMessageList()[i].From);//NPC 이름
            }
            else
            {
                _spawnPoint.GetChild((_spawnPoint.childCount - 1) - i).gameObject.SetActive(false);
            }
        }
    }

    private void OnClickCloseButton()
    {
        _detailView.SetActive(false);
        GameManager.Instance.Player.Messages[0] = _mailList; //정보 저장
        UpdateList();
    }

    private void OnClickGiftCloseButton()
    {
        _giftDetailView.SetActive(false);
    }

    private void OnClickMessageSlot(int index)
    {
        DataBind.SetTextValue("MailDetailContent", _mailList.GetMessageList()[index].Content);
        DataBind.SetTextValue("MailDetailFrom", "From. " + GetNPCName(_mailList.GetMessageList()[index].From));
        DataBind.SetSpriteValue("MailDetailImage", _mailList.GetMessageList()[index].PaperImage);
        DataBind.SetButtonValue("MailDetailGiftButton", OnClickGiftButton);
        
        _mailList.GetMessageList()[index].IsCheck = true;
        NoticeHandler?.Invoke();

        _detailView.gameObject.SetActive(true);

        if (_mailList.GetMessageList()[index].IsReceived)
        {
            _giftButton.SetActive(false);
        }
        else
        {
            _giftButton.SetActive(true);
        }

        _currentIndex = index;
    }

    private string GetNPCName(string id)
    {
        for(int i=0;i<DatabaseManager.Instance.NPCDatabase.NpcList.Count;i++)
        {
            if (DatabaseManager.Instance.NPCDatabase.NpcList[i].Id.Equals(id))
            {
                return DatabaseManager.Instance.NPCDatabase.NpcList[i].Name;
            }
        }
        return null;
    }

    private void OnClickGiftButton()
    {
        AddGift(_currentIndex);
        SetDetail(_currentIndex);
        _giftDetailView.SetActive(true);
        _giftButton.SetActive(false);
    }

    private void AddGift(int index)
    {
        string giftId = _mailList.GetMessageList()[index].Gift.Id;
        giftId = giftId.Substring(0, 3);
        switch (giftId)
        {
            case "IBG":
                GameManager.Instance.Player.GatheringItemInventory[0].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Bug, _mailList.GetMessageList()[index].Gift.Id);
                break;
            case "IFI":
                GameManager.Instance.Player.GatheringItemInventory[1].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Fish, _mailList.GetMessageList()[index].Gift.Id);
                break;
            case "IFR":
                GameManager.Instance.Player.GatheringItemInventory[2].AddById(InventoryItemField.GatheringItem, (int)GatheringItemType.Fruit, _mailList.GetMessageList()[index].Gift.Id);
                break;
            case "ITG":
                GameManager.Instance.Player.ToolItemInventory[0].AddById(InventoryItemField.Tool, (int)ToolItemType.GatheringTool, _mailList.GetMessageList()[index].Gift.Id);
                break;
        }
        Debug.Log("receiveIndex : " + index);
        _mailList.GetMessageList()[index].IsReceived = true;
    }

    private void SetDetail(int index)
    {
        DataBind.SetSpriteValue("MailGiftDetailImage", _mailList.GetMessageList()[index].Gift.Image);
        DataBind.SetTextValue("MailGiftDetailName", _mailList.GetMessageList()[index].Gift.Name);
        DataBind.SetTextValue("MailGiftDetailDescription", _mailList.GetMessageList()[index].Gift.Description);
    }
}
