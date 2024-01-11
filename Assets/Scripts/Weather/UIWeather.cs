using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;


public class UIWeather : UIView
{
    [SerializeField] private WeatherApp _weatherApp;

    //레이아웃을 넣는 곳
    [SerializeField] private GameObject _slotLayoutGroup;

    [SerializeField] private GameObject _daySlotLayoutGroup;

    private List<UIWeatherSlot> _slots;

    private List<UIDaySlot> _daySlots;

    private List<WeatherData> _weekWeathers;

    private WeatherData _todayWeatherData;

    public event Action OnRewardedHandler;

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }


    public void InitSlot()
    {
        _slots = new List<UIWeatherSlot>();
        _slots = _slotLayoutGroup.GetComponentsInChildren<UIWeatherSlot>().ToList();
        _daySlots = _daySlotLayoutGroup.GetComponentsInChildren<UIDaySlot>().ToList();

        for (int i = 0, count = _slots.Count; i < count; i++)
        {
            _slots[i].UpdateUI(_weekWeathers[i], i + 1);
            _daySlots[i].SetDayText(i + 1);
        }

        //슬롯들을 세팅해준다.
        for (int i = 0, count = _slots.Count; i < count; i++)
        {
            //만약 보상 지급 슬롯이라면?
            if ((DatabaseManager.Instance.UserInfo.DayCount % 6) - 1 == i)
            {
                _todayWeatherData = _weekWeathers[i];
                _daySlots[i].Init(true);

                //보상이 지급되는 날이면?
                if (_weatherApp.IsCanReward)
                {

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
                _daySlots[i].Init(true);
            }
        }


    }


    public void Init()
    {
        _weekWeathers = _weatherApp.GetWeekWeathers().ToList();
        InitSlot();
        SetBind();
        gameObject.SetActive(false);
    }


    private void SetBind()
    {
        DataBind.SetSpriteValue("TodayWeatherImage", _todayWeatherData.WeatherSprite);
        string weekText = DatabaseManager.Instance.UserInfo.TODAY.DayOfWeek.ToString().Substring(0, 3);
        DataBind.SetTextValue("WeekText", weekText);
    }




    /* //출석체크를 눌렀을때 발생될 이벤트
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
 */
}

