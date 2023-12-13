using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIAlbum : MonoBehaviour
{
    [Tooltip("슬롯들의 부모 오브젝트를 넣는 곳")]
    [SerializeField] private GameObject _layoutGroup;

    [Tooltip("슬롯 프리팹을 넣는 곳")]
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
        LoadPhotoData();
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


    public void CreateSlot(PhotoData photoData)
    {
        UIAlbumSlot uiAlbumSlot = Instantiate(_slotPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<UIAlbumSlot>();
        uiAlbumSlot.transform.parent = _layoutGroup.transform;
        uiAlbumSlot.transform.localScale = Vector3.one;

        int index = _slot.Count;
        uiAlbumSlot.Init(index, photoData);
        _slot.Add(uiAlbumSlot);
    }

    public void LoadPhotoData()
    {
        List<PhotoData> photoDatas = DatabaseManager.Instance.PhotoDatabase.GetPhotoDataList();

        foreach(PhotoData photoData in photoDatas)
        {
            CreateSlot(photoData);
        }
    }


    public void ResizeImage(int width, int height)
    {
        float heightRatio = (float)height / width;

        float imageWidth = _thisRectTransform.rect.width;

        _thisRectTransform.sizeDelta = new Vector2(imageWidth, imageWidth * heightRatio);
    }


}
