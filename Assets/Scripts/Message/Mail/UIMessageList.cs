using Muks.DataBind;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageList : UIList<Message, MessageField>
{
    [SerializeField] private Button _removeButton;
    [SerializeField] private GameObject _removePopup;

    private Player _player;
    private int _currentItemIndex;

    public Action NoticeHandler;

    private void Awake()
    {
        _player = GameManager.Instance.Player;
        for (int i = 0; i < _player.Messages.Length; i++)
        {
            _maxCount[i] = _player.Messages[i].MaxMessageCount;
        }

        Init();
    }

    private void Start()
    {
        _removeButton.onClick.AddListener(OnClickRemoveButton);
        _removePopup.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(OnRemoveMessage);
    }

    private void UpdateList()
    {
        for (int i = 0; i < _player.Messages.Length; i++)
        {
            _lists[i] = _player.Messages[i].GetMessageList();

        }
    }

    private void OnAddInventoryItem(int index)
    {
        //if (!_player.Messages[0].IsReceiveGift[index])
        //{
        //    _player.Messages[0].IsReceiveGift[index] = true;

        //    //선물 버튼 클릭하면 인벤토리에 추가
        //    ItemField itemField = _player.Messages[0].GetMessageList()[index].Gift.ItemField;
        //    _player.Inventory[(int)itemField].Add(_player.Messages[(int)_currentField].GetMessageList()[index].Gift);

        //    _detailView.transform.GetChild(3).gameObject.SetActive(false); //선물 버튼 안보이게
        //}
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

                    _player.Messages[(int)_currentField].RemoveByIndex(i);
                    //_player.Messages[0].IsReceiveGift.RemoveAt(i);
                    //_player.Messages[0].IsCheckMessage.RemoveAt(i);

                }

            }
        }

        _removePopup.SetActive(false);

        UpdateListSlots();
    }

    protected override void GetContent(int index)
    {
        _currentItemIndex = index;

        //player 메시지 확인
        _spawnPoint[(int)_currentField].GetChild(index).GetChild(2).gameObject.SetActive(false);
        //_player.Messages[(int)_currentField].IsCheckMessage[index] = true;
        NoticeHandler?.Invoke();

        //DataBind.SetTextValue("MessageDetailTo", _lists[(int)_currentField][index].To);
        DataBind.SetTextValue("MessageDetailContent", _lists[(int)_currentField][index].Content);
        DataBind.SetTextValue("MessageDetailFrom", _lists[(int)_currentField][index].From);

        if (_currentField == MessageField.Mail)
        {

            //_spawnPoint[(int)_currentField].GetChild(index).gameObject.SetActive(!_player.Messages[0].IsReceiveGift[index]);
            //DataBind.SetSpriteValue("MessageDetailGift", _lists[(int)_currentField][index].Gift.Image);
            DataBind.SetButtonValue("MessageDetailGiftButton", () => OnAddInventoryItem(index));
        }
    }

    protected override void UpdateListSlots()
    {
        UpdateList();

        for (int j = 0; j < _maxCount[(int)_currentField]; j++) //현재 player의 message에 저장된 개수
        {
            if (j < _player.Messages[(int)_currentField].GetMessageList().Count)
            {
                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(true);
                _spawnPoint[(int)_currentField].GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = _lists[(int)_currentField][j].From; //Text 변경

            }
            else
            {

                _spawnPoint[(int)_currentField].GetChild(j).gameObject.SetActive(false);


            }
        }
    }

    protected override void ClearContent()
    {
    }
}
