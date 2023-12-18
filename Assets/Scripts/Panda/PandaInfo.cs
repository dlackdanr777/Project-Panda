using System;
using System.IO;
using UnityEngine;

public class PandaInfo
{
    public string Mbti;
    public float Intimacy;
    public float Happiness;

    public bool IsExistingPandaInfo;

    // 판다 데이터 저장 경로 (추후 DB에 업로드해야함)
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
            Debug.Log("판다 저장 문서가 존재하지 않습니다.");

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

