using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private Image _alarmImage;

    private bool _alarmCheck;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);     
        _canvasGroup = GetComponent<CanvasGroup>();
        _tmpPos = _targetRect.anchoredPosition;

        GameManager.Instance.Player.OnAddItemHandler += AlarmCheck;
        GameManager.Instance.Player.OnRemoveItemHandler += AlarmCheck;
        LoadingSceneManager.OnLoadSceneHandler += OnChangeSceneEvent;

        _inventoryContoller.Init(SlotButtonClicked);
        _detailView.Init(() => _detailView.gameObject.SetActive(false));
        _detailView.gameObject.SetActive(false);

        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);

        gameObject.SetActive(false);
        AlarmCheck();


    }


    public override void Show()
    {
        VisibleState = VisibleState.Appearing; //열리는 중 상태로 변경
        gameObject.SetActive(true);
        _slotParent.gameObject.SetActive(false);

        //타겟 오브젝트를 열기 위한 기본 설정을 진행한다.
        _targetRect.anchoredPosition = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        //일정 시간동안 타겟 오브젝트를 이동시킨다.
        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos, _duration, _tweenMode);

        //일정 시간 동안 CanvasGroup의 Alpha값을 변경 시킨다.
        Tween.CanvasGroupAlpha(gameObject, _targetAlpha, _duration, _tweenMode, () =>
        {
            VisibleState = VisibleState.Appeared; // 열린 상태로 변경
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

        item.InvenAlarmCheck = false;
        _detailView.Show(item);
        _inventoryContoller.UpdateUI();
        AlarmCheck();
    }


    private void OnBackgroundButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Inventory");
    }


    private void AlarmCheck()
    {
        Inventory[] invens = GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem);
        List<InventoryItem> itemList;

        for (int i = 0, countI = invens.Length; i < countI; i++)
        {
            itemList = invens[i].GetItemList();

            for (int j = 0, countJ = itemList.Count; j < countJ; j++)
            {
                if (itemList[j].InvenAlarmCheck)
                {
                    DataBind.SetBoolValue("InvenAlarm", true);
                    return;
                }
            }

        }

        invens = GameManager.Instance.Player.GetItemInventory(InventoryItemField.Cook);
        for (int i = 0, countI = invens.Length; i < countI; i++)
        {
            itemList = invens[i].GetItemList();
            for (int j = 0, countJ = itemList.Count; j < countJ; j++)
            {
                if (itemList[j].InvenAlarmCheck)
                {
                    DataBind.SetBoolValue("InvenAlarm", true);
                    return;
                }
            }
        }

        invens = GameManager.Instance.Player.GetItemInventory(InventoryItemField.Tool);
        for (int i = 0, countI = invens.Length; i < countI; i++)
        {
            itemList = invens[i].GetItemList();
            for (int j = 0, countJ = itemList.Count; j < countJ; j++)
            {
                if (itemList[j].InvenAlarmCheck)
                {
                    DataBind.SetBoolValue("InvenAlarm", true);
                    return;
                }
            }
        }

        DataBind.SetBoolValue("InvenAlarm", false);
        _alarmCheck = false;
    }


    private void OnChangeSceneEvent()
    {
        GameManager.Instance.Player.OnAddItemHandler -= AlarmCheck;
        GameManager.Instance.Player.OnRemoveItemHandler -= AlarmCheck;
        LoadingSceneManager.OnLoadSceneHandler -= OnChangeSceneEvent;
    }


}
