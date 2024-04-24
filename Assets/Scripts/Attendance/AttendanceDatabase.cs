using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using Muks.BackEnd;


public class AttendanceDatabase
{
    public UserInfo UserInfo => DatabaseManager.Instance.UserInfo;

    //=======================���� ������==========================

    private Dictionary<int, AttendanceRewardData> _attendanceDataDic = new Dictionary<int, AttendanceRewardData>();

    //===========================================================

    public bool IsAttendanced{get; private set;}

    public void Register()
    {
        WeatherParseByLocal();
    }


    /// <summary>�⼮ üũ �Լ�</summary> 
    public void AttendanceCheck()
    {
        UserInfo.AttendanceUserData.AttendanceCheck();

        int attendanceDayCount = UserInfo.AttendanceUserData.AttendanceDayCount;
        GiveReward(_attendanceDataDic[attendanceDayCount].Item, _attendanceDataDic[attendanceDayCount].Amount);
        DatabaseManager.Instance.UserInfo.AttendanceUserData.AsyncSaveAttendanceData(10);
    }


    /// <summary>���� �⼮ üũ�� �߳� Ȯ�� �ϴ� �Լ�</summary>
    public bool CheckTodayAttendance()
    {
        DateTime nowDay = UserInfo.TODAY;
        DateTime lastLoginDate = DateTime.Parse(UserInfo.AttendanceUserData.LastAttendanceDay);

        if (nowDay.Month != lastLoginDate.Month || nowDay.Day != lastLoginDate.Day)
        {
            IsAttendanced = false;
            return false;
        }
        IsAttendanced = true;
        return true;
    }


    //�������� �Լ�
    private void GiveReward(Item item, int value)
    {
        GameManager.Instance.Player.AddItemById(item.Id, value);
        Debug.Log("������ ���މ���ϴ�.");
    }


    /// <summary>DayCount�� �������� DayCount + 6�ϱ����� List�� ��ȯ�ϴ� �Լ�</summary>
    public List<AttendanceRewardData> GetAttendanceRewardDataForDayCount()
    {
        int count = 0;

        if(!CheckTodayAttendance())
        {
            count = UserInfo.AttendanceUserData.AttendanceDayCount + 1;
        }
        else
        {
            count = UserInfo.AttendanceUserData.AttendanceDayCount;
        }
        
        List<AttendanceRewardData> list = new List<AttendanceRewardData>();
        for (int i = count; i < count + 6; i++)
        {
            list.Add(_attendanceDataDic[i]);
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
        _attendanceDataDic.Clear();
        JsonData json = callback.FlattenRows();

        for (int i = 0, count = json.Count; i < count; i++)
        {
            int day = int.Parse(json[i]["Day"].ToString());
            string weather = json[i]["Weather"].ToString();
            string itemId = json[i]["ItemId"].ToString();
            int amount = int.Parse(json[i]["Amount"].ToString());

            Item item = DatabaseManager.Instance.ItemDatabase.GetGatheringItemById(itemId);
            Sprite sprite = DatabaseManager.Instance.WeatherImage.GetWeatherImage(weather);

            _attendanceDataDic.Add(day, new AttendanceRewardData(day, weather, amount, item, sprite));
        }


        Debug.Log("���� ���� �޾ƿ��� ����");
    }


    private void WeatherParseByLocal()
    {
        if(_attendanceDataDic.Count <= 0)
        {
            Parser parser = new Parser();
            _attendanceDataDic = parser.WeatherParse("Weather");
        }
    }
    #endregion


    public Dictionary<int, AttendanceRewardData> GetWeekAttendanceData()
    {
        return _attendanceDataDic;
    }
}
