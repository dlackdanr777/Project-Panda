using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;
using Muks.Tween;
using UnityEngine.U2D;
using Unity.VisualScripting;
using System.Reflection;

public class UIWeather : MonoBehaviour
{
    [SerializeField] private WeatherApp _weatherApp;

    //레이아웃을 넣는 곳
    [SerializeField] private GameObject _layoutGroup;

    //날씨를 보여주는 일반적인 슬롯
    [SerializeField] private GameObject _uiSlot;

    //현재 날짜를 보여주는 슬롯
    [SerializeField] private GameObject _uiTodaySlot;

    [SerializeField] private Image _attendanceStamp;

    [SerializeField] private Sprite _attendanceSprite;

    [SerializeField]
    private List<UIWeatherSlot> _slots;

    private List<WeatherData> _weekWeathers;

    private WeatherData _todayWeatherData;

    public event Action OnRewardedHandler;

    public void InitSlot()
    {
        _slots = new List<UIWeatherSlot>();
        _slots = _layoutGroup.GetComponentsInChildren<UIWeatherSlot>().ToList();

        for(int i = 0, count = _slots.Count; i < count; i++)
        {
            _slots[i].UpdateUI(_weekWeathers[i], i + 1);
        }

        //슬롯들을 세팅해준다.
        for (int i = 0, count = _slots.Count; i < count; i++)
        {

            //만약 보상 지급 슬롯이라면?
            if ((UserInfo.DayCount % 7) -1 == i)
            {
                //보상이 지급되는 날이 아니면?
                if (!_weatherApp.IsCanReward)
                {
                    _slots[i].AttendanceComplated(_attendanceSprite);
                }
                else
                {
                    //보상획득 이벤트
                    RewardAnime(_slots[i]);

                    _todayWeatherData = _weekWeathers[i];
                    //이벤트핸들러 실행
                    OnRewardedHandler?.Invoke();
                }

                //반복문을 빠져나온다.
                break;
            }

            //아니라면
            else
            {
                //출석도장을 찍어준다.
                _slots[i].AttendanceComplated(_attendanceSprite);
            }
        }

    }

    public void Init()
    {
        _weekWeathers = _weatherApp.GetWeekWeathers().ToList();
        InitSlot();

        SetBind();
    }

   /* public void Init()
    {
        _slots = new List<UIWeatherSlot>();
        _weekWeathers = _weatherApp._weekWeathers.ToList();
        bool isRewardComplated = false;

        for (int i = 0; i < 7; i++)
        {
            GameObject slot;
            if (_weatherApp.UserInfo.DayCount % 7 == i)
            {
                int index = i;
                slot = Instantiate(_uiTodaySlot, new Vector3(0, 0, 0), Quaternion.identity);
                _slots.Add(slot.GetComponent<UIWeatherSlot>());
                _slots[index].Button.onClick.AddListener(() => OnWeatherSlotClicked(_slots[index]));
                _todayWeatherData = _weekWeathers[index];
                //_slots[i].AttendanceComplatedAnime(_attendanceSprite);
                isRewardComplated = true;
            }
            else
            {
                slot = Instantiate(_uiSlot, new Vector3(0, 0, 0), Quaternion.identity);
                _slots.Add(slot.GetComponent<UIWeatherSlot>());
                if (!isRewardComplated)
                {
                    _slots[i].AttendanceComplated(_attendanceSprite);
                }
            }

            slot.transform.SetParent(_layoutGroup.transform);
            _slots[i].UpdateUI(_weekWeathers[i], i+1);
        }
        SetBind();
    }*/

    private void SetBind()
    {
        DataBind.SetButtonValue("UI Weather Exit Button", () => gameObject.SetActive(false));
        DataBind.SetButtonValue("UI Weather Open Button", () => gameObject.SetActive(!gameObject.activeSelf));
        DataBind.SetSpriteValue("Today Weather Image", _todayWeatherData.WeatherSprite);
    }


    //출석체크를 눌렀을때 발생될 이벤트
    private void RewardAnime(UIWeatherSlot uiWeatherSlot)
    {
        _attendanceStamp.gameObject.SetActive(true);

        RectTransform rectTransform = !_attendanceStamp.GetComponent<RectTransform>()
            ? _attendanceStamp.AddComponent<RectTransform>()
            : _attendanceStamp.GetComponent<RectTransform>();

        _attendanceStamp.transform.position = uiWeatherSlot.AttendanceStamp.transform.position;
        _attendanceStamp.transform.rotation = uiWeatherSlot.AttendanceStamp.transform.rotation;
        _attendanceStamp.sprite = _attendanceSprite;

        Vector2 tmepSizeDelta = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(200, 200);


        Tween.RectTransfromSizeDelta(_attendanceStamp.gameObject, rectTransform.sizeDelta, 0.1f);
        Tween.RectTransfromSizeDelta(_attendanceStamp.gameObject, tmepSizeDelta, 0.5f, TweenMode.Quadratic, () =>
        {
            uiWeatherSlot.AttendanceStamp.gameObject.SetActive(true);
            uiWeatherSlot.AttendanceStamp.sprite = _attendanceSprite;
            _attendanceStamp.gameObject.SetActive(false);
            
            //이쪽에 보상을 획득할 수 있도록 해야함
        });

    }
}
