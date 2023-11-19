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
    public StateData stateData;
    [SerializeField]
    private UIPanda _uiPanda;
    private bool _isUISetActive;

    // ���� �ʿ�
    public StarterPanda(string mbti, Sprite image)
    {
        Nature = mbti;
        State = "�ູ";
        Intimacy = 0; //ģ�е� 0���� ����
        Image = image;
    }

    // ���� �ʿ�
    private void Awake()
    {
        //test
        Nature = "intp";
        State = "�ູ";
        Intimacy = 0; //ģ�е� 0���� ����

        //�Ǵ� ���� �ʱ� ����
        stateData = new StateData();
        stateData.InitStateData();
    }

    private void Update()
    {
        PandaMouseClick();
        stateData.ChangeState();
    }

    //�Ǵ� Ŭ���ϸ� ��ư ǥ��
    private void PandaMouseClick()
    {
        //(����)�Ǵ� Ŭ���ϴ� ������ ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleUIPanda();
        }
    }
    private void ToggleUIPanda()
    {
        _uiPanda.gameObject.SetActive(_isUISetActive);
        _isUISetActive = !_isUISetActive;
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
