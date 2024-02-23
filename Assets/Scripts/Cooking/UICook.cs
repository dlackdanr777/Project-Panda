using Muks.DataBind;
using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook : MonoBehaviour
    {
        [Header("Step")]
        [SerializeField] private UICookStep _uiStep1;

        [SerializeField] private UICookStep _uiStep2;

        [SerializeField] private UICookStep _uiStep3;

        [Space]
        [SerializeField] private UISelectSlot _cookStepSlot;
        public UISelectSlot CookStepSlot => _cookStepSlot;

        [SerializeField] private int _materialSlotMaxCount;
        public int MaterialSlotMaxCount => _materialSlotMaxCount;

        [Space]
        [Header("Animation")]
        [SerializeField] private GameObject _targetObj;

        [SerializeField] private float _animeDuration;

        [SerializeField] private TweenMode _tweenMode;

        [SerializeField] private Transform _step1Target;

        [SerializeField] private Transform _step2Target;

        [SerializeField] private Transform _step3Target;


        private int _materialSlotCount = 1;
        public int MaterialSlotCount => _materialSlotCount;


        private CookSystem _cookSystem;
        public CookSystem CookSystem => _cookSystem;

        


        public void Init(CookSystem cookSystem)
        {
            _cookSystem = cookSystem;
            _uiStep1.Init(this);
            _uiStep2.Init(this);
            _uiStep3.Init(this);

            _uiStep1.StartStep();
            _uiStep2.StopStep();
            _uiStep3.StopStep();

            _cookStepSlot.Init(() => ChangeCookStep(-1), () => ChangeCookStep(1));
            ChangeCookStep(0, true);
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
                    SelectStep1();
                    break;

                case CookStep.SelectGathering:
                    SelectStep2();
                    break;

                case CookStep.Start:
                    SelectStep3();
                    break;
            }
        }

        private void SelectStep1()
        {
            Tween.Stop(_targetObj);

            _uiStep2.StopStep();
            _cookStepSlot.gameObject.SetActive(false);
            Tween.TransformMove(_targetObj, _step1Target.position, _animeDuration, _tweenMode,() =>
            {
                _cookStepSlot.gameObject.SetActive(true);

                _uiStep1.StartStep();
            });
        }


        private void SelectStep2()
        {
            Tween.Stop(_targetObj);

            _uiStep1.StopStep();
            _uiStep3.StopStep();
            _cookStepSlot.gameObject.SetActive(false);
            Tween.TransformMove(_targetObj, _step2Target.position, _animeDuration, _tweenMode, () =>
            {
                _cookStepSlot.gameObject.SetActive(true);

                _uiStep2.StartStep();
            });
        }


        private void SelectStep3()
        {
            Tween.Stop(_targetObj);

            _uiStep2.StopStep();
            _cookStepSlot.gameObject.SetActive(false);
            Tween.TransformMove(_targetObj, _step3Target.position, _animeDuration, _tweenMode, () =>
            {
                _uiStep3.StartStep();
            });
        }







        private void ChangeCookStep(int dir, bool isFirstStart = false)
        {
            int currentCookStep = _cookSystem.ChangeCookStep(dir);

            if (currentCookStep == 0)
                _cookStepSlot.DisableLeftButton();
            else
                _cookStepSlot.EnableLeftButton();

            if ((int)CookStep.Length - 1 <= currentCookStep)
                _cookStepSlot.DisableRightButton();
            else
                _cookStepSlot.EnableRightButton();

            if(!isFirstStart)
                ChangeStepEvent(currentCookStep);
        }
    }

}
