using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cooking
{
    /// <summary> 조리 도구 </summary>
    public enum Cookware
    {
        Oven,
        Pan,
        Pot,
        Length,
    }


    /// <summary> 조리 단계 </summary>
    public enum CookStep
    {
        SelectCookware,
        SelectGathering,
        Start,
        Cooking,
        End,
        Length
    }


    public class CookSystem : MonoBehaviour
    {
        [SerializeField] private UICook _uiCook;

        [Space]
        [Header("CookData")]
        [SerializeField] private CookUserData _userData;

        [SerializeField] private float _cookTime;
        public float CookTime => _cookTime;

        [Space]
        [Header("Images")]
        [SerializeField] private Sprite _cookwareOvenImage;

        [SerializeField] private Sprite _cookwarePanImage;

        [SerializeField] private Sprite _cookwarePotImage;


        public Inventory[] Inventory => GameManager.Instance.Player.GatheringItemInventory;

        private Dictionary<Tuple<string, string, int>, RecipeData> _recipeDataDic => DatabaseManager.Instance.RecipeDatabase.RecipeDataDic;

        private RecipeData _currentRecipeData;

        private Item _materialItem1;

        private Item _materialItem2;

        private Cookware _currentCookware;

        private CookStep _currentCookStep;

        private float _currentFireValue;
        public float CurrentFireValue => _currentFireValue;

        private int _currentStamina;
        public int CurrentStamina => _currentStamina;


        public event Action<float, float> OnFireValueChanged; //화력 게이지가 변경되면 실행할 대리자

        public event Action<float, float> OnStaminaValueChanged; //스테미너 게이지가 변경되면 실행할 대리자

        public event Action OnCookStarted; //요리가 시작되면 실행할 대리자


        private void Start()
        {
            Init();
            _uiCook.Init(this);
        }


        private void Init()
        {
            //만약 데이터를 네트워크에서 받아오지 못해 정보가 없을 경우 로컬에서 정보를 받아온다.
            //차후 삭제 예정
            if (_recipeDataDic.Count <= 0)
            {
                DatabaseManager.Instance.RecipeDatabase.LocalRecipeParse();
                Debug.Log("레시피 정보가 없어 로컬에서 정보를 받아옵니다...");
            }
        }


        /// <summary>요리 게임이 시작되면 쓰레기 아이템을 획득하고, 레시피 재료 아이템을 삭제시키는 함수</summary>
        //게임중 강제 종료로 게임의 내용을 없던일로 하는 것을 막기 위해 필요합니다.
        public bool StartCook()
        {
            if (_currentRecipeData == null)
            {
                return false;
            }

            //만약 재료 아이템 ID값이 있으면 아이템 삭제
            if (_materialItem1 != null)
            {
                GameManager.Instance.Player.RemoveItemById(_materialItem1.Id, 1);
            }

            if (_materialItem2 != null)
            {
                GameManager.Instance.Player.RemoveItemById(_materialItem2.Id, 1);
            }

            //쓰레기 아이템을 획득시킨다.
            GameManager.Instance.Player.AddItemById("CookFd53", 1, ItemAddEventType.None);

            OnCookStarted?.Invoke();

            return true;
        }


        public void AddFoodItem(string foodItemId, int count = 1)
        {
            GameManager.Instance.Player.AddItemById(foodItemId, count);
        }


        public void SetCurrentRecipe(InventoryItem item1,  InventoryItem item2)
        {
            _materialItem1 = item1;
            _materialItem2 = item2;
            _currentRecipeData = GetRecipeByItems(item1, item2);
        }


        public RecipeData GetCurrentRecipe()
        {
            return _currentRecipeData;
        }


        private RecipeData GetRecipeByItems(InventoryItem item1, InventoryItem item2)
        {
            // 아이템이 존재하지 않는 경우
            if (item1 == null && item2 == null)
            {
                Debug.Log("아이템이 존재하지 않습니다.");
                return null;
            }

            if (item1 == item2 && item1.Count < 2)
            {
                return null;
            }


            string item1ID = item1 != null ? item1.Id : "";
            string item2ID = item2 != null ? item2.Id : "";

            Tuple<string, string, int> tuple1 = Tuple.Create(item1ID, item2ID, (int)_currentCookware);
            Tuple<string, string, int> tuple2 = Tuple.Create(item2ID, item1ID, (int)_currentCookware);

            if (_recipeDataDic.TryGetValue(tuple1, out RecipeData recipe) || _recipeDataDic.TryGetValue(tuple2, out recipe))
            {
                return recipe;
            }

            //레시피가 존재하지 않을 경우엔 애매한 아이템을 준다.
            string foodId = "CookFd54";
            float successPos = 0.5f;
            float pos_S = 0.1f;
            float pos_A = 0.1f;
            float pos_B = 0.15f;
            recipe = new RecipeData(null, foodId, successPos, pos_S, pos_A, pos_B, -1);

            return recipe;
        }


        public string CheckItemGrade()
        {
            RecipeData data = _currentRecipeData;
            float fireValue = _currentFireValue * 0.01f;
            float rangeS = data.SuccessRangeLevel_S * 0.5f;
            float rangeA = rangeS + (data.SuccessRangeLevel_A * 0.5f);
            float rangeB = rangeA +  (data.SuccessRangeLevel_B * 0.5f);

            bool checkLevel_S = data.SuccessLocation - rangeS <= fireValue
                && data.SuccessLocation + rangeS >= fireValue;

            bool checkLevel_A = data.SuccessLocation - rangeA <= fireValue 
                && data.SuccessLocation + rangeA >= fireValue;

            bool checkLevel_B = data.SuccessLocation - rangeB <= fireValue
                && data.SuccessLocation + rangeB >= fireValue;

            if (checkLevel_S)
            {
                return "S";
            }
            else if (checkLevel_A)
            {
                return "A";
            }
            else if (checkLevel_B)
            {
                return "B";
            }
            else
            {
                return "F";
            }
        }


        public int ChangeCookware(int value)
        {
            int tmpInt = (int)_currentCookware + value;
            int cookwareLength = (int)Cookware.Length;
            tmpInt = tmpInt < 0
                ? tmpInt + cookwareLength
                : tmpInt % cookwareLength;

            tmpInt = Mathf.Clamp(tmpInt, 0, cookwareLength - 1);

            _currentCookware = (Cookware)tmpInt;

            return tmpInt;
        }


        public int ChangeCookStep(int value)
        {
            int tmpInt = (int)_currentCookStep + value;
            int cookStepLength = (int)CookStep.Length;
            tmpInt = tmpInt < 0
                ? tmpInt + cookStepLength
                : tmpInt % cookStepLength;

            tmpInt = Mathf.Clamp(tmpInt, 0, cookStepLength - 1);

            _currentCookStep = (CookStep)tmpInt;

            return tmpInt;
        }


        public int SetCookStep(CookStep cookStep)
        {
            _currentCookStep = cookStep;
            return (int)cookStep;
        }


        public Cookware GetCookware()
        {
            return _currentCookware;
        }


        public Sprite GetCookwareImage(Cookware cookware)
        {
            switch (cookware)
            {
                case Cookware.Oven:
                    return _cookwareOvenImage;
                case Cookware.Pan:
                    return _cookwarePanImage;
                case Cookware.Pot:
                    return _cookwarePotImage;
                default:
                    return null;
            }
        }


        public void ResetStatus()
        {
            _currentFireValue = 0;
            _currentStamina = _userData.MaxStamina;
        }


        public bool CheckDecreaseStaminaEnabled(CookValue cookValue)
        {
            int value = 0;
            switch (cookValue)
            {
                case CookValue.LargeValue:
                    value = _userData.LargeAddValueStamina;
                    break;

                case CookValue.NomalValue:
                    value = _userData.AddValueStamina;
                    break;

                case CookValue.SmallValue:
                    value = _userData.SmallAddValueStamina;
                    break;
            }

            if (_currentStamina < value)
                return false;

            return true;
        }


        public bool CheckAddFireValueEnabled()
        {
            if (_userData.MaxFireValue <= _currentFireValue)
                return false;

            return true;
        }


        public void DecreaseStamina(CookValue cookValue)
        {
            int value = 0;
            switch (cookValue)
            {
                case CookValue.LargeValue:
                    value = _userData.LargeAddValueStamina;
                    break;

                case CookValue.NomalValue:
                    value = _userData.AddValueStamina;
                    break;

                case CookValue.SmallValue:
                    value = _userData.SmallAddValueStamina;
                    break;
            }

            _currentStamina -= value;
            OnStaminaValueChanged?.Invoke(_userData.MaxStamina, _currentStamina);
        }


        public void AddFireValue(CookValue cookValue)
        {
            int value = 0;
            int randInt = 0;
            switch (cookValue)
            {
                case CookValue.LargeValue:
                    randInt = Random.Range(0, _userData.LargeAddValue.Length);
                    value = _userData.LargeAddValue[randInt];
                    break;

                case CookValue.NomalValue:
                    randInt = Random.Range(0, _userData.AddValue.Length);
                    value = _userData.AddValue[randInt];
                    break;

                case CookValue.SmallValue:
                    randInt = Random.Range(0, _userData.SmallAddValue.Length);
                    value = _userData.SmallAddValue[randInt];
                    break;
            }

            _currentFireValue += value;
            _currentFireValue = Mathf.Clamp(_currentFireValue, 0, _userData.MaxFireValue);

            OnFireValueChanged?.Invoke(_userData.MaxFireValue, _currentFireValue);

        }
    }
}


