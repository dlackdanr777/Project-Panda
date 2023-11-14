using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;

public class UIWeather : MonoBehaviour
{
    [SerializeField] private WeatherApp _weatherApp;

    //���̾ƿ��� �ִ� ��
    [SerializeField] private GameObject _layoutGroup;

    //������ �����ִ� �Ϲ����� ����
    [SerializeField] private GameObject _uiSlot;

    //���� ��¥�� �����ִ� ����
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
