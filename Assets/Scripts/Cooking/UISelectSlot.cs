using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cooking
{
    public class UISelectSlot : MonoBehaviour
    {
        [SerializeField] private Image _slotImage;

        [SerializeField] private Button _leftButton;

        [SerializeField] private Button _rightButton;


        public void Init(UnityAction leftButtonClicked, UnityAction rightButtonClicked)
        {
            _leftButton.onClick.AddListener(leftButtonClicked);
            _rightButton.onClick.AddListener(rightButtonClicked);
        }


        public void SetSlotImage(Sprite sprite)
        {
            _slotImage.sprite = sprite;
        }


        public void EnableLeftButton()
        {
            _leftButton.gameObject.SetActive(true);
        }


        public void EnableRightButton()
        {
            _rightButton.gameObject.SetActive(true);
        }


        public void DisableRightButton()
        {
            _rightButton.gameObject.SetActive(false);
        }


        public void DisableLeftButton()
        {
            _leftButton.gameObject.SetActive(false);
        }


        public void rightButtonDisabled()
        {
            _rightButton.gameObject.SetActive(false);
        }
    }
}


