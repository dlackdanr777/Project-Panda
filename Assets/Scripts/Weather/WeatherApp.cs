using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.WeightedRandom;


public enum Seasons { Spring, Summer, Fall, Winter, Count }
public enum Weather {Sunny, Cloudy, Rainy, Count }

/// <summary>���� ����ġ �����͸� ������ �ִ� Ŭ����</summary>
[Serializable]
public class RamdomWeather
{

    [SerializeField] private int _sunnyWeighted;
    /// <summary>���� ���� ����ġ</summary>
    public int SunnyWeighted => _sunnyWeighted;


    [SerializeField] private int _cloudyWeighted;
    /// <summary>�帰 ���� ����ġ</summary>
    public int CloudyWeighted => _cloudyWeighted;

    [SerializeField] private int _rainyWeighted;

    /// <summary>�� ���� ����ġ</summary>
    public int RainyWeighted => _rainyWeighted;

}


public class WeatherApp : MonoBehaviour
{
    [SerializeField] private UIWeather _uiWeather;

    public UserInfo UserInfo {get; private set; }

    private DataManager _dataManager => DataManager.Instance;

    //=======================���� ������==========================

    [SerializeField] private WeatherData _sunnyData;
    [SerializeField] private WeatherData _cloudyData;
    [SerializeField] private WeatherData _rainyData;

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>���� ����</summary>
    [SerializeField] private Seasons _currentSeason;


    public List<WeatherData> _weekWeathers { get; private set; }
    private WeightedRandom<WeatherData> _weatherDatas;

    //===========================================================

    private void Awake()
    {
        Login();
        Init();
        AttendanceCheck();
    }

    private void Init()
    {
        _weatherDatas = new WeightedRandom<WeatherData>();
        _weekWeathers = new List<WeatherData>();

        _weatherDatas.Add(_sunnyData, _ramdomWeathers[(int)_currentSeason].SunnyWeighted);
        _weatherDatas.Add(_cloudyData, _ramdomWeathers[(int)_currentSeason].CloudyWeighted);
        _weatherDatas.Add(_rainyData, _ramdomWeathers[(int)_currentSeason].RainyWeighted);

        SetWeekWeather();

        _uiWeather.Init();
    }

    //������ġ ������ �����ϴ� �Լ�
    private void SetWeekWeather()
    {
        for (int i = 0; i < 7; i++)
        {
            _weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
        }
    }

    //�⼮ üũ
    private void AttendanceCheck()
    {
        //���� �����߾��ٸ� �ѱ��.
        if (RewardedCheck())
            return;

        //���� ������ġ �⼮üũ�� �Ϸ��ߴٸ�
        if (WeeksCheck())
        {
            //�ٽ� �����Ѵ�.
            SetWeekWeather();
        }


        UserInfo._lastAccessDay = DateTime.Now;
        UserInfo.DayCount++;
    }

    //���� ������ üũ�ϴ� �Լ�
    private bool RewardedCheck()
    {
        //���� ���� ��¥ ������ �����ߴٸ�?
        if (UserInfo.TODAY.Day -1 == UserInfo._lastAccessDay.Day)
        {
            return true;
        }

        return false;
    }

    //������ ������ üũ�ϴ� �Լ�
    private bool WeeksCheck()
    {
        if(UserInfo.DayCount % 7 == 0)
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
}
