using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public event Action OnUseItem;
    public event Action OnPutInItem;

    //Drop
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private Button _itemDropButton;
    [SerializeField] private Button _itemNoDropButton;

    //Scoop
    [SerializeField] private GameObject _itemScoopPopup;
    [SerializeField] private Button _itemScoopButton;
    [SerializeField] private Button _itemNoScoopButton;

    private Image _selectImage;
    private Button _itemSlotButton;

    //Test 
    public Sprite Image;


    private void Start()
    {
        _itemSlotButton = GetComponent<Button>();

        _itemSlotButton.onClick.AddListener(OnClickedItemSlot);

        _itemDropButton.onClick.AddListener(OnClickedItemDrop);
        _itemNoDropButton.onClick.AddListener(OnClickedNoItemDrop);

        _itemScoopButton.onClick.AddListener(OnClickedItemScoop);
        _itemNoScoopButton.onClick.AddListener(OnClickedItemNoScoop);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() != null) //드래그를 가지고 있는 객체만 drop
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //Test
            eventData.pointerDrag.GetComponent<Image>().sprite = Image;



            if (GetComponent<Image>().sprite == null)
            {
                _itemDropPopup.SetActive(true); //popup 띄움

                _selectImage = eventData.pointerDrag.GetComponent<Image>();
                
                GetComponent<Image>().sprite = _selectImage.sprite;
                _selectImage.sprite = null;
                ChangeAlpha(GetComponent<Image>(), 1f);
            }


        }
    }
    private void ChangeAlpha(Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

    private void OnClickedItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup 사라짐
         OnUseItem?.Invoke();
        

    }
    private void OnClickedNoItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup 사라짐
        DeleteItemSprite();

    }
    private void OnClickedItemSlot()
    {
        _itemScoopPopup.SetActive(true);
    }

    private void OnClickedItemScoop()
    {
        if (GetComponent<Image>() != null)
        {
            _itemScoopPopup.SetActive(false);

            OnPutInItem?.Invoke();

            DeleteItemSprite();
        }

    }
    private void OnClickedItemNoScoop()
    {
        _itemScoopPopup.SetActive(false);

    }

    private void DeleteItemSprite()
    {
        //지우기
        GetComponent<Image>().sprite = null;
        ChangeAlpha(GetComponent<Image>(), 0f);
    }
}
