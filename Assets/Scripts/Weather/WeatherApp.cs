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


    private bool _isCanReward; //����ȹ�� ���� ����
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
        //���� ������ġ �⼮üũ�� �Ϸ��ߴٸ�
        if (WeeksCheck())
        {
            //�����Ѵ�.
            SetWeekWeather();
        }

        //���� �����߾��ٸ� �ѱ��.
        if (!RewardedCheck())
            return;



    }

    //���� ������ üũ�ϴ� �Լ�
    private bool RewardedCheck()
    {
        //���� ���� ��¥ ������ �����ߴٸ�?
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

    //������ ������ üũ�ϴ� �Լ�
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

    //�������� �Լ�
    private void GiveReward()
    {
        Debug.Log("������ ���މ���ϴ�.");
    }

    public List<WeatherData> GetWeekWeathers()
    {
        return _weekWeathers;
    }

}
