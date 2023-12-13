using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
{

    [SerializeField] private GameObject _fieldSlotParent;

    //플레이어에게 아이템 저장정보가 있으므로 가져온다.
    private Player _player => GameManager.Instance.Player;

    //채집 아이템의 정보를 가져오는 역할을 한다.
    private DatabaseManager _database => DatabaseManager.Instance;

    //작물이 성장할 슬롯들
    //Init에서 _fieldSlotParent자식을 전부 불러올 예정
    //차후 FieldSlot 클래스로 변경해야할것입니다.
    private GameObject[] _fieldSlots;


    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<GameObject>();
    }



    //접속 했을때 이전 접속시간과 시간 차이를 확인 후 작물 성장시간을 지났나 확인하는 함수
    private bool FirstCheckGrowth()
    {
        //현재 접속 시간과 마지막 접속 시간을 비교한다.
        //if((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes <= 작물성장시간 기입)
            //return false;

        return true;
    }


}
