using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TmpNoticeManagement : IDisposable
{
    private int _maxRepeatCount;

    public TmpNoticeManagement() 
    {
        _maxRepeatCount = 10;
    }


    /// <summary>�ڳ� �ӽ� ���� Ȯ�� �Լ�</summary>
    public bool TmpNoticeCheck(UnityAction onButtonClicked = null)
    {
        if (_maxRepeatCount <= 0)
        {
            string errorName = "���� ���� ����";
            string errorDescription = "������ �������� ���߽��ϴ�. \n�� ���� ���ּ���.";
            BackendManager.Instance.ShowPopup(errorName, errorDescription, () => Application.Quit());
            return false;
        }

        string tmpNotice = Backend.Notice.GetTempNotice();

        //������ ������
        if (string.IsNullOrEmpty(tmpNotice))
            return false;

        //������
        JsonData data = JsonMapper.ToObject(tmpNotice);
        bool isUse = bool.Parse(data["isUse"].ToString());

        if (isUse)
        {
            string title = "��������";
            string description = data["contents"].ToString();
            BackendManager.Instance.ShowPopup(title, description, onButtonClicked);
            return true;
        }

        return false;
    }


    public void Dispose()
    {
    }


}
