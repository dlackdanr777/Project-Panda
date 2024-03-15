using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public class UICook_Step1 : UICookStep
    {
        [Header("Components")]
        [SerializeField] private UISelectSlot _cookwareSlot;
        [SerializeField] private UISelectSlot _uiLeftMaterialSlotInfo;
        [SerializeField] private UISelectSlot _uiRightMaterialSlotInfo;
        [SerializeField] private Button _exitButton;

        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);
            _cookwareSlot.Init(
                () => 
                {
                    SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
                    ChangeCookware(-1);
                }, 
                
                () =>
                {
                    SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
                    ChangeCookware(1);
                });

            ChangeCookware(0);

            _exitButton.onClick.AddListener(OnExitButtonClicked);
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


        private void ChangeCookware(int dir)
        {
            int currentCookware = _cookSystem.ChangeCookware(dir);
            Sprite sprite = _cookSystem.GetCookwareImage((Cookware)currentCookware);
            _cookwareSlot.SetSlotImage(sprite);
        }


        private void  OnLeftButtonClicked()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

            _uiCook.AddMaterialCount(1);
            _uiLeftMaterialSlotInfo.DisableRightButton();
            _uiRightMaterialSlotInfo.gameObject.SetActive(true);
        }


        private void OnRightButtonClicked()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

            _uiCook.AddMaterialCount(-1);
            _uiLeftMaterialSlotInfo.EnableRightButton();
            _uiRightMaterialSlotInfo.gameObject.SetActive(false);
        }


        private void OnExitButtonClicked()
        {
            SoundManager.Instance.StopBackgroundAudio(1);
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
            LoadingSceneManager.LoadScene("24_01_09_Integrated");
        }
    }
}

