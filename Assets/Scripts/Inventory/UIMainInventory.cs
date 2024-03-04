using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainInventory : UIView
{

    [SerializeField] private UIMainInventoryContoller _inventoryContoller;

    [SerializeField] private UIDetailView _detailView;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private Transform _slotParent;

    [Space]
    [Header("Animations")]
    [SerializeField] private float _showDuration;

    [SerializeField] private TweenMode _showTweenMode;

    [Space]
    [SerializeField] private float _hideDuration;

    [SerializeField] private TweenMode _hideTweenMode;

    [SerializeField] private Vector3 _startScale;

    [SerializeField] private Vector3 _targetScale;

    [SerializeField] private Transform _startPos;

    [SerializeField] private Transform _targetPos;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _inventoryContoller.Init(SlotButtonClicked);

        _detailView.Init(() => _detailView.gameObject.SetActive(false));
        _detailView.gameObject.SetActive(false);
    }


    public override void Show()
    {
        VisibleState = VisibleState.Appearing;
        gameObject.SetActive(true);
        _slotParent.gameObject.SetActive(false);
        _detailView.gameObject.SetActive(false);

        _canvasGroup.blocksRaycasts = false;

        _inventoryContoller.transform.position = _startPos.position;
        Tween.TransformMove(_inventoryContoller.gameObject, _targetPos.position, _showDuration, _showTweenMode);

        _inventoryContoller.transform.localScale = _startScale;
        Tween.TransformScale(_inventoryContoller.gameObject, _targetScale, _showDuration, _showTweenMode, () =>
        {
            VisibleState = VisibleState.Appeared;
            _canvasGroup.blocksRaycasts = true;
            _slotParent.gameObject.SetActive(true);
        });
    }


    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;
        _slotParent.gameObject.SetActive(false);
        _detailView.gameObject.SetActive(false);
        _canvasGroup.blocksRaycasts = false;

        _inventoryContoller.transform.position = _targetPos.position;
        Tween.TransformMove(_inventoryContoller.gameObject, _startPos.position, _hideDuration, _hideTweenMode);

        _inventoryContoller.transform.localScale = _targetScale;
        Tween.TransformScale(_inventoryContoller.gameObject, _startScale, _hideDuration, _hideTweenMode, () =>
        {
            VisibleState = VisibleState.Disappeared;
            gameObject.SetActive(false);
        });
    }


    private void SlotButtonClicked(InventoryItem item)
    {
        _detailView.Show(item);
    }



}
