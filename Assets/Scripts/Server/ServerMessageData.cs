using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMessageData
{
    public string Id;
    public bool IsCheck;
    public bool IsReceived;
    public ServerMessageData(string id, bool isCheck, bool isReceived)
    {
        Id = id;
        IsCheck = isCheck;
        IsReceived = isReceived;
    }
}
