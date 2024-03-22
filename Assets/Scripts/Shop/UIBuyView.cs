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
    public class UIBuyView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private GameObject _completeImage;
        [SerializeField] private GameObject _notBuyImage;

        private Item _buyItem;
        private int _buyMoney; //판매 금액
        private int _buyCount;

        private CanvasGroup _canvasGroup;
        private CanvasGroup _completeImageCanvasGroup;
        private CanvasGroup _notBuyImageCanvasGroup;

        public static event Action OnBuyCompleteHandler;


        public void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _completeImageCanvasGroup = _completeImage.GetComponent<CanvasGroup>();
            _notBuyImageCanvasGroup = _notBuyImage.GetComponent<CanvasGroup>();

            _exitButton.onClick.AddListener(Hide);
            _buyButton.onClick.AddListener(OnBuyButtonClicked);

            _completeImage.SetActive(false);
            _notBuyImage.SetActive(false);

        }


        public void Show(Item buylItem, int sellCount)
        {
            gameObject.SetActive(true);
            _completeImage.SetActive(false);
            _notBuyImage.SetActive(false);
            _canvasGroup.blocksRaycasts = true;
            _buyItem = buylItem;
            _buyCount = sellCount;
            _buyMoney = buylItem.Price * sellCount;
        }


        private void Hide()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
            gameObject.SetActive(false);
        }


        private void OnBuyButtonClicked()
        {
            //소지 금액이 구매 금액보다 작을 경우
            if (!GameManager.Instance.Player.SpendBamboo(_buyMoney))
            {
                _canvasGroup.blocksRaycasts = false;

                _notBuyImage.SetActive(true);
                _notBuyImageCanvasGroup.alpha = 0.1f;
                Tween.CanvasGroupAlpha(_notBuyImage, 1, 0.1f, TweenMode.Constant, () =>
                {
                    //2초 대기 후 닫기
                    Tween.TransformMove(_notBuyImage, _notBuyImage.transform.position, 2, TweenMode.Constant, () =>
                    {
                        _notBuyImage.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        OnBuyCompleteHandler?.Invoke();
                    });
                });

                return;
            }

            GameManager.Instance.Player.AddItemById(_buyItem.Id, _buyCount);

            DatabaseManager.Instance.Challenges.UsingShop(false);
            _canvasGroup.blocksRaycasts = false;

            _completeImage.SetActive(true);
            _completeImageCanvasGroup.alpha = 0.1f;
            Tween.CanvasGroupAlpha(_completeImage, 1, 0.1f, TweenMode.Constant, () =>
            {
                //2초 대기 후 닫기
                Tween.TransformMove(_completeImage, _completeImage.transform.position, 2, TweenMode.Constant, () =>
                {
                    _completeImage.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    OnBuyCompleteHandler?.Invoke();
                });
            });

        }

    }

}
