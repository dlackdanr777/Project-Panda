using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
///<summary>메인 씬 편지UI 클래스</summary>
public class UIMail : UIView
{
    [Header("ShowUI Animation Setting")]
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _duration;
    [SerializeField] private TweenMode _tweenMode;

    [Space]
    [Header("Components")]
    [SerializeField] private Button _backgroundButton;

    private CanvasGroup _canvasGroup;
    private Vector3 _tmpPos;
    private Vector3 _movePos => new Vector3(0, 50, 0);



    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpPos = _targetRect.anchoredPosition;
        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);

        gameObject.SetActive(false);
    }


    public override void Show()
    {
        VisibleState = VisibleState.Appearing;
        gameObject.SetActive(true);

        _targetRect.anchoredPosition = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos, _duration, _tweenMode);
        Tween.CanvasGroupAlpha(gameObject, _targetAlpha, _duration, _tweenMode, () =>
        {
            VisibleState = VisibleState.Appeared;
            _canvasGroup.blocksRaycasts = true;
        });
    }


    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;

        _targetRect.anchoredPosition = _tmpPos;
        _canvasGroup.alpha = _targetAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos - _movePos, _duration, _tweenMode);
        Tween.CanvasGroupAlpha(gameObject, _startAlpha, _duration, _tweenMode, () =>
        {
            VisibleState = VisibleState.Disappeared;
            _canvasGroup.blocksRaycasts = true;

            gameObject.SetActive(false);
        });
    }


    private void OnBackgroundButtonClicked()
    {
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Mail");
    }
}
