using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MessageDetail : MonoBehaviour
{
    private Button _messageButton;
    private GameObject _checkedImage;
    private GameObject MessageView;
    private Player _player;
    private int _currentIndex;

    public Action NoticeHandler;

    // Start is called before the first frame update
    void Start()
    {
        _checkedImage = transform.GetChild(2).gameObject;
        _player = GameManager.Instance.Player;

        _messageButton = GetComponent<Button>();
        _messageButton.onClick.AddListener(OnCheckMessage);
        MessageView = transform.parent.parent.GetChild(1).gameObject;
        MessageView.transform.Find("Gift").GetComponent<Button>().onClick.AddListener(OnAddInventoryItem);
    }

    private void OnCheckMessage()
    {
        _currentIndex = _player.MaxMessageCount - 1 - GetIndex();
        Debug.Log(_currentIndex);
        _checkedImage.SetActive(false);
        _player.IsCheckMessage[_currentIndex] = true;
        //알림 이벤트
        NoticeHandler?.Invoke();

        //UI
        MessageView.SetActive(true);
        MessageView.transform.Find("To").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].To;
        MessageView.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].Content;
        MessageView.transform.Find("From").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].From;
        MessageView.transform.Find("Gift").GetComponent<Image>().sprite = _player.Messages[_currentIndex].GiftImage;

        //gift 버튼 처리
        MessageView.transform.Find("Gift").gameObject.SetActive(!_player.IsReceiveGift[_currentIndex]);

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

    private void OnAddInventoryItem()
    {
        if(!_player.IsReceiveGift[_currentIndex])
        {
            _player.IsReceiveGift[_currentIndex] = true;
            //선물 버튼 클릭하면 인벤토리에 추가
            _player.Inventory.Add(_player.Messages[_currentIndex].Gift);
            MessageView.transform.Find("Gift").gameObject.SetActive(false); //버튼 안보이게

        }

    }
}
