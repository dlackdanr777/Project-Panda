using Muks.DataBind;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _selectedItem;
    [SerializeField] private GameObject _spawnItem;
    [SerializeField] private GameObject _dropPopup;

    private Transform _spawnPoint;
    private bool _isDropPossible = false;
    public event Action OnUseItem;
    public event Action OnMoveItem;

    private void Start()
    {
        DataBind.SetButtonValue("ItemDropPopupCloseBtn", OnClickedClosePopup);
        DataBind.SetButtonValue("ItemDropBtn", OnClickedItemDrop);
        
    }

    private void RemoveSelectedItem()
    {
        _selectedItem.GetComponent<SpriteRenderer>().sprite = null;
        _selectedItem.SetActive(false);
    }

    private void OnClickedItemDrop()
    {
        _dropPopup.gameObject.SetActive(false);
        if (_isDropPossible)
        {
            //����
            //GameObject dropItem = Instantiate(_spawnItem, new Vector3(0, 0, 0), Quaternion.identity);
            //dropItem.transform.SetParent(_spawnPoint, false);//spawn
            //dropItem.GetComponent<Image>().sprite = _selectedItem.GetComponent<SpriteRenderer>().sprite;

            OnUseItem?.Invoke();
        }
        else
        {
            OnMoveItem?.Invoke();
        }
    }
    private void OnClickedClosePopup()
    {
        _dropPopup.gameObject.SetActive(false);
        RemoveSelectedItem();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _spawnPoint = eventData.pointerCurrentRaycast.gameObject.transform;

        if(_spawnPoint != null)
        {
            GameObject dropItem = Instantiate(_spawnItem, new Vector3(0, 0, 0), Quaternion.identity);
            dropItem.transform.SetParent(_spawnPoint, false);//spawn
            dropItem.GetComponent<Image>().sprite = _selectedItem.GetComponent<SpriteRenderer>().sprite;
            RemoveSelectedItem();

            //��ġ�� ������ popup
            _dropPopup.gameObject.SetActive(true);

            //�ȿ� �̹����� ������(���õ� �������� �ִٸ�), �Ҵ�� �������� ���ٸ�
            if (_selectedItem.GetComponent<SpriteRenderer>().sprite != null && _spawnPoint.childCount == 0)
            {
                _isDropPossible = true;
            }
            else
            {
                _isDropPossible = false;
            }

        }
    }
}
