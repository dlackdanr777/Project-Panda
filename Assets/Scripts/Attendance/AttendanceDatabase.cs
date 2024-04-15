using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using Muks.BackEnd;


public class AttendanceDatabase
{
    public UserInfo UserInfo => DatabaseManager.Instance.UserInfo;

    //=======================날씨 데이터==========================

    private Dictionary<int, AttendanceRewardData> _attendanceDataDic = new Dictionary<int, AttendanceRewardData>();

    //===========================================================

    public bool IsAttendanced{get; private set;}

    public void Register()
    {
        WeatherParseByLocal();
    }


    /// <summary>출석 체크 함수</summary> 
    public void AttendanceCheck()
    {
        UserInfo.AttendanceUserData.AttendanceCheck();

        int attendanceDayCount = UserInfo.AttendanceUserData.AttendanceDayCount;
        GiveReward(_attendanceDataDic[attendanceDayCount].Item, _attendanceDataDic[attendanceDayCount].Amount);
        DatabaseManager.Instance.UserInfo.AttendanceUserData.AsyncSaveAttendanceData(10);
    }


    /// <summary>오늘 출석 체크를 했나 확인 하는 함수</summary>
    public bool CheckTodayAttendance()
    {
        DateTime nowDay = UserInfo.TODAY;
        DateTime lastLoginDate = DateTime.Parse(UserInfo.AttendanceUserData.LastAttendanceDay);

        if (nowDay.Month != lastLoginDate.Month || nowDay.Day != lastLoginDate.Day)
        {
            IsAttendanced = false;
            return false;
        }
        IsAttendanced = true;
        return true;
    }


    //보상지급 함수
    private void GiveReward(Item item, int value)
    {
        GameManager.Instance.Player.AddItemById(item.Id, value);
        Debug.Log("보상이 지급됬습니다.");
    }


    /// <summary>DayCount를 기준으로 DayCount + 6일까지의 List를 반환하는 함수</summary>
    public List<AttendanceRewardData> GetAttendanceRewardDataForDayCount()
    {
        int count = 0;

        if(!CheckTodayAttendance())
        {
            count = UserInfo.AttendanceUserData.AttendanceDayCount + 1;
        }
        else
        {
            count = UserInfo.AttendanceUserData.AttendanceDayCount;
        }
        
        List<AttendanceRewardData> list = new List<AttendanceRewardData>();
        for (int i = count; i < count + 6; i++)
        {
            list.Add(_attendanceDataDic[i]);
        }

        return list;
    }


    #region LoadWeatherData
    public void LoadData()
    {
        BackendManager.Instance.GetChartData("107804", 10, WeatherParseByServer);
    }


    /// <summary>서버에서 코스튬 정보를 받아와 딕셔너리에 넣는 함수</summary>
    private void WeatherParseByServer(BackendReturnObject callback)
    {
        _attendanceDataDic.Clear();
        JsonData json = callback.FlattenRows();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            int day = int.Parse(json[i]["Day"].ToString());
            string weather = json[i]["Weather"].ToString();
            string itemId = json[i]["ItemId"].ToString();
            int amount = int.Parse(json[i]["Amount"].ToString());

            Item item = DatabaseManager.Instance.ItemDatabase.GetGatheringItemById(itemId);
            Sprite sprite = DatabaseManager.Instance.WeatherImage.GetWeatherImage(weather);

            _attendanceDataDic.Add(day, new AttendanceRewardData(day, weather, amount, item, sprite));
        }


        Debug.Log("날씨 정보 받아오기 성공");
    }


    private void WeatherParseByLocal()
    {
        if(_attendanceDataDic.Count <= 0)
        {
            Parser parser = new Parser();
            _attendanceDataDic = parser.WeatherParse("Weather");
        }
    }
    #endregion


    public Dictionary<int, AttendanceRewardData> GetWeekAttendanceData()
    {
        return _attendanceDataDic;
    }
}
