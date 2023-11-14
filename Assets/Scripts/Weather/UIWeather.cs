using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;

public class UIWeather : MonoBehaviour
{
    [SerializeField] private WeatherApp _weatherApp;

    //레이아웃을 넣는 곳
    [SerializeField] private GameObject _layoutGroup;

    //날씨를 보여주는 일반적인 슬롯
    [SerializeField] private GameObject _uiSlot;

    //현재 날짜를 보여주는 슬롯
    [SerializeField] private GameObject _uiTodaySlot;

    [SerializeField] private Sprite _attendanceSprite;

    private List<UIWeatherSlot> _slots;

    private List<WeatherData> _weekWeathers;

    private WeatherData _todayWeatherData;

    private bool _isRewardComplated;
    public void Init()
    {
        _slots = new List<UIWeatherSlot>();
        _weekWeathers = _weatherApp._weekWeathers.ToList();

        for(int i = 0; i < 7; i++)
        {
            GameObject slot;
            if (_weatherApp.UserInfo.DayCount % 7 == i)
            {
                slot = Instantiate(_uiTodaySlot, new Vector3(0, 0, 0), Quaternion.identity);
                _slots.Add(slot.GetComponent<UIWeatherSlot>());
                _todayWeatherData = _weekWeathers[i];
                _slots[i].AttendanceComplatedAnime(_attendanceSprite);
                _isRewardComplated = true;
            }
            else
            {
                slot = Instantiate(_uiSlot, new Vector3(0, 0, 0), Quaternion.identity);
                _slots.Add(slot.GetComponent<UIWeatherSlot>());
                if (!_isRewardComplated)
                {
                    _slots[i].AttendanceComplated(_attendanceSprite);
                }
            }

            slot.transform.parent = _layoutGroup.transform;

            _slots[i].UpdateUI(_weekWeathers[i], i+1);
        }
        SetBind();
    }

    private void SetBind()
    {
        DataBind.SetButtonValue("UI Weather Exit Button", () => gameObject.SetActive(false));
        DataBind.SetButtonValue("UI Weather Open Button", () => gameObject.SetActive(!gameObject.activeSelf));
        DataBind.SetSpriteValue("Today Weather Image", _todayWeatherData.WeatherSprite);
    }
}
