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


    public void Init()
    {
        List<string> _weekWeathers = new List<string>();
        _weekWeathers = _weatherApp._weekWeathers.ToList();


    }



}
