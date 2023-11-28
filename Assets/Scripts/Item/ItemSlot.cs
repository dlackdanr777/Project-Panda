using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private Button _itemDropButton;
    [SerializeField] private Button _itemNoDropButton;

    //Test 
    public Sprite Image;


    private void Start()
    {
        _itemDropButton.onClick.AddListener(OnClickedItemDrop);
        _itemNoDropButton.onClick.AddListener(OnClickedNoItemDrop);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //Test
            eventData.pointerDrag.GetComponent<Image>().sprite = Image;

            Image image = eventData.pointerDrag.GetComponent<Image>();
            if (image != null)
            {
                GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeAlpha(1f);
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeAlpha(0f);
    }

    private void ChangeAlpha(float alpha)
    {
        Image image = GetComponent<Image>();
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

    private void OnClickedItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup »ç¶óÁü

    }
    private void OnClickedNoItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup »ç¶óÁü
        
    }

}
