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

    //���̾ƿ��� �ִ� ��
    [SerializeField] private GameObject _layoutGroup;

    //������ �����ִ� �Ϲ����� ����
    [SerializeField] private GameObject _uiSlot;

    //���� ��¥�� �����ִ� ����
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
