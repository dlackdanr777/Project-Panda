//using System;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class MessageDetail : MonoBehaviour
//{
//    private Button _messageButton;
//    private GameObject _checkedImage;
//    private GameObject _messageView;
//    private Player _player;

//    public Action NoticeHandler;

//    //void Start()
//    {
//        _checkedImage = transform.GetChild(2).gameObject;
//        _player = GameManager.Instance.Player;

//        _messageButton = GetComponent<Button>();
//        _messageButton.onClick.AddListener(OnCheckMessage);

//        _messageView = transform.root.Find("Phone/Message").GetComponent<UIMessage>().MessageView;

//    }

//    private void OnCheckMessage()
//    {
//        _checkedImage.SetActive(false);
//        _player.Messages[0//�˸� �̺�Ʈ
//        NoticeHandler?.Invoke();].IsCheckMessage[_currentIndex] = true;

        

        
//        //gift ��ư ó��
//        _messageView.transform.Find("Gift").gameObject.SetActive(!_player.IsReceiveGift[_currentIndex]);
//        //���� �� ���⸦ ������ �� ��ư ������ �Ҵ� => �ش� ��ư �Ű������� �Ѱ���
//        int index = _currentIndex;
//        _messageView.transform.Find("Gift").GetComponent<Button>().onClick.AddListener(() => OnAddInventoryItem(index));

//    }
//    private int GetIndex()
//    {
//        for (int i = 0; i < _player.MaxMessageCount; i++)
//        {
//            if (transform == transform.parent.GetChild(i).transform)
//            {
//                return i;
//            }
//        }
//        return -1;
//    }

//    private void OnAddInventoryItem(int index)
//    {
//        if (!_player.IsReceiveGift[index])
//        {
//            _player.IsReceiveGift[index] = true;
//            ���� ��ư Ŭ���ϸ� �κ��丮�� �߰�
//            _player.Inventory[0].Add(_player.Messages[index].Gift);
//            _messageView.transform.Find("Gift").gameObject.SetActive(false); //��ư �Ⱥ��̰�
//        }
//    }
//}
