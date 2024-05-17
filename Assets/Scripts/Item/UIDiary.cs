using Muks.DataBind;
using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIDiary : UIView
{
    [Header("ShowUI Animation Setting")]
    [SerializeField] private float _animeDuration;
    [Tooltip("애니메이션 곡선 그래프")]
    [SerializeField] private TweenMode _tweenMode;


    [Space]
    [Header("Components")]
    [SerializeField] private RectTransform _book;
    [Tooltip("StartCover의 animator을 불러와야한다.")]
    [SerializeField] private Animator _coverAnimator;
    [SerializeField] private Button _backgroundButton;
    [SerializeField] private UIDetailView _detailView;
    [SerializeField] private UIBookList[] _bookLists;


    [Space]
    [Header("Audio Clip")]
    [SerializeField] private AudioClip _bookOpenAudio;


    private CanvasGroup _canvasGroup;
    private Vector3 _showPos;
    private Vector3 _hidePos;

    //책 펴기 접기 애니메이션의 소요 시간( 현재는 1.1초 ) 
    private float _bookAnimeDuration => 1.1f;



    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();
        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);

        _showPos = _book.anchoredPosition;
        _hidePos = _book.anchoredPosition + new Vector2(0, -1800);

        GameManager.Instance.Player.OnAddItemHandler += AlarmCheck;
        MainStoryController.OnStartStoryHandler += AlarmCheck;
        LoadingSceneManager.OnLoadSceneHandler += ChangeSceneEvent;


        for(int i = 0, count = _bookLists.Length; i < count; i++)
        {
            _bookLists[i].Init(AlarmCheck);
        }

        gameObject.SetActive(false);

        AlarmCheck();
    }

    public override void Hide()
    {
        SoundManager.Instance.PlayEffectAudio(_bookOpenAudio, 0.03f);

        _detailView.gameObject.SetActive(false);
        VisibleState = VisibleState.Disappearing;
        _canvasGroup.blocksRaycasts = false;

        _book.anchoredPosition = _showPos;
        _coverAnimator.SetTrigger("close");

        Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _bookAnimeDuration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_book.gameObject, _hidePos, _animeDuration, _tweenMode, () =>
        {
            //_uiNav.ShowMainUI();
            _book.anchoredPosition = _hidePos;
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = false;
        VisibleState = VisibleState.Appearing;

        //_uiNav.HideMainUI();
        _book.anchoredPosition = _hidePos;

        Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _animeDuration, _tweenMode, () => 
        {
            SoundManager.Instance.PlayEffectAudio(_bookOpenAudio, 0.05f);
            _coverAnimator.SetTrigger("open");
            _book.anchoredPosition = _showPos;

            //책 오픈 애니메이션 대기 시간
            Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _bookAnimeDuration, TweenMode.Constant, () =>
            {
                _canvasGroup.blocksRaycasts = true;
                VisibleState = VisibleState.Appeared;
            });
        });
    }

    private void OnBackgroundButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Diary");
    }

    private void AlarmCheck()
    {
        bool checkAlarm = false;
        for(int i = 0, count = _bookLists.Length; i < count; i++)
        {
            checkAlarm = _bookLists[i].AlarmCheck() || checkAlarm;
        }

        if (checkAlarm)
        {
            DataBind.SetBoolValue("DiaryAlarm", true);
        }

        else
        {
            DataBind.SetBoolValue("DiaryAlarm", false);
        }

    }

    private void ChangeSceneEvent()
    {
        GameManager.Instance.Player.OnAddItemHandler -= AlarmCheck;
        MainStoryController.OnStartStoryHandler -= AlarmCheck;
        LoadingSceneManager.OnLoadSceneHandler -= ChangeSceneEvent;
    }
}
