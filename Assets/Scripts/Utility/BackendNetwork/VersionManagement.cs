using BackEnd;
using Muks.BackEnd;
using System;
using UnityEngine;

public class VersionManagement : IDisposable
{
    private const string _playStoreLink = "market://details?id=��Ű�� ����";
    private const string _appStoreLink = "itms-apps://itunes.apple.com/app/��ID";


    private int _maxRepeatCount;


    public VersionManagement()
    {
        _maxRepeatCount = 10;
    }


    /// <summary>���� ������ Ŭ���̾�Ʈ ������ �´��� Ȯ�� �ϴ� �Լ� </summary>
    public bool UpdateCheck()
    {
#if UNITY_EDITOR
        return true;

#endif

        if (_maxRepeatCount <= 0)
        {
            string errorName = "���� ���� ����";
            string errorDescription = "������ �������� ���߽��ϴ�. \n�� ���� ���ּ���.";
            BackendManager.Instance.ShowPopup(errorName, errorDescription, () => Application.Quit());
            return false;
        }

        Version client = new Version(Application.version);
        Debug.Log("client Version: " + client);

        BackendReturnObject bro = Backend.Utils.GetLatestVersion();

        if (!bro.IsSuccess())
        {
            Debug.LogError("���� ��ȸ ����");
            return false;
        }

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                return false;

            case BackendState.Maintainance:
                return false;

            case BackendState.Retry:
                _maxRepeatCount--;
                return UpdateCheck();

            case BackendState.Success:
                string version = bro.GetReturnValuetoJSON()["version"].ToString();
                Version server = new Version(version);

                int result = server.CompareTo(client);
                if (result == 0)
                {
                    return true;
                }

                else if (client == null)
                {
                    Debug.Log("Ŭ���̾�Ʈ ���� ���� null");
                    return false;
                }

                OpenStoreLink();
                return false;
        }
        return false;
    }



    private void OpenStoreLink()
    {
        string errorName = "������Ʈ";
        string errorDescription = "�ֽ� ������ �����մϴ�. \n������Ʈ�� ������ �ּ���.";
        BackendManager.Instance.ShowPopup(errorName, errorDescription, () => Application.OpenURL("https://www.notion.so/Unity-Engine-Engineer-65cb31e1caac4a2d9a686c0bf22d355d"));

    }

    public void Dispose()
    {
    }
}
