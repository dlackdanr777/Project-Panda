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
                    GetIdByName(DataBind.GetTextValue("ShopBuyItemDetailName").Item), int.Parse(_count.text)); //아이템에 따라 달라짐(현재는 도구)
                ShopItemHandler?.Invoke();
                InventoryItemHandler?.Invoke();
                _checkView.SetActive(false);
                _detailView.SetActive(false);

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
            if(GameManager.Instance.Player.ToolItemInventory[0].RemoveById(GetIdByName(DataBind.GetTextValue("ShopBuyItemDetailName").Item), int.Parse(_count.text)))
            {
                GameManager.Instance.Player.GainBamboo(int.Parse(_price.text));
                InventoryItemHandler?.Invoke();
                _checkView.SetActive(false);
                _detailView.SetActive(false);

                // 도전 과제 달성 체크
                DatabaseManager.Instance.Challenges.UsingShop(false);
            }
            else
            {
                Debug.Log(GameManager.Instance.Player.ToolItemInventory[0].ItemsCount);
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

    private string GetIdByName(string name)
    {
        for(int i=0;i<DatabaseManager.Instance.GetGatheringToolItemList().Count;i++) //shop database에서 아이디 찾기
        {
            if (DatabaseManager.Instance.GetGatheringToolItemList()[i].Name.Equals(name))
            {
                return DatabaseManager.Instance.GetGatheringToolItemList()[i].Id;
            }
        }
        return null;
    }
}
