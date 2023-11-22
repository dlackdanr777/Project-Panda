using System;
using System.Collections;
using UnityEngine;

//Ư�� ���� ��, ���� �߼�
public class SendMessage : MonoBehaviour
{
    private Player _player;

    private Predicate<int> _condition1Handler;
    private Predicate<int> _condition2Handler;
    private Predicate<int> _condition3Handler;

    public event Action NoticeHandler;

    private void Start()
    {
        _player = GameManager.Instance.Player;

        //���� ���� �� ���� �ش��ϴ� ���� ����(���� �޶���)
        _condition1Handler = (int amount) => (amount > 10);
        _condition2Handler = (int amount) => (amount > 20);
        _condition3Handler = (int amount) => (amount > 30);

        //Start���� �ڷ�ƾ
        StartCoroutine(SendMessageRoutine(_condition1Handler, GameManager.Instance.MessageDatabase.Messages[0]));
        StartCoroutine(SendMessageRoutine(_condition2Handler, GameManager.Instance.MessageDatabase.Messages[1]));
        StartCoroutine(SendMessageRoutine(_condition3Handler, GameManager.Instance.MessageDatabase.Messages[2]));
    }

    private IEnumerator SendMessageRoutine(Predicate<int> condition, Message message)
    {
        while (!message.IsSend)
        {
            if (condition(_player.Familiarity))
            {
                //message �����ͺ��̽����� �������� Ȯ��
                message.IsSend = true;
                yield return new WaitForSeconds(3); //5���Ŀ� ���� ������
                Send(message);
                yield break;
            }
            yield return null;
        }
    }

    private void Send(Message message)
    {
        _player.Messages[0].Add(message);
        _player.Messages[0].IsCheckMessage.Add(false); //����Ȯ���ߴ��� �� �� �ִ� ����Ʈ���� ���� ����
        _player.Messages[0].IsReceiveGift.Add(false);

        //UI��ȯ
        NoticeHandler?.Invoke();
    }
}
