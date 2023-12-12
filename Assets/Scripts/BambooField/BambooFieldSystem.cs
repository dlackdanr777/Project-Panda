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

    // �÷��̾��� ���� �ð� Ȯ�� �� ���� ���� �Ǻ�
    public DateTime ConnectionTerminationTime;
    
    /// <summary>
    /// ���� ���� ���� �� �ð� ���� </summary>
    public void ConnectionTermination()
    {
        ConnectionTerminationTime = DateTime.Now;
    }

    /// <summary>
    /// ���� ���� �� �ð� üũ �� �۹� ���� </summary>
    private void CheckTime()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeDif = now - ConnectionTerminationTime;

        // ���߿� ���� ����
        if(timeDif.Days == 0)
        {
            //������ ������ ������ ������ ��� - �۹� �������� ����
            Debug.Log("�ð� ����: " + timeDif.Hours);
        }
        else if(timeDif.Days > 0)
        {
            //������ ������ ������ 1�� �ʰ��� ��� - �۹� ����
            Debug.Log("24�ð� ���� ��: "+ timeDif.Days);
            for(int i = 0; i< _fieldSlots.Length; i++)
            {
                _fieldSlots[i].GrowingCrops(1);
            }
        }
        else if(timeDif.Days > 1)
        {
            // �۹� 2�ܰ� ����
            Debug.Log("�۹� 2�ܰ� ����");
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



    //���� ������ ���� ���ӽð��� �ð� ���̸� Ȯ�� �� �۹� ����ð��� ������ Ȯ���ϴ� �Լ�
    private bool FirstCheckGrowth()
    {
        for(int i = 0; i< _fieldSlots.Length; i++)
        {
            //���� ���� �ð��� ������ ���� �ð��� ���Ѵ�.
            if ((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes <= _fieldSlots[i].HarvestItem.HarvestTime) // �۹� ����ð� ����
                return false;
        }

        return true;
    }


}
