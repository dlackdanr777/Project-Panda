using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTIData
{
    //아이템 ID 순서로 MBTI enum 생성하기,,,
    public enum MBTI
    {
        intj,
        intp
    }
    //MBTI에 따른 취향 생성
    public PreferenceData[] _preferenceDatas = new PreferenceData[16];

    public void SetMBTI()
    {
        // test 우선 intp만 설정
        _preferenceDatas[1].FavoriteToy = "T-I02";
        _preferenceDatas[1].FavoriteSnack = "S-I02";
    }
}
