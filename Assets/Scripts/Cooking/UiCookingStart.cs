using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCookingStart : MonoBehaviour
{
    [SerializeField] private UIAddValueButton _moreAddButton;

    [SerializeField] private UIAddValueButton _addButton;

    [SerializeField] private UIAddValueButton _smallAddButton;

    [SerializeField] private Button _complatedButton;

    [Space]
    [SerializeField] private UICookingBar _uiStaminaBar;

    [SerializeField] private UICookingBar _uiFireBar;

    [SerializeField] private UISuccessLocation _uiSuccessLocation;

    [SerializeField] private UICookingTimer _uiCookingTimer;

    [SerializeField] private UICookingEnd _uiCookingEnd;

    [Space]
    [SerializeField] private int _maxFireValue;

    private UICooking _uiCooking;

    private CookingSystem _cookingSystem;

    private CookingUserData _cookingUserData;

    private RecipeData _currentRecipeData;

    private int _fireValue;

    private int _stamina;

    public void Init(CookingSystem cookingSystem, UICooking uiCooking)
    {
        _uiSuccessLocation.Init(this);
        _uiCookingEnd.Init(OnComplatedButtonClicked);
        _cookingSystem = cookingSystem;
        _uiCooking = uiCooking;
        _cookingUserData = _cookingSystem.CookingData;
        _complatedButton.onClick.AddListener(CookingComplated);

        gameObject.SetActive(false);
    }

    public void StartCooking(RecipeData recipe)
    {
        _currentRecipeData = recipe;
        _stamina = _cookingUserData.MaxStamina;
        _fireValue = 0;
        _uiStaminaBar.Reset(1);
        _uiFireBar.Reset(0);

        _uiSuccessLocation.SetSuccessRange(recipe, _uiFireBar.GetBarWedth());
        _uiCookingTimer.StartTimer(60);
        gameObject.SetActive(true);
        CheckAllAddValueButtons();
    }


    /// <summary>조리완료 버튼 클릭시 실행되는 함수</summary>
    public void CookingComplated()
    {
        int InventoryIndex = (int)_currentRecipeData.Item.ItemField;

        Debug.Log(_uiCooking.CookingSystem.CheckItemGrade(_currentRecipeData, _uiFireBar.GetBarValue()) + " 획득");

        GameManager.Instance.Player.Inventory[InventoryIndex].Add(_currentRecipeData.Item);

        _uiCookingTimer.EndTimer();
        _uiCookingEnd.Show(_currentRecipeData.Item);
    }


    private void OnComplatedButtonClicked()
    {
        _uiCooking.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }


    private void CheckAllAddValueButtons()
    {
        MoreAddValueButtonClicked();
        AddValueButtonClicked();
        SmallAddValueButtonClicked();

    }

    private void MoreAddValueButtonClicked()
    {
        bool check = _cookingUserData.MoreAddValueStamina <= _stamina;
        check = check && _fireValue < _maxFireValue;
        
        _moreAddButton.CheckUsabled(check, () =>
        {
            DecreaseStamina(_cookingUserData.MoreAddValueStamina);
            AddFireValue(_cookingUserData.MoreAddValue);
            CheckAllAddValueButtons();
        });
    }


    private void AddValueButtonClicked()
    {
        bool check = _cookingUserData.AddValueStamina <= _stamina;
        check = check && _fireValue < _maxFireValue;

        _addButton.CheckUsabled(check, () =>
        {
            DecreaseStamina(_cookingUserData.AddValueStamina);
            AddFireValue(_cookingUserData.AddValue);
            CheckAllAddValueButtons();
        });
    }


    private void SmallAddValueButtonClicked()
    {
        bool check = _cookingUserData.SmallAddValueStamina <= _stamina;
        check = check && _fireValue < _maxFireValue; 

        _smallAddButton.CheckUsabled(check, () =>
        {
            DecreaseStamina(_cookingUserData.SmallAddValueStamina);
            AddFireValue(_cookingUserData.SmallAddValue);
            CheckAllAddValueButtons();
        });

    }


    private void DecreaseStamina(int value)
    {
        _stamina -= value;

        if(_stamina < 0)
            _stamina = 0;

        _uiStaminaBar.DecreaseGauge(_cookingUserData.MaxStamina, _stamina);
    }


    private void AddFireValue(int value)
    {
        _fireValue += value;

        if (_maxFireValue < _fireValue)
            _fireValue = _maxFireValue;

        _uiFireBar.DecreaseGauge(_maxFireValue, _fireValue);
    }


}
