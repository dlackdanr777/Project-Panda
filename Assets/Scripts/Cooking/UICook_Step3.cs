using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cooking
{
    public class UICook_Step3 : UICookStep
    {
        //(Enum)Cooware과 연동 
        [SerializeField] private UICookware[] _cookwares;

        [SerializeField] private Button _startButton;

        [Space]
        [Header("AddButtons")]
        [SerializeField] private GameObject _addButtons;

        [SerializeField] private UIAddValueButton _moreAddButton;

        [SerializeField] private UIAddValueButton _addButton;

        [SerializeField] private UIAddValueButton _smallAddButton;

        private CookingUserData _userData => _cookSystem.UserData;
        private Cookware _currentCookware => _cookSystem.GetCookware();

        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);

            _startButton.onClick.AddListener(CookStart);
        }


        public override void StartStep()
        {
            gameObject.SetActive(true);
            for(int i = 0, count = _cookwares.Length; i < count; i++)
            {
                _cookwares[i].gameObject.SetActive(false);
            }

            _cookwares[(int)_currentCookware].gameObject.SetActive(true);
            _cookwares[(int)_currentCookware].CookSet();

            _addButtons.SetActive(false);
        }


        public override void StopStep()
        {
            gameObject.SetActive(false);
        }


        public override bool CheckNextStep()
        {
            return true;
        }


        private void CookStart()
        {
            _cookSystem.ResetStatus();
            _cookwares[(int)_currentCookware].CookStart();

            _startButton.gameObject.SetActive(false);
            _addButtons.SetActive(true);
            CheckAllAddValueButtons();
        }


        private void CookEnd()
        {
            _cookwares[(int)_currentCookware].CookEnd();

            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(CookStart);

            _addButtons.SetActive(false);
        }


        private void CheckAllAddValueButtons()
        {
            MoreAddValueButtonClicked();
            AddValueButtonClicked();
            SmallAddValueButtonClicked();
        }


        private void MoreAddValueButtonClicked()
        {
            int fireValue = _userData.MoreAddValueStamina;

            bool check = _cookSystem.CheckAddFireValueEnabled() && _cookSystem.CheckDecreaseStaminaEnabled(fireValue);
            _moreAddButton.CheckUsabled(check, () =>
            {
                _cookSystem.DecreaseStamina(_userData.MoreAddValueStamina);
                int rendInt = Random.Range(0, _userData.MoreAddValue.Length);
                _cookSystem.AddFireValue(_userData.MoreAddValue[rendInt]);
                _cookwares[(int)_currentCookware].CookAnimationPlay();

                CheckAllAddValueButtons();
            });
        }


        private void AddValueButtonClicked()
        {
            int fireValue = _userData.AddValueStamina;

            bool check = _cookSystem.CheckAddFireValueEnabled() && _cookSystem.CheckDecreaseStaminaEnabled(fireValue);
            _addButton.CheckUsabled(check, () =>
            {
                _cookSystem.DecreaseStamina(_userData.AddValueStamina);
                int rendInt = Random.Range(0, _userData.AddValue.Length);
                _cookSystem.AddFireValue(_userData.AddValue[rendInt]);
                _cookwares[(int)_currentCookware].CookAnimationPlay();
                CheckAllAddValueButtons();
            });
        }


        private void SmallAddValueButtonClicked()
        {
            int fireValue = _userData.SmallAddValueStamina;

            bool check = _cookSystem.CheckAddFireValueEnabled() && _cookSystem.CheckDecreaseStaminaEnabled(fireValue);

            Debug.Log(check);
            _smallAddButton.CheckUsabled(check, () =>
            {
                Debug.Log("시작");
                _cookSystem.DecreaseStamina(_userData.SmallAddValueStamina);
                int rendInt = Random.Range(0, _userData.SmallAddValue.Length);
                _cookSystem.AddFireValue(_userData.SmallAddValue[rendInt]);
                _cookwares[(int)_currentCookware].CookAnimationPlay();
                CheckAllAddValueButtons();
            });
        }


    }
}

