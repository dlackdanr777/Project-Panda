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
    [SerializeField] private int _maxFireValue;

    private UICooking _uiCooking;

    private CookingSystem _cookingSystem;

    private CookingUserData _cookingUserData;

    private RecipeData _currentRecipeData;

    private int _fireValue;

    private int _stamina;

    public void Init(CookingSystem cookingSystem, UICooking uiCooking)
    {
        _cookingSystem = cookingSystem;
        _uiCooking = uiCooking;

        _cookingUserData = _cookingSystem.CookingData;
        _complatedButton.onClick.AddListener(CookingComplated);
        gameObject.SetActive(false);
    }

    public void StartCooking(RecipeData currentRecipe)
    {
        _currentRecipeData = currentRecipe;
        _stamina = _cookingUserData.MaxStamina;
        _fireValue = 0;
        gameObject.SetActive(true);

        CheckAllAddValueButtons();
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
        Debug.Log(check);
        _moreAddButton.CheckUsabled(check, () =>
        {
            _stamina -= _cookingUserData.MoreAddValueStamina;
            AddFireValue(_cookingUserData.MoreAddValue);
            CheckAllAddValueButtons();
        });
    }


    private void AddValueButtonClicked()
    {
        bool check = _cookingUserData.AddValueStamina <= _stamina;
        Debug.Log(check);
        _addButton.CheckUsabled(check, () =>
        {
            _stamina -= _cookingUserData.AddValueStamina;
            AddFireValue(_cookingUserData.AddValue);
            CheckAllAddValueButtons();
        });
    }

    private void SmallAddValueButtonClicked()
    {
        bool check = _cookingUserData.SmallAddValueStamina <= _stamina;
        _smallAddButton.CheckUsabled(check, () =>
        {
            _stamina -= _cookingUserData.SmallAddValueStamina;
            AddFireValue(_cookingUserData.SmallAddValue);
            CheckAllAddValueButtons();
        });

    }

    private void CookingComplated()
    {
        _uiCooking.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }


    private void AddFireValue(int value)
    {
        _fireValue += value;

        if (_maxFireValue < _fireValue)
            _fireValue = _maxFireValue;
        Debug.Log(_stamina + "스테미나");
        Debug.Log(_fireValue + "불 게이지");
        //애니메이션 추가
    }


}
