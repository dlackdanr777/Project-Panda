using UnityEngine;
using UnityEngine.UI;
using Muks.Tween;
using System;

public class UIShowTitle : MonoBehaviour
{

    [SerializeField] private Image[] _leafs1;

    [SerializeField] private Image[] _leafs2;

    [SerializeField] private Image _titleImage1;

    [SerializeField] private Image _titleImage2;

    [SerializeField] private Button _backgroundButton;

    private ShowTitle _showTitle;

    public event Action OnButtonClickHandler;


    public void Init(ShowTitle showTitle)
    {
        _showTitle = showTitle;
        _showTitle.OnStartHandler += ShowLeaf;
        _showTitle.OnStartHandler += ShowTitle;

        Color color;

        foreach (Image leaf in _leafs1)
        {
            color = leaf.color;
            color.a = 0;
            leaf.color = color;
        }

        foreach (Image leaf in _leafs2)
        {
            color = leaf.color;
            color.a = 0;
            leaf.color = color;
        }

        color = _titleImage1.color;
        color.a = 0;
        _titleImage1.color = color;

        color = _titleImage2.color;
        color.a = 0;
        _titleImage2.color = color;

        _backgroundButton.gameObject.SetActive(false);
    }


    public void ShowLeaf()
    {
        for(int i = 0, count = _leafs1.Length; i < count; i++)
        {
            Tween.IamgeAlpha(_leafs1[i].gameObject, 1, 1 + i * 0.3f, TweenMode.Smootherstep);
        }

        for (int i = 0, count = _leafs2.Length; i < count; i++)
        {
            Tween.IamgeAlpha(_leafs2[i].gameObject, 1, 1 + i * 0.3f, TweenMode.Smootherstep);
        }
    }


    public void ShowTitle()
    {


        Tween.IamgeAlpha(_titleImage1.gameObject, 1, 2, TweenMode.Quadratic, () =>
        {
            Tween.IamgeAlpha(_titleImage2.gameObject, 1, 2, TweenMode.Quadratic, EndShowTitle);
        });
    }


    public void EndShowTitle()
    {
        _backgroundButton.gameObject.SetActive(true);
        _backgroundButton.onClick.AddListener(OnButtonClicked);
    }


    private void OnButtonClicked()
    {
        OnButtonClickHandler?.Invoke();
    }
}
