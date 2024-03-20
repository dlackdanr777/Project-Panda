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

            _sellDetailView.Show(item);
        }
    }
}

