using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler //arrange button에 등록
{
    //이미지를 가져오기
    [SerializeField] private GameObject _selectedItem; //선택한 아이템
    [SerializeField] private GameObject _dropPopup;
    [SerializeField] private GameObject _phone;
    [SerializeField] private GameObject _itemDropZone;

    private float speed = 20f;
    private Transform _spawnPoint;
    public event Action OnUseItem;
    public event Action DontUseItem;

    //시작되었을 때 뜸 => pointerEnter
    //drag
    //끝났을 때 사라짐

    private void Start()
    {
        DataBind.SetButtonValue("ItemDropPopupCloseBtn", OnClickedNoItemDrop);
        DataBind.SetButtonValue("ItemDropBtn", OnClickedItemDrop);
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
        StartCoroutine(MovePhone(80));//폰 내려감
        RemoveSelectedItem(); //selected image 사라짐
        _dropPopup.SetActive(false); //popup 사라짐
        OnUseItem?.Invoke(); //item 사용
        _itemDropZone.gameObject.SetActive(false);
    }
    private void OnClickedNoItemDrop()
    {
        StartCoroutine(MovePhone(80)); //폰 내려감
        SpawnImage(false); //
        _dropPopup.SetActive(false);//popup 사라짐
        _selectedItem.SetActive(false);
        DontUseItem?.Invoke(); //detailview 사라짐
        _itemDropZone.gameObject.SetActive(false);
    }

    private IEnumerator MovePhone(int gap)
    {
        Tween.Move(_phone, new Vector3(_phone.transform.position.x, _phone.transform.position.y + gap, _phone.transform.position.z), 1f, TweenMode.Smoothstep);
        if (gap < 0)
        {   
            yield return new WaitForSeconds(1f);
            _phone.transform.GetChild(0).GetChild(0).gameObject.SetActive(false); //border button set active false

        }
        else
        {

            _phone.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); //border button set active true
        }
        yield return null;
    }

    public void RemoveSelectedItem()//따라다니는 객체 삭제
    {
        _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
        _selectedItem.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = 10;
        _selectedItem.transform.position = Camera.main.ScreenToWorldPoint(pos);
        _selectedItem.SetActive(true);

        StartCoroutine(MovePhone(-80));

        _itemDropZone.gameObject.SetActive(true);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _selectedItem.SetActive(false);

        _spawnPoint = eventData.pointerCurrentRaycast.gameObject.transform; //누른 지점
        if (_spawnPoint != null)
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

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        //Tween
        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        _selectedItem.transform.position = Vector2.Lerp(_selectedItem.transform.position, mousePosition, Time.deltaTime * speed);
    }
}
