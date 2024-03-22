using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shop
{
    public class UIShopInventory : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIMainInventoryContoller _inventoryContoller;
        [SerializeField] private UIShopSellDetailView _sellDetailView;

        private void Awake()
        {
            _sellDetailView.Init();
            _sellDetailView.gameObject.SetActive(false);

            _inventoryContoller.Init(SlotButtonClicked);
        }


        private void SlotButtonClicked(InventoryItem item)
        {
            if (item == null)
                return;

            //판매가격이 0이거나 음수면 판매불가
            if (item.Price < 0)
                return;

            SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
            _sellDetailView.Show(item);
        }
    }
}

