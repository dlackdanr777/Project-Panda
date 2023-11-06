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
    public enum Seasons { Spring, Summer, Fall, Winter }

    [SerializeField] private RamdomWeather[] _ramdomWeathers;

    /// <summary>���� ����</summary>
    [SerializeField] private Seasons _currentSeason;

    [SerializeField] private List<string> _weekWeathers;

    private WeightedRandom<string> _weatherDatas;


    private void Awake()
    {
        Init();
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


}
