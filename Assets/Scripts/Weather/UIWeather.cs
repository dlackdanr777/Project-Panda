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

    private List<UIWeatherSlot> _slots;

    private List<WeatherData> _weekWeathers;

    private WeatherData _todayWeatherData;

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
                _todayWeatherData = _weekWeathers[i];
            }
            else
            {
                slot = Instantiate(_uiSlot, new Vector3(0, 0, 0), Quaternion.identity);
            }

            slot.transform.parent = _layoutGroup.transform;
            _slots.Add(slot.GetComponent<UIWeatherSlot>());

            _slots[i].UpdateUI(_weekWeathers[i]);
        }
        SetBind();
    }

    private void SetBind()
    {
        DataBind.SetButtonValue("UI Weather Exit Button", () => gameObject.SetActive(false));
        DataBind.SetButtonValue("UI Weather Open Button", () => gameObject.SetActive(!gameObject.activeSelf));
        DataBind.SetSpriteValue("Today Weather Image", _todayWeatherData.Sprite);
    }
}
