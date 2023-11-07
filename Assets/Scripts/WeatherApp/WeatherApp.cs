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

    public static DateTime TODAY;
    private DateTime _endDay; //이벤트 종료일
    private UserInfo _userInfo;


    private DataManager _dataManager => DataManager.Instance;

    //=======================날씨 데이터==========================


    public enum Seasons { Spring, Summer, Fall, Winter }

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>현재 날씨</summary>
    [SerializeField] private Seasons _currentSeason;

    [SerializeField] private List<string> _weekWeathers;

    private WeightedRandom<string> _weatherDatas;


    //===========================================================

    private void Awake()
    {
        Init();
        Init2();
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

        //====테스트====


    }

    private void Init2()
    {
        TODAY = DateTime.Now;
        _userInfo = new UserInfo();

        _endDay = TODAY.AddDays(1);
    }

    //받은 아이템을 출력해주는 함수
    private void GetItemPrint(ItemData item)
    {
        Debug.LogFormat("{0}일자 아이템입니다.", _userInfo.DayCount);
        Debug.LogFormat("{0} , {1}", item.Name, item.Amount);
    }

    private void View(string check)
    {
        //데이터를 불러온다.
        Dictionary<int,EverydayData> data = _dataManager.GetEverydayDatas();
        List<int> list = new List<int>(data.Keys);

        if(check != "1")
        {
            Debug.Log("---하루소요---");
            Debug.LogFormat("오늘날짜:{0} 월{1} 일{2} / 이벤트 남은 일자:[{3}일]", TODAY.Month, TODAY.Day, _endDay.Day - TODAY.Day);
            return;
        }

        foreach(var key in list)
        {
            var item = _dataManager.GetItemData(data[key].Item);

            if (data[key].Date % 7 == 0) { }
        }
    } 

    public void GetItem(ItemData rewardedItem, int amount)
    {

    }

    private void Login()
    {
        _userInfo.LoadUserInfoData();
    }
}
