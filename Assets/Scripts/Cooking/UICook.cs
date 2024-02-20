using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook : MonoBehaviour
    {
        [SerializeField] private UICookStep _uiStep1;

        [SerializeField] private UICookStep _uiStep2;

        [SerializeField] private UISelectSlot _cookStepSlot;

        [SerializeField] private int _materialSlotMaxCount;
        public int MaterialSlotMaxCount => _materialSlotMaxCount;

        private int _materialSlotCount = 1;
        public int MaterialSlotCount => _materialSlotCount;


        private CookSystem _cookSystem;
        public CookSystem CookSystem => _cookSystem;

        


        public void Init(CookSystem cookSystem)
        {
            _cookSystem = cookSystem;
            _uiStep1.Init(this);
            _uiStep2.Init(this);
            _cookStepSlot.Init(() => ChangeCookStep(-1), () => ChangeCookStep(1));

            ChangeCookStep(0);
        }


        public void AddMaterialCount(int value)
        {
            _materialSlotCount += value;
            _materialSlotCount = Mathf.Clamp(_materialSlotCount, 1, _materialSlotMaxCount);
        }

            
        private void ChangeStepEvent(int cookStep)
        {
            switch ((CookStep)cookStep)
            {
                case CookStep.SelectCookware:
                    _uiStep1.StartStep();
                    _uiStep2.StopStep();
                    break;

                case CookStep.SelectGathering:
                    _uiStep1.StopStep();
                    _uiStep2.StartStep();
                    break;

                case CookStep.Start:

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
