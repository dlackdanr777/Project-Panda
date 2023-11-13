using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeatherSlot : MonoBehaviour
{
    [SerializeField] private Text _nameText;

    [SerializeField] private Text _amountText;

    [SerializeField] private Image _image;

    private WeatherData _weatherData;
    

    public void UpdateUI(WeatherData weatherData)
    {
        _weatherData = weatherData;
        if (_weatherData != null)
        {
            _nameText.text = _weatherData.Name;
            _amountText.text = _weatherData.Amount.ToString();
            _image.sprite = _weatherData.Sprite;
        }

        else
        {
            _nameText.text = string.Empty;
            _amountText.text = "0";
            _image.sprite = null;
        }

    }
}
