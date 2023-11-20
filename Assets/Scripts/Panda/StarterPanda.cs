using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��� ������Ʈ�� �����ϴ� �����̳� Ŭ����
/// �̰����� ��� ������Ʈ�� �Լ��� ���� �� �����Ѵ�.
/// ����: �Ǵٿ� ���� ������Ʈ���� ���� ������ ���� �ʵ��� �Ͽ� Ȯ�强�� �������� �ø� �� �ִ�.
/// </summary>
public class StarterPanda : Panda
{
    [SerializeField]
    private UIPanda _uiPanda;
    private bool _isUISetActive;
    public PreferenceData preferenceData;
    public MBTIData mbtiData;

    // ���� �ʿ� - NPC���� ���� �κ� Panda�� �ű��
    private void Awake()
    {
        //test - �Ǵ� ���� �ʱ� ����
        Nature = MBTIData.MBTI.intp;
        Intimacy = 0; //ģ�е� 0���� ����
        //(����)mbti ���� ����
        //mbtiData = new MBTIData();
        //mbtiData.SetMBTI();
        //preferenceData = mbtiData._preferenceDatas[(int)Nature];

        stateData = new StateData();
        stateData.InitStateData();
        _uiPanda.gameObject.SetActive(true);
        _isUISetActive = false;
    }

    private void Update()
    {
        PandaMouseClick();
        stateData.CheckState();
    }

    //�Ǵ� Ŭ���ϸ� ��ư ǥ��
    private void PandaMouseClick()
    {
        //(����)�Ǵ� Ŭ���ϴ� ������ ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleUIPandaButton();
        }
    }
    public void ToggleUIPandaButton()
    {
        _isUISetActive = !_isUISetActive;
        _uiPanda.transform.GetChild(0).gameObject.SetActive(_isUISetActive);
    }

    private void TakeGift()
    {
        // �峭������ �������� Ȯ��,.
        // ���� ������ ���� Ŭ�������� �������� Ȯ��
        // ���⿡ ���� ���� Ŭ������ �̿��� ���� ����

    }
    public override void AddIntimacy()
    {
        throw new System.NotImplementedException();
    }
    public override void SubIntimacy()
    {
        throw new System.NotImplementedException();
    }
}
