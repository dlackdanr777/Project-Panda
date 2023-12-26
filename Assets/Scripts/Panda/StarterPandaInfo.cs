using BT;
using System;
using System.IO;
using UnityEngine;

public class StarterPandaInfo
{
    public StarterPanda StarterPanda; // 저장 필요 x
    public string Mbti;
    public float Intimacy;
    public float Happiness;
    public bool IsExistingUser; // 나중에 userInfo에 있는 거 사용하기

    #region 코스튬
    public CostumeViewModel CostumeViewModel; // 저장 필요 x
    public int WearingHeadCostumeID = -1;
    public int CostumeCount = -1;
    public bool[] IsMine;
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

        IsExistingUser = true;
        // 저장된 게임이 있다면 저장된 파일 읽어오기
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
        // 클래스를 Json 형식으로 전환
        string json = JsonUtility.ToJson(this, true);

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(_path, json );

        Debug.Log(_path);
    }

    // 현재 가지고 있는 옷 불러오기
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

            // 코스튬 불러온 후 저장하기
            SaveMyCostume();
        }
        else
        {
            Debug.Log("코스튬 매니저 null");
        }
    }

    // 현재 가지고 있는 옷 저장하기
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
            Debug.Log("코스튬 매니저 null");
        }
    }
}

