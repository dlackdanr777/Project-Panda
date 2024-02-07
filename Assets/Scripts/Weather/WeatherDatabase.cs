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

    //=======================���� ������==========================

    public Dictionary<int, WeatherRewardData> _weatherDataDic { get; private set; }

    private bool _isCanReward; //����ȹ�� ���� ����
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


    //������ġ ������ �����ϴ� �Լ�
    private void SetWeekWeather()
    {
        for (int i = 0; i < 7; i++)
        {
            //_weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
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
        if (!UserInfo.IsTodayRewardReceipt)
        {
            //_uiWeather.OnRewardedHandler += GiveReward;
            UserInfo.IsTodayRewardReceipt = true;
            return true;
        }

        return false;
    }


    //������ ������ üũ�ϴ� �Լ�
    private bool WeeksCheck()
    {
        if (UserInfo.DayCount % 7 == 1)
        {
            return true;
        }
        return false;
    }


    //�������� �Լ�
    private void GiveReward(Item item)
    {
        Debug.Log("������ ���މ���ϴ�.");
    }

    /// <summary>DayCount�� �������� DayCount+ 4�ϱ����� List�� ��ȯ�ϴ� �Լ�</summary>
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


    /// <summary>�������� �ڽ�Ƭ ������ �޾ƿ� ��ųʸ��� �ִ� �Լ�</summary>
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

            Debug.Log("���� ���� �޾ƿ��� ����");
        }
    }
    #endregion


    public Dictionary<int, WeatherRewardData> GetWeekWeathers()
    {
        return _weatherDataDic;
    }
}
