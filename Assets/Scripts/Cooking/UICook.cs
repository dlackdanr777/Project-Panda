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

        [SerializeField] private UICookStep _uiStep4;

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

        [Space]
        [Header("Cookware")]
        //원래 CookStep3에서 관리했으나 CookStep4에서도 필요하여 UI관리 클래스인 UICook으로 이관
        [SerializeField] private UICookware[] _cookwares; //(Enum)Cooware과 연동 
        public UICookware[] Cookwares => _cookwares;


        private int _materialSlotCount = 1;
        public int MaterialSlotCount => _materialSlotCount;


        private CookSystem _cookSystem;
        public CookSystem CookSystem => _cookSystem;

        public Cookware CurrentCookware => _cookSystem.GetCookware();


        public void Init(CookSystem cookSystem)
        {
            _cookSystem = cookSystem;
            _uiStep1.Init(this);
            _uiStep2.Init(this);
            _uiStep3.Init(this);
            _uiStep4.Init(this);

            _uiStep1.StartStep();
            _uiStep2.StopStep();
            _uiStep3.StopStep();
            _uiStep4.StopStep();


            _cookStepSlot.Init(() => ChangeCookStep(-1), () => ChangeCookStep(1));
            HideCookware();
            ChangeCookStep(0, true);
        }


        public void AddMaterialCount(int value)
        {
            _materialSlotCount += value;
            _materialSlotCount = Mathf.Clamp(_materialSlotCount, 1, _materialSlotMaxCount);
        }

            
        public void ChangeStepEvent(int cookStep)
        {
            _cookSystem.SetCookStep((CookStep)cookStep);
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

                case CookStep.Cooking:
                    SelectStep4();
                    break;
            }
        }


        private void SelectStep1()
        {
            Tween.Stop(_targetObj);

            _uiStep2.StopStep();
            _uiStep3.StopStep();
            _uiStep4.StopStep();

            HideCookware();
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
            _uiStep4.StopStep();

            HideCookware();
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

            _uiStep1.StopStep();
            _uiStep2.StopStep();
            _uiStep4.StopStep();

            _cookStepSlot.gameObject.SetActive(false);
            Tween.TransformMove(_targetObj, _step3Target.position, _animeDuration, _tweenMode, () =>
            {
                _uiStep3.StartStep();
            });
        }

        private void SelectStep4()
        {
            Tween.Stop(_targetObj);

            _uiStep1.StopStep();
            _uiStep2.StopStep();
            _uiStep3.StopStep();
            _cookStepSlot.gameObject.SetActive(false);
            _uiStep4.StartStep();
        }



        /// <summary>요리 도구를 현재 요리 도구 값에 맞게 설정하여 활성화 하는 함수</summary>
        public void ShowCookware()
        {
            for (int i = 0, count = _cookwares.Length; i < count; i++)
            {
                _cookwares[i].gameObject.SetActive(false);
            }

            _cookwares[(int)CurrentCookware].gameObject.SetActive(true);
            _cookwares[(int)CurrentCookware].CookSet();
        }


        /// <summary>요리 도구를 비활성화하는 함수</summary>
        public void HideCookware()
        {
            for (int i = 0, count = _cookwares.Length; i < count; i++)
            {
                _cookwares[i].gameObject.SetActive(false);
            }
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
