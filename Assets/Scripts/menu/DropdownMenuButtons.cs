using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownMenuButtons : MonoBehaviour
{
    [SerializeField] private DropdownMenuButton[] _buttons;

    [Space]
    [SerializeField] private float _buttonAnimeDuration;

    [SerializeField] private TweenMode _buttonAnimeTweenMode;

    public void Init()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].Init();
        }

    }

    public void ShowAnime(Action onCompleted = null)
    {
        _buttons[0].ShowAnime(_buttonAnimeDuration, _buttonAnimeTweenMode, onCompleted);
        for (int i = 1, count = _buttons.Length; i < count; i++)
        {
            _buttons[i].ShowAnime(_buttonAnimeDuration, _buttonAnimeTweenMode);
        }
    }

    public void HideAnime(Action onCompleted = null)
    {
        _buttons[0].HideAnime(_buttonAnimeDuration, _buttonAnimeTweenMode, onCompleted);
        for (int i = 1, count = _buttons.Length; i < count; i++)
        {
            _buttons[i].HideAnime(_buttonAnimeDuration, _buttonAnimeTweenMode);
        }
    }
}
