using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageDetail : MonoBehaviour
{
    private Button _messageButton;
    private GameObject _checkedImage;
    private GameObject _messageView;
    private Player _player;

    public Action NoticeHandler;

    // Start is called before the first frame update
    void Start()
    {
        _checkedImage = transform.GetChild(2).gameObject;
        _player = GameManager.Instance.Player;

        _messageButton = GetComponent<Button>();
        _messageButton.onClick.AddListener(OnCheckMessage);

        _messageView = transform.root.Find("Phone/Message").GetComponent<UIMessage>().MessageView;
        
    }

    private void OnCheckMessage()
    {
        int _currentIndex = _player.MaxMessageCount - 1 - GetIndex();
        _checkedImage.SetActive(false);
        _player.IsCheckMessage[_currentIndex] = true;

        //알림 이벤트
        NoticeHandler?.Invoke();

        //UI
        _messageView.SetActive(true);
        _messageView.transform.Find("To").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].To;
        _messageView.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].Content;
        _messageView.transform.Find("From").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].From;
        _messageView.transform.Find("Gift").GetComponent<Image>().sprite = _player.Messages[_currentIndex].GiftImage;

        //gift 버튼 처리
        _messageView.transform.Find("Gift").gameObject.SetActive(!_player.IsReceiveGift[_currentIndex]);
        //문자 상세 보기를 눌렀을 때 버튼 리스너 할당 => 해당 버튼 매개변수로 넘겨줌
        int index = _currentIndex;
        _messageView.transform.Find("Gift").GetComponent<Button>().onClick.AddListener(() => OnAddInventoryItem(index));

    }
    private int GetIndex()
    {
        for(int i=0;i< _player.MaxMessageCount; i++)
        {
            if(transform == transform.parent.GetChild(i).transform)
            {
                return i;
            }
        }
        return -1;
    }

    private void OnAddInventoryItem(int index)
    {
        if(!_player.IsReceiveGift[index])
        {
            _player.IsReceiveGift[index] = true;
            //선물 버튼 클릭하면 인벤토리에 추가
            _player.Inventory.Add(_player.Messages[index].Gift);
            _messageView.transform.Find("Gift").gameObject.SetActive(false); //버튼 안보이게
        }
    }
}
