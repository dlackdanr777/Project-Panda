using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
{

    [SerializeField] private GameObject _fieldSlotParent;

    //�÷��̾�� ������ ���������� �����Ƿ� �����´�.
    private Player _player => GameManager.Instance.Player;

    //ä�� �������� ������ �������� ������ �Ѵ�.
    private DatabaseManager _database => DatabaseManager.Instance;

    //�۹��� ������ ���Ե�
    //Init���� _fieldSlotParent�ڽ��� ���� �ҷ��� ����
    //���� FieldSlot Ŭ������ �����ؾ��Ұ��Դϴ�.
    private GameObject[] _fieldSlots;


    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<GameObject>();
    }



    //���� ������ ���� ���ӽð��� �ð� ���̸� Ȯ�� �� �۹� ����ð��� ������ Ȯ���ϴ� �Լ�
    private bool FirstCheckGrowth()
    {
        //���� ���� �ð��� ������ ���� �ð��� ���Ѵ�.
        //if((UserInfo.TODAY - UserInfo._lastAccessDay).Minutes <= �۹�����ð� ����)
            //return false;

        return true;
    }


}
