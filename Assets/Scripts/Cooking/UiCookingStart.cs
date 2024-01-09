using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCookingStart : MonoBehaviour
{
    [SerializeField] private UIAddValueButton _moreAddButton;

    [SerializeField] private UIAddValueButton _addButton;

    [SerializeField] private UIAddValueButton _smallAddButton;

    [SerializeField] private Button _complatedButton;

    [SerializeField] private TextMeshProUGUI _complatedText;

    [Space]
    [SerializeField] private UICookingBar _uiStaminaBar;

    [SerializeField] private UICookingBar _uiFireBar;

    [SerializeField] private UISuccessLocation _uiSuccessLocation;

    [SerializeField] private UICookingTimer _uiCookingTimer;

    [SerializeField] private UICookingEnd _uiCookingEnd;

    [SerializeField] private UICookwares _uiCookwares;

    [Space]
    [SerializeField] private GameObject _dontTouchArea;

    [Space]
    [SerializeField] private int _maxFireValue;


    [Space]
    [SerializeField] private Transform _animeParent;

    [SerializeField] private GameObject _complatedAnimePrefab;

    [SerializeField] private GameObject _failedAnimePrefab;


    private UICooking _uiCooking;

    private CookingSystem _cookingSystem;

    private CookingUserData _cookingUserData;

    private RecipeData _currentRecipeData;

    private int _fireValue;

    private int _stamina;

    private int _tempfireValue;

    public void Init(CookingSystem cookingSystem, UICooking uiCooking)
    {
        _uiSuccessLocation.Init(this);
        _uiCookingTimer.Init(this);
        _uiCookingEnd.Init(OnComplatedButtonClicked);
        _cookingSystem = cookingSystem;
        _uiCooking = uiCooking;
        _cookingUserData = _cookingSystem.UserData;
        gameObject.SetActive(false);
    }

    public void StartCooking(RecipeData recipe)
    {
        _currentRecipeData = recipe;
        _stamina = _cookingUserData.MaxStamina;
        _fireValue = 0;
        _uiStaminaBar.ResetBar(1);
        _uiFireBar.ResetBar(0);
        _complatedButton.onClick.AddListener(FilpFood);
        _uiCookwares.StartCooking();
        _uiSuccessLocation.SetSuccessRange(recipe, _uiFireBar.GetBarWedth());
        _uiCookingTimer.StartTimer(60);

        gameObject.SetActive(true);
        CheckAllAddValueButtons();

        _complatedText.text = "뒤집기";
    }


    public void FilpFood()
    {
        SaveFireValue();
        CheckAllAddValueButtons();
        _uiCookwares.StartAnime();
        _complatedButton.onClick.RemoveListener(FilpFood);
        _complatedButton.onClick.AddListener(CookingComplated);
        _dontTouchArea.SetActive(true);
        Invoke("DisabledDontTouchArea", 1);
        _complatedText.text = "완성";
    }


    private void DisabledDontTouchArea()
    {
        _dontTouchArea.SetActive(false);
    }


    /// <summary>조리완료 버튼 클릭시 실행되는 함수</summary>
    public void CookingComplated()
    {
        StartCoroutine(CookingComplatedRoutine());
    }

    private IEnumerator CookingComplatedRoutine()
    {
        float totalFireValue = (_tempfireValue + _fireValue) * 0.5f * 0.01f;

        string grade = _uiCooking.CookingSystem.CheckItemGrade(_currentRecipeData, totalFireValue);
        Debug.Log(grade + " 획득");

        GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem)[(int)_cookingSystem.InventoryType].Add(_currentRecipeData.Item);
        //TODO : 여기서 오류

        GameObject animeObj = grade == "F" ? Instantiate(_failedAnimePrefab) : Instantiate(_complatedAnimePrefab);
        animeObj.transform.SetParent(_animeParent);
        //여기에 애니메이션

        _uiCookingTimer.EndTimer();
        _uiCookwares.EndCooking();
        _complatedButton.onClick.RemoveAllListeners();

        yield return new WaitForSeconds(1.1f);

        Destroy(animeObj);
        _uiCookingEnd.Show(_currentRecipeData.Item);
        _uiCooking.UpdateUI();
    }


    private void OnComplatedButtonClicked()
    {
        _uiCooking.gameObject.SetActive(true);

        for (int i = 0, count = _uiCooking.UICookingCenterSlot.Length; i < count; i++)
        {
            _uiCooking.UICookingCenterSlot[i].CheckCurrentItem();
        }

        _uiCooking.CheckCookEnabled();

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
            int rendInt = Random.Range(0, _cookingUserData.MoreAddValue.Length);
            AddFireValue(_cookingUserData.MoreAddValue[rendInt]);
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
            int rendInt = Random.Range(0, _cookingUserData.AddValue.Length);
            AddFireValue(_cookingUserData.AddValue[rendInt]);
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
            int rendInt = Random.Range(0, _cookingUserData.SmallAddValue.Length);
            AddFireValue(_cookingUserData.SmallAddValue[rendInt]);
            CheckAllAddValueButtons();
        });

    }


    private void DecreaseStamina(int value)
    {
        _stamina -= value;

        if(_stamina < 0)
            _stamina = 0;

        _uiStaminaBar.UpdateGauge(_cookingUserData.MaxStamina, _stamina);
    }


    private void AddFireValue(int value)
    {
        _fireValue += value;

        if (_maxFireValue < _fireValue)
            _fireValue = _maxFireValue;

        _uiCookwares.SetFoodSprite(_currentRecipeData, _fireValue);
        _uiFireBar.UpdateGauge(_maxFireValue, _fireValue);
    }


    private void SaveFireValue()
    {
        _tempfireValue = 0;
        _tempfireValue += _fireValue;
        _fireValue = 0;
        _uiFireBar.UpdateGauge(_maxFireValue, _fireValue);
    }


}
