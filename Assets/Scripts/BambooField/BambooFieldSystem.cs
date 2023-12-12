using Muks.Tween;
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

    [SerializeField] private HarvestButton _harvestButton;

    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<FieldSlot>();
    }

    /// <summary>
    /// 작물 수확 버튼 클릭 </summary>
    public void ClickHavestButton()
    {
        // 수확
        for(int i = 0; i < _fieldSlots.Length; i++)
        {
            // 애니메이션 - 죽순이 수집되며 우측 상단의 죽순보유량으로 빨려들어가는 느낌의 애니메이션, 하나씩 빨려들어갈 때마다 죽순량이 동적으로 변화
            _player.GainBamboo(_fieldSlots[i].Yield);
            _fieldSlots[i].Yield = 0;

            // 성장 단계 초기화
            _fieldSlots[i].GrowthStage = 0;
            _fieldSlots[i].ChangeGrowthStageImage(_fieldSlots[i].GrowthStage);
        }
        Tween.SpriteRendererAlpha(_harvestButton.gameObject, 0, 0.5f, TweenMode.Quadratic, () => { _harvestButton.IsSet = false; });
    }

    /// <summary>
    /// 접속 했을때 이전 접속시간과 시간 차이를 확인 후 작물 성장 </summary>
    private void FirstCheckGrowth()
    {
        for(int i = 0; i< _fieldSlots.Length; i++)
        {
            //현재 접속 시간과 마지막 접속 시간을 비교
            if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].HarvestItem.HarvestTime * 10)
            {
                Debug.Log("작물 2단계 성장");
                _fieldSlots[i].GrowingCrops(2);
            }
            else if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].HarvestItem.HarvestTime * 5)
            {
                Debug.Log("작물 1단계 성장");
                _fieldSlots[i].GrowingCrops(1);
            }
        }
    }

}
