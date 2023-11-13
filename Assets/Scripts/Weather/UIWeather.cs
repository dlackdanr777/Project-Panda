using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using Muks.Tween;
using System.Linq;

public class UIWeather : MonoBehaviour
{
    [SerializeField] private WeatherApp _weatherApp;

    [SerializeField] private List<string> _weekWeathers;

    //레이아웃을 넣는 곳
    [SerializeField] private GameObject _layoutGroup;

    //날씨를 보여주는 일반적인 슬롯
    [SerializeField] private GameObject _uiSlot;

    //현재 날짜를 보여주는 슬롯
    [SerializeField] private GameObject _uiTodaySlot;

    public void Init()
    {
        List<string> _weekWeathers = new List<string>();
        _weekWeathers = _weatherApp._weekWeathers.ToList();

        for(int i = 0; i < 7; i++)
        {
            GameObject slot;
            if (_weatherApp.UserInfo.DayCount % 7 == i)
            {
                slot = Instantiate(_uiTodaySlot, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                slot = Instantiate(_uiSlot, new Vector3(0, 0, 0), Quaternion.identity);
            }

            slot.transform.parent = _layoutGroup.transform;
        }
    }
}
