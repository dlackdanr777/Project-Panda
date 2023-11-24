using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemButtonBehaviour : MonoBehaviour
{
    public GameObject ItemPanel;
    public Image SelectedItemIconImage;
    public TextMeshProUGUI SelectedItemName;
    public TextMeshProUGUI SelectedItemDescription;

    public ShopItemDatabase ItemDatabase;

    public ShopManager ShopManager;
    public SellButton SellButton;

    public TextMeshProUGUI ItemAmount;    

    void Start()
    {
        GetComponent<Image>().sprite = ItemDatabase.Icon;
        GetComponent<Button>().onClick.AddListener(OnSelectSellItem);                
    }    

    public void OnSelectSellItem()
    {
        ItemPanel.SetActive(true);
        SelectedItemIconImage.sprite = ItemDatabase.Icon;

        SelectedItemName.text = ItemDatabase.Name;
        SelectedItemDescription.text = ItemDatabase.Description;

        SellButton.ItemDatabase = ItemDatabase;
        
        SellButton.SellItemButton = gameObject;        
    }
}
