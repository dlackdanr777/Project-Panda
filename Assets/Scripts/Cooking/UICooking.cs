using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooking : UIView
{
    [SerializeField] private CookingSystem _cookingSystem;
    public CookingSystem CookingSystem => _cookingSystem;

    [SerializeField] private UiCookingStart _uiCookingStart;

    [SerializeField] private UICookingSlotParent _uiCookingSlotParent;

    [SerializeField] private UICookingSlot[] _uiCookingSlots;

    [SerializeField] private UICookingDragSlot _uiCookingDragSlot;
    public UICookingDragSlot UICookingDragSlot => _uiCookingDragSlot;

    [SerializeField] private UICookingCenterSlot _uiCookingCenterSlot;

    [SerializeField] private GameObject _hideButtonImage;
    public GameObject HideButtonImage => _hideButtonImage;

    private Inventory[] _inventory => GameManager.Instance.Player.Inventory;

    private RecipeData _currentRecipeData;


    public void Start()
    {
        Init(null);
        _uiCookingDragSlot.Init(this);
        _uiCookingCenterSlot.Init(this);

        _hideButtonImage.SetActive(true);
    }


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        _uiCookingSlots = _uiCookingSlotParent.GetComponentsInChildren<UICookingSlot>();

        _uiCookingStart.Init(_cookingSystem, this);

        for (int i = 0, count = _uiCookingSlots.Length; i < count; i++)
        {
            _uiCookingSlots[i].Init(this);
            _uiCookingSlots[i].UpdateUI(null);
        }

        for (int i = 0, count = _inventory[0].ItemsCount; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(_inventory[0].Items[i]);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0, count = _uiCookingSlots.Length; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(null);
        }

        for (int i = 0, count = _inventory[0].ItemsCount; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(_inventory[0].Items[i]);
        }
    }

    public void StartCooking(RecipeData data)
    {
        _currentRecipeData = data;

        gameObject.SetActive(false);
        _uiCookingStart.gameObject.SetActive(true);
        _uiCookingStart.StartCooking(_currentRecipeData);
    }

    public override void Show()
    {

    }

    public override void Hide()
    {

    }


}
