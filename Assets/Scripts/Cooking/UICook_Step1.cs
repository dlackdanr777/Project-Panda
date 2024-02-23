using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook_Step1 : UICookStep
    {
        [SerializeField] private UISelectSlot _cookwareSlot;

        [SerializeField] private UISelectSlot _uiLeftMaterialSlotInfo;

        [SerializeField] private UISelectSlot _uiRightMaterialSlotInfo;


        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);
            _cookwareSlot.Init(() => ChangeCookware(-1), () => ChangeCookware(1));
            ChangeCookware(0);

            _uiLeftMaterialSlotInfo.Init(null, OnLeftButtonClicked);
            _uiRightMaterialSlotInfo.Init(OnRightButtonClicked, null);

            _uiRightMaterialSlotInfo.gameObject.SetActive(false);
        }


        public override void StartStep()
        {
            gameObject.SetActive(true);
        }


        public override void StopStep()
        {
            gameObject.SetActive(false);
        }


        public override bool CheckNextStep()
        {
            return true;
        }


        private void ChangeCookware(int dir)
        {
            int currentCookware = _cookSystem.ChangeCookware(dir);
            Sprite sprite = _cookSystem.GetCookwareImage((Cookware)currentCookware);
            _cookwareSlot.SetSlotImage(sprite);
        }


        private void  OnLeftButtonClicked()
        {
            _uiCook.AddMaterialCount(1);
            _uiLeftMaterialSlotInfo.DisableRightButton();
            _uiRightMaterialSlotInfo.gameObject.SetActive(true);
        }


        private void OnRightButtonClicked()
        {
            _uiCook.AddMaterialCount(-1);
            _uiLeftMaterialSlotInfo.EnableRightButton();
            _uiRightMaterialSlotInfo.gameObject.SetActive(false);
        }
    }
}

