using Muks.Tween;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIIntroTitle : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image[] _leafs1;
    [SerializeField] private Image[] _leafs2;
    [SerializeField] private Image _titleImage1;
    [SerializeField] private Image _titleImage2;
    [SerializeField] private TextMeshProUGUI _touchText;
    [SerializeField] private Button _backgroundButton;


    public void Init()
    {
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

        _touchText.color = new Color(_touchText.color.r, _touchText.color.g, _touchText.color.b, 0.6f);
        _touchText.gameObject.SetActive(false);
        _backgroundButton.gameObject.SetActive(false);
    }


    public void Show()
    {
        ShowLeaf();
        ShowTitle();
    }


    private void ShowLeaf()
    {
        for (int i = 0, count = _leafs1.Length; i < count; i++)
        {
            Tween.IamgeAlpha(_leafs1[i].gameObject, 1, 1 + i * 0.3f, TweenMode.Smootherstep);
        }

        for (int i = 0, count = _leafs2.Length; i < count; i++)
        {
            Tween.IamgeAlpha(_leafs2[i].gameObject, 1, 1 + i * 0.3f, TweenMode.Smootherstep);
        }
    }


    private void ShowTitle()
    {
        Tween.IamgeAlpha(_titleImage1.gameObject, 1, 2, TweenMode.Quadratic, () =>
        {
            Tween.IamgeAlpha(_titleImage2.gameObject, 1, 2, TweenMode.Quadratic, EndShowTitle);
        });
    }


    private void EndShowTitle()
    {
        //2초 대기후 터치 시작
        Tween.TransformMove(gameObject, transform.position, 2, TweenMode.Constant, () =>
        {
            _backgroundButton.gameObject.SetActive(true);
            _backgroundButton.onClick.AddListener(OnButtonClicked);

            _touchText.gameObject.SetActive(true);
            Tween.TMPAlpha(_touchText.gameObject, 1, 2).Loop(LoopType.Yoyo);
        });
    }


    private void OnButtonClicked()
    { 
        Tween.Stop(_touchText.gameObject);
        _touchText.color = new Color(_touchText.color.r, _touchText.color.g, _touchText.color.b, 1);

        GameManager.Instance.Player.AddItemById("ITG05", 1, ItemAddEventType.None, false);
        GameManager.Instance.Player.AddItemById("IFR02", 5);
        DatabaseManager.Instance.UserInfo.IsExistingUser = true;
        DatabaseManager.Instance.UserInfo.SaveUserInfoData(10);
        LoadingSceneManager.LoadScene("24_01_09_Integrated");
    }
}
