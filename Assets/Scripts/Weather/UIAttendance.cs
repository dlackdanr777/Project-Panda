using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;
using Muks.Tween;

public class UIAttendance : UIView
{
    [Space]
    [SerializeField] private GameObject _backgroundButton;

    [Space]
    [SerializeField] private Transform _slotLayoutParent;

    [SerializeField] private UIAttendanceSlot _slotPrefab;

    [Space]
    [SerializeField] private UIAttendanceButton _attendanceButton;

    [SerializeField] private Image _attendanceCheckIamge;

    [SerializeField] private Sprite _todayBackroundImage;

    [Space]
    [SerializeField] private GameObject _dontTouchArea;


    private AttendanceDatabase _attendanceDatabase => DatabaseManager.Instance.AttendanceDatabase;

    private List<UIAttendanceSlot> _slots = new List<UIAttendanceSlot>();

    private UIAttendanceSlot _todayUISlot;

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
        _dontTouchArea.SetActive(false);

        _tmpSize = gameObject.transform.localScale;

        if (!_attendanceDatabase.CheckTodayAttendance())
        {
            Debug.Log("아직 출첵 안함");
            _attendanceButton.EnableButtonClick(OnAttendanceButtonClicked);
        }
        else
        {
            _attendanceCheckIamge.gameObject.SetActive(true);
            _attendanceButton.DisableButtonClick();
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
        List<AttendanceRewardData> list = _attendanceDatabase.GetAttendanceRewardDataForDayCount();

        for (int i = 0, count = list.Count; i < count; i++)
        {
            UIAttendanceSlot slot = Instantiate(_slotPrefab);
            slot.transform.parent = _slotLayoutParent;

            //현재 보상UI의 경우 맨 앞에 나와야 하기 때문에 i == 0일 경우 설정합니다.
            if (i == 0)
            {
                slot.UpdateUI(list[i], _todayBackroundImage);
                _todayUISlot = slot;
            }
            else
            {
                slot.UpdateUI(list[i]);
                _slots.Add(slot);
            }

        }
    }

    private void OnAttendanceButtonClicked()
    {
        _attendanceButton.gameObject.SetActive(true);
        _attendanceCheckIamge.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        _attendanceCheckIamge.transform.position = _todayUISlot.transform.position;

        _attendanceCheckIamge.transform.localScale = new Vector3(2f, 2f, 2f);
        Tween.TransformScale(_attendanceCheckIamge.gameObject, new Vector3(1, 1, 1), 0.35f, TweenMode.EaseInQuint, () => _dontTouchArea.SetActive(false));

        _attendanceDatabase.ChecktAttendance();
    }

}

