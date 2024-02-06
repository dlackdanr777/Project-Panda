using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.WeightedRandom;
using BackEnd;
using LitJson;
using Muks.BackEnd;

public enum Weather {Sunny, Cloudy, Rainy, Count }


public class WeatherDatabase
{
    public UserInfo UserInfo => DatabaseManager.Instance.UserInfo;

    //=======================날씨 데이터==========================

    public Dictionary<int, WeatherRewardData> _weatherDataDic { get; private set; }

    private bool _isCanReward; //보상획득 가능 유무
    public bool IsCanReward => _isCanReward;

    //===========================================================


    public void Register()
    {
        Init();
        AttendanceCheck();
    }


    private void Init()
    {
        _weatherDataDic = new Dictionary<int, WeatherRewardData>();

        //_weatherDatas.Add(_sunnyData, _ramdomWeathers[(int)_currentSeason].SunnyWeighted);
        //_weatherDatas.Add(_cloudyData, _ramdomWeathers[(int)_currentSeason].CloudyWeighted);
        //_weatherDatas.Add(_rainyData, _ramdomWeathers[(int)_currentSeason].RainyWeighted);

        SetWeekWeather();
    }


    //일주일치 날씨를 설정하는 함수
    private void SetWeekWeather()
    {
        for (int i = 0; i < 7; i++)
        {
            //_weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
        }
    }


    //출석 체크
    private void AttendanceCheck()
    {
        //만약 일주일치 출석체크를 완료했다면
        if (WeeksCheck())
        {
            //설정한다.
            SetWeekWeather();
        }

        //전날 접속했었다면 넘긴다.
        if (!RewardedCheck())
            return;
    }


    //현재 보상을 체크하는 함수
    private bool RewardedCheck()
    {
        //만약 현재 날짜 전날에 접속했다면?
        if (!UserInfo.IsTodayRewardReceipt)
        {
            //_uiWeather.OnRewardedHandler += GiveReward;
            UserInfo.IsTodayRewardReceipt = true;
            return true;
        }

        return false;
    }


    //일주일 단위를 체크하는 함수
    private bool WeeksCheck()
    {
        if (UserInfo.DayCount % 7 == 1)
        {
            return true;
        }
        return false;
    }


    //보상지급 함수
    private void GiveReward(Item item)
    {
        Debug.Log("보상이 지급됬습니다.");
    }

    /// <summary>DayCount를 기준으로 DayCount+ 4일까지의 List를 반환하는 함수</summary>
    public List<WeatherRewardData> GetWeatherRewardDataForDayCount()
    {
        int count = UserInfo.DayCount + 1;

        List<WeatherRewardData> list = new List<WeatherRewardData>();

        for(int i = count; i < count + 4; i++)
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

            Debug.Log("날씨 정보 받아오기 성공");
        }
    }
    #endregion


    public Dictionary<int, WeatherRewardData> GetWeekWeathers()
    {
        return _weatherDataDic;
    }
}
