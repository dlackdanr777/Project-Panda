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
    [SerializeField] private Image _attendanceCheckIamge;

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
        SlotInit();
        _attendanceCheckIamge.gameObject.SetActive(false);
        _backgroundButton.SetActive(false);
        _tmpSize = gameObject.transform.localScale;

        if (!_watherDatabase.CheckTodayAttendance())
        {
            Debug.Log("아직 출첵 안함");
            _attendanceButton.onClick.AddListener(() =>
            {
                _attendanceButton.gameObject.SetActive(true);
                _watherDatabase.ChecktAttendance();
                _attendanceCheckIamge.gameObject.SetActive(true);
            });
        }
        else
        {
            _attendanceCheckIamge.gameObject.SetActive(true);
            _attendanceButton.gameObject.SetActive(false);
        }
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


}

