using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private GameObject _itemPf;

    private int _currentItemIndex;
    private Image _selectImage;
    private Button _itemSlotButton;


    //Test 
    public Sprite Image;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() != null) //�巡�׸� ������ �ִ� ��ü�� drop
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //Test
            eventData.pointerDrag.GetComponent<Image>().sprite = Image;


            //�̹����� ���� -> 2d ������Ʈ�� ����
            //ī�޶� ��ġ �����ؼ� �ش� ��ġ�� ����

            if (GetComponent<Image>().sprite == null)
            {
                _itemDropPopup.SetActive(true); //popup ���

                _selectImage = eventData.pointerDrag.GetComponent<Image>();
                

                _itemPf.GetComponent<SpriteRenderer>().sprite = _selectImage.sprite; //�̹���
                
                _selectImage.sprite = null;

                _currentItemIndex = transform.parent.GetComponent<DropZone>().CurrentItemIndex;

                //_itemPf.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.pointerDrag.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text; //id

                ChangeAlpha(GetComponent<Image>(), 1f);

                //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(transform.position.x, transform.position.y, 10));

                //Canva canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
                //Vector3 itemScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(_itemPf.transform.position.x, _itemPf.transform.position.y, 10));

                //itemScreenPos = transform.position;
                //_itemPf.transform.position = itemScreenPos;

                //Vector3 targetPosition = GetComponent<RectTransform>().position;
                //Debug.Log("targetPostion" + targetPosition);
                //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(targetPosition);
                //Debug.Log("wPostion" + worldPosition);

                //Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera.main,  )

                GameObject spawnItem = Instantiate(_itemPf);
                spawnItem.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                

            }


        }
    }
    private void ChangeAlpha(Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }
}
