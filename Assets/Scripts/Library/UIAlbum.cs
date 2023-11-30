using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlbum : MonoBehaviour
{
    [Tooltip(" ����� ��ũ��Ʈ�� �ִ� ��")]
    [SerializeField] private Library _library;

    [Tooltip("���Ե��� �θ� ������Ʈ�� �ִ� ��")]
    [SerializeField] private GameObject _layoutGroup;

    [Tooltip("���� �������� �ִ� ��")]
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
