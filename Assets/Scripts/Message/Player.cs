using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    public int Familiarity;

    [Header("Inventory")]
    public Inventory[] Inventory = new Inventory[2]; //0:toy, 1:snack

    [Header("Message")]
    public List<Message> Messages = new List<Message>(); 
    public int MaxMessageCount { get; private set; }
    public List<bool> IsCheckMessage = new List<bool>();
    public List<bool> IsReceiveGift = new List<bool>();

    [Header("Bamboo")]
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private GameObject _dontUseBambooPanel;
    [SerializeField] private GameObject _dontGainBambooPanel;
    public int Bamboo { get; private set; }
    public int MaxBamboo;


    private void Awake()
    {
        MaxMessageCount = 20;
        MaxBamboo = 1000;
        
        for(int i=0; i < Inventory.Length; i++)
        {
            Inventory[i] = new Inventory();
        }
    }

    private void Start()
    {
        DataBind.SetTextValue("BambooCount", Bamboo.ToString());
    }

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

    public bool SpendBamboo(int amount)
    {
        if(Bamboo > 0) 
        {
            Bamboo -= amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            return true;
        }
        else
        {
            StartCoroutine(PopupCoroutine(_dontUseBambooPanel));
            return false;
        }
    }

    public bool GainBamboo(int amount)
    {
        if(Bamboo < MaxBamboo)
        {
            Bamboo += amount;
            DataBind.SetTextValue("BambooCount", Bamboo.ToString());
            return true;
        }
        else
        {
            StartCoroutine(PopupCoroutine(_dontGainBambooPanel));
            return false;
        }
    }

    private IEnumerator PopupCoroutine(GameObject panel)
    {
        _popupPanel.SetActive(true);
        panel.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        panel.SetActive(false);
        _popupPanel.SetActive(false);
    }
}
