using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
{

    [SerializeField] private GameObject _fieldSlotParent;

    //플레이어에게 아이템 저장정보가 있으므로 가져온다. + 접속시간 확인 필요
    private Player _player => GameManager.Instance.Player;

    //채집 아이템의 정보를 가져오는 역할을 한다.
    //채집 아이템 정보 DatabaseManager에 추가
    private DatabaseManager _database => DatabaseManager.Instance;

    //작물이 성장할 슬롯들
    //Init에서 _fieldSlotParent자식을 전부 불러올 예정
    //차후 FieldSlot 클래스로 변경해야할것입니다.
    private FieldSlot[] _fieldSlots;

    // 플레이어의 접속 시간 확인 후 성장 여부 판별
    public DateTime ConnectionTerminationTime;
    
    /// <summary>
    /// 게임 연결 종료 시 시간 저장 </summary>
    public void ConnectionTermination()
    {
        ConnectionTerminationTime = DateTime.Now;
    }

    /// <summary>
    /// 게임 시작 시 시간 체크 후 작물 성장 </summary>
    private void CheckTime()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeDif = now - ConnectionTerminationTime;

        // 나중에 조건 변경
        if(timeDif.Days == 0)
        {
            //지난번 마지막 접속이 오늘인 경우 - 작물 성장하지 않음
            Debug.Log("시간 차이: " + timeDif.Hours);
        }
        else if(timeDif.Days > 0)
        {
            //지난번 마지막 접속이 1일 초과인 경우 - 작물 성장
            Debug.Log("24시간 지난 후: "+ timeDif.Days);
            for(int i = 0; i< _fieldSlots.Length; i++)
            {
                _fieldSlots[i].GrowingCrops(1);
            }
        }
        else if(timeDif.Days > 1)
        {
            // 작물 2단계 성장
            Debug.Log("작물 2단계 성장");
            for (int i = 0; i < _fieldSlots.Length; i++)
            {
                _fieldSlots[i].GrowingCrops(2);
            }
        }
    }

    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<FieldSlot>();
    }



    //접속 했을때 이전 접속시간과 시간 차이를 확인 후 작물 성장시간을 지났나 확인하는 함수
    private bool FirstCheckGrowth()
    {
        for(int i = 0; i< _fieldSlots.Length; i++)
        {
            //현재 접속 시간과 마지막 접속 시간을 비교한다.
            if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes <= _fieldSlots[i].HarvestItem.HarvestTime) // 작물 성장시간 기입
                return false;
        }

        return true;
    }


}
