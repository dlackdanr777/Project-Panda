using System;
using System.IO;
using UnityEngine;

public class PandaInfo
{
    public string Mbti;
    public float Intimacy;
    public float Happiness;

    public bool IsExistingPandaInfo;

    // �Ǵ� ������ ���� ��� (���� DB�� ���ε��ؾ���)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        LoadPandaInfoData();
    }

    private void LoadPandaInfoData()
    {
        Debug.Log(_path);
        if (!File.Exists(_path))
        {
            Debug.Log("�Ǵ� ���� ������ �������� �ʽ��ϴ�.");

            CreateUserInfoData();
            return;
        }

        PandaInfo pandaInfo = new PandaInfo();
        string loadJson = File.ReadAllText(_path);
        pandaInfo = JsonUtility.FromJson<PandaInfo>(loadJson);

        Mbti = pandaInfo.Mbti;
        Intimacy = pandaInfo.Intimacy;
        Happiness = pandaInfo.Happiness;
    }

    private void CreateUserInfoData()
    {
        IsExistingPandaInfo = false;

        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }

    public void SavePandaInfoData()
    {
        string json = JsonUtility.ToJson(this, true);

        File.WriteAllText(_path, json );
        Debug.Log(_path);
    }
}

