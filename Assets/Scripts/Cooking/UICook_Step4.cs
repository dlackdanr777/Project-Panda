using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook_Step4 : UICookStep
    {

        [SerializeField] private Button _endButton;

        [Space]
        [Header("AddButtons")]
        [Tooltip("(Enum)CookValue�� �����Ǿ������Ƿ� CookValue�� ���� �����ϰ� �־��ּž� �մϴ�.")]
        [SerializeField] private UIAddValueButton[] _addButtons = new UIAddValueButton[(int)CookValue.Count]; //CookValue�� ����


        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);

            //_endButton.onClick.AddListener(CookEnd);
        }


        public override void StartStep()
        {
            gameObject.SetActive(true);

            _cookSystem.ResetStatus();
            CheckAddValueButtons();
            _uiCook.Cookwares[(int)_uiCook.CurrentCookware].CookStart();
        }


        public override void StopStep()
        {
            gameObject.SetActive(false);
        }


        private void CookEnd()
        {
            _uiCook.Cookwares[(int)_uiCook.CurrentCookware].CookEnd();
        }


        private void CheckAddValueButtons()
        {
            for(int i = 0, count = (int)CookValue.Count; i < count; i++)
            {
                AddValueButtonClicked((CookValue)i);
            }         
        }


        private void AddValueButtonClicked(CookValue cookValue)
        {
            bool check = _cookSystem.CheckAddFireValueEnabled() && _cookSystem.CheckDecreaseStaminaEnabled(cookValue);
            Debug.Log(check);
            _addButtons[(int)cookValue].CheckUsabled(check, () =>
            {
                _cookSystem.DecreaseStamina(cookValue);
                _cookSystem.AddFireValue(cookValue);
                _uiCook.Cookwares[(int)_uiCook.CurrentCookware].CookAnimationPlay();

                CheckAddValueButtons();
            });

        }

    }
}

