using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using TMPro;

public class UIWeatherSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private Image _rewardImage;

    [SerializeField] private Image _weatherImage;

    private WeatherRewardData _weatherData;

    public void UpdateUI(WeatherRewardData weatherData)
    {
        _weatherData = weatherData;
        if (_weatherData != null)
        {
            _amountText.text = _weatherData.Amount.ToString();
            _rewardImage.sprite = _weatherData.Item.Image;
            _weatherImage.sprite = _weatherData.Sprite;
            _rewardImage.preserveAspect = true;
        }

        else
        {
            _amountText.text = string.Empty;
            _rewardImage.sprite = null;
        }
    }

}
