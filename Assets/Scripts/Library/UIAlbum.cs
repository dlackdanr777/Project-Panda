using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlbum : MonoBehaviour
{
    [Tooltip(" 저장소 스크립트를 넣는 곳")]
    [SerializeField] private Library _library;

    [Tooltip("슬롯들의 부모 오브젝트를 넣는 곳")]
    [SerializeField] private GameObject _layoutGroup;

    [Tooltip("슬롯 프리팹을 넣는 곳")]
    [SerializeField] private GameObject _slotPrefab;

    public int _slotCount => _slot.Count;

    private List<UIAlbumSlot> _slot;
     
    private void Awake()
    {
        Init();
        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }


    private void Init()
    {
        _slot = new List<UIAlbumSlot>();
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
}
