using TMPro;
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
    private GameObject _messageList;
    [SerializeField]
    private GameObject _notice;
    [SerializeField]
    private GameObject _removePopup;
    [SerializeField]
    private Transform _instantiatePosition;

    private Player _player;
    private SendMessage _sendMessage;

    public GameObject MessageView;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _sendMessage = GetComponent<SendMessage>();

        for (int i = 0; i < _player.MaxMessageCount; i++)
        {
            GameObject content = Instantiate(_messagePf, _instantiatePosition);
            content.GetComponent<MessageDetail>().NoticeHandler += SetNotice;
        }

        _messageButton.onClick.AddListener(OnClickMessageButton);
        _removeButton.onClick.AddListener(OnClickRemoveButton);
        _sendMessage.NoticeHandler += SendMessage_NoticeHandler;
        _removePopup.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(OnRemoveMessage);
        
    }

    private void SendMessage_NoticeHandler()
    {
        OnChangeaReveivedMessage();
    }
    
    private void OnClickMessageButton()
    {
        _messageList.gameObject.SetActive(true);
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
                    _removePopup.SetActive(true);
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
                    _player.IsReceiveGift.RemoveAt(_player.MaxMessageCount - 1 - i);         
                    _player.Messages.RemoveAt(_player.MaxMessageCount - 1 - i);

                }

            }
        }

        _removePopup.SetActive(false);

        OnChangeaReveivedMessage() ;
    }

    private void OnChangeaReveivedMessage()
    {
        int index = _player.MaxMessageCount - _player.Messages.Count;

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
                _instantiatePosition.GetChild(i).gameObject.SetActive(true); //스크롤 뷰의 자식 setActive true
                _instantiatePosition.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _player.Messages[_player.MaxMessageCount- 1 -i].From; //text를 받은 문자로 변환
                _instantiatePosition.GetChild(i).transform.GetChild(2).gameObject.SetActive(!_player.IsCheckMessage[_player.MaxMessageCount - 1 - i]); //문자 확인 이미지 변경
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
