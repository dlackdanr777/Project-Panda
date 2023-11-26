using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PurchaseButton : MonoBehaviour
{
    public ShopManager ShopManager;
    public SellButton SellButton;
    public GameObject PurchaseItemPanel;

    public GameObject SellButtonPrefab;
    public GameObject Content;

    public GameObject SellItemPanel;
    public Image SellItemIcon;
    public TextMeshProUGUI SellItemName;
    public TextMeshProUGUI SellItemDescription;    

    public GameObject ItemPanel;

    public SellItemButtonBehaviour itemButton;

    public List<SellItemButtonBehaviour> itemButtons;

    public void PurchaseItem()
    {       
        ShopItemDatabase _purchasedItem = ShopManager.SelectedItemDatabase;

        for(int i = 0; i < ShopManager.ItemsArray.Length; i++)
        {
            if(_purchasedItem.Id == ShopManager.ItemsArray[i].Id)
            {
                if (ShopManager.ItemsArray[i].Amount == 0)
                {                   
                    PurchaseItemPanel.SetActive(false);

                    GameObject button = Instantiate(SellButtonPrefab);
                    button.transform.SetParent(Content.transform);
                    button.transform.localScale = Vector3.one;
                    itemButtons.Add(button.GetComponent<SellItemButtonBehaviour>());
                    button.GetComponent<Button>().onClick.AddListener(OnClick);

                    itemButton = button.GetComponent<SellItemButtonBehaviour>();
                    
                    ShopManager.ItemsArray[i].Amount++;
                    itemButton.ItemAmount.text = ShopManager.ItemsArray[i].Amount.ToString(); 

                    itemButton.ItemPanel = SellItemPanel;

                    SellItemIcon.sprite = ShopManager.ItemsArray[i].Icon;
                    itemButton.SelectedItemIconImage = SellItemIcon;

                    SellItemName.text = ShopManager.ItemsArray[i].Name;
                    itemButton.SelectedItemName = SellItemName;

                    SellItemDescription.text = ShopManager.ItemsArray[i].Description;
                    itemButton.SelectedItemDescription = SellItemDescription;

                    itemButton.ItemDatabase = ShopManager.ItemsArray[i];

                    itemButton.ShopManager = ShopManager;
                    itemButton.SellButton = SellButton;                    
                }
                else
                {
                    Debug.Log(ShopManager.ItemsArray[i].Id);                    

                    for(int j = 0; j < itemButtons.Count; j++)
                    {
                        if (ShopManager.ItemsArray[i].Id == itemButtons[j].ItemDatabase.Id)
                        {
                            ShopManager.ItemsArray[i].Amount++;
                            itemButtons[j].ItemAmount.text = ShopManager.ItemsArray[i].Amount.ToString();
                            break;
                        }
                    }
                    
                    PurchaseItemPanel.SetActive(false);
                }
                break;
            }            
        }        
    }

    public void OnClick()
    {
        ItemPanel.SetActive(true);
    }
}
