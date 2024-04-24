using Muks.Tween;
using UnityEngine;
using System;


/// <summary>늘어나는 애니메이션 버전의 드롭다운 메뉴 나가기 버튼</summary>
public class DropdownExitButton_Ver1 : DropdownButton
{
    [SerializeField] private Vector3 _targetButtonPos;

    private Vector3 _tmpButtonPos;

    public override void Init()
    {
        _button.gameObject.SetActive(false);
        _dontTouchArea.SetActive(true);

        _tmpButtonPos = _button.rectTransform.anchoredPosition;
    }


    public override void ShowAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        _button.gameObject.SetActive(true);
        _dontTouchArea.SetActive(true);

        Tween.Stop(_button.gameObject);

        Tween.RectTransfromAnchoredPosition(_button.gameObject, _targetButtonPos, duration, tweenMode, () =>
        {
            _dontTouchArea.SetActive(false);
            onCompleted?.Invoke();
        });
    }


    public override void HideAnime(float duration, TweenMode tweenMode, Action onCompleted = null)
    {
        Tween.Stop(_button.gameObject);
        _dontTouchArea.SetActive(true);
        Tween.RectTransfromAnchoredPosition(_button.gameObject, _tmpButtonPos, duration, tweenMode, () =>
        {
            _button.gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
    }
}

