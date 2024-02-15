using Muks.DataBind;
using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;


/// <summary>늘어나는 애니메이션 버전의 드롭다운 메뉴</summary>
public class DropdownMenu_Ver1 : MonoBehaviour
{
    [SerializeField] private Image _centerMenu;

    [SerializeField] private Image _underMenu;

    [SerializeField] private HorizontalOrVerticalLayoutGroup _layoutGroup;

    [SerializeField] private DropdownMenuButtonGroup_Ver1 _menuButtons;

    [Space]
    [SerializeField] private float _showStartDuration;

    [SerializeField] private TweenMode _showStartTweenMode;

    [SerializeField] private float _showEndDuration;

    [SerializeField] private TweenMode _showEndTweenMode;


    [Space]
    [SerializeField] private float _hideStartDuration;

    [SerializeField] private TweenMode _hideStartTweenMode;

    [SerializeField] private float _hideEndDuration;

    [SerializeField] private TweenMode _hideEndTweenMode;

    private Vector3 _underTmpPos;
    private Vector3 _centerTmpPos;
    private Vector2 _centerTmpSizeDelta;
    private float _layoutGroupTmpSpacing;
    private Vector2 _layoutGroupTmpPos;

    private Vector3 _underStartTargetPos => new Vector3(0, -162.5f, 0);
    private Vector3 _centerStartTargetPos => new Vector3(0, -49f, 0);
    private float _centerStartTargetHeight => 200;
    private float _layoutGroupStartTargetSpacing => -90;

    private Vector3 _underEndTargetPos => new Vector3(0, -664f, 0);
    private Vector3 _centerEndTargetPos => new Vector3(0, -300f, 0);
    private float _centerEndTargetHeight => 700;
    private float _layoutGroupEndTargetSpacing => -740;

    private void Start()
    {
        _menuButtons.Init();
        _underTmpPos = _underMenu.rectTransform.anchoredPosition;
        _centerTmpPos = _centerMenu.rectTransform.anchoredPosition;
        _centerTmpSizeDelta = _centerMenu.rectTransform.sizeDelta;
        _layoutGroupTmpSpacing = _layoutGroup.spacing;
        _layoutGroupTmpPos = _layoutGroup.GetComponent<RectTransform>().anchoredPosition;
        _menuButtons.HideButtons();

        DataBind.SetButtonValue("ShowDropdownMenu", () => ShowAnime());
        DataBind.SetButtonValue("HideDropdownMenu", () => _menuButtons.HideAnime(HideAnime));
    }


    private void ShowAnime()
    {
        _menuButtons.HideShowButton();
        _menuButtons.ShowButtons();

        Tween.Stop(_underMenu.gameObject);
        Tween.Stop(_centerMenu.gameObject);
        Tween.Stop(_layoutGroup.gameObject);

        _underMenu.rectTransform.anchoredPosition = _underTmpPos;
        _centerMenu.rectTransform.anchoredPosition = _centerTmpPos;
        _centerMenu.rectTransform.sizeDelta = _centerTmpSizeDelta;
        _layoutGroup.spacing = _layoutGroupTmpSpacing;
        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = _layoutGroupTmpPos;

        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underStartTargetPos, _showStartDuration, _showStartTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerStartTargetPos, _showStartDuration, _showStartTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, new Vector2(_centerTmpSizeDelta.x, _centerStartTargetHeight), _showStartDuration, _showStartTweenMode);
        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underEndTargetPos, _showEndDuration, _showEndTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerEndTargetPos, _showEndDuration, _showEndTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, new Vector2(_centerTmpSizeDelta.x, _centerEndTargetHeight), _showEndDuration, _showEndTweenMode);

        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerStartTargetPos, _showStartDuration, _showStartTweenMode);
        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerEndTargetPos, _showEndDuration, _showEndTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupStartTargetSpacing, _showStartDuration, _showStartTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupEndTargetSpacing, _showEndDuration, _showEndTweenMode, () =>
        {
            _menuButtons.ShowAnime();
        });
    }


    private void HideAnime()
    {
        Tween.Stop(_underMenu.gameObject);
        Tween.Stop(_centerMenu.gameObject);
        Tween.Stop(_layoutGroup.gameObject);

        _underMenu.rectTransform.anchoredPosition = _underEndTargetPos;
        _centerMenu.rectTransform.anchoredPosition = _centerEndTargetPos;
        _centerMenu.rectTransform.sizeDelta = new Vector2(_centerTmpSizeDelta.x, _centerEndTargetHeight);
        _layoutGroup.spacing = _layoutGroupEndTargetSpacing;
        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = _centerEndTargetPos;

        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerStartTargetPos, _hideStartDuration, _hideStartTweenMode);
        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerTmpPos, _hideEndDuration, _hideEndTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupStartTargetSpacing, _hideStartDuration, _hideStartTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupTmpSpacing, _hideEndDuration, _hideEndTweenMode);

        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underStartTargetPos, _hideStartDuration, _hideStartTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerStartTargetPos, _hideStartDuration, _hideStartTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, new Vector2(_centerTmpSizeDelta.x, _centerStartTargetHeight), _hideStartDuration, _hideStartTweenMode);
        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underTmpPos, _hideEndDuration, _hideEndTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerTmpPos, _hideEndDuration, _hideEndTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, _centerTmpSizeDelta, _hideEndDuration, _hideEndTweenMode, () =>
        {
            _menuButtons.HideButtons();
            _menuButtons.ShowShowButton();
        });
    }

}
