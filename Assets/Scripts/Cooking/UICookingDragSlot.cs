using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class UICookingDragSlot : MonoBehaviour
{
    public static UICookingDragSlot Instance;

    private InventoryItem _tempItem;

    private Image _image;


    private void Awake()
    {
        Instance = this;
        _image = GetComponent<Image>();
        gameObject.SetActive(false);
    }


    public void StartDrag(InventoryItem item)
    {
        gameObject.SetActive(true);
        _tempItem = item;
        _image.sprite = _tempItem.Image;
    }

    public void EndDrag()
    {
        _tempItem = null;
        gameObject.SetActive(false);
    }

    /// <summary> null�̸� true �ƴҰ�� false ��ȯ</summary>
    public InventoryItem GetItem()
    {
        return _tempItem;
    }
}
