using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.VersionControl;

public class MessageList
{
    public Action NoticeHandler;
    public int MaxMessageCount { get; private set; } = 20;
    public int MessagesCount => _messageList.Count;
    private List<Message> _messageList = new List<Message>();


    public int CurrentNotCheckedMessage
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _messageList.Count; i++)
            {
                if (!_messageList[i].IsCheck)
                    count++;
            }
            return count;
        }
    }

    public List<Message> GetMessageList()
    {
        return _messageList;
    }


    private void Add(Message message)
    {
        if (_messageList.Count >= MaxMessageCount)
        {
            _messageList.RemoveAt(0);
        }
        _messageList.Add(message);
        NoticeHandler?.Invoke();

    }

    /// <summary>
    /// AddById
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fieldIndex">0:Mail, 1:WishTree</param>
    public void AddById(string id, MessageField field, bool isServerSaved = true)
    {

        switch (field)
        {
            case MessageField.Mail:
                for (int i = 0; i < DatabaseManager.Instance.GetMailList().Count; i++)
                {
                    if (DatabaseManager.Instance.GetMailList()[i].Id.Equals(id))
                    {
                        if (GameManager.Instance.Player.FindMailReceivedById(id))
                        {
                            UnityEngine.Debug.Log("이미 있는 메일입니다.");
                            return;
                        }

                        Add(DatabaseManager.Instance.GetMailList()[i]);

                        if (isServerSaved)
                        {
                            GameManager.Instance.Player.AddMailReceived(DatabaseManager.Instance.GetMailList()[i].Id);
                            GameManager.Instance.Player.SaveMailData(3);
                        }
                    }
                }
                break;
            case MessageField.Wish:
                break;
        }


    }

    public void AddByStoryId(string id, MessageField field, bool isServerSaved = true)
    {

        switch (field)
        {
            case MessageField.Mail:
                for (int i = 0; i < DatabaseManager.Instance.GetMailList().Count; i++)
                {
                    if (DatabaseManager.Instance.GetMailList()[i].StoryStep.Equals(id))
                    {
                        if (GameManager.Instance.Player.FindMailReceivedById(DatabaseManager.Instance.GetMailList()[i].Id))
                        {
                            UnityEngine.Debug.Log("이미 있는 메일입니다.");
                            return;
                        }

                        Add(DatabaseManager.Instance.GetMailList()[i]);
                        if (isServerSaved)
                        {
                            GameManager.Instance.Player.AddMailReceived(DatabaseManager.Instance.GetMailList()[i].Id);
                            GameManager.Instance.Player.SaveMailData(3);
                        }
                        UnityEngine.Debug.Log("메일 추가됨");
                        return;
                    }
                }
                break;
            case MessageField.Wish:
                break;
        }

    }


    public void RemoveByIndex(int index)
    {
        if(_messageList.Count > 0)
        {
            _messageList.RemoveAt(index);
        }
        GameManager.Instance.Player.SaveMailData(3);
    }

    private bool IsAlreadyHave(string id)
    {
        for(int i=0;i<_messageList.Count;i++)
        {
            if (_messageList[i].Id.Equals(id))
            {
                return true;
            }
        }
        return false;
    }

}
