using Muks.DataBind;
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

    private List<UICookingCenterSlot> _uiCookingCenterSlotList = new List<UICookingCenterSlot>();

    [SerializeField] private UICookwares _uiCookwares;

    [Space]

    [SerializeField] private Button _cookButton;

    [SerializeField] private GameObject _hideButtonImage;
    public GameObject HideButtonImage => _hideButtonImage;

    [SerializeField] private Button _leftCookwareChangeButton;

    [SerializeField] private Button _rightCookwareChangeButton;

    private Inventory[] _inventory => GameManager.Instance.Player.GatheringItemInventory;
    //ToDo: 수정 예정

    private RecipeData _currentRecipeData;



    public void Start()
    {
        Init(null);
        _uiCookingDragSlot.Init(this);

        for(int i = 0, count = _uiCookingCenterSlot.Length; i < count; i++)
        {
            _uiCookingCenterSlot[i].Init(this);
        }


        DataBind.SetButtonValue("CookingExitButton", () => LoadingSceneManager.LoadScene("24_01_09_Integrated"));
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

        List<InventoryItem> items = _inventory[(int)CookingSystem.InventoryType].GetItemList();

        for (int i = 0, count = items.Count; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(items[i]);
        }
        //TODO : 수정예정

        _leftCookwareChangeButton.onClick.AddListener(() => ChangeCookware(-1));
        _rightCookwareChangeButton.onClick.AddListener(() => ChangeCookware(1));
        _leftCookwareChangeButton.gameObject.SetActive(false);
        _rightCookwareChangeButton.gameObject.SetActive(true);

        SetCookwareCount(0);
    }

    public void UpdateUI()
    {
        for (int i = 0, count = _uiCookingSlots.Length; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(null);
        }

        List<InventoryItem> items = _inventory[(int)CookingSystem.InventoryType].GetItemList();
        for (int i = 0, count = items.Count; i < count; i++)
        {
            _uiCookingSlots[i].UpdateUI(items[i]);
        }
        //TODO: 수정예정
    }


    private void StartCooking()
    {
        if(CookingSystem.IsEnabledCooking(_uiCookingCenterSlot[0].CurrentItem, _uiCookingCenterSlot[1].CurrentItem))
        {
            for(int i = 0, count = _uiCookingCenterSlot.Length; i < count; i++)
            {
                _uiCookingCenterSlot[i].ResetItem();
            }
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
            SetCookwareCount(currentCookware);
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
        foreach (UICookingCenterSlot slot in _uiCookingCenterSlotList)
        {
            if (slot.CurrentItem != null)
                continue;

            slot.ChoiceItem(item);
            return;
        }

        _uiCookingCenterSlotList.Last().ChoiceItem(item);
        CheckCookEnabled();
    }


    private void SetCookwareCount(int value)
    {
        _uiCookingCenterSlotList.Clear();
        for(int i = 0; i < _uiCookingCenterSlot.Length; i++)
        {
            _uiCookingCenterSlot[i].ResetItem();
            _uiCookingCenterSlot[i].SetActiveSlot(false);
        }

        for(int i = 0; i < value + 1; i++)
        {
            _uiCookingCenterSlot[i].SetActiveSlot(true);
            _uiCookingCenterSlotList.Add(_uiCookingCenterSlot[i]);
        }
    }


    public override void Show()
    {

    }

    public override void Hide()
    {

    }


}
