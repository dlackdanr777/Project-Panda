using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIShopSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _soldOutImage;
    [SerializeField] private TextMeshProUGUI _priceText;


    public void Init()
    {
        _soldOutImage.gameObject.SetActive(false);
    }

    
    public void AddOnButtonClicked(UnityAction onButtonClicked)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(onButtonClicked);
    }


    public void SetItemImage(Sprite sprite)
    {
        _itemImage.sprite = sprite;
    }


    public void SetPriceText(string text)
    {
        _priceText.text = text;
    }


    public void SoldOut()
    {
        _button.interactable = false;
        _soldOutImage.gameObject.SetActive(true);
    }

    public void NotSoldOut()
    {
        _button.interactable = true;
        _soldOutImage.gameObject.SetActive(false);
    }

}
