using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UISellView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _completeImage;

        private InventoryItem _sellItem;
        private int _sellMoney; //판매 금액
        private int _sellCount;

        private CanvasGroup _canvasGroup;

        public static event Action OnSellCompleteHandler;


        public void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _exitButton.onClick.AddListener(Hide);
            _sellButton.onClick.AddListener(OnSellButtonClicked);

            _completeImage.SetActive(false);

        }


        public void Show(InventoryItem sellItem, int sellCount)
        {
            gameObject.SetActive(true);
            _completeImage.SetActive(false);
            _canvasGroup.blocksRaycasts = true;
            _sellItem = sellItem;
            _sellCount = sellCount;
            _sellMoney = sellItem.Price * sellCount;
        }


        private void Hide()
        {
            gameObject.SetActive(false);
        }


        private void OnSellButtonClicked()
        {
            GameManager.Instance.Player.RemoveItemById(_sellItem.Id, _sellCount);
            GameManager.Instance.Player.GainBamboo(_sellMoney);

            _canvasGroup.blocksRaycasts = false;

            _completeImage.SetActive(true);
            Tween.CanvasGroupAlpha(_completeImage, 1, 0.3f, TweenMode.Constant, () =>
            {
                //2초 대기 후 닫기
                Tween.TransformMove(_completeImage, _completeImage.transform.position, 2, TweenMode.Constant, () =>
                {
                    _completeImage.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    OnSellCompleteHandler?.Invoke();
                });
            });

        }

    }

}
