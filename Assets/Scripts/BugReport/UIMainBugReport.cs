using Muks.Tween;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIMainBugReport : UIView
{
    [Header("Components")]
    [SerializeField] private UIBugReport _uiBugReport;


    [Space]
    [Header("Animation Options")]
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private float _duration;


    private CanvasGroup _canvasGroup;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _canvasGroup = GetComponent<CanvasGroup>();

        _uiBugReport.Init(OnCancelButtonClicked);
        gameObject.SetActive(false);
    }


    public override void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = false;
        VisibleState = VisibleState.Appearing;
        _uiBugReport.Show();

        _target.transform.localScale = _startScale;
        Tween.TransformScale(_target, _targetScale, _duration, TweenMode.EaseOutBack, () =>
        {
            _canvasGroup.blocksRaycasts = true;
            VisibleState = VisibleState.Appeared;
        });
    }


    public override void Hide()
    {
        _canvasGroup.blocksRaycasts = false;
        VisibleState = VisibleState.Disappearing;
        _uiBugReport.Hide();

        _target.transform.localScale = _targetScale;
        Tween.TransformScale(_target, _startScale * 0.5f, _duration, TweenMode.EaseInBack, () =>
        {
            gameObject.SetActive(false);
            VisibleState = VisibleState.Disappeared;
        });
    }


    private void OnCancelButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("UIBugReport");
    }
}
