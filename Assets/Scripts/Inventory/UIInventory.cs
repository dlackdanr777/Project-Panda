using Muks.Tween;
using UnityEngine;

public class UIInventory : UIView
{
    [SerializeField] private GameObject _moveUI;

    [SerializeField] private GameObject _showButton;

    [SerializeField] private GameObject _hideButton;

    [SerializeField] private float _duration;

    private Vector2 _tempPos;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _tempPos = _moveUI.GetComponent<RectTransform>().anchoredPosition;
    }

    public override void Hide()
    {

        Anime1_Hide();
    }


    public override void Show()
    {
        Anime1_Show();
    }

    /// <summary>위에서 아래로 올라오는 방식</summary>
    private void Anime1_Show()
    {
        gameObject.SetActive(true);
        _showButton.SetActive(false);
        VisibleState = VisibleState.Appearing;

        Tween.RectTransfromAnchoredPosition(_hideButton, new Vector2(0, 530), 0.7f, TweenMode.EaseInOutBack, () =>
        {
            Tween.RectTransfromAnchoredPosition(_moveUI, new Vector2(0, -800), _duration, TweenMode.EaseInOutBack, () =>
            {
                VisibleState = VisibleState.Appeared;
            });
        });
        
    }

    /// <summary>중앙에서 나타나서 뽀용하고 뛰는 방식</summary>
    private void Anime2_Show()
    {
        gameObject.SetActive(true);
        _moveUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        _moveUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Tween.TransformScale(_moveUI, new Vector3(1, 1, 1), _duration, TweenMode.EaseInOutBack, () =>
        {
            _moveUI.transform.localScale = new Vector3(1, 1, 1);
            VisibleState = VisibleState.Appeared;
        });
    }

    private void Anime1_Hide()
    {
        VisibleState = VisibleState.Disappearing;

        Tween.RectTransfromAnchoredPosition(_moveUI, _tempPos, _duration, TweenMode.EaseInOutBack, () =>
        {
            Tween.RectTransfromAnchoredPosition(_hideButton, new Vector2(0, 630), 0.7f, TweenMode.EaseInOutBack, () =>
            {
                Tween.RectTransfromAnchoredPosition(_hideButton, new Vector2(0, 663), 0.2f, TweenMode.Quadratic, () => 
                {
                    VisibleState = VisibleState.Disappeared;
                    _showButton.SetActive(true);
                    gameObject.SetActive(false);
                });


            });
        });
    }

    private void Anime2_Hide()
    {
        VisibleState = VisibleState.Disappearing;
        _moveUI.transform.localScale = new Vector3(1, 1, 1);
        Tween.TransformScale(_moveUI, new Vector3(0.5f, 0.5f, 0.5f), _duration, TweenMode.EaseInOutBack, () =>
        {
            _moveUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });
    }
}
