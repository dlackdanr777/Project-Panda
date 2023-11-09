using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    public int Familiarity;

    [Header("Inventory")]
    public Inventory[] Inventory = new Inventory[2];

    [Header("Message")]
    public List<Message> Messages = new List<Message>(); 
    public int MaxMessageCount { get; private set; }
    public List<bool> IsCheckMessage = new List<bool>();
    public List<bool> IsReceiveGift = new List<bool>();

    public int CurrentNotCheckedMessage {
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
    private void Awake()
    {
        MaxMessageCount = 20;
        
        for(int i=0; i < Inventory.Length; i++)
        {
            Inventory[i] = new Inventory();
        }
    }
}
