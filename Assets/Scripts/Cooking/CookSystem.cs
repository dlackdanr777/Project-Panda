using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        Length
    }



    [Serializable]
    public class CookingUserData
    {
        [SerializeField] private int _maxStamina;
        public int MaxStamina => _maxStamina;

        [Space]
        [SerializeField] private int[] _moreAddValue;
        [SerializeField] private int _moreAddValueStamina;

        [Space]
        [SerializeField] private int[] _addValue;
        [SerializeField] private int _addValueStamina;

        [Space]
        [SerializeField] private int[] _smallAddValue;
        [SerializeField] private int _smallAddValueStamina;

        public int[] MoreAddValue => _moreAddValue;
        public int MoreAddValueStamina => _moreAddValueStamina;
        public int[] AddValue => _addValue;
        public int AddValueStamina => _addValueStamina;
        public int[] SmallAddValue => _smallAddValue;
        public int SmallAddValueStamina => _smallAddValueStamina;
    }


    public class CookSystem : MonoBehaviour
    {
        [SerializeField] private UICook _uiCook;

        [Space]

        [SerializeField] private CookingUserData _userData;
        public CookingUserData UserData => _userData;

        [Space]

        [SerializeField] private Sprite _cookwareOvenImage;

        [SerializeField] private Sprite _cookwarePanImage;

        [SerializeField] private Sprite _cookwarePotImage;


        public Inventory[] Inventory => GameManager.Instance.Player.GatheringItemInventory;

        private Dictionary<Tuple<string, string>, RecipeData> _recipeDataDic => DatabaseManager.Instance.RecipeDatabase.RecipeDataDic;

        private RecipeData _currentRecipeData;

        private Cookware _currentCookware;

        private CookStep _currentCookStep;


        private void Start()
        {
            //Init();
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


        public RecipeData GetkRecipeByItems(InventoryItem item1, InventoryItem item2)
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

            if (GetCookwareSlotNum() == 2 && (item1 == null || item2 == null))
            {
                Debug.Log("2구 짜리에선 1구 요리 불가능");
                return null;
            }

            string item1ID = item1 != null ? item1.Id : "";
            string item2ID = item2 != null ? item2.Id : "";

            Tuple<string, string> tuple1 = Tuple.Create(item1ID, item2ID);
            Tuple<string, string> tuple2 = Tuple.Create(item2ID, item1ID);


            if (_recipeDataDic.TryGetValue(tuple1, out RecipeData recipe) || _recipeDataDic.TryGetValue(tuple2, out recipe))
            {
                return recipe;
            }

            Debug.Log("아무것도 없었다..");
            return null;
        }


        public string CheckItemGrade(RecipeData data, float fireValue)
        {
            bool checkLevel_S = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_S) <= fireValue &&
                data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_S) >= fireValue;

            bool checkLevel_A = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_A) <= fireValue &&
                data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_A) >= fireValue;

            bool checkLevel_B = data.SuccessLocation - (data.SuccessLocation * data.SuccessRangeLevel_B) <= fireValue &&
        data.SuccessLocation + (data.SuccessLocation * data.SuccessRangeLevel_B) >= fireValue;

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


        public int GetCookwareSlotNum()
        {
            return (int)_currentCookware + 1;
        }
    }
}


