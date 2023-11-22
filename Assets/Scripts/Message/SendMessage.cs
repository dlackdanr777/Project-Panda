using System;
using System.Collections;
using UnityEngine;

//특정 조건 시, 문자 발송
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

        //조건 여러 개 만들어서 해당하는 문자 보냄(조건 달라짐)
        _condition1Handler = (int amount) => (amount > 10);
        _condition2Handler = (int amount) => (amount > 20);
        _condition3Handler = (int amount) => (amount > 30);

        //Start에서 코루틴
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
                //message 데이터베이스에서 보냈음을 확인
                message.IsSend = true;
                yield return new WaitForSeconds(3); //5초후에 문자 오도록
                Send(message);
                yield break;
            }
            yield return null;
        }
    }

    private void Send(Message message)
    {
        _player.Messages[0].Add(message);
        _player.Messages[0].IsCheckMessage.Add(false); //문자확인했는지 알 수 있는 리스트에도 정보 저장
        _player.Messages[0].IsReceiveGift.Add(false);

        //UI변환
        NoticeHandler?.Invoke();
    }
}
