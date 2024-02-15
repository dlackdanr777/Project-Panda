using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using TMPro;

public class UIAttendanceSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private Image _backgroundImage;

    [SerializeField] private Image _rewardImage;

    private AttendanceRewardData _attendanceData;

    public void UpdateUI(AttendanceRewardData attendanceData, Sprite backgroundImage = null)
    {
        _attendanceData = attendanceData;
        if (_attendanceData != null)
        {
            _amountText.text = _attendanceData.Amount.ToString();
            _rewardImage.sprite = _attendanceData.Item.Image;
            //_weatherImage.sprite = _weatherData.Sprite;
            _rewardImage.preserveAspect = true;
        }

        else
        {
            _amountText.text = string.Empty;
            _rewardImage.sprite = null;
        }

        if(backgroundImage != null)
        {
            _backgroundImage.sprite = backgroundImage;
        }

    }

}
