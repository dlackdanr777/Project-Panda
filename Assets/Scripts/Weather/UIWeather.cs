using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;

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

    public void Init()
    {
        DataBind.SetButtonValue("UI Weather Exit Button", () => gameObject.SetActive(false));

        List<WeatherData> _weekWeathers = new List<WeatherData>();
        _slots = new List<UIWeatherSlot>();
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
            _slots.Add(slot.GetComponent<UIWeatherSlot>());

            _slots[i].UpdateUI(_weekWeathers[i]);
        }
    }
}
