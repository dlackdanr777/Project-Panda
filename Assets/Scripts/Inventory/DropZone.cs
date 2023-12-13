using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    public static Action<string> OnUseItem = delegate { };
    public static Action<string> OnPutInItem = delegate { };

    //Drop
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private Button _itemDropButton;
    [SerializeField] private Button _itemNoDropButton;

    //Scoop
    [SerializeField] private GameObject _itemScoopPopup;
    [SerializeField] private Button _itemScoopButton;
    [SerializeField] private Button _itemNoScoopButton;

    private GameObject _currentItem;
    private string _id;

    private void Awake()
    {
        DragDrop.OnDropEvent += HandleDropPopup;
        ScoopItem.OnScoop += HandleScoopItem;

        _itemDropButton.onClick.AddListener(OnClickedItemDrop);
        _itemNoDropButton.onClick.AddListener(OnClickedNoItemDrop);

        _itemScoopButton.onClick.AddListener(OnClickedItemScoop);
        _itemNoScoopButton.onClick.AddListener(OnClickedItemNoScoop);
    }
    private void OnDestroy()
    {
        DragDrop.OnDropEvent -= HandleDropPopup;
        ScoopItem.OnScoop -= HandleScoopItem;
    }

    private void HandleDropPopup(GameObject currentItem)
    {
        _itemDropPopup.SetActive(true);
        _currentItem = currentItem;
        _id = _currentItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
    private void HandleScoopItem(GameObject currentItem)
    {
        if (currentItem != null)
        {
            _currentItem = currentItem;
            _id = currentItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            _itemScoopPopup.SetActive(true);

        }
    }

    private void OnClickedItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup »ç¶óÁü

        OnUseItem?.Invoke(_id);


    }
    private void OnClickedNoItemDrop()
    {
        _itemDropPopup.SetActive(false); //popup »ç¶óÁü
        Destroy(_currentItem);

    }

    private void OnClickedItemScoop()
    {

        _itemScoopPopup.SetActive(false);

        OnPutInItem?.Invoke(_id);

        Destroy(_currentItem);

    }
    private void OnClickedItemNoScoop()
    {
        _itemScoopPopup.SetActive(false);

    }

    private void OnClickItemSlot(int index)
    {
        if (transform.GetChild(index).GetComponent<Image>() != null)
        {
            _itemScoopPopup.SetActive(true);

        }
    }
}
