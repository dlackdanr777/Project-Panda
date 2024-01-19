using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStickerContent : MonoBehaviour
{
    [SerializeField] private GameObject _stickerZone;
    [SerializeField] private Button _clearButton;
    [SerializeField] private GameObject _stickerContentPf;
    [SerializeField] private Transform _spawnPoint;

    private StickerList _userSticker;

    private void Awake()
    {
        _userSticker = GameManager.Instance.Player.StickerInventory;
        CreateSlots();
        _clearButton.onClick.AddListener(OnClickClearButton);
    }

    private void OnEnable()
    {
        UpdateSlots();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < _userSticker.MaxStickerCount; i++)
        {
            GameObject slot = Instantiate(_stickerContentPf, _spawnPoint);
            slot.GetComponent<SpawnSticker>().StickerZone = _stickerZone.transform;
        }
        
    }

    private void UpdateSlots()
    {
        Debug.Log(GameManager.Instance.Player.StickerInventory.Count);
        for(int i = 0; i < GameManager.Instance.Player.StickerInventory.MaxStickerCount; i++)
        {
            if(i < GameManager.Instance.Player.StickerInventory.Count)
            {
                _spawnPoint.GetChild(i).GetChild(0).gameObject.SetActive(true);
                _spawnPoint.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Image;
            }
            else
            {
                _spawnPoint.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void OnClickClearButton()
    {
        foreach (GameObject sticker in _stickerZone.GetComponentsInChildren<GameObject>())
        {
            Destroy(sticker);
        }
    }
}
