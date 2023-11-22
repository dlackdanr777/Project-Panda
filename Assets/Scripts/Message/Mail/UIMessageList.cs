using JetBrains.Annotations;
using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageList : UIList<Message,MessageField>
{
    [SerializeField] private Button _removeButton;
    [SerializeField] private GameObject _notice;
    [SerializeField] private GameObject _removePopup;

    private SendMessage _sendMessage;
    private Player _player;
    private int _currentItemIndex;

    private void Awake()
    { 
        _player = GameManager.Instance.Player;
        for(int i = 0; i < _player.Messages.Length; i++)
        {
            _maxCount[i] = _player.Messages[i].MaxMessageCount;

        }
        UpdateList();
        Init();      
    }

    private void Start()
    {
        _sendMessage = transform.parent.parent.GetComponent<SendMessage>();
        _removeButton.onClick.AddListener(OnClickRemoveButton);
        _sendMessage.NoticeHandler += SendMessage_NoticeHandler;
        _removePopup.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(OnRemoveMessage);
    }

    private void UpdateList()
    {
        for (int i = 0; i < _player.Messages.Length; i++)
        {
            _lists[i] = _player.Messages[i].GetMessageList();

        }
    }
    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        DataBind.SetTextValue("MessageDetailTo", _lists[(int)_currentField][index].To);
        DataBind.SetTextValue("MessageDetailContent", _lists[(int)_currentField][index].Content);
        DataBind.SetSpriteValue("MessageDetailGift", _lists[(int)_currentField][index].Gift.Image);
    }

    protected override void UpdateListSlots()
    {
        UpdateList();

        for (int j = 0; j < _maxCount[(int)_currentField]; j++) //현재 player의 message에 저장된 개수
        {
            if (j < GameManager.Instance.Player.Messages[(int)_currentField].GetMessageList().Count)
            {
                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(true);

            }
            else
            {

                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(false);


            }
        }
    }

    private void SendMessage_NoticeHandler()
    {
        UpdateListSlots();
    }


    private void OnClickRemoveButton()
    {
        for (int i = 0; i < _maxCount[0]; i++)
        {
            GameObject message = _spawnPoint[0].GetChild(i).gameObject; //message, 위치
            if (message.activeSelf)
            {
                if (message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn)
                {
                    _removePopup.SetActive(true);
                }
            }
        }
    }

    private void OnRemoveMessage()
    {
        for (int i = 0; i < _maxCount[0]; i++)
        {
            GameObject message = _spawnPoint[0].GetChild(i).gameObject; //message, 위치
            if (message.activeSelf)
            {
                if (message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn)
                {
                    message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn = false;
                    _player.Messages[(int)_currentField].RemoveById(message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToString());

                }

            }
        }

        _removePopup.SetActive(false);

        UpdateListSlots();
    }

    private void SetNotice() //알림 설정, 알림 갯수 설정 //UI로 가야할 것 같음
    {
        if (_player.Messages[(int)_currentField].CurrentNotCheckedMessage == 0)
        {
            _notice.SetActive(false);
        }
        else
        {
            //알림 보이기
            _notice.SetActive(true);
            _notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _player.Messages[(int)_currentField].CurrentNotCheckedMessage.ToString();// 알림 갯수 변경
        }
    }
}
