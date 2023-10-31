using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    [SerializeField]
    private Button _messageButton;
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private GameObject _messagePf;
    [SerializeField]
    private GameObject MessageList;
    [SerializeField]
    private GameObject Notice;
    [SerializeField]
    private GameObject RemovePopup;
    [SerializeField]
    private Transform _instantiatePosition;

    private Player _player;
    private SendMessage _sendMessage;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _sendMessage = GetComponent<SendMessage>();
        Debug.Log(_player.MaxMessageCount);

        for (int i = 0; i < _player.MaxMessageCount; i++)
        {
            GameObject content = Instantiate(_messagePf, _instantiatePosition);
            content.GetComponent<MessageDetail>().NoticeHandler += SetNotice;
        }

        _messageButton.onClick.AddListener(OnClickMessageButton);
        _removeButton.onClick.AddListener(OnClickRemoveButton);
        _sendMessage.NoticeHandler += SendMessage_NoticeHandler;
        RemovePopup.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(OnRemoveMessage);
        
    }

    private void SendMessage_NoticeHandler()
    {
        OnTakeMessage();
    }
    
    private void OnClickMessageButton()
    {
        MessageList.gameObject.SetActive(true);
    }

    private void OnClickRemoveButton()
    {
        for (int i = 0; i < _player.MaxMessageCount; i++)
        {
            GameObject message = _instantiatePosition.GetChild(i).gameObject; //message, 위치
            if (message.activeSelf)
            {
                if (message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn)
                {
                    RemovePopup.SetActive(true);
                }
            }
        }
        
    }

    private void OnRemoveMessage()
    {
        for (int i = 0; i < _player.MaxMessageCount ; i++)
        {
            GameObject message = _instantiatePosition.GetChild(i).gameObject; //message, 위치
            if (message.activeSelf)
            {
                if (message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn)
                {
                    message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn = false;
                    _player.IsCheckMessage.RemoveAt(_player.MaxMessageCount - 1 - i); //마지막 자리에서부터 삭제           
                    _player.Messages.RemoveAt(_player.MaxMessageCount - 1 - i);

                }

            }
        }

        RemovePopup.SetActive(false);

        OnTakeMessage() ;
    }

    private void OnTakeMessage()
    {
        Debug.Log("messages" + _player.Messages.Count);
        int index = _player.MaxMessageCount - _player.Messages.Count;
        Debug.Log("CApacity" + index);

        SetNotice();

        for (int i = 0; i < index; i++)
        {
            _instantiatePosition.GetChild(i).gameObject.SetActive(false);
        }
        if (_player.Messages.Count > 0)
        {
            //문자 UI
            for (int i = index; i < _player.MaxMessageCount; i++)
            {
                _instantiatePosition.GetChild(index).gameObject.SetActive(true); //스크롤 뷰의 자식 setActive true
                _instantiatePosition.GetChild(index).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _player.Messages[_player.MaxMessageCount- 1 -index].From; //text를 받은 문자로 변환

            }
        }
    }

    private void SetNotice() //알림 설정, 알림 갯수 설정 //UI로 가야할 것 같음
    {
        if (_player.CurrentNotCheckedMessage == 0)
        {
            Notice.SetActive(false);
        }
        else
        {
            //알림 보이기
            Notice.SetActive(true);
            Notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _player.CurrentNotCheckedMessage.ToString();// 알림 갯수 변경
        }
    }
}
