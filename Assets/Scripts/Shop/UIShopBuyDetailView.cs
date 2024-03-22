using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shop
{
    /// <summary>상점 아이템 구매 UI 스크립트</summary>
    public class UIShopBuyDetailView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIBuyView _buyView;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _itemCount;
        [SerializeField] private Button _leftCountButton;
        [SerializeField] private Button _rightCountButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _buyButton;


        private Item _currentItem;
        private int _currentItemCount;
        private int _maxItemCount;


        public void Init(UnityAction onButtonClicked = null)
        {
            _buyView.Init();
            LoadingSceneManager.OnLoadSceneHandler += OnSceneChanged;
            UIBuyView.OnBuyCompleteHandler += Hide;

            _buyView.gameObject.SetActive(false);

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
            _leftCountButton.onClick.AddListener(OnLeftCountButtonClicked);
            _rightCountButton.onClick.AddListener(OnRightCountButtonClicked);

            if (onButtonClicked != null)
                _exitButton.onClick.AddListener(onButtonClicked);

            _exitButton.onClick.AddListener(() => gameObject.SetActive(false));
        }


        public void Show(Item showItem)
        {
            gameObject.SetActive(true);

            _currentItem = showItem;

            _nameText.text = showItem.Name;
            _descriptionText.text = showItem.Description;
            _priceText.text = showItem.Price.ToString();
            _itemImage.sprite = showItem.Image;

            _maxItemCount = 10;
            _currentItemCount = 1;
            _itemCount.text = _currentItemCount.ToString();
        }


        private void Hide()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
            gameObject.SetActive(false);
        }


        private void OnLeftCountButtonClicked()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

            //만약 도구 아이템이면 한개만 살 수 있다.
            string itemType = _currentItem.Id.Substring(0, 3);
            if (itemType == "ITG")
            {
                _currentItemCount = 1;
                _itemCount.text = _currentItemCount.ToString();
                return;
            }

            _currentItemCount = Mathf.Clamp(_currentItemCount - 1, 1, _maxItemCount);
            _itemCount.text = _currentItemCount.ToString();
            _priceText.text = (_currentItem.Price * _currentItemCount).ToString();
        }


        private void OnRightCountButtonClicked()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);

            //만약 도구 아이템이면 한개만 살 수 있다.
            string itemType = _currentItem.Id.Substring(0, 3);
            if(itemType == "ITG")
            {
                _currentItemCount = 1;
                _itemCount.text = _currentItemCount.ToString();
                return;
            }

            _currentItemCount = Mathf.Clamp(_currentItemCount + 1, 1, _maxItemCount);
            _itemCount.text = _currentItemCount.ToString();
            _priceText.text = (_currentItem.Price * _currentItemCount).ToString();
        }


        private void OnBuyButtonClicked()
        {
            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
            _buyView.Show(_currentItem, _currentItemCount);
        }


        private void OnSceneChanged()
        {
            UIBuyView.OnBuyCompleteHandler -= Hide;
            LoadingSceneManager.OnLoadSceneHandler -= OnSceneChanged;
        }

    }
}
