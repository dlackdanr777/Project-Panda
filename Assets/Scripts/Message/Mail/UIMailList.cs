using Muks.DataBind;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMailList : MonoBehaviour
{
    public Action NoticeHandler;

    [Header("Components")]
    [SerializeField] private GameObject _detailView;
    [SerializeField] private GameObject _giftDetailView;
    [SerializeField] private GameObject _giftButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _giftCloseButton;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Sprite[] _checkMailImage;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private UIMailSlot _mailSlotPrefab;

    private MessageList _mailList;
    private List<UIMailSlot> _slotList = new List<UIMailSlot>();
    private int _currentIndex;

    private void Awake()
    {
        for (int i = 0, count = GameManager.Instance.Player.GetMailList(Player.MailType.Mail).MaxMessageCount; i < count; i++) //미리 slot 생성
        {
            int index = GameManager.Instance.Player.GetMailList(Player.MailType.Mail).MaxMessageCount - 1 - i;
            UIMailSlot mailSlot = Instantiate(_mailSlotPrefab, _spawnPoint);
            mailSlot.Init(() => OnClickMessageSlot(index));

           _slotList.Add(mailSlot);
        }

    }


    private void OnEnable()
    {
        UpdateList();
        _detailView.SetActive(false);
    }


    private void Start()
    {
        _closeButton.onClick.AddListener(OnClickCloseButton);
        _giftCloseButton.onClick.AddListener(OnClickGiftCloseButton);
    }


    private void UpdateList()
    {
        _mailList = GameManager.Instance.Player.GetMailList(Player.MailType.Mail); //메시지리스트 받아옴

        NoticeHandler?.Invoke();
        for (int i = 0, count = _slotList.Count; i < count; i++)
        {
            if (i < _mailList.MessagesCount)
            {
                _slotList[count - 1 - i].gameObject.SetActive(true); //활성화

                //메일을 안 읽은 상태면?
                if (!_mailList.GetMessageList()[i].IsCheck)
                {
                    _slotList[count - 1 - i].SetMailImage(_checkMailImage[0]);
                    _slotList[count - 1 - i].SetActiveCheckImage(true);
                }
                else
                {
                    _slotList[count - 1 - i].SetMailImage(_checkMailImage[1]);
                    _slotList[count - 1 - i].SetActiveCheckImage(false); //느낌표

                }

                string npcId = _mailList.GetMessageList()[i].From;
                string npcName = DatabaseManager.Instance.NPCDatabase.NpcDic[npcId].Name;
                _slotList[count - 1 - i].SetNpcNameText(npcName);
            }
            else
            {
                _slotList[count - 1 - i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClickCloseButton()
    {
        _detailView.SetActive(false);
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
        DataBind.SetUnityActionValue("MailDetailGiftButton", OnClickGiftButton);

        if (!_mailList.GetMessageList()[index].IsCheck)
        {
            GameManager.Instance.Player.SaveMailData(3);
            _mailList.GetMessageList()[index].IsCheck = true;
        }


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
        GameManager.Instance.Player.AddItemById(_mailList.GetMessageList()[index].Gift.Id);
        _mailList.GetMessageList()[index].IsReceived = true;
        GameManager.Instance.Player.SaveMailData(3);
    }

    private void SetDetail(int index)
    {
        DataBind.SetSpriteValue("MailGiftDetailImage", _mailList.GetMessageList()[index].Gift.Image);
        DataBind.SetTextValue("MailGiftDetailName", _mailList.GetMessageList()[index].Gift.Name);
        DataBind.SetTextValue("MailGiftDetailDescription", _mailList.GetMessageList()[index].Gift.Description);
    }
}
