using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.WeightedRandom;


public enum Seasons { Spring, Summer, Fall, Winter, Count }
public enum Weather {Sunny, Cloudy, Rainy, Count }

/// <summary>날씨 가중치 데이터를 가지고 있는 클래스</summary>
[Serializable]
public class RamdomWeather
{

    [SerializeField] private int _sunnyWeighted;
    /// <summary>맑은 날씨 가중치</summary>
    public int SunnyWeighted => _sunnyWeighted;


    [SerializeField] private int _cloudyWeighted;
    /// <summary>흐린 날씨 가중치</summary>
    public int CloudyWeighted => _cloudyWeighted;

    [SerializeField] private int _rainyWeighted;

    /// <summary>비 날씨 가중치</summary>
    public int RainyWeighted => _rainyWeighted;

}


public class WeatherApp : MonoBehaviour
{
    [SerializeField] private UIWeather _uiWeather;

    public UserInfo UserInfo {get; private set; }

    private DataManager _dataManager => DataManager.Instance;

    //=======================날씨 데이터==========================

    [SerializeField] private WeatherData _sunnyData;
    [SerializeField] private WeatherData _cloudyData;
    [SerializeField] private WeatherData _rainyData;

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>현재 날씨</summary>
    [SerializeField] private Seasons _currentSeason;


    public List<WeatherData> _weekWeathers { get; private set; }
    private WeightedRandom<WeatherData> _weatherDatas;


    private bool _isCanReward; //보상획득 가능 유무
    public bool IsCanReward => _isCanReward;

    //===========================================================

    public void Awake()
    {
        Login();
        Init();
        AttendanceCheck();
        _uiWeather.Init();
    }

    private void Init()
    {
        _weatherDatas = new WeightedRandom<WeatherData>();
        _weekWeathers = new List<WeatherData>();

        _weatherDatas.Add(_sunnyData, _ramdomWeathers[(int)_currentSeason].SunnyWeighted);
        _weatherDatas.Add(_cloudyData, _ramdomWeathers[(int)_currentSeason].CloudyWeighted);
        _weatherDatas.Add(_rainyData, _ramdomWeathers[(int)_currentSeason].RainyWeighted);

        SetWeekWeather();
    }

    //일주일치 날씨를 설정하는 함수
    private void SetWeekWeather()
    {
        for (int i = 0; i < 7; i++)
        {
            _weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
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
        if (UserInfo.TODAY.Day > UserInfo.LastAccessDay.Day)
        {

            //UserInfo.LastAccessDay = DateTime.Now;
            UserInfo.DayCount++;
            _uiWeather.OnRewardedHandler += GiveReward;
            _isCanReward = true;

            return true;
        }

        return false;
    }

    //일주일 단위를 체크하는 함수
    private bool WeeksCheck()
    {
        if(UserInfo.DayCount % 7 == 1)
        {
            return true;
        }
        return false;
    }


    private void Login()
    {
        UserInfo = new UserInfo();
        UserInfo.LoadUserInfoData();
    }

    //보상지급 함수
    private void GiveReward()
    {
        Debug.Log("보상이 지급됬습니다.");
    }

    public List<WeatherData> GetWeekWeathers()
    {
        return _weekWeathers;
    }

}
