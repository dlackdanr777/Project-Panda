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
    [SerializeField] private Transform _dropItemSpawnPoint;

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

                ChangeAlpha(GetComponent<Image>(), 1f);

                Vector3 targetPosition = transform.position;
                
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(targetPosition);

                GameObject spawnItem = Instantiate(_itemPf, _dropItemSpawnPoint, true);
                spawnItem.transform.position = worldPosition;
                

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
