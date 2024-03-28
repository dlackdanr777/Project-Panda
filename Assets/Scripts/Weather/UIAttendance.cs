using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;
using Muks.Tween;

[RequireComponent(typeof(CanvasGroup))]
public class UIAttendance : UIView
{
    [Header("Components")]
    [SerializeField] private Transform _slotLayoutParent;
    [SerializeField] private UIAttendanceSlot _slotPrefab;
    [SerializeField] private UIAttendanceButton _attendanceButton;
    [SerializeField] private Image _attendanceCheckIamge;
    [SerializeField] private Sprite _todayBackroundImage;
    [SerializeField] private Button _backgroundButton;

    [Space]
    [Header("ShowUI Animation Setting")]
    [SerializeField] private GameObject _target;
    [SerializeField] private float _showDuration;
    [SerializeField] private float _hideDuration;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _attendanceSound;

    


    private AttendanceDatabase _attendanceDatabase => DatabaseManager.Instance.AttendanceDatabase;

    private List<UIAttendanceSlot> _slots = new List<UIAttendanceSlot>();

    private UIAttendanceSlot _todayUISlot;

    private Vector3 _tmpSize;

    private Vector3 _targetSize => new Vector3(0.7f, 0.7f, 0.7f);

    private CanvasGroup _canvasGroup;



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
        SlotInit();
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();

        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);
        _attendanceCheckIamge.gameObject.SetActive(false);

        _tmpSize = gameObject.transform.localScale;

        if (!_attendanceDatabase.IsAttendanced)
        {
            _attendanceButton.EnableButtonClick(OnAttendanceButtonClicked);
        }
        else
        {
            _attendanceCheckIamge.gameObject.SetActive(true);
            _attendanceButton.DisableButtonClick();
        }

        gameObject.SetActive(false);
    }

    
    private void ShowAnime()
    {
        VisibleState = VisibleState.Appearing;
        gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = false;

        _target.transform.localScale = _targetSize;
        Tween.TransformScale(_target, _tmpSize, _showDuration, TweenMode.EaseOutBack, () =>
        {
            _canvasGroup.blocksRaycasts = true;
            VisibleState = VisibleState.Appeared;
        });

    }


    private void HideAnime()
    {
        VisibleState = VisibleState.Disappearing;

        _canvasGroup.blocksRaycasts = false;
        _target.transform.localScale = _tmpSize;
        Tween.TransformScale(_target, _targetSize * 0.5f, _hideDuration, TweenMode.EaseInBack, () =>
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
            slot.transform.localScale = new Vector3(1, 1, 1);
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
        _canvasGroup.blocksRaycasts = false;

        _attendanceCheckIamge.transform.position = _todayUISlot.transform.position;

        _attendanceCheckIamge.transform.localScale = new Vector3(2f, 2f, 2f);
        Tween.TransformScale(_attendanceCheckIamge.gameObject, new Vector3(1, 1, 1), 0.35f, TweenMode.EaseInQuint, () => _canvasGroup.blocksRaycasts = true);

        _attendanceDatabase.ChecktAttendance();
        SoundManager.Instance.PlayEffectAudio(_attendanceSound, 0.22f);
    }


    private void OnBackgroundButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Attendance");
    }

}

