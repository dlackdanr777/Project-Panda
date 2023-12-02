using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<string> OnUseItem;
    public event Action<string> OnPutInItem;

    //Drop
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private Button _itemDropButton;
    [SerializeField] private Button _itemNoDropButton;

    //Scoop
    [SerializeField] private GameObject _itemScoopPopup;
    [SerializeField] private Button _itemScoopButton;
    [SerializeField] private Button _itemNoScoopButton;

    public int CurrentItemIndex;

    private void Start()
    {
        _itemDropButton.onClick.AddListener(OnClickedItemDrop);
        _itemNoDropButton.onClick.AddListener(OnClickedNoItemDrop);

        _itemScoopButton.onClick.AddListener(OnClickedItemScoop);
        _itemNoScoopButton.onClick.AddListener(OnClickedItemNoScoop);


        for(int i = 0; i < transform.childCount; i++)
        {
            int index = i;
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(()=>OnClickItemSlot(index));  
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Image item in transform.GetComponentsInChildren<Image>())
        {
            if (item.sprite == null)
            {
                ChangeAlpha(item, 0.6f);      
            }
        } 
    } 

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Image item in transform.GetComponentsInChildren<Image>())
        {
            if (item.sprite == null)
            {
                ChangeAlpha(item, 0f);
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

        string id = transform.GetChild(CurrentItemIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        OnUseItem?.Invoke(id);


    }
    private void OnClickedNoItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup 사라짐
        DeleteItem();

    }

    private void OnClickedItemScoop()
    {
        if (transform.GetChild(CurrentItemIndex).GetComponent<Image>() != null)
        {
            _itemScoopPopup.SetActive(false);

            string id = transform.GetChild(CurrentItemIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            OnPutInItem?.Invoke(id);

            DeleteItem();
        }

    }
    private void OnClickedItemNoScoop()
    {
        _itemScoopPopup.SetActive(false);

    }

    private void OnClickItemSlot(int index)
    {
        if(transform.GetChild(index).GetComponent<Image>() != null)
        {
            _itemScoopPopup.SetActive(true);

        }
    }


    private void DeleteItem()
    {
        //지우기
        transform.GetChild(CurrentItemIndex).GetComponent<Image>().sprite = null;
        ChangeAlpha(transform.GetChild(CurrentItemIndex).GetComponent<Image>(), 0f);

        transform.GetChild(CurrentItemIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text = null; //id

        //worldPosition의 object 지우기
        GameObject spawnItem = transform.GetChild(CurrentItemIndex).GetComponent<ItemSlot>()._dropItemSpawnPoint.GetChild(CurrentItemIndex).GetChild(0).gameObject;
        Destroy(spawnItem);
    }

}
