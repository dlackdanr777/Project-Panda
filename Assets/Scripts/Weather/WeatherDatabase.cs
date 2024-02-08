using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.WeightedRandom;
using BackEnd;
using LitJson;
using Muks.BackEnd;
using static UserInfo;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;

public enum Weather {Sunny, Cloudy, Rainy, Count }


public class WeatherDatabase
{
    public UserInfo UserInfo => DatabaseManager.Instance.UserInfo;

    //=======================날씨 데이터==========================

    private Dictionary<int, WeatherRewardData> _weatherDataDic = new Dictionary<int, WeatherRewardData>();

    private bool _isCanReward; //보상획득 가능 유무
    public bool IsCanReward => _isCanReward;

    //===========================================================


    public void Register()
    {
        WeatherParseByLocal();
    }


    /// <summary>출석 체크 함수</summary> 
    public void ChecktAttendance()
    {
        UserInfo.AttendanceDayCount++;
        UserInfo.LastAttendanceDay = UserInfo.TODAY.ToString();
        GiveReward(_weatherDataDic[UserInfo.AttendanceDayCount].Item);
        Debug.Log(UserInfo.AttendanceDayCount);
    }


    /// <summary>오늘 출석 체크를 했나 확인 하는 함수</summary>
    public bool CheckTodayAttendance()
    {
        DateTime nowDay = UserInfo.TODAY; //ToDo: 서버 시간으로 변경 예정
        DateTime lastLoginDate = DateTime.Parse(UserInfo.LastAttendanceDay);

        if (nowDay.Month != lastLoginDate.Month || nowDay.Day != lastLoginDate.Day)
        {
            Debug.Log("출석체크 아직 안함");
            return false;
        }
        Debug.Log("출석체크 함");
        return true;
    }


    //보상지급 함수
    private void GiveReward(Item item)
    {
        InventoryItemField field = GameManager.Instance.Player.GetField(item.Id);
        int itemType = GameManager.Instance.Player.GetItemType(item.Id);
        GameManager.Instance.Player.GetItemInventory(field)[itemType].AddById(field, item.Id);
        Debug.Log("보상이 지급됬습니다.");
    }


    /// <summary>DayCount를 기준으로 DayCount+ 4일까지의 List를 반환하는 함수</summary>
    public List<WeatherRewardData> GetWeatherRewardDataForDayCount()
    {
        int count = 0;

        if(!CheckTodayAttendance())
        {
            count = UserInfo.AttendanceDayCount + 1;
        }
        else
        {
            count = UserInfo.AttendanceDayCount;
        }
        
        List<WeatherRewardData> list = new List<WeatherRewardData>();
        for (int i = count; i < count + 4; i++)
        {
            list.Add(_weatherDataDic[i]);
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
        _weatherDataDic.Clear();
        JsonData json = callback.FlattenRows();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            int day = int.Parse(json[i]["Day"].ToString());
            string weather = json[i]["Weather"].ToString();
            string itemId = json[i]["ItemId"].ToString();
            int amount = int.Parse(json[i]["Amount"].ToString());

            Item item = DatabaseManager.Instance.ItemDatabase.GetGatheringItemById(itemId);
            Sprite sprite = DatabaseManager.Instance.WeatherImage.GetWeatherImage(weather);

            _weatherDataDic.Add(day, new WeatherRewardData(day, weather, amount, item, sprite));
        }


        Debug.Log("날씨 정보 받아오기 성공");
    }


    private void WeatherParseByLocal()
    {
        if(_weatherDataDic.Count <= 0)
        {
            Parser parser = new Parser();
            _weatherDataDic = parser.WeatherParse("Weather");
        }
    }
    #endregion


    public Dictionary<int, WeatherRewardData> GetWeekWeathers()
    {
        return _weatherDataDic;
    }
}
