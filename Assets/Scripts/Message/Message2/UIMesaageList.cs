using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIMesaageList : UIList<Message>
{
    [SerializeField] private Button _removeButton;
    [SerializeField] private GameObject _notice;
    [SerializeField] private GameObject _removePopup;

    private List<Message> _messageList;
    private SendMessage _sendMessage;
    private Player _player;
    private int _currentItemIndex;

    private void Awake()
    {
        _maxCount[0] = GameManager.Instance.Player.MaxMessageCount;
        UpdateList();
        Init();      
    }

    private void Start()
    {
        _removeButton.onClick.AddListener(OnClickRemoveButton);
        _sendMessage.NoticeHandler += SendMessage_NoticeHandler;
        _removePopup.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(OnRemoveMessage);
    }

    private void UpdateList()
    {
        _lists[0] = GameManager.Instance.Player.Messages;
    }
    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        //DataBind.SetTextValue("MessageTo", _lists[(int)_currentField][index])
    }

    protected override void UpdateListSlots()
    {
        throw new System.NotImplementedException();
    }

    private void SendMessage_NoticeHandler()
    {
        OnChangeaReveivedMessage();
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
                    _player.IsCheckMessage.RemoveAt(_player.MaxMessageCount - 1 - i); //마지막 자리에서부터 삭제           
                    _player.IsReceiveGift.RemoveAt(_player.MaxMessageCount - 1 - i);
                    _player.Messages.RemoveAt(_player.MaxMessageCount - 1 - i);

                }

            }
        }

        _removePopup.SetActive(false);

        OnChangeaReveivedMessage();
    }

    private void OnChangeaReveivedMessage()
    {
        int index = _player.MaxMessageCount - _player.Messages.Count;

        SetNotice();

        for (int i = 0; i < index; i++)
        {
            _spawnPoint[0].GetChild(i).gameObject.SetActive(false);
        }
        if (_player.Messages.Count > 0)
        {
            //문자 UI
            for (int i = index; i < _player.MaxMessageCount; i++)
            {
                _spawnPoint[0].GetChild(i).gameObject.SetActive(true); //스크롤 뷰의 자식 setActive true
                _spawnPoint[0].GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _player.Messages[_player.MaxMessageCount - 1 - i].From; //text를 받은 문자로 변환
                _spawnPoint[0].GetChild(i).transform.GetChild(2).gameObject.SetActive(!_player.IsCheckMessage[_player.MaxMessageCount - 1 - i]); //문자 확인 이미지 변경
            }
        }
    }

    private void SetNotice() //알림 설정, 알림 갯수 설정 //UI로 가야할 것 같음
    {
        if (_player.CurrentNotCheckedMessage == 0)
        {
            _notice.SetActive(false);
        }
        else
        {
            //알림 보이기
            _notice.SetActive(true);
            _notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _player.CurrentNotCheckedMessage.ToString();// 알림 갯수 변경
        }
    }
}
