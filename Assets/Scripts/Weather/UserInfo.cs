using Muks.WeightedRandom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo
{

    //유저 데이터
    //==========================================================================================================

    public string UserId;    //아이디
    public static DateTime TODAY => DateTime.Now;    //컴퓨터의 현재 날짜와 시간을 가져옴(추후 서버 시간으로 변경해야함)

    public static DateTime _lastAccessDay; //마지막 접속일

    public static int DayCount; //몇일 접속했나?

    public static bool IsTodayRewardReceipt; //오늘 아이템을 수령했나?

    public static bool IsExistingUser => false; //기존 유저인가?

    public static Dictionary<int, ItemData> DicRewardedItems;

    private static List<string> _weekWeathers;

    private static WeightedRandom<string> _weatherDatas;

    //==========================================================================================================

    //유저 데이터 저장 경로 (추후 DB에 업로드해야함)
    private static string _path => Path.Combine(Application.dataPath, "UserInfo.json");

    public static string PhotoPath => Application.persistentDataPath;

    //유저의 데이터를 가져오는 함수
    public void LoadUserInfoData()
    {

    }

    public static void SaveUserInfoData()
    {
        UserInfo userInfo = new UserInfo();
        string json = JsonUtility.ToJson(userInfo, true);
        File.WriteAllText(_path, json);
        Debug.Log(_path);
    }


}
