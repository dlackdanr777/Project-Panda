using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButtonBehaviour : MonoBehaviour
{
    public GameObject ItemPanel;
    public Image SelectedItemIconImage;
    public TextMeshProUGUI SelectedItemName;
    public TextMeshProUGUI SelectedItemDescription;

    public ShopItemDatabase ItemDatabase;    

    public ShopManager ShopManager;

    public TextMeshProUGUI ItemAmount;   

    void Start()
    {        
        GetComponent<Image>().sprite = ItemDatabase.Icon;
    }   

    public void SelectItem()
    {
        ItemPanel.SetActive(true);
        SelectedItemIconImage.sprite = ItemDatabase.Icon;

        SelectedItemName.text = ItemDatabase.Name;
        SelectedItemDescription.text = ItemDatabase.Description;

        ShopManager.SelectedItemDatabase = ItemDatabase;
    }
}
