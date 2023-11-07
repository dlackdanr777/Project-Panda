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

    public static DateTime TODAY;
    private DateTime _endDay; //�̺�Ʈ ������
    private UserInfo _userInfo;


    private DataManager _dataManager => DataManager.Instance;

    //=======================���� ������==========================


    public enum Seasons { Spring, Summer, Fall, Winter }

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>���� ����</summary>
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

        _weatherDatas.Add("����", _ramdomWeathers[(int)_currentSeason].SunnyWeighted);
        _weatherDatas.Add("�帲", _ramdomWeathers[(int)_currentSeason].CloudyWeighted);
        _weatherDatas.Add("��", _ramdomWeathers[(int)_currentSeason].RainyWeighted);

        for(int i = 0; i < 7; i++)
        {
            _weekWeathers.Add(_weatherDatas.GetRamdomItemBySub());
        }

        //====�׽�Ʈ====


    }

    private void Init2()
    {
        TODAY = DateTime.Now;
        _userInfo = new UserInfo();

        _endDay = TODAY.AddDays(1);
    }

    //���� �������� ������ִ� �Լ�
    private void GetItemPrint(ItemData item)
    {
        Debug.LogFormat("{0}���� �������Դϴ�.", _userInfo.DayCount);
        Debug.LogFormat("{0} , {1}", item.Name, item.Amount);
    }

    private void View(string check)
    {
        //�����͸� �ҷ��´�.
        Dictionary<int,EverydayData> data = _dataManager.GetEverydayDatas();
        List<int> list = new List<int>(data.Keys);

        if(check != "1")
        {
            Debug.Log("---�Ϸ�ҿ�---");
            Debug.LogFormat("���ó�¥:{0} ��{1} ��{2} / �̺�Ʈ ���� ����:[{3}��]", TODAY.Month, TODAY.Day, _endDay.Day - TODAY.Day);
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
