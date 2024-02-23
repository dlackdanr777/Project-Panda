using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook_Step3 : UICookStep
    {

        [SerializeField] private Button _startButton;


        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);

            _startButton.onClick.AddListener(CookStart);
        }


        public override void StartStep()
        {
            gameObject.SetActive(true);
            _uiCook.ShowCookware();
        }


        public override void StopStep()
        {
            gameObject.SetActive(false);
        }


        private void CookStart()
        {
            _uiCook.ChangeStepEvent((int)CookStep.Cooking);
        }
    }
}

