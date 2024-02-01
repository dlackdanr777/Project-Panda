using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopContent : MonoBehaviour
{
    [SerializeField] private string _mapId;
    [SerializeField] private GameObject _shopSlotPf;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _detailView;
    [SerializeField] private Button _closeButton;

    private List<ToolItem> _shopItem; //아직 상점 데이터베이스가 없어서 툴로 사용
    private int _currentIndex;


    private void Awake()
    {
        ShopButton.ShopItemHandler += UpdateSoldOut;
        DataBind.SetButtonValue("ShopExitButton", OnExitButtonClicked);
    }


    void Start()
    {
        _shopItem = DatabaseManager.Instance.GetGatheringToolItemList();
        _closeButton.onClick.AddListener(OnClickCloseButton);

        CreateSlots();
    }


    private void CreateSlots()
    {
        for(int i = 0; i < _shopItem.Count; i++)
        {
            if (_shopItem[i].Map.Equals(_mapId))
            {
                int index = i;
                GameObject slot = Instantiate(_shopSlotPf, _spawnPoint);
                slot.GetComponent<Button>().onClick.AddListener(()=>OnClickDetailView(index));
                slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _shopItem[i].Image; //이미지
                slot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _shopItem[i].Price.ToString(); //가격
            }
        }
    }


    private void OnClickDetailView(int index)
    {   
        GetContent(index);
        _detailView.SetActive(true);
    }


    private void OnClickCloseButton()
    {
        _detailView.SetActive(false);
        ClearContent();
    }


    private void ClearContent()
    {
        DataBind.SetTextValue("ShopBuyItemDetailName", "");
        DataBind.SetTextValue("ShopBuyItemDetailDescription", "");
        DataBind.SetTextValue("ShopBuyItemDetailPrice", "");
        DataBind.SetSpriteValue("ShopBuyItemDetailImage", null);
    }


    private void GetContent(int index)
    {
        _currentIndex = index;

        DataBind.SetTextValue("ShopBuyItemDetailName", _shopItem[index].Name);
        DataBind.SetTextValue("ShopBuyItemDetailDescription", _shopItem[index].Description);
        DataBind.SetSpriteValue("ShopBuyItemDetailImage", _shopItem[index].Image);
        DataBind.SetTextValue("ShopBuyItemDetailPrice", _shopItem[index].Price.ToString());
    }


    private void UpdateSoldOut()
    {
        //soldout
        _spawnPoint.GetChild(_currentIndex).GetComponent<Button>().interactable = false;
        _spawnPoint.GetChild(_currentIndex).GetChild(2).gameObject.SetActive(true);
    }


    private void OnExitButtonClicked()
    {
        LoadingSceneManager.LoadScene("24_01_09_Integrated");
    }

}
