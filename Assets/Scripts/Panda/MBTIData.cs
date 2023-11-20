using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MBTI 데이터를 저장하는 클래스
/// MBTI 데이터를 통해 취향, 성격 등 다양한 데이터를 게임 시작 시 설정
/// </summary>
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
