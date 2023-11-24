using System;
using System.Collections;
using TMPro;
using UnityEngine;

//Ư�� ���� ��, ���� �߼�
public class SendMessage : MonoBehaviour
{
    private Player _player;

    [SerializeField] private GameObject _notice;
    [SerializeField] private UIMessageList _uiMessageList;
    [SerializeField] private SpawnWishes _spawnWishes;

    //Test
    private Predicate<int> _condition1Handler;
    private Predicate<int> _condition2Handler;
    private Predicate<int> _condition3Handler;

    private void Start()
    {
        _player = GameManager.Instance.Player;
        _uiMessageList.NoticeHandler += UIMessageList_NoticeHandler;
        _spawnWishes.NoticeHandler += UIMessageList_NoticeHandler;


        //���� ���� �� ���� �ش��ϴ� ���� ����(���� �޶���)
        _condition1Handler = (int amount) => (amount > 10);
        _condition2Handler = (int amount) => (amount > 20);
        _condition3Handler = (int amount) => (amount > 30);

        //Start���� �ڷ�ƾ
        StartCoroutine(SendMessageRoutine(_condition1Handler, GameManager.Instance.MessageDatabase.Messages[0], MessageField.Mail));
        StartCoroutine(SendMessageRoutine(_condition2Handler, GameManager.Instance.MessageDatabase.Messages[1], MessageField.Mail));
        StartCoroutine(SendMessageRoutine(_condition3Handler, GameManager.Instance.MessageDatabase.Messages[2], MessageField.Mail));
    }

    private void UIMessageList_NoticeHandler()
    {
        SetNotice();
    }

    private IEnumerator SendMessageRoutine(Predicate<int> condition, Message message, MessageField messageField)
    {
        while (!message.IsSend)
        {
            if (condition(_player.Familiarity))
            {
                //message �����ͺ��̽����� �������� Ȯ��
                message.IsSend = true;
                yield return new WaitForSeconds(3); //5���Ŀ� ���� ������
                Send(message,messageField);
                yield break;
            }
            yield return null;
        }
    }

    private void Send(Message message ,MessageField messageField)
    {
        _player.Messages[(int)messageField].Add(message);

        //UI��ȯ
        SetNotice();
    }

    private void SetNotice() //�˸� ����, �˸� ���� ����
    {
        int count = 0;
        for(int i = 0; i < _player.Messages.Length; i++)
        {
            count += _player.Messages[i].CurrentNotCheckedMessage;
            Debug.Log(count);
        }
        if (count == 0)
        {
            _notice.SetActive(false);
        }
        else
        {
            //�˸� ���̱�
            _notice.SetActive(true);
            _notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = count.ToString();// �˸� ���� ����
        }
    }
}
