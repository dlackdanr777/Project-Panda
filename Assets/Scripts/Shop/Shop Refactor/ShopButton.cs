using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public static Action ShopItemHandler;
    public static Action InventoryItemHandler;

    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private GameObject _checkView;
    [SerializeField] private GameObject _detailView;
    [SerializeField] private GameObject _popupView;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private bool _isBuy;
    
    private Button _shopButton;
    
    void Start()
    {
        _shopButton = GetComponent<Button>();
        _shopButton.onClick.AddListener(OnClickShopButton);
        _yesButton.onClick.AddListener(OnClickYesButton);
        _noButton.onClick.AddListener(OnClickNoButton);
    }

    private void OnClickShopButton()
    {
        _checkView.SetActive(true);
    }

    private void OnClickYesButton()
    {
        if (_isBuy)
        {
            if (GameManager.Instance.Player.SpendBamboo(int.Parse(_price.text)))
            {
                GameManager.Instance.Player.ToolItemInventory[0].AddById(InventoryItemField.Tool,
                    DataBind.GetTextValue("ShopBuyItemDetailID").Item, int.Parse(_count.text)); //아이템에 따라 달라짐(현재는 도구)
                _checkView.SetActive(false);
                _detailView.SetActive(false);
                ShopItemHandler?.Invoke(); //SoldOut
                InventoryItemHandler?.Invoke(); //인벤토리 초기화

                // 도전 과제 달성 체크
                DatabaseManager.Instance.Challenges.UsingShop(true);
            }
            else
            {
                StartCoroutine(BuyItemRoutine());
            }
           
        }
        else
        {
            if(RemoveItem(DataBind.GetTextValue("InventoryDetailID").Item, int.Parse(_count.text)))
            {
                GameManager.Instance.Player.GainBamboo(int.Parse(_price.text));
                _checkView.SetActive(false);
                _detailView.SetActive(false);
                InventoryItemHandler?.Invoke(); 

                // 도전 과제 달성 체크
                DatabaseManager.Instance.Challenges.UsingShop(false);
            }
            else
            {
                StartCoroutine(BuyItemRoutine());
            }
        }
    }

    private void OnClickNoButton()
    {
        _checkView.SetActive(false);
    }

    private IEnumerator BuyItemRoutine()
    {
        _popupView.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _popupView.SetActive(false);
        _checkView.SetActive(false);
    }

    private bool RemoveItem(string id, int count)
    {
        string code = id.Substring(0, 3);

        switch (code)
        {
            case "IBG": 
                return GameManager.Instance.Player.GatheringItemInventory[(int)GatheringItemType.Bug].RemoveById(id, count);
            case "IFI":
                return GameManager.Instance.Player.GatheringItemInventory[(int)GatheringItemType.Fish].RemoveById(id, count);
            case "IFR":
                Debug.Log(GameManager.Instance.Player.GatheringItemInventory[(int)GatheringItemType.Fruit].GetInventoryList().Count);
                return GameManager.Instance.Player.GatheringItemInventory[(int)GatheringItemType.Fruit].RemoveById(id, count);
                //요리 추가해야함

        }

        return false;
    }
}
