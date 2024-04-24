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
        [SerializeField] private GameObject _maxBambooImage;

        private InventoryItem _sellItem;
        private int _sellMoney; //판매 금액
        private int _sellCount;

        private CanvasGroup _canvasGroup;
        private CanvasGroup _completeImageCanvasGroup;
        private CanvasGroup _maxBambooImageCanvasGroup;

        public static event Action OnSellCompleteHandler;


        public void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _completeImageCanvasGroup = _completeImage.GetComponent<CanvasGroup>();
            _maxBambooImageCanvasGroup = _maxBambooImage.GetComponent<CanvasGroup>();

            _exitButton.onClick.AddListener(Hide);
            _sellButton.onClick.AddListener(OnSellButtonClicked);

            _completeImage.SetActive(false);
            _maxBambooImage.SetActive(false);

        }


        public void Show(InventoryItem sellItem, int sellCount)
        {
            gameObject.SetActive(true);
            _completeImage.SetActive(false);
            _canvasGroup.blocksRaycasts = true;
            _sellItem = sellItem;
            _sellCount = sellCount;
            _sellMoney = (int)(sellItem.Price * 0.8f) * sellCount;
        }


        private void Hide()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
            gameObject.SetActive(false);
        }


        private void OnSellButtonClicked()
        {
            _sellButton.interactable = false;
            _exitButton.interactable = false;
            if (GameManager.Instance.Player.GainBamboo(_sellMoney))
            {
                SoundManager.Instance.PlayEffectAudio(SoundEffectType.Sell);
                GameManager.Instance.Player.RemoveItemById(_sellItem.Id, _sellCount);
                _canvasGroup.blocksRaycasts = false;

                _completeImage.SetActive(true);
                _completeImageCanvasGroup.alpha = 0.1f;
                Tween.CanvasGroupAlpha(_completeImage, 1, 0.1f, TweenMode.Constant, () =>
                {
                    //1초 대기 후 닫기
                    Tween.TransformMove(_completeImage, _completeImage.transform.position, 1, TweenMode.Constant, () =>
                    {
                        _sellButton.interactable = true;
                        _exitButton.interactable = true;
                        _completeImage.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        OnSellCompleteHandler?.Invoke();
                    });
                });
            }

            else
            {
                _canvasGroup.blocksRaycasts = false;

                _maxBambooImage.SetActive(true);
                _maxBambooImageCanvasGroup.alpha = 0.1f;
                Tween.CanvasGroupAlpha(_maxBambooImage, 1, 0.1f, TweenMode.Constant, () =>
                {
                    //1초 대기 후 닫기
                    Tween.TransformMove(_maxBambooImage, _maxBambooImage.transform.position, 1, TweenMode.Constant, () =>
                    {
                        _sellButton.interactable = true;
                        _exitButton.interactable = true;
                        _maxBambooImage.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        OnSellCompleteHandler?.Invoke();
                    });
                });
            }
            

        }

    }

}
