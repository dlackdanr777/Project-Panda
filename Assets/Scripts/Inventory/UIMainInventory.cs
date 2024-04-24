using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIMainInventory : UIView
{

    [Header("ShowUI Animation Setting")]
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _duration;
    [SerializeField] private TweenMode _tweenMode;

    private CanvasGroup _canvasGroup;
    private Vector3 _tmpPos;
    private Vector3 _movePos => new Vector3(0, 50, 0);


    [Space]
    [Header("Components")]
    [SerializeField] private UIMainInventoryContoller _inventoryContoller;
    [SerializeField] private UIDetailView _detailView;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Button _backgroundButton;



    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);     
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpPos = _targetRect.anchoredPosition;

        _inventoryContoller.Init(SlotButtonClicked);
        _detailView.Init(() => _detailView.gameObject.SetActive(false));
        _detailView.gameObject.SetActive(false);

        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);

        gameObject.SetActive(false);
    }


    public override void Show()
    {
        VisibleState = VisibleState.Appearing;
        gameObject.SetActive(true);
        _slotParent.gameObject.SetActive(false);

        _targetRect.anchoredPosition = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos, _duration, _tweenMode);
        Tween.CanvasGroupAlpha(gameObject, _targetAlpha, _duration, _tweenMode, () =>
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

        _targetRect.anchoredPosition = _tmpPos;
        _canvasGroup.alpha = _targetAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos - _movePos, _duration, _tweenMode);
        Tween.CanvasGroupAlpha(gameObject, _startAlpha, _duration, _tweenMode, () =>
        {
            VisibleState = VisibleState.Disappeared;
            _canvasGroup.blocksRaycasts = true;

            gameObject.SetActive(false);
        });
    }


    private void SlotButtonClicked(InventoryItem item)
    {
        if (item == null)
            return;

        _detailView.Show(item);
    }


    private void OnBackgroundButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Inventory");
    }



}
