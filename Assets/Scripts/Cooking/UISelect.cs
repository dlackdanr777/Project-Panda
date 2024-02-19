using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UISelect : MonoBehaviour
    {
        [Header("Step 1")]
        [SerializeField] private UISelectSlot _cookwareSlot;

        [Space]
        [Header("Step 2")]
        [SerializeField] private UIMaterialItemSlot[] _materialItemSlots;

        private UICook _uiCook;

        private CookSystem _cookSystem => _uiCook.CookSystem;

        private RecipeData _currentRecipeData;


        public void Init(UICook uiCook)
        {
            _uiCook = uiCook;

            _cookwareSlot.Init(() => ChangeCookware(-1), () => ChangeCookware(1));
            ChangeCookware(0);

            for(int i = 0, count = _materialItemSlots.Length; i < count; i++)
            {
                _materialItemSlots[i].Init(this);
            }

            StartStep1();
        }

        
        public void StartStep1()
        {
            _cookwareSlot.gameObject.SetActive(true);
            StopStep2();
        }


        public void StopStep1()
        {
            _cookwareSlot.gameObject.SetActive(false);
        }


        public void StartStep2()
        {
            StopStep1();
            for (int i = 0, count = _materialItemSlots.Length; i < count; i++)
            {
                _materialItemSlots[i].gameObject.SetActive(true);
            }
        }


        public void StopStep2()
        {
            for (int i = 0, count = _materialItemSlots.Length; i < count; i++)
            {
                _materialItemSlots[i].gameObject.SetActive(false);
            }
        }


        private void ChangeCookware(int dir)
        {
            int currentCookware = _cookSystem.ChangeCookware(dir);
            Sprite sprite = _cookSystem.GetCookwareImage((Cookware)currentCookware);
            _cookwareSlot.SetSlotImage(sprite);
        }


        public bool CheckMaterialItemSlot()
        {
            if(_materialItemSlots[0].CurrentItem != null || _materialItemSlots[1].CurrentItem != null)
            {
                return true;
            }

            return false;
        }
    }
}

