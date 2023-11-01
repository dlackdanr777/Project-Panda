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

        //�˸� �̺�Ʈ
        NoticeHandler?.Invoke();

        //UI
        _messageView.SetActive(true);
        _messageView.transform.Find("To").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].To;
        _messageView.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].Content;
        _messageView.transform.Find("From").GetComponent<TextMeshProUGUI>().text = _player.Messages[_currentIndex].From;
        _messageView.transform.Find("Gift").GetComponent<Image>().sprite = _player.Messages[_currentIndex].GiftImage;

        //gift ��ư ó��
        _messageView.transform.Find("Gift").gameObject.SetActive(!_player.IsReceiveGift[_currentIndex]);
        //���� �� ���⸦ ������ �� ��ư ������ �Ҵ� => �ش� ��ư �Ű������� �Ѱ���
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
            //���� ��ư Ŭ���ϸ� �κ��丮�� �߰�
            _player.Inventory.Add(_player.Messages[index].Gift);
            _messageView.transform.Find("Gift").gameObject.SetActive(false); //��ư �Ⱥ��̰�
        }
    }
}
