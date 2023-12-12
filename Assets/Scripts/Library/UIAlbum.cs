using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIAlbum : MonoBehaviour
{
    [Tooltip("���Ե��� �θ� ������Ʈ�� �ִ� ��")]
    [SerializeField] private GameObject _layoutGroup;

    [Tooltip("���� �������� �ִ� ��")]
    [SerializeField] private GameObject _slotPrefab;

    public int _slotCount => _slot.Count;

    private List<UIAlbumSlot> _slot;

    private RectTransform _thisRectTransform;

    private void OnEnable()
    {
        UpdateUI();
    }


    public void Init()
    {
        _slot = new List<UIAlbumSlot>();
        _thisRectTransform = GetComponent<RectTransform>();
        ScreenshotCamera.OnStartHandler += ResizeImage;
        UpdateUI();
    }


    private void UpdateUI()
    {
        int count = Database.Instance.Photos.Count - 1;

        for (int i = 0; i < count; i++)
        {
            _slot[i].Init(i, Database.Instance.Photos.GetData(i));
        }
    }


    public void CreateSlot()
    {
        UIAlbumSlot uiAlbumSlot = Instantiate(_slotPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<UIAlbumSlot>();
        uiAlbumSlot.transform.parent = _layoutGroup.transform;
        uiAlbumSlot.transform.localScale = Vector3.one;

        int index = Database.Instance.Photos.Count - 1;
        uiAlbumSlot.Init(index, Database.Instance.Photos.GetData(index));
        _slot.Add(uiAlbumSlot);
    }


    public void ResizeImage(int width, int height)
    {
        float heightRatio = (float)height / width;

        float imageWidth = _thisRectTransform.rect.width;

        _thisRectTransform.sizeDelta = new Vector2(imageWidth, imageWidth * heightRatio);
    }


}
