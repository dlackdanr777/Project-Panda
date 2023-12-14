using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class UserInfo
{

    //유저 데이터
    //==========================================================================================================

    public string UserId;    //아이디
    public DateTime TODAY => DateTime.Now;    //컴퓨터의 현재 날짜와 시간을 가져옴(추후 서버 시간으로 변경해야함)

    public DateTime LastAccessDay => DateTime.Parse(_lastAccessDay); //마지막 접속일

    public string _lastAccessDay;

    public int DayCount; //몇일 접속했나?

    public bool IsTodayRewardReceipt; //오늘 아이템을 수령했나?

    public bool IsExistingUser; //기존 유저인가?


    //==========================================================================================================

    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    //public static string PhotoPath => Application.persistentDataPath;

    public static string PhotoPath => "Data/";


    public void Register()
    {
        LoadUserInfoData();
    }


    //유저의 데이터를 가져오는 함수
    public void LoadUserInfoData()
    {
        Debug.Log(_path);
        if (!File.Exists(_path))
        {
            Debug.Log("유저 저장 문서가 존재하지 않습니다.");

            CreateUserInfoData();
            return;
        }

        UserInfo userInfo = new UserInfo();

        string loadJson = File.ReadAllText(_path);
        userInfo = JsonUtility.FromJson<UserInfo>(loadJson);

        UserId = userInfo.UserId;
        string paser = userInfo._lastAccessDay.ToString();
        _lastAccessDay = paser;
        DayCount = userInfo.DayCount;  
        IsExistingUser = userInfo.IsExistingUser;

        IsTodayRewardReceipt = true;

        if (TODAY.Day > LastAccessDay.Day)
        {
            Debug.Log("실행");
            DayCount++;
            IsTodayRewardReceipt = false;
        }
    }

    private void CreateUserInfoData()
    {
        IsExistingUser = false;
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;

        DayCount++;
        IsTodayRewardReceipt = false;

        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
    }


    public void SaveUserInfoData()
    {
        string paser = DateTime.Now.ToString();
        _lastAccessDay = paser;
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(_path, json);
        Debug.Log(_path);
    }


}
