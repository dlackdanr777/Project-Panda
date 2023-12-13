using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
{
    #region 외부 정보
    //플레이어에게 아이템 저장정보가 있으므로 가져온다.
    private Player _player => GameManager.Instance.Player;

    //채집 아이템의 정보를 가져오는 역할을 한다.
    //채집 아이템 정보 DatabaseManager에 추가
    private DatabaseManager _database => DatabaseManager.Instance;
    #endregion

    #region FieldSlot 관련
    [SerializeField] private GameObject _fieldSlotParent;
    //작물이 성장할 슬롯들
    private FieldSlot[] _fieldSlots;
    private int _fieldIndex;
    #endregion

    #region UI
    public GameObject _UIBambooField;
    [SerializeField] private GameObject _UIBamboo;
    #endregion

    public HarvestButton HarvestButton;
    private Vector3 _targetPos;

    [SerializeField] private GameObject _harvestBamboo;
    private GameObject[] _bambooPrefabs = new GameObject[20];
    private int _bambooPrefabCount;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _fieldIndex = 3; // 나중에 수정
        FirstCheckGrowth();
    }

    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<FieldSlot>();
        for(int i = 0; i < _fieldIndex; i++)
        {
            _fieldSlots[i].Init(this);
        }
    }


    public void AddFieldSlot()
    {
        _fieldSlots[++_fieldIndex].gameObject.SetActive(true);
    }

    /// <summary>
    /// 작물 수확 버튼 클릭 </summary>
    public void ClickHavestButton()
    {
        // 수확
        _targetPos = Camera.main.ScreenToWorldPoint(_UIBamboo.transform.position);
        for (int i = 0; i <= _fieldIndex; i++)
        {

            // 애니메이션 - 죽순이 수집되며 우측 상단의 죽순보유량으로 빨려들어가는 느낌의 애니메이션, 하나씩 빨려들어갈 때마다 죽순량이 동적으로 변화
            for (int j = 0; j < _fieldSlots[i].Yield; j++)
            {
                int count = _bambooPrefabCount;
                _bambooPrefabs[count] = Instantiate(_harvestBamboo);
                _bambooPrefabs[count].transform.position = _fieldSlots[i].transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
                _bambooPrefabs[count].GetComponent<Animator>().enabled = true;

                Tween.TransformMove(_bambooPrefabs[count], _targetPos, 1.5f, TweenMode.Quadratic, () =>
                {
                    _player.GainBamboo(1);
                    Destroy(_bambooPrefabs[count]);
                });
                _bambooPrefabCount = (_bambooPrefabCount + 1) % 20;
            }
            //HarvestBamboo(0, _fieldSlots[i].Yield, i);
            _fieldSlots[i].Yield = 0;

            // 성장 단계 초기화
            _fieldSlots[i].GrowthStage = 0;
            _fieldSlots[i].ChangeGrowthStageImage(_fieldSlots[i].GrowthStage);
        }
        Tween.SpriteRendererAlpha(HarvestButton.gameObject, 0, 0.5f, TweenMode.Quadratic, () => { HarvestButton.IsSet = false; });
    }

    // 버리든가 고치든가 하기
    //private void HarvestBamboo(int currentCount, int totalCount, int fieldSlotNum)
    //{
    //    int count = _bambooPrefabCount;
    //    _bambooPrefabs[count] = Instantiate(_harvestBamboo);
    //    _bambooPrefabs[count].transform.position = _fieldSlots[fieldSlotNum].transform.position;
    //    Vector3 addPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
    //    _bambooPrefabs[count].GetComponent<Animator>().enabled = true;


    //    Tween.TransformMove(_harvestBamboo, _fieldSlots[fieldSlotNum].transform.position + addPosition, 0.05f, TweenMode.Quadratic, () =>
    //    {
    //        if(currentCount != totalCount)
    //        {
    //            _bambooPrefabCount = (_bambooPrefabCount + 1) % 20;
    //            HarvestBamboo(currentCount+1, totalCount, fieldSlotNum);
    //        }
    //    });

    //    Tween.TransformMove(_bambooPrefabs[count], _targetPos, 3, TweenMode.Quadratic, () =>
    //    {
    //        _player.GainBamboo(1);
    //        Destroy(_bambooPrefabs[count]);
    //    });
    //}

    /// <summary>
    /// 접속 했을때 이전 접속시간과 시간 차이를 확인 후 작물 성장 </summary>
    private void FirstCheckGrowth()
    {
        for(int i = 0; i <= _fieldIndex; i++)
        {
            //현재 접속 시간과 마지막 접속 시간을 비교
            if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].GrowthTime * 2)
            {
                Debug.Log("작물 2단계 성장");
                _fieldSlots[i].GrowingCrops(2);
            }
            else if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].GrowthTime)
            {
                Debug.Log("작물 1단계 성장");
                _fieldSlots[i].GrowingCrops(1);
            }
        }
    }

}
