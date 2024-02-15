using System;
using UnityEngine.UI;
using UnityEngine;
using Muks.Tween;
using System.Security.Cryptography;
using System.Linq;
using System.Reflection;

/// <summary>메뉴들이 펼쳐지는 애니메이션 버전의 드롭다운 메뉴 버튼 그룹</summary>
public class DropdownMenuButtonGroup_Ver2 : MonoBehaviour
{
    [SerializeField] private DropdownButton[] _buttons;

    [SerializeField] private GameObject _showButton;

    [SerializeField] private DropdownExitButton_Ver2 _exitButton;

    [Space]
    [SerializeField] private float _groupAnimeDuration;

    [SerializeField] private TweenMode _groupAnimeTweenMode;


    [Space]
    [SerializeField] private float _buttonAnimeDuration;

    [SerializeField] private TweenMode _buttonAnimeTweenMode;

    [Space]
    [SerializeField] private float _exitButtonAnimeDuration;

    [SerializeField] private TweenMode _exitButtonAnimeTweenMode;

    [SerializeField] private float _exitButtonPaddingY;

    private Vector2[] _tmpButtonsPos;
    private Vector2[] _targetButtonsPos;

    private Vector2 _tmpExitButtonPos;
    private Vector2 _startExitButtonPos; //애니메이션이 시작될 시작지점
    private Vector2 _targetExitButtonPos;


    public void Init()
    {
        int buttonsLength = _buttons.Length;
        _tmpButtonsPos = new Vector2[buttonsLength];
        _targetButtonsPos = new Vector2[buttonsLength];

        Vector2 targetHeight = Vector2.zero;

        for(int i = 0; i < buttonsLength; i++)
        {
            _tmpButtonsPos[i] = _buttons[i].AnchoredPosition;
            _targetButtonsPos[i] = _buttons[i].AnchoredPosition + targetHeight;
            targetHeight.y -= _buttons[i].SizeDelta.y;

            _buttons[i].Init();
        }

        _tmpExitButtonPos = _exitButton.AnchoredPosition;
        _startExitButtonPos = _targetButtonsPos.Last();

        _targetExitButtonPos = _startExitButtonPos - new Vector2(0, _buttons[0].SizeDelta.y + _exitButtonPaddingY);

        _exitButton.Init();
    }

    public void ShowAnime()
    {
        _showButton.SetActive(false);

        for (int i = 0, count = _buttons.Length; i < count; i++)
        {
            int index = i;
            Tween.RectTransfromAnchoredPosition(_buttons[index].gameObject, _targetButtonsPos[index],
                _groupAnimeDuration, _groupAnimeTweenMode, () =>
                {
                    _buttons[index].ShowAnime(_buttonAnimeDuration, _buttonAnimeTweenMode);
                });
        }

        Tween.RectTransfromAnchoredPosition(_exitButton.gameObject, _startExitButtonPos, _groupAnimeDuration, _groupAnimeTweenMode, () =>
        {
            Tween.RectTransfromAnchoredPosition(_exitButton.gameObject, _targetExitButtonPos, _exitButtonAnimeDuration, _exitButtonAnimeTweenMode);
            _exitButton.ShowAnime(_buttonAnimeDuration, _buttonAnimeTweenMode);
        });


    }


    public void HideAnime()
    {
        _exitButton.DontTouchArea.SetActive(true);

        Tween.RectTransfromAnchoredPosition(_exitButton.gameObject, _startExitButtonPos, _exitButtonAnimeDuration, _exitButtonAnimeTweenMode);

        for (int i = 0, count = _buttons.Length; i < count; i++)
        {
            int index = i;
            _buttons[index].HideAnime(_buttonAnimeDuration, _buttonAnimeTweenMode, () =>
            {
                Tween.RectTransfromAnchoredPosition(_buttons[index].gameObject, _tmpButtonsPos[index], _groupAnimeDuration, _groupAnimeTweenMode);
            });
        }

        _exitButton.HideAnime(_buttonAnimeDuration, _buttonAnimeTweenMode, () =>
        {
            Tween.RectTransfromAnchoredPosition(_exitButton.gameObject, _tmpExitButtonPos, _groupAnimeDuration, _groupAnimeTweenMode, () =>
            {
                _showButton.SetActive(true);
            });
        });
    }
}
