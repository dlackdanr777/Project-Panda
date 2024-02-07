using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;
using Muks.Tween;

public class UIWeather : UIView
{
    [Space]
    [SerializeField] private GameObject _backgroundButton;

    [Space]
    [SerializeField] private Transform _slotLayoutParent;

    [SerializeField] private GameObject _daySlotLayoutGroup;

    [Space]
    [SerializeField] private UIWeatherSlot _todayUISlot;
    [SerializeField] private Button _attendanceButton;

    [SerializeField] private UIWeatherSlot _slotPrefab;


    private WeatherDatabase _watherDatabase => DatabaseManager.Instance.WeatherDatabase;

    private List<UIWeatherSlot> _slots = new List<UIWeatherSlot>();

    private Vector3 _tmpSize;

    private Vector3 _targetSize => new Vector3(0.7f, 0.7f, 0.7f);



    public event Action OnRewardedHandler;

    public override void Show()
    {
        ShowAnime();
    }

    public override void Hide()
    {
        HideAnime();
    }


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        //SlotInit();

        _tmpSize = gameObject.transform.localScale;

        if (!_watherDatabase.CheckTodayAttendance())
        {
            Debug.Log("아직 출첵 안함");
            _attendanceButton.onClick.AddListener(() =>
            {
                _watherDatabase.ChecktAttendance();
                _attendanceButton.gameObject.SetActive(false);
            });
        }
        else
        {
            _attendanceButton.gameObject.SetActive(false);
        }

        _backgroundButton.SetActive(false);
    }

    
    private void ShowAnime()
    {
        VisibleState = VisibleState.Appearing;

        gameObject.SetActive(true);
        _backgroundButton.SetActive(true);
        gameObject.transform.localScale = _targetSize;

        Tween.TransformScale(gameObject, _tmpSize, 0.3f, TweenMode.EaseOutBack, () =>
        {
                VisibleState = VisibleState.Appeared;
        });

    }


    private void HideAnime()
    {
        VisibleState = VisibleState.Disappearing;
        _backgroundButton.SetActive(false);
        Tween.TransformScale(gameObject, _targetSize * 0.5f, 0.4f, TweenMode.EaseInBack, () =>
        {
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });
    }


    private void SlotInit()
    {
        List<WeatherRewardData> list = _watherDatabase.GetWeatherRewardDataForDayCount();

        for (int i = 0, count = list.Count; i < count; i++)
        {
            UIWeatherSlot slot;

            //현재 보상UI의 경우 크게 출력해야 하므로 미리 설정해둔 UISlot을 활용해야합니다.
            //그렇기에 0번째 배열의 오늘 보상의 경우 if처리로 따로 설정을 진행합니다.
            if (i != 0)
            {
                slot = Instantiate(_slotPrefab);
                slot.transform.parent = _slotLayoutParent;
            }
            else
            {
                slot = _todayUISlot;
            }

            slot.UpdateUI(list[i]);
            _slots.Add(slot);
        }
    }

    public void CheckAttendance()
    {
        _watherDatabase.ChecktAttendance();
        //Todo:애니메이션
    }

   /*public void InitSlot()
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
            if ((DatabaseManager.Instance.UserInfo.DayCount -1 % 6) == i)
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
    }*/




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

