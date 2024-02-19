using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook : MonoBehaviour
    {
        [SerializeField] private UISelect _uiSelect;

        [SerializeField] private UISelectSlot _cookStepSlot;

        private CookSystem _cookSystem;
        public CookSystem CookSystem => _cookSystem;


        public void Init(CookSystem cookSystem)
        {
            _cookSystem = cookSystem;
            _uiSelect.Init(this);
            _cookStepSlot.Init(() => ChangeCookStep(-1), () => ChangeCookStep(1));

            ChangeCookStep(0);
        }


        private void ChangeStepEvent(int cookStep)
        {
            switch ((CookStep)cookStep)
            {
                case CookStep.SelectCookware:
                    _uiSelect.StartStep1();
                    break;

                case CookStep.SelectGathering:
                    _uiSelect.StartStep2();
                    break;

                case CookStep.Start:
                    _uiSelect.StopStep1();
                    _uiSelect.StopStep2();
                    break;
            }
        }



        private void ChangeCookStep(int dir)
        {
            int currentCookStep = _cookSystem.ChangeCookStep(dir);

            ChangeStepEvent(currentCookStep);

            if (currentCookStep == 0)
                _cookStepSlot.DisableLeftButton();
            else
                _cookStepSlot.EnableLeftButton();

            if ((int)CookStep.Length - 1 <= currentCookStep)
                _cookStepSlot.DisableRightButton();
            else
                _cookStepSlot.EnableRightButton();
        }
    }

}
