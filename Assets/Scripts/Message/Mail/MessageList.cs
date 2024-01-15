using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class MessageList
{
    public Action NoticeHandler;
    public int MaxMessageCount { get; private set; } = 20;
    public int MessagesCount => _messages.Count;
    private List<Message> _messages = new List<Message>();

    public int CurrentNotCheckedMessage
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _messages.Count; i++)
            {
                if (!_messages[i].IsCheck)
                    count++;
            }
            return count;
        }
    }

    public List<Message> GetMessageList()
    {
        return _messages;
    }
    public void Add(Message message)
    {
        if (_messages.Count >= MaxMessageCount)
        {
            _messages.RemoveAt(0);
        }
        _messages.Add(message);
        NoticeHandler?.Invoke();
    }

    /// <summary>
    /// AddById
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fieldIndex">0:Mail, 1:WishTree</param>
    public void AddById(string id, MessageField field)
    {
        switch (field)
        {
            case MessageField.Mail:
                for (int i = 0; i < DatabaseManager.Instance.GetMailList().Count; i++)
                {
                    if (DatabaseManager.Instance.GetMailList()[i].Id.Equals(id))
                    {
                        Add(DatabaseManager.Instance.GetMailList()[i]);
                    }
                }
                break;
            case MessageField.Wish:
                break;
        }
        
    }

    public void AddByStoryId(string id, MessageField field)
    {
        switch (field)
        {
            case MessageField.Mail:
                for (int i = 0; i < DatabaseManager.Instance.GetMailList().Count; i++)
                {
                    if (DatabaseManager.Instance.GetMailList()[i].StoryStep.Equals(id))
                    {
                        Add(DatabaseManager.Instance.GetMailList()[i]);
                    }
                }
                break;
            case MessageField.Wish:
                break;
        }
    }

    public void RemoveByIndex(int index)
    {
        if(_messages.Count > 0)
        {
            _messages.RemoveAt(index);
        } 
    }
}
