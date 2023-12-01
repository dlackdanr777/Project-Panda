using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;

public class UIWeatherSlot : MonoBehaviour
{
    [SerializeField] private Text _amountText;

    [SerializeField] private Image _rewardImage;

    [SerializeField] private Image _attendanceStamp;
    public Image AttendanceStamp => _attendanceStamp;

    private WeatherData _weatherData;

    public void UpdateUI(WeatherData weatherData, int day)
    {
        _weatherData = weatherData;
        if (_weatherData != null)
        {
            _amountText.text = _weatherData.Amount.ToString();
            _rewardImage.sprite = _weatherData.RewardSprite;
        }

        else
        {
            _amountText.text = string.Empty;
            _rewardImage.sprite = null;
        }

    }

/*    //이미 출석되어있는 것을 출력하는 함수
    public void AttendanceComplated(Sprite sprite)
    {
        _attendanceStamp.gameObject.SetActive(true);
        _attendanceStamp.sprite = sprite;
    }*/
}
