using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessage : UIManager
{
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private GameObject _messageList;
    [SerializeField]
    private GameObject _notice;
    [SerializeField]
    private GameObject _removePopup;

    private SendMessage _sendMessage;

    // Start is called before the first frame update
    void Start()
    {
        _sendMessage = GetComponent<SendMessage>();

        for (int i = 0; i < Player.MaxMessageCount; i++)
        {
           
            _prefab.GetComponent<MessageDetail>().NoticeHandler += SetNotice;
        }

        _removeButton.onClick.AddListener(OnClickRemoveButton);
        _sendMessage.NoticeHandler += SendMessage_NoticeHandler;
        _removePopup.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(OnRemoveMessage);
        
    }
 
    private void SendMessage_NoticeHandler()
    {
        OnChangeaReveivedMessage();
    }

    private void OnClickRemoveButton()
    {
        for (int i = 0; i < Player.MaxMessageCount; i++)
        {
            GameObject message = SpawnPoint.GetChild(i).gameObject; //message, 위치
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
        for (int i = 0; i < Player.MaxMessageCount ; i++)
        {
            GameObject message = SpawnPoint.GetChild(i).gameObject; //message, 위치
            if (message.activeSelf)
            {
                if (message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn)
                {
                    message.transform.Find("ClickMessage").GetComponent<Toggle>().isOn = false;
                    Player.IsCheckMessage.RemoveAt(Player.MaxMessageCount - 1 - i); //마지막 자리에서부터 삭제           
                    Player.IsReceiveGift.RemoveAt(Player.MaxMessageCount - 1 - i);         
                    Player.Messages.RemoveAt(Player.MaxMessageCount - 1 - i);

                }

            }
        }

        _removePopup.SetActive(false);

        OnChangeaReveivedMessage() ;
    }

    private void OnChangeaReveivedMessage()
    {
        int index = Player.MaxMessageCount - Player.Messages.Count;

        SetNotice();

        for (int i = 0; i < index; i++)
        {
            SpawnPoint.GetChild(i).gameObject.SetActive(false);
        }
        if (Player.Messages.Count > 0)
        {
            //문자 UI
            for (int i = index; i < Player.MaxMessageCount; i++)
            {
                SpawnPoint.GetChild(i).gameObject.SetActive(true); //스크롤 뷰의 자식 setActive true
                SpawnPoint.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Player.Messages[Player.MaxMessageCount- 1 -i].From; //text를 받은 문자로 변환
                SpawnPoint.GetChild(i).transform.GetChild(2).gameObject.SetActive(!Player.IsCheckMessage[Player.MaxMessageCount - 1 - i]); //문자 확인 이미지 변경
            }
        }
    }

    private void SetNotice() //알림 설정, 알림 갯수 설정 //UI로 가야할 것 같음
    {
        if (Player.CurrentNotCheckedMessage == 0)
        {
            _notice.SetActive(false);
        }
        else
        {
            //알림 보이기
            _notice.SetActive(true);
            _notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Player.CurrentNotCheckedMessage.ToString();// 알림 갯수 변경
        }
    }
}
