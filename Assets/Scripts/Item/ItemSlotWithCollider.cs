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
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<DragDrop>() != null) //�巡�׸� ������ �ִ� ��ü�� drop
        {
            Debug.Log("drag�� ������ �ִ� ��ü");
            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Room"))
            {
                Debug.Log("Room tag�� ������ �ִ� ��ü");
                Instantiate(_itemPf, transform);
            }
            else
            {
                Debug.Log("���� �� ���� ����!");
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
        //���� ���� ���° �ڽ����� Ȯ��
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
