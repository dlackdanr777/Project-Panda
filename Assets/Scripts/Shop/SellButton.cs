using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour
{
    public ShopManager ShopManager;
    public ShopItemDatabase ItemDatabase;

    public GameObject SellItemPanel;

    public SellItemButtonBehaviour SellItemButtonBehaviour;
    public GameObject SellItemButton;
    public PurchaseButton _purchaseButton;    

    public void SellItem()
    {
        for(int i = 0; i < ShopManager.ItemsArray.Length; i++)
        {            
            if (ShopManager.ItemsArray[i].Id == ItemDatabase.Id)
            {
                SellItemPanel.SetActive(false);
                ShopManager.ItemsArray[i].Amount--;                

                if (ShopManager.ItemsArray[i].Amount != 0)
                {
                    for (int j = 0; j < _purchaseButton.itemButtons.Count; j++)
                    {
                        if (ShopManager.ItemsArray[i].Id == _purchaseButton.itemButtons[j].ItemDatabase.Id)
                        {                            
                            _purchaseButton.itemButtons[j].ItemAmount.text = ShopManager.ItemsArray[i].Amount.ToString();
                            break;
                        }
                    }                    
                }
                else
                {
                    ShopManager.ItemsArray[i].Amount = 0;

                    for(int j = 0; j < _purchaseButton.itemButtons.Count; j++)
                    {
                        if (_purchaseButton.itemButtons[j].ItemDatabase.Id == ShopManager.ItemsArray[i].Id)
                        {
                            _purchaseButton.itemButtons.RemoveAt(j);
                            break;
                        }
                    }
                    
                    Destroy(SellItemButton);
                }
            }       
        }
        //Destroy(SellItemButton);
        //ShopManager.PurchasedItems.Remove(ItemDatabase);
        //SellItemPanel.SetActive(false);
        //ItemDatabase = null;
        //SellItemButton = null;

        //PurchaseButton.ListCount--;
    }
}
