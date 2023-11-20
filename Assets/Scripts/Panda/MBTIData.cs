using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTIData
{
    //������ ID ������ MBTI enum �����ϱ�,,,
    public enum MBTI
    {
        intj,
        intp
    }
    //MBTI�� ���� ���� ����
    public PreferenceData[] _preferenceDatas = new PreferenceData[16];

    public void SetMBTI()
    {
        // test �켱 intp�� ����
        _preferenceDatas[1].FavoriteToy = "T-I02";
        _preferenceDatas[1].FavoriteSnack = "S-I02";
    }
}
