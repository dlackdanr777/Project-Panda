using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotWithCollider : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _itemDropPopup;
    [SerializeField] private GameObject _itemPf;

    private int _currentItemIndex;
    private Image _selectImage;
    private Vector3 _worldPosition;


    //Test 
    public Sprite Image;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() != null) //드래그를 가지고 있는 객체만 drop
        {
            Debug.Log("drag를 가지고 있는 객체");
            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Room"))
            {
                Debug.Log("Room tag를 가지고 있는 객체");
                Instantiate(_itemPf, transform);
            }
            else
            {
                Debug.Log("놓을 수 없는 공간!");
            }

        }

    }
    private void ChangeAlpha(Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

    private int FindChildIndex()
    {
        //현재 내가 몇번째 자식인지 확인
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
            {
                return i;
            }
        }
        return -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentItemIndex = FindChildIndex();
        transform.parent.GetComponent<DropZone>().CurrentItemIndex = _currentItemIndex;
    }
}
