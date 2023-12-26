using BT;
using System;
using System.IO;
using UnityEngine;

public class StarterPandaInfo
{
    public StarterPanda StarterPanda; // ���� �ʿ� x
    public string Mbti;
    public float Intimacy;
    public float Happiness;
    public bool IsExistingUser; // ���߿� userInfo�� �ִ� �� ����ϱ�

    #region �ڽ�Ƭ
    public CostumeViewModel CostumeViewModel; // ���� �ʿ� x
    public int WearingHeadCostumeID = -1;
    public int CostumeCount = -1;
    public bool[] IsMine;
    #endregion


    // �Ǵ� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        LoadPandaInfoData();
    }

    // �ҷ�����
    private void LoadPandaInfoData()
    {
        Debug.Log(_path);

        // ����� ������ ���ٸ�
        if (!File.Exists(_path))
        {
            Debug.Log("�Ǵ� ���� ������ �������� �ʽ��ϴ�.");

            CreateUserInfoData();
            return;
        }

        IsExistingUser = true;
        // ����� ������ �ִٸ� ����� ���� �о����
        StarterPandaInfo pandaInfo = new StarterPandaInfo();
        string loadJson = File.ReadAllText(_path);
        pandaInfo = JsonUtility.FromJson<StarterPandaInfo>(loadJson);

        Mbti = pandaInfo.Mbti;
        Intimacy = pandaInfo.Intimacy;
        Happiness = pandaInfo.Happiness;
        WearingHeadCostumeID = pandaInfo.WearingHeadCostumeID;
        CostumeCount = pandaInfo.CostumeCount;
        IsMine = new bool[CostumeCount];
        for (int i = 0; i < CostumeCount; i++)
        {
            IsMine[i] = pandaInfo.IsMine[i];
        }

    }

    private void CreateUserInfoData()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }

    public void SavePandaInfoData()
    {
        // Ŭ������ Json �������� ��ȯ
        string json = JsonUtility.ToJson(this, true);

        // �̹� ����� ������ �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(_path, json );

        Debug.Log(_path);
    }

    // ���� ������ �ִ� �� �ҷ�����
    public void LoadMyCostume()
    {
        if (CostumeManager.Instance.CostumeDic != null)
        {
            for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
            {
                if (CostumeCount >= i)
                {
                    CostumeManager.Instance.CostumeDic[i].IsMine = IsMine[i];
                }
                else
                {
                    CostumeManager.Instance.CostumeDic[i].IsMine = false;
                }
            }

            // �ڽ�Ƭ �ҷ��� �� �����ϱ�
            SaveMyCostume();
        }
        else
        {
            Debug.Log("�ڽ�Ƭ �Ŵ��� null");
        }
    }

    // ���� ������ �ִ� �� �����ϱ�
    public void SaveMyCostume()
    {
        if (CostumeManager.Instance.CostumeDic != null)
        {
            CostumeCount = CostumeManager.Instance.CostumeDic.Count;
            IsMine = new bool[CostumeCount];
            for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
            {
                IsMine[i] = CostumeManager.Instance.CostumeDic[i].IsMine;
            }
            SavePandaInfoData();
        }
        else
        {
            Debug.Log("�ڽ�Ƭ �Ŵ��� null");
        }
    }
}

