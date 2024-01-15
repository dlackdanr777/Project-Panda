//using System;
//using System.Collections;
//using TMPro;
//using UnityEngine;

////특정 조건 시, 문자 발송
//public class SendMessage : MonoBehaviour
//{
//    private Player _player;

//    [SerializeField] private GameObject _notice;
//    [SerializeField] private UIMessageList _uiMessageList;
//    [SerializeField] private SpawnWishes _spawnWishes;

//    //Test
//    private Predicate<int> _condition1Handler;
//    private Predicate<int> _condition2Handler;
//    private Predicate<int> _condition3Handler;

//    private void Start()
//    {
//        _player = GameManager.Instance.Player;
//        _uiMessageList.NoticeHandler += UIMessageList_NoticeHandler;
//        _spawnWishes.NoticeHandler += UIMessageList_NoticeHandler;


//        //조건 여러 개 만들어서 해당하는 문자 보냄(조건 달라짐)
//        _condition1Handler = (int amount) => (amount > 10);
//        _condition2Handler = (int amount) => (amount > 20);
//        _condition3Handler = (int amount) => (amount > 30);

//        //Start에서 코루틴
//        StartCoroutine(SendMessageRoutine(_condition1Handler, GameManager.Instance.MessageDatabase.Messages[0], MessageField.Mail));
//        StartCoroutine(SendMessageRoutine(_condition2Handler, GameManager.Instance.MessageDatabase.Messages[1], MessageField.Mail));
//        StartCoroutine(SendMessageRoutine(_condition3Handler, GameManager.Instance.MessageDatabase.Messages[2], MessageField.Mail));
//    }

//    private void UIMessageList_NoticeHandler()
//    {
//        SetNotice();
//    }

//    private IEnumerator SendMessageRoutine(Predicate<int> condition, Message message, MessageField messageField)
//    {
//        while (!message.IsSend)
//        {
//            if (condition(_player.Familiarity))
//            {
//                //message 데이터베이스에서 보냈음을 확인
//                message.IsSend = true;
//                yield return new WaitForSeconds(3); //5초후에 문자 오도록
//                Send(message, messageField);
//                yield break;
//            }
//            yield return null;
//        }
//    }

//    private void Send(Message message, MessageField messageField)
//    {
//        _player.Messages[(int)messageField].Add(message);

//        //UI변환
//        SetNotice();
//    }

//    private void SetNotice() //알림 설정, 알림 갯수 설정
//    {
//        int count = 0;
//        for (int i = 0; i < _player.Messages.Length; i++)
//        {
//            count += _player.Messages[i].CurrentNotCheckedMessage;
//            Debug.Log(count);
//        }
//        if (count == 0)
//        {
//            _notice.SetActive(false);
//        }
//        else
//        {
//            //알림 보이기
//            _notice.SetActive(true);
//            _notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = count.ToString();// 알림 갯수 변경
//        }
//    }
//}