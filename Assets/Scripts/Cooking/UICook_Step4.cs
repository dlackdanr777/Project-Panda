using Muks.Tween;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook_Step4 : UICookStep
    {
        [Header("Components")]
        [SerializeField] private UIDetailView _view;

        [Header("Anime")]
        [SerializeField] private Transform _animeParent;

        [SerializeField] private TextMeshProUGUI _startText;

        [SerializeField] private GameObject _complatedAnimePrefab;

        [SerializeField] private GameObject _failedAnimePrefab;


        [Space]
        [Header("Button")]
        [SerializeField] private Button _endButton;

        [Tooltip("(Enum)CookValue와 연동되어있으므로 CookValue의 값과 동일하게 넣어주셔야 합니다.")]
        [SerializeField] private UIAddValueButton[] _addButtons = new UIAddValueButton[(int)CookValue.Count]; //CookValue와 연동

        [Space]
        [Header("Bar")]
        [SerializeField] private UICookBar _staminaValueBar;

        [SerializeField] private UICookBar _fireValueBar;

        [SerializeField] private UISuccessLocation _uiSuccessLocation;

        [Space]
        [Header("Clock")]
        [SerializeField] private UICookTimer _uiTimer;



        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);
            _uiSuccessLocation.Init();
            _uiTimer.Init();
            _endButton.onClick.AddListener(CookEnd);
            _view.Init(() => _uiCook.ChangeStepEvent((int)CookStep.SelectCookware));
            _cookSystem.OnStaminaValueChanged += _staminaValueBar.UpdateGauge;
            _cookSystem.OnFireValueChanged += _fireValueBar.UpdateGauge;
        }


        public override void StartStep()
        {
            gameObject.SetActive(true);
            _view.gameObject.SetActive(false);

            _cookSystem.ResetStatus();
            _staminaValueBar.ResetBar(1);
            _fireValueBar.ResetBar(0);
            _uiSuccessLocation.SetSuccessRange(_cookSystem.GetCurrentRecipe(), _fireValueBar.GetBarWedth());

            GameManager.Instance.Player.AddItemById("CookFd53", 1, ItemAddEventType.None);

            StartAnime(() =>
            {
                _uiTimer.StartTimer(_cookSystem.CookTime, CookEnd);
                _uiCook.Cookwares[(int)_uiCook.CurrentCookware].CookStart();

                CheckAddValueButtons();
            });
        }


        public override void StopStep()
        {
            gameObject.SetActive(false);
        }


        private void CookEnd()
        {
            _uiTimer.EndTimer();
            _canvasGroup.blocksRaycasts = false;
            _uiCook.Cookwares[(int)_uiCook.CurrentCookware].CookEnd();

            string grade = _cookSystem.CheckItemGrade();
            Debug.Log(grade + " 획득");

            GameObject animeObj = grade == "F" ? Instantiate(_failedAnimePrefab) : Instantiate(_complatedAnimePrefab);
            animeObj.transform.SetParent(_animeParent);
            animeObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            animeObj.transform.localScale = Vector3.one;

            FoodItem item = DatabaseManager.Instance.ItemDatabase.ItemFoodDic["CookFd53"]; //여기에 쓰레기 아이템
            if (grade != "F")
            {
                item = _cookSystem.GetCurrentRecipe().Item;
                GameManager.Instance.Player.RemoveItemById("CookFd53", 1);
                GameManager.Instance.Player.AddItemById(item.Id, 1);
            }

            //1초를 기다린 후 
            Tween.TransformMove(gameObject, transform.position, 1, TweenMode.Constant, () =>
            {
                _canvasGroup.blocksRaycasts = true;
                _view.Show(item);
            });
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

            _addButtons[(int)cookValue].CheckUsabled(check, () =>
            {
                _cookSystem.DecreaseStamina(cookValue);
                _cookSystem.AddFireValue(cookValue);

                RecipeData data = _cookSystem.GetCurrentRecipe();
                float fireValue = _cookSystem.CurrentFireValue;
                _uiCook.Cookwares[(int)_uiCook.CurrentCookware].CookPlayAnime(data, fireValue);

                CheckAddValueButtons();
            });
        }


        private void StartAnime(Action onCompleted)
        {
            _startText.gameObject.SetActive(true);
            _canvasGroup.blocksRaycasts = false;

            Color startColor = new Color(_startText.color.r, _startText.color.g, _startText.color.b, 0.5f);
            _startText.color = startColor;
            _startText.text = "3";

            Tween.TMPAlpha(_startText.gameObject, 1, 1, TweenMode.EaseOutBack, () =>
            {
                _startText.color = startColor;
                _startText.text = "2";

                Tween.TMPAlpha(_startText.gameObject, 1, 1, TweenMode.EaseOutBack, () =>
                {
                    _startText.color = startColor;
                    _startText.text = "1";

                    Tween.TMPAlpha(_startText.gameObject, 1, 1, TweenMode.EaseOutBack, () =>
                    {

                        _startText.color = startColor;
                        _canvasGroup.blocksRaycasts = true;
                        _startText.gameObject.SetActive(false);
                        onCompleted?.Invoke();
                    });
                });
            });
        }
    }
}

