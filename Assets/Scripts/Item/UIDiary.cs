using Muks.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDiary : UIView
{
    [SerializeField] private RectTransform _book;

    [Tooltip("StartCover�� animator�� �ҷ��;��Ѵ�.")]
    [SerializeField] private Animator _coverAnimator;

    [Tooltip("��ġ ���� ���� (�ִϸ��̼� ���� �� ��ư ��ġ ������ ���� ���)")]
    [SerializeField] private GameObject _dontTouchArea;

    [Space]
    [SerializeField] private float _animeDuration;

    [Tooltip("�ִϸ��̼� � �׷���")]
    [SerializeField] private TweenMode _tweenMode;



    private Vector3 _showPos;
    private Vector3 _hidePos;

    //å ��� ���� �ִϸ��̼��� �ҿ� �ð�( ����� 1.9�� ) 
    private float _bookAnimeDuration => 1.9f;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        _showPos = _book.anchoredPosition;
        _hidePos = _book.anchoredPosition + new Vector2(0, -1800); 
    }

    public override void Hide()
    {

        VisibleState = VisibleState.Disappearing;
        _dontTouchArea.SetActive(true);

        _book.anchoredPosition = _showPos;
        _coverAnimator.SetTrigger("close");

        Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _bookAnimeDuration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_book.gameObject, _hidePos, _animeDuration, _tweenMode, () =>
        {
            _uiNav.ShowMainUI();
            _book.anchoredPosition = _hidePos;
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);
        VisibleState = VisibleState.Appearing;

        _uiNav.HideMainUI();
        _book.anchoredPosition = _hidePos;

        Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _animeDuration, _tweenMode, () => 
        {
            _coverAnimator.SetTrigger("open");
            _book.anchoredPosition = _showPos;
            Tween.RectTransfromAnchoredPosition(_book.gameObject, _showPos, _bookAnimeDuration, TweenMode.Constant, () =>
            {

                _dontTouchArea.SetActive(false);
                VisibleState = VisibleState.Appeared;
            });
        });
    }
}
