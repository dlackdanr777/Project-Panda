using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTIManager : SingletonHandler<MBTIManager>
{
    private enum EMBTI
    {
        INTJ,
        INTP,
        INFJ,
        INFP,
        ISTJ,
        ISFJ,
        ISTP,
        ISFP,
        ENTJ,
        ENTP,
        ENFJ,
        ENFP,
        ESTJ,
        ESFJ,
        ESTP,
        ESFP
    }
    
    private List<Preference> DataMBTI = new List<Preference>();

    public override void Awake()
    {
        base.Awake();
        SetMBTI();
    }
    public void SetMBTI()
    {
        List<Item> Snacks = DatabaseManager.Instance.ItemDatabase.ItemList[0];
        List<Item> Toys = DatabaseManager.Instance.ItemDatabase.ItemList[1];
        for (int i = 0; i < System.Enum.GetValues(typeof(EMBTI)).Length; i++)
        {
            DataMBTI.Add(new Preference(Snacks[i].Id, Toys[i].Id));
        }
    }

    public Preference SetPreference(string mbti)
    {
        EMBTI Embti = StringToEnum<EMBTI>(mbti);
        return DataMBTI[(int)Embti];
    }

    private T StringToEnum<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }
}
