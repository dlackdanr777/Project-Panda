using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIDiary : UIView
{
    [Header("ShowUI Animation Setting")]
    [SerializeField] private float _animeDuration;
    [Tooltip("�ִϸ��̼� � �׷���")]
    [SerializeField] private TweenMode _tweenMode;


    [Space]
    [Header("Components")]
    [SerializeField] private RectTransform _book;
    [Tooltip("StartCover�� animator�� �ҷ��;��Ѵ�.")]
    [SerializeField] private Animator _coverAnimator;
    [SerializeField] private Button _backgroundButton;


    private CanvasGroup _canvasGroup;
    private Vector3 _showPos;
    private Vector3 _hidePos;

    //å ��� ���� �ִϸ��̼��� �ҿ� �ð�( ����� 1.1�� ) 
    private float _bookAnimeDuration => 1.1f;



    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();
        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);

        _showPos = _book.anchoredPosition;
        _hidePos = _book.anchoredPosition + new Vector2(0, -1800);

        gameObject.SetActive(false);
    }

    public override void Hide()
    {

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
            _coverAnimator.SetTrigger("open");
            _book.anchoredPosition = _showPos;

            //å ���� �ִϸ��̼� ��� �ð�
            Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _bookAnimeDuration, TweenMode.Constant, () =>
            {
                _canvasGroup.blocksRaycasts = true;
                VisibleState = VisibleState.Appeared;
            });
        });
    }

    private void OnBackgroundButtonClicked()
    {
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Diary");
    }
}
