using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private UICookingCenterSlot[] _uiCookingCenterSlot;
    public UICookingCenterSlot[] UICookingCenterSlot => _uiCookingCenterSlot;

    [SerializeField] private UICookwares _uiCookwares;

    [Space]

    [SerializeField] private Button _cookButton;

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

        for(int i = 0, count = _uiCookingCenterSlot.Length; i < count; i++)
        {
            _uiCookingCenterSlot[i].Init(this);
        }
        _cookButton.onClick.AddListener(StartCooking);
        _hideButtonImage.SetActive(true);
    }


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _uiCookingSlots = _uiCookingSlotParent.GetComponentsInChildren<UICookingSlot>();

        _uiCookingStart.Init(_cookingSystem, this);
        _uiCookwares.Init();

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
        _leftCookwareChangeButton.gameObject.SetActive(false);
        _rightCookwareChangeButton.gameObject.SetActive(true);
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


    private void StartCooking()
    {
        if(CookingSystem.IsEnabledCooking(_currentRecipeData))
        {
            gameObject.SetActive(false);
            _uiCookingStart.gameObject.SetActive(true);
            _uiCookingStart.StartCooking(_currentRecipeData);
        }

    }

    private void ChangeCookware(int value)
    {
        int currentCookware = _cookingSystem.ChangeCookware(value);
        _leftCookwareChangeButton.gameObject.SetActive(false);
        _rightCookwareChangeButton.gameObject.SetActive(false);

        _uiCookwares.ChangeImage(value, () =>
        {
            CheckCookEnabled();

            if (currentCookware == 0)
                _leftCookwareChangeButton.gameObject.SetActive(false);
            else
                _leftCookwareChangeButton.gameObject.SetActive(true);

            if ((int)Cookware.Sizeof - 1 <= currentCookware)
                _rightCookwareChangeButton.gameObject.SetActive(false);
            else
                _rightCookwareChangeButton.gameObject.SetActive(true);
        });
      
    }

    public void CheckCookEnabled()
    {
        if (CookingSystem.GetkRecipeByItems(_uiCookingCenterSlot[0].CurrentItem, _uiCookingCenterSlot[1].CurrentItem) != null)
        {
            _currentRecipeData = CookingSystem.GetkRecipeByItems(_uiCookingCenterSlot[0].CurrentItem, _uiCookingCenterSlot[1].CurrentItem);
            HideButtonImage.SetActive(false);
            return;
        }
        _hideButtonImage.SetActive(true);
    }

    public void ChoiceItem(InventoryItem item)
    {
        foreach (UICookingCenterSlot slot in _uiCookingCenterSlot)
        {
            if (slot.CurrentItem != null)
                continue;

            slot.ChoiceItem(item);
            return;
        }

        _uiCookingCenterSlot.Last().ChoiceItem(item);

        CheckCookEnabled();
    }

    private void OnCookButtonCilcked()
    {
        //StartCooking(CookingSystem.GetRecipeByItem(_currentItem));
        //CheckItem();
    }

    


    public override void Show()
    {

    }

    public override void Hide()
    {

    }


}
