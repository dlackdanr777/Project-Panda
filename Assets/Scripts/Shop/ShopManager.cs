using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject SelectedItemPanel;
    public GameObject PurchasedPanel;
    public GameObject SoldPanel;

    public ShopItemDatabase SelectedItemDatabase;
    //public List<ItemDatabase> PurchasedItems;

    public ShopItemDatabase[] ItemsArray = new ShopItemDatabase[6];

    public void PurchasedButton()
    {
        StartCoroutine(ItemPurchased());
    }

    public void SoldButton()
    {
        StartCoroutine(ItemSold());
    }

    IEnumerator ItemPurchased()
    {
        SelectedItemPanel.SetActive(false);
        PurchasedPanel.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        PurchasedPanel.SetActive(false);
    }

    IEnumerator ItemSold()
    {
        SelectedItemPanel.SetActive(false);
        SoldPanel.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        SoldPanel.SetActive(false);
    }
}
