using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWishTreeInventory : UIView
{
    [SerializeField] private float _duration;

    private Vector2 tempPos;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        tempPos = RectTransform.anchoredPosition;

    }
    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;
        RectTransform.anchoredPosition = tempPos;
        Tween.RectTransfromAnchoredPosition(gameObject, new Vector2(200, 0), _duration, TweenMode.EaseInOutBack, () =>
        {
            RectTransform.anchoredPosition = new Vector2(200, 0);
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });

    }

    public override void Show() 
    {
        VisibleState = VisibleState.Appearing;

        RectTransform.anchoredPosition = new Vector2(200, 0);
        gameObject.SetActive(true);

        Tween.RectTransfromAnchoredPosition(gameObject, tempPos, _duration, TweenMode.EaseInOutBack, () =>
        {
            RectTransform.anchoredPosition = tempPos;
            VisibleState = VisibleState.Appeared;
        });
    }

    public void ShowAnime()
    {

    }
}
