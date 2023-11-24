using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTIData
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

    // ID만 가져와도 될 것 같음.....
    List<Item> Snacks = Database_Ssun.Instance.ItemList[0];
    List<Item> Toys = Database_Ssun.Instance.ItemList[1];

    public void SetMBTI()
    {
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
