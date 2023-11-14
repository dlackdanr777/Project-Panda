using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;

public class UIWeatherSlot : MonoBehaviour
{
    [SerializeField] private Text _dayText;

    [SerializeField] private Text _amountText;

    [SerializeField] private Image _weatherImage;

    [SerializeField] private Image _rewardImage;

    [SerializeField] private Image _attendanceStamp;

    [SerializeField] private Button _button;

    private WeatherData _weatherData;

    private bool _isComplate;

    public void UpdateUI(WeatherData weatherData, int day)
    {
        _weatherData = weatherData;
        if (_weatherData != null)
        {
            _dayText.text = day.ToString() + "일 차";
            _amountText.text = _weatherData.Amount.ToString();
            _weatherImage.sprite = _weatherData.WeatherSprite;
            _rewardImage.sprite = _weatherData.RewardSprite;
        }

        else
        {
            _dayText.text = string.Empty;
            _amountText.text = string.Empty;
            _weatherImage.sprite = null;
            _rewardImage.sprite = null;
        }

    }

    //출석완료 함수
    public void AttendanceComplatedAnime(Sprite sprite)
    {
        if(!_isComplate)
        {
            if (_attendanceStamp.TryGetComponent(out RectTransform rectTransform))
            {
                _attendanceStamp.gameObject.SetActive(true);
                _attendanceStamp.sprite = sprite;
                Vector2 tmepSizeDelta = rectTransform.sizeDelta;
                rectTransform.sizeDelta = new Vector2(200, 200);
                Tween.RectTransfromSizeDelta(_attendanceStamp.gameObject, rectTransform.sizeDelta, 0.1f);
                Tween.RectTransfromSizeDelta(_attendanceStamp.gameObject, tmepSizeDelta, 0.3f, TweenMode.Quadratic);

                _isComplate = true;
            }
        }
    }

    //이미 출석되어있는 것을 출력하는 함수
    public void AttendanceComplated(Sprite sprite)
    {
        _attendanceStamp.gameObject.SetActive(true);
        _attendanceStamp.sprite = sprite;
        _isComplate = true;
    }
}
