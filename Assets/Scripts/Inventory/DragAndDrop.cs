using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler //arrange button�� ���
{
    //�̹����� ��������
    [SerializeField] private GameObject _selectedItem; //������ ������
    [SerializeField] private GameObject _dropPopup;
    [SerializeField] private GameObject _phone;
    [SerializeField] private GameObject _itemDropZone;

    private float speed = 20f;
    private Transform _spawnPoint;
    public event Action OnUseItem;
    public event Action DontUseItem;

    //���۵Ǿ��� �� �� => pointerEnter
    //drag
    //������ �� �����

    private void Start()
    {
        DataBind.SetButtonValue("ItemDropPopupCloseBtn", OnClickedNoItemDrop);
        DataBind.SetButtonValue("ItemDropBtn", OnClickedItemDrop);
    }
    private void SpawnImage(bool isSpawn) //spawn�ϰų� ���� �� ��
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
        StartCoroutine(MovePhone(80));//�� ������
        RemoveSelectedItem(); //selected image �����
        _dropPopup.SetActive(false); //popup �����
        OnUseItem?.Invoke(); //item ���
        _itemDropZone.gameObject.SetActive(false);
    }
    private void OnClickedNoItemDrop()
    {
        StartCoroutine(MovePhone(80)); //�� ������
        SpawnImage(false); //
        _dropPopup.SetActive(false);//popup �����
        _selectedItem.SetActive(false);
        DontUseItem?.Invoke(); //detailview �����
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

    public void RemoveSelectedItem()//����ٴϴ� ��ü ����
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

        _spawnPoint = eventData.pointerCurrentRaycast.gameObject.transform; //���� ����
        if (_spawnPoint != null)
        {
            //�ȿ� �̹����� ������(���õ� �������� �ִٸ�), �Ҵ�� �������� ���ٸ�
            if (_selectedItem.GetComponent<SpriteRenderer>().sprite != null && _spawnPoint.transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                //��ġ�� ������ popup
                _dropPopup.gameObject.SetActive(true);
                SpawnImage(true); //�ش� ��ġ�� spawn
                _selectedItem.SetActive(false); //����ٴϴ� ������ ������ �ʵ���
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
