using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private GameObject _selectedItem;
    [SerializeField] private GameObject _spawnItem;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_selectedItem.GetComponent<SpriteRenderer>().sprite != null)//안에 이미지가 있으면
        {
            GameObject dropItem = Instantiate(_spawnItem, new Vector3(0, 0, 0), Quaternion.identity);
            dropItem.transform.SetParent(transform, false);//현재 위치의 자식으로 spawn
            dropItem.GetComponent<Image>().sprite = _selectedItem.GetComponent<SpriteRenderer>().sprite;

            _selectedItem.SetActive(false);
        }        
    }
}
