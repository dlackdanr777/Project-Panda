using System;
using UnityEngine.UI;
using UnityEngine;


/// <summary>늘어나는 애니메이션 버전의 드롭다운 메뉴 버튼 그룹</summary>
public class DropdownMenuButtonGroup_Ver1 : MonoBehaviour
{
    [SerializeField] private DropdownButton[] _buttons;

    [SerializeField] private GameObject _showButton;

    [SerializeField] private DropdownButton _exitButton;

    [Space]
    [SerializeField] private float _buttonAnimeDuration;

    [SerializeField] private TweenMode _buttonAnimeTweenMode;

    [Space]
    [SerializeField] private float _exitButtonAnimeDuration;

    [SerializeField] private TweenMode _exitButtonAnimeTweenMode;

    public void Init()
    {
        _exitButton.Init();
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].Init();
        }

    }

    public void ShowAnime(Action onCompleted = null)
    {
        for (int i = 0, count = _buttons.Length; i < count; i++)
        {
            _buttons[i].ShowAnime(_buttonAnimeDuration, _buttonAnimeTweenMode);
        }

        _exitButton.ShowAnime(_exitButtonAnimeDuration, _exitButtonAnimeTweenMode, onCompleted);
    }


    public void HideAnime(Action onCompleted = null)
    {
        for (int i = 0, count = _buttons.Length; i < count; i++)
        {
            _buttons[i].HideAnime(_buttonAnimeDuration, _buttonAnimeTweenMode);
        }

        _exitButton.HideAnime(_exitButtonAnimeDuration, _exitButtonAnimeTweenMode, onCompleted);
    }


    public void ShowShowButton()
    {
        _showButton.SetActive(true);
    }


    public void HideShowButton()
    {
        _showButton.SetActive(false);
    }


    public void ShowButtons()
    {
        for (int i = 0, count = _buttons.Length; i < count; i++)
        {
            _buttons[i].gameObject.SetActive(true);
        }
    }


    public void HideButtons()
    {
        for (int i = 0, count = _buttons.Length; i < count; i++)
        {
            _buttons[i].gameObject.SetActive(false);
        }
    }
}
