using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
{
    #region �ܺ� ����
    //�÷��̾�� ������ ���������� �����Ƿ� �����´�.
    private Player _player => GameManager.Instance.Player;

    //ä�� �������� ������ �������� ������ �Ѵ�.
    //ä�� ������ ���� DatabaseManager�� �߰�
    private DatabaseManager _database => DatabaseManager.Instance;
    #endregion

    #region FieldSlot ����
    [SerializeField] private GameObject _fieldSlotParent;
    //�۹��� ������ ���Ե�
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
        _fieldIndex = 3; // ���߿� ����
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
    /// �۹� ��Ȯ ��ư Ŭ�� </summary>
    public void ClickHavestButton()
    {
        // ��Ȯ
        _targetPos = Camera.main.ScreenToWorldPoint(_UIBamboo.transform.position);
        for (int i = 0; i <= _fieldIndex; i++)
        {

            // �ִϸ��̼� - �׼��� �����Ǹ� ���� ����� �׼����������� �������� ������ �ִϸ��̼�, �ϳ��� ������ ������ �׼����� �������� ��ȭ
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

            // ���� �ܰ� �ʱ�ȭ
            _fieldSlots[i].GrowthStage = 0;
            _fieldSlots[i].ChangeGrowthStageImage(_fieldSlots[i].GrowthStage);
        }
        Tween.SpriteRendererAlpha(HarvestButton.gameObject, 0, 0.5f, TweenMode.Quadratic, () => { HarvestButton.IsSet = false; });
    }

    // �����簡 ��ġ�簡 �ϱ�
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
    /// ���� ������ ���� ���ӽð��� �ð� ���̸� Ȯ�� �� �۹� ���� </summary>
    private void FirstCheckGrowth()
    {
        for(int i = 0; i <= _fieldIndex; i++)
        {
            //���� ���� �ð��� ������ ���� �ð��� ��
            if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].GrowthTime * 2)
            {
                Debug.Log("�۹� 2�ܰ� ����");
                _fieldSlots[i].GrowingCrops(2);
            }
            else if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].GrowthTime)
            {
                Debug.Log("�۹� 1�ܰ� ����");
                _fieldSlots[i].GrowingCrops(1);
            }
        }
    }

}
