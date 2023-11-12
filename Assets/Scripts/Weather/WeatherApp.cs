using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.WeightedRandom;


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

    private UserInfo _userInfo;

    private DataManager _dataManager => DataManager.Instance;

    //=======================���� ������==========================


    public enum Seasons { Spring, Summer, Fall, Winter }

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>���� ����</summary>
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

        _weatherDatas.Add("����", _ramdomWeathers[(int)_currentSeason].SunnyWeighted);
        _weatherDatas.Add("�帲", _ramdomWeathers[(int)_currentSeason].CloudyWeighted);
        _weatherDatas.Add("��", _ramdomWeathers[(int)_currentSeason].RainyWeighted);

        for(int i = 0; i < 7; i++)
        {
            _weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
        }
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


        _userInfo._lastAccessDay = DateTime.Now;
        _userInfo.DayCount++;
    }

    //���� ������ üũ�ϴ� �Լ�
    private bool RewardedCheck()
    {
        //���� ���� ��¥ ������ �����ߴٸ�?
        if (UserInfo.TODAY.Day -1 == _userInfo._lastAccessDay.Day)
        {
            return true;
        }

        return false;
    }

    //������ ������ üũ�ϴ� �Լ�
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
