using System;
using System.IO;
using UnityEngine;

public class StarterPandaInfo
{
    public string Mbti;
    public float Intimacy;
    public float Happiness;

    #region �ڽ�Ƭ
    public CostumeViewModel CostumeViewModel; // ���� �ʿ� x
    public int WearingHeadCostumeID = -1;
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

        // ����� ������ �ִٸ� ����� ���� �о����
        StarterPandaInfo pandaInfo = new StarterPandaInfo();
        string loadJson = File.ReadAllText(_path);
        pandaInfo = JsonUtility.FromJson<StarterPandaInfo>(loadJson);

        Mbti = pandaInfo.Mbti;
        Intimacy = pandaInfo.Intimacy;
        Happiness = pandaInfo.Happiness;
        WearingHeadCostumeID = pandaInfo.WearingHeadCostumeID;
        // ���� ������ �ִ� �� ����
        if(CostumeManager.Instance.CostumeDic != null)
        {
            for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
            {
                CostumeManager.Instance.CostumeDic[i].IsMine = pandaInfo.IsMine[i];
            }
        }
        else
        {
            Debug.Log("�ڽ�Ƭ �Ŵ��� null"); // ���� �Լ� ���� �ڽ�Ƭ ���� �ҷ��� �� �����ؾ� ��
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
}

