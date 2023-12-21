using System;
using System.IO;
using UnityEngine;

public class StarterPandaInfo
{
    public string Mbti;
    public float Intimacy;
    public float Happiness;

    #region 코스튬
    public int WearingHeadCostumeID = -1;
    public CostumeViewModel CostumeViewModel;
    #endregion


    // 판다 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "PandaInfo.json");


    public void Register()
    {
        LoadPandaInfoData();
    }

    // 불러오기
    private void LoadPandaInfoData()
    {
        Debug.Log(_path);

        // 저장된 게임이 없다면
        if (!File.Exists(_path))
        {
            Debug.Log("판다 저장 문서가 존재하지 않습니다.");

            CreateUserInfoData();
            return;
        }

        // 저장된 게임이 있다면 저장된 파일 읽어오기
        StarterPandaInfo pandaInfo = new StarterPandaInfo();
        string loadJson = File.ReadAllText(_path);
        pandaInfo = JsonUtility.FromJson<StarterPandaInfo>(loadJson);

        Mbti = pandaInfo.Mbti;
        Intimacy = pandaInfo.Intimacy;
        Happiness = pandaInfo.Happiness;
        WearingHeadCostumeID = pandaInfo.WearingHeadCostumeID;
}

    private void CreateUserInfoData()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }

    public void SavePandaInfoData()
    {
        // 클래스를 Json 형식으로 전환
        string json = JsonUtility.ToJson(this, true);

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(_path, json );

        Debug.Log(_path);
    }
}

