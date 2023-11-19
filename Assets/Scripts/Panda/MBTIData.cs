using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MBTI 데이터를 저장하는 클래스
/// MBTI 데이터를 통해 취향, 성격 등 다양한 데이터를 게임 시작 시 설정
/// </summary>
public class MBTIData : MonoBehaviour
{
    //아이템 ID 순서로 MBTI enum 생성하기,,,
    private enum MBTI
    {

    }
    //MBTI에 따른 취향 생성
    private PreferenceData[] _preferenceDatas = new PreferenceData[16];

}
