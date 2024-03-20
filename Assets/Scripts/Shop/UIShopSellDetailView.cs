using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shop
{
    /// <summary>상점 아이템 판매 UI 스크립트</summary>
    public class UIShopSellDetailView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UISellView _sellView;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _itemCount;
        [SerializeField] private Button _leftCountButton;
        [SerializeField] private Button _rightCountButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _sellButton;


        private InventoryItem _currentItem;
        private int _currentItemCount;
        private int _maxItemCount;


        public void Init(UnityAction onButtonClicked = null)
        {
            _sellView.Init();
            LoadingSceneManager.OnLoadSceneHandler += OnSceneChanged;
            UISellView.OnSellCompleteHandler += Hide;

            _sellView.gameObject.SetActive(false);

            _sellButton.onClick.AddListener(OnSellButtonClicked);
            _leftCountButton.onClick.AddListener(OnLeftCountButtonClicked);
            _rightCountButton.onClick.AddListener(OnRightCountButtonClicked);

            if (onButtonClicked != null)
                _exitButton.onClick.AddListener(onButtonClicked);

            _exitButton.onClick.AddListener(() => gameObject.SetActive(false));
        }


        public void Show(InventoryItem showItem)
        {
            gameObject.SetActive(true);

            _currentItem = showItem;

            _nameText.text = showItem.Name;
            _descriptionText.text = showItem.Description;
            _priceText.text = showItem.Price.ToString();
            _itemImage.sprite = showItem.Image;

            _maxItemCount = showItem.Count;
            _currentItemCount = 1;
            _itemCount.text = _currentItemCount.ToString();
        }


        private void Hide()
        {
            gameObject.SetActive(false);
        }


        private void OnLeftCountButtonClicked()
        {
            _currentItemCount = Mathf.Clamp(_currentItemCount - 1, 1, _maxItemCount);
            _itemCount.text = _currentItemCount.ToString();
        }


        private void OnRightCountButtonClicked()
        {
            _currentItemCount = Mathf.Clamp(_currentItemCount + 1, 1, _maxItemCount);
            _itemCount.text = _currentItemCount.ToString();
        }


        private void OnSellButtonClicked()
        {
            _sellView.Show(_currentItem, _currentItemCount);
        }


        private void OnSceneChanged()
        {
            UISellView.OnSellCompleteHandler -= Hide;
            LoadingSceneManager.OnLoadSceneHandler -= OnSceneChanged;
        }

    }
}
