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

    //=======================���� ������==========================

    private Dictionary<int, WeatherRewardData> _weatherDataDic = new Dictionary<int, WeatherRewardData>();

    private bool _isCanReward; //����ȹ�� ���� ����
    public bool IsCanReward => _isCanReward;

    //===========================================================


    public void Register()
    {
        WeatherParseByLocal();
    }


    /// <summary>�⼮ üũ �Լ�</summary> 
    public void ChecktAttendance()
    {
        UserInfo.AttendanceDayCount++;
        UserInfo.LastAttendanceDay = UserInfo.TODAY.ToString();
        GiveReward(_weatherDataDic[UserInfo.AttendanceDayCount].Item);
        Debug.Log(UserInfo.AttendanceDayCount);
    }


    /// <summary>���� �⼮ üũ�� �߳� Ȯ�� �ϴ� �Լ�</summary>
    public bool CheckTodayAttendance()
    {
        DateTime nowDay = UserInfo.TODAY; //ToDo: ���� �ð����� ���� ����
        DateTime lastLoginDate = DateTime.Parse(UserInfo.LastAttendanceDay);

        if (nowDay.Month != lastLoginDate.Month || nowDay.Day != lastLoginDate.Day)
        {
            Debug.Log("�⼮üũ ���� ����");
            return false;
        }
        Debug.Log("�⼮üũ ��");
        return true;
    }


    //�������� �Լ�
    private void GiveReward(Item item)
    {
        InventoryItemField field = GameManager.Instance.Player.GetField(item.Id);
        int itemType = GameManager.Instance.Player.GetItemType(item.Id);
        GameManager.Instance.Player.GetItemInventory(field)[itemType].AddById(field, item.Id);
        Debug.Log("������ ���މ���ϴ�.");
    }


    /// <summary>DayCount�� �������� DayCount+ 4�ϱ����� List�� ��ȯ�ϴ� �Լ�</summary>
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
        }


        Debug.Log("���� ���� �޾ƿ��� ����");
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
