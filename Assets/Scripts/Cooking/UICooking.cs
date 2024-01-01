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
    public UICookingCenterSlot UICookingCenterSlot => _uiCookingCenterSlot;

    [Space]
    [SerializeField] private GameObject _hideButtonImage;
    public GameObject HideButtonImage => _hideButtonImage;

    [SerializeField] private Button _leftCookwareChangeButton;

    [SerializeField] private Button _rightCookwareChangeButton;

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

        _leftCookwareChangeButton.onClick.AddListener(() => ChangeCookware(-1));
        _rightCookwareChangeButton.onClick.AddListener(() => ChangeCookware(1));
        ChangeCookware(0);
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

    private void ChangeCookware(int value)
    {
        int currentCookware = _cookingSystem.ChangeCookware(value);
        CheckCookEnabled(_uiCookingCenterSlot.CurrentItem);

        if (currentCookware == 0)
            _leftCookwareChangeButton.gameObject.SetActive(false);
        else
            _leftCookwareChangeButton.gameObject.SetActive(true);

        if ((int)Cookware.Sizeof -1 <= currentCookware)
            _rightCookwareChangeButton.gameObject.SetActive(false);
        else
            _rightCookwareChangeButton.gameObject.SetActive(true);
    }

    public void CheckCookEnabled(InventoryItem item)
    {
        if (CookingSystem.CheckRecipe(item))
        {
            HideButtonImage.SetActive(false);
            return;
        }

        HideButtonImage.SetActive(true);
    }
   

    public override void Show()
    {

    }

    public override void Hide()
    {

    }


}
