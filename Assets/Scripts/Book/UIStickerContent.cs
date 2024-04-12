using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.DataBind;

public class UIStickerContent : MonoBehaviour
{
    [SerializeField] private GameObject _stickerZone;
    [SerializeField] private Button _clearButton;
    [SerializeField] private GameObject _stickerContentPf;
    [SerializeField] private Transform _spawnPoint;

    private StickerList _userSticker;

    private void Awake()
    {
        DataBind.SetUnityActionValue("StickerSaveButton", Save);
        _userSticker = GameManager.Instance.Player.StickerInventory;
        _clearButton.onClick.AddListener(OnClickClearButton);

        CreateSlots();

        List<StickerData> stickerList = DatabaseManager.Instance.UserInfo.GetStickerList();

        for (int i = 0; i < stickerList.Count; i++)
        {
            GameObject sticker = Instantiate(_stickerContentPf.GetComponent<SpawnSticker>().StickerClone, _stickerZone.transform);
            sticker.transform.localPosition = stickerList[i].Pos;
            sticker.transform.localRotation = stickerList[i].Rot;
            sticker.transform.localScale = stickerList[i].Scale;
            sticker.GetComponent<Image>().sprite = GetStickerImage(stickerList[i].Id);
            sticker.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stickerList[i].Id;
            sticker.transform.GetChild(0).gameObject.SetActive(false); //tool ����
        }
    }


    private void OnEnable()
    {
        UpdateSlots();
    }


    public void Save()
    {
        List<StickerData> stickerList = DatabaseManager.Instance.UserInfo.GetStickerList();
        stickerList.Clear();
        for (int i = 0; i < _stickerZone.transform.childCount; i++)
        {
            stickerList.Add(new StickerData(
                _stickerZone.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text,
                _stickerZone.transform.GetChild(i).localPosition,
                _stickerZone.transform.GetChild(i).localRotation,
                _stickerZone.transform.GetChild(i).localScale));
        }
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
        for(int i = 0; i < GameManager.Instance.Player.StickerInventory.MaxStickerCount; i++)
        {
            if(i < GameManager.Instance.Player.StickerInventory.Count)
            {
                _spawnPoint.GetChild(i).GetChild(0).gameObject.SetActive(true);
                _spawnPoint.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Image;
                _spawnPoint.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id;
            }
            else
            {
                _spawnPoint.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void OnClickClearButton()
    {
        foreach(DragSticker sticker in _stickerZone.transform.GetComponentsInChildren<DragSticker>())
        {
            Destroy(sticker.gameObject);
        }
    }

    private Sprite GetStickerImage(string id)
    {
        for(int i=0;i< GameManager.Instance.Player.StickerInventory.Count; i++)
        {
            if (GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id.Equals(id))
            {
                return GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Image;
            }
        }
        return null;
    }
}
