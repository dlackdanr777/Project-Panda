using System;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Progress;

[Serializable]
public class MessageList
{
    public int MaxMessageCount { get; private set; } = 20;
    public int MessagesCount => Messages.Count;
    public List<Message> Messages = new List<Message>();

    public List<bool> IsCheckMessage = new List<bool>();
    public List<bool> IsReceiveGift = new List<bool>();
    public int CurrentNotCheckedMessage
    {
        get
        {
            int count = 0;
            for (int i = 0; i < IsCheckMessage.Count; i++)
            {
                if (!IsCheckMessage[i])
                    count++;
            }
            return count;
        }
    }

    public List<Message> GetMessageList()
    {
        return Messages;
    }
    public void Add(Message message)
    {
        if (Messages.Count >= MaxMessageCount)
        {
            Messages.RemoveAt(0);
        }
        Messages.Add(message);
        IsCheckMessage.Add(false);
        IsReceiveGift.Add(false);
    }
    public void RemoveByIndex(int index)
    {
        if(Messages.Count > 0)
        {
            Messages.RemoveAt(index);
        } 
    }

}
