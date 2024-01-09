using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICookingBar : MonoBehaviour
{
    private enum BarType
    {
        Decrease,
        Increase
    }

    [Tooltip("게이지가 감소하는가, 채워지는가 확인")]
    [SerializeField] private BarType _barType;

    [SerializeField] private Image _bar;
    [SerializeField] private Image _backgroundBar;

    [SerializeField] private float _animeTotalDuration;

    private float _duration;

    public void Update()
    {
        UpdataBar();
    }

    public float GetBarWedth()
    {
        return _bar.rectTransform.rect.width;
    }

    public float GetBarValue()
    {
        return _bar.fillAmount;
    }

    public void ResetBar(float fillAmount)
    {
        _bar.fillAmount = fillAmount;
        _backgroundBar.fillAmount = fillAmount;
    }


    public void UpdateGauge(float maxStamina, float stamina)
    {
        if (_barType == BarType.Decrease)
        {
            _backgroundBar.fillAmount = _bar.fillAmount;
            _bar.fillAmount = stamina / maxStamina;
        }
           
        else if (_barType == BarType.Increase)
        {
            _bar.fillAmount = _backgroundBar.fillAmount;
            _backgroundBar.fillAmount = stamina / maxStamina;
        }

        _duration = 0;
    }


    private void UpdataBar()
    {
        if(_barType == BarType.Decrease)
        {
            if (_bar.fillAmount != _backgroundBar.fillAmount)
            {
                _duration += Time.deltaTime;
                float percent = _duration / _animeTotalDuration;
                percent = percent * percent * (3f - 2f * percent);

                _backgroundBar.fillAmount = Mathf.Lerp(_backgroundBar.fillAmount, _bar.fillAmount, percent);
            }

            else
            {
                _duration = 0;
            }
        }

        else if(_barType == BarType.Increase)
        {
            if (_bar.fillAmount != _backgroundBar.fillAmount)
            {
                _duration += Time.deltaTime;
                float percent = _duration / _animeTotalDuration;
                percent = percent * percent * (3f - 2f * percent);

                _bar.fillAmount = Mathf.Lerp(_bar.fillAmount, _backgroundBar.fillAmount, percent);
            }

            else
            {
                _duration = 0;
            }
        }
      
    }
}
