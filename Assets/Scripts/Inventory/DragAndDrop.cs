using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler //arrange button에 등록
{
    //이미지를 가져오기
    [SerializeField] private GameObject _selectedItem; //선택한 아이템
    [SerializeField] private GameObject _dropPopup;
    [SerializeField] private GameObject _phone;
    [SerializeField] private GameObject _itemDropZone;
    [SerializeField] private Button _itemDropButton;
    [SerializeField] private Button _itemNoDropButton;

    private float speed = 20f;
    private Transform _spawnPoint;

    public event Action OnUseItem;
    public event Action DontUseItem;

    private void Start()
    {
        _itemDropButton.onClick.AddListener(OnClickedItemDrop);
        _itemNoDropButton.onClick.AddListener(OnClickedNoItemDrop);
    }
    private void SpawnImage(bool isSpawn) //slot에 spawn하거나 제거 할 때
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
        _dropPopup.SetActive(false); //popup 사라짐
        _itemDropZone.gameObject.SetActive(false); //map item zone 사라짐
        MovePhone(1); //폰 올라감
        OnUseItem?.Invoke(); //item 사용 detailview 사라짐
        RemoveSelectedItem(); //selected image 사라짐
    }
    private void OnClickedNoItemDrop()
    {
        SpawnImage(false); //slot에 아이템 보여짐
        _dropPopup.SetActive(false);//popup 사라짐
        _itemDropZone.gameObject.SetActive(false); //map item zone 사라짐
        MovePhone(1); //폰 올라감
        DontUseItem?.Invoke(); //detailview 사라짐
        _selectedItem.SetActive(false);
    }

    private void MovePhone(int gap)
    {
        _phone.transform.GetChild(0).GetChild(0).gameObject.SetActive(!_phone.transform.GetChild(0).GetChild(0).gameObject.activeSelf); //폰 border button set active false
        Tween.Move(_phone, new Vector3(_phone.transform.position.x,  gap, _phone.transform.position.z), 1f, TweenMode.Smoothstep);
    }

    private void RemoveSelectedItem()//따라다니는 객체 삭제
    {
        _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
        _selectedItem.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData) //drag 시작하면 아이템 보여짐
    {
        Vector3 pos = eventData.position;
        pos.z = 10;
        _selectedItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
        _selectedItem.SetActive(true);

        MovePhone(-80);

        _itemDropZone.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData) //아이템이 해당 위치에 따라 움직임
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        _selectedItem.transform.position = Vector2.Lerp(_selectedItem.transform.position, mousePosition, Time.deltaTime * speed);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            _spawnPoint = eventData.pointerCurrentRaycast.gameObject.transform; //누른 지점
            if (_spawnPoint != null)
            {
                //할당된 아이템이 없다면
                if ( _spawnPoint.transform.GetChild(0).GetComponent<Image>().sprite == null)
                {
                    //위치에 놓으면 popup
                    _dropPopup.gameObject.SetActive(true);
                    SpawnImage(true); //해당 위치에 spawn
                }
                else //할당된 아이템이 있다면, map이 아닌 곳에 
                {
                    _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
                    MovePhone(1);
                    _itemDropZone.SetActive(false);
                }
            }
        }
        catch (Exception) //현재는 배경에 아무것도 없어서 예외 처리해놓음,, 나중에 map 깔리면 삭제
        {
            _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
            MovePhone(1);
            _itemDropZone.SetActive(false);
        }

        _selectedItem.SetActive(false); //따라다니는 아이템 보이지 않도록

        

    }
}
