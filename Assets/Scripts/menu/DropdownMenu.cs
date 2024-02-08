using Muks.DataBind;
using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour
{
    [SerializeField] private Image _centerMenu;

    [SerializeField] private Image _underMenu;

    [SerializeField] private HorizontalOrVerticalLayoutGroup _layoutGroup;

    [SerializeField] private DropdownMenuButton[] _buttons;

    [SerializeField] private Button _showButton;

    [Space]
    [SerializeField] private float _startDuration;

    [SerializeField] private TweenMode _startTweenMode;

    [Space]
    [SerializeField] private float _endDuration;

    [SerializeField] private TweenMode _endTweenMode;

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
        _underTmpPos = _underMenu.rectTransform.anchoredPosition;
        _centerTmpPos = _centerMenu.rectTransform.anchoredPosition;
        _centerTmpSizeDelta = _centerMenu.rectTransform.sizeDelta;
        _layoutGroupTmpSpacing = _layoutGroup.spacing;
        _layoutGroupTmpPos = _layoutGroup.GetComponent<RectTransform>().anchoredPosition;
        _showButton.gameObject.SetActive(true);
        _showButton.onClick.AddListener(ShowAnime);
        DataBind.SetButtonValue("HideDropdownMenu", () =>
        {
            _buttons[0].HideAnime(HideAnime);
            for(int i = 1; i <  _buttons.Length; i++)
            {
                _buttons[i].HideAnime();
            }
        });

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].Init();
        }
    }


    private void ShowAnime()
    {
        _showButton.gameObject.SetActive(false);
        Tween.Stop(_underMenu.gameObject);
        Tween.Stop(_centerMenu.gameObject);
        Tween.Stop(_layoutGroup.gameObject);

        _underMenu.rectTransform.anchoredPosition = _underTmpPos;
        _centerMenu.rectTransform.anchoredPosition = _centerTmpPos;
        _centerMenu.rectTransform.sizeDelta = _centerTmpSizeDelta;
        _layoutGroup.spacing = _layoutGroupTmpSpacing;
        _layoutGroup.GetComponent<RectTransform>().anchoredPosition = _layoutGroupTmpPos;

        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underStartTargetPos, _startDuration, _startTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerStartTargetPos, _startDuration, _startTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, new Vector2(_centerTmpSizeDelta.x, _centerStartTargetHeight), _startDuration, _startTweenMode);
        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underEndTargetPos, _endDuration, _endTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerEndTargetPos, _endDuration, _endTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, new Vector2(_centerTmpSizeDelta.x, _centerEndTargetHeight), _endDuration, _endTweenMode);

        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerStartTargetPos, _startDuration, _startTweenMode);
        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerEndTargetPos, _endDuration, _endTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupStartTargetSpacing, _startDuration, _startTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupEndTargetSpacing, _endDuration, _endTweenMode, () =>
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].ShowAnime();
            }

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

        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerStartTargetPos, _endDuration, _startTweenMode);
        Tween.RectTransfromAnchoredPosition(_layoutGroup.gameObject, _centerTmpPos, _startDuration, _endTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupStartTargetSpacing, _endDuration, _startTweenMode);
        Tween.LayoutGroupSpacing(_layoutGroup.gameObject, _layoutGroupTmpSpacing, _startDuration * 0.35f, TweenMode.EaseOutExpo);

        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underStartTargetPos, _endDuration, _startTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerStartTargetPos, _endDuration, _startTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, new Vector2(_centerTmpSizeDelta.x, _centerStartTargetHeight), _endDuration, _startTweenMode);
        Tween.RectTransfromAnchoredPosition(_underMenu.gameObject, _underTmpPos, _startDuration, _endTweenMode);
        Tween.RectTransfromAnchoredPosition(_centerMenu.gameObject, _centerTmpPos, _startDuration, _endTweenMode);
        Tween.RectTransfromSizeDelta(_centerMenu.gameObject, _centerTmpSizeDelta, _startDuration, _endTweenMode, () =>
        {
            _showButton.gameObject.SetActive(true);
        });
    }

}
