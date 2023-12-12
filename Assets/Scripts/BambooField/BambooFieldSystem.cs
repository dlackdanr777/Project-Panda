using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
{

    [SerializeField] private GameObject _fieldSlotParent;

    //�÷��̾�� ������ ���������� �����Ƿ� �����´�. + ���ӽð� Ȯ�� �ʿ�
    private Player _player => GameManager.Instance.Player;

    //ä�� �������� ������ �������� ������ �Ѵ�.
    //ä�� ������ ���� DatabaseManager�� �߰�
    private DatabaseManager _database => DatabaseManager.Instance;

    //�۹��� ������ ���Ե�
    //Init���� _fieldSlotParent�ڽ��� ���� �ҷ��� ����
    //���� FieldSlot Ŭ������ �����ؾ��Ұ��Դϴ�.
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
    /// �۹� ��Ȯ ��ư Ŭ�� </summary>
    public void ClickHavestButton()
    {
        // ��Ȯ
        for(int i = 0; i < _fieldSlots.Length; i++)
        {
            // �ִϸ��̼� - �׼��� �����Ǹ� ���� ����� �׼����������� �������� ������ �ִϸ��̼�, �ϳ��� ������ ������ �׼����� �������� ��ȭ
            _player.GainBamboo(_fieldSlots[i].Yield);
            _fieldSlots[i].Yield = 0;

            // ���� �ܰ� �ʱ�ȭ
            _fieldSlots[i].GrowthStage = 0;
            _fieldSlots[i].ChangeGrowthStageImage(_fieldSlots[i].GrowthStage);
        }
        Tween.SpriteRendererAlpha(_harvestButton.gameObject, 0, 0.5f, TweenMode.Quadratic, () => { _harvestButton.IsSet = false; });
    }

    /// <summary>
    /// ���� ������ ���� ���ӽð��� �ð� ���̸� Ȯ�� �� �۹� ���� </summary>
    private void FirstCheckGrowth()
    {
        for(int i = 0; i< _fieldSlots.Length; i++)
        {
            //���� ���� �ð��� ������ ���� �ð��� ��
            if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].HarvestItem.HarvestTime * 10)
            {
                Debug.Log("�۹� 2�ܰ� ����");
                _fieldSlots[i].GrowingCrops(2);
            }
            else if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes >= _fieldSlots[i].HarvestItem.HarvestTime * 5)
            {
                Debug.Log("�۹� 1�ܰ� ����");
                _fieldSlots[i].GrowingCrops(1);
            }
        }
    }

}
