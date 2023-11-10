using Muks.DataBind;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _selectedItem;
    [SerializeField] private GameObject _dropPopup;
    //[SerializeField] private ClickPhone _clickPhone;

    private Transform _spawnPoint;
    public event Action OnUseItem;

    private void Start()
    {
        DataBind.SetButtonValue("ItemDropPopupCloseBtn", OnClickedNoItemDrop);
        DataBind.SetButtonValue("ItemDropBtn", OnClickedItemDrop);

        //_clickPhone.OnRemoveSelectedItem += _clickPhone_OnRemoveSelectedItem;
    }

    private void _clickPhone_OnRemoveSelectedItem()
    {
        RemoveSelectedItem();
    }

    private void SpawnImage(bool isSpawn) //spawn하거나 제거 할 때
    {
        Image spawnImage = _spawnPoint.transform.GetChild(0).GetComponent<Image>();
        Color tempColor = spawnImage.color;

        if (isSpawn)
        {
            spawnImage.gameObject.transform.position = _spawnPoint.position;
            //image setactive
            tempColor.a = 255f;
            spawnImage.color = tempColor;

            _spawnPoint.transform.GetChild(0).GetComponent<Image>().sprite = _selectedItem.GetComponent<SpriteRenderer>().sprite;

        }
        else
        {
            tempColor.a = 0f;
            spawnImage.color = tempColor;

            spawnImage.sprite = null;
        }
    }

    private void OnClickedItemDrop()
    {
        _dropPopup.SetActive(false);
        RemoveSelectedItem();
        OnUseItem?.Invoke();
        
    }
    private void OnClickedNoItemDrop()
    {
        SpawnImage(false);
        _dropPopup.SetActive(false);
        _selectedItem.SetActive(true);
    }

    public void RemoveSelectedItem()//따라다니는 객체 삭제
    {
        _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
        _selectedItem.SetActive(false);
    }

    //slot을 눌렀을 때
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        _spawnPoint = eventData.pointerCurrentRaycast.gameObject.transform; //누른 지점
        if(_spawnPoint != null)
        {
            //안에 이미지가 있으면(선택된 아이템이 있다면), 할당된 아이템이 없다면
            if (_selectedItem.GetComponent<SpriteRenderer>().sprite != null && _spawnPoint.transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                //위치에 놓으면 popup
                _dropPopup.gameObject.SetActive(true);
                SpawnImage(true); //해당 위치에 spawn
                _selectedItem.SetActive(false); //따라다니는 아이템 보이지 않도록
            }

        }
    }
}
