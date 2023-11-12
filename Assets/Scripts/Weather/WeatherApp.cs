using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.WeightedRandom;


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

    private UserInfo _userInfo;

    private DataManager _dataManager => DataManager.Instance;

    //=======================날씨 데이터==========================


    public enum Seasons { Spring, Summer, Fall, Winter }

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>현재 날씨</summary>
    [SerializeField] private Seasons _currentSeason;


    public List<string> _weekWeathers { get; private set; }
    private WeightedRandom<string> _weatherDatas;

    //===========================================================

    private void Awake()
    {
        Init();
        Login();
        AttendanceCheck();
    }

    private void Init()
    {
        _weatherDatas = new WeightedRandom<string>();
        _weekWeathers = new List<string>();

        _weatherDatas.Add("맑음", _ramdomWeathers[(int)_currentSeason].SunnyWeighted);
        _weatherDatas.Add("흐림", _ramdomWeathers[(int)_currentSeason].CloudyWeighted);
        _weatherDatas.Add("비", _ramdomWeathers[(int)_currentSeason].RainyWeighted);

        for(int i = 0; i < 7; i++)
        {
            _weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
        }
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
        //오늘 접속했었다면 넘긴다.
        if (RewardedCheck())
            return;

        //만약 일주일치 출석체크를 완료했다면
        if (WeeksCheck())
        {
            //다시 설정한다.
            SetWeekWeather();
        }


        _userInfo._lastAccessDay = DateTime.Now;
        _userInfo.DayCount++;
    }

    //현재 보상을 체크하는 함수
    private bool RewardedCheck()
    {
        //만약 현재 날짜 전날에 접속했다면?
        if (UserInfo.TODAY.Day -1 == _userInfo._lastAccessDay.Day)
        {
            return true;
        }

        return false;
    }

    //일주일 단위를 체크하는 함수
    private bool WeeksCheck()
    {
        if(_userInfo.DayCount % 7 == 0)
        {
            return true;
        }
        return false;
    }




    private void Login()
    {
        _userInfo = new UserInfo();
        _userInfo.LoadUserInfoData();
    }
}
