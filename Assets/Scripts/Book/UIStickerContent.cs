using Muks.DataBind;
using System.Collections.Generic;
using TMPro;
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
        DataBind.SetUnityActionValue("StickerSaveButton", Save);
        _userSticker = GameManager.Instance.Player.StickerInventory;
        _clearButton.onClick.AddListener(OnClickClearButton);

        CreateSlots();

        List<ServerStickerData> stickerList = DatabaseManager.Instance.UserInfo.BookUserData.GetStickerList();

        for (int i = 0; i < stickerList.Count; i++)
        {
            GameObject sticker = Instantiate(_stickerContentPf.GetComponent<SpawnSticker>().StickerClone, _stickerZone.transform);
            sticker.transform.localPosition = stickerList[i].Pos;
            sticker.transform.localRotation = stickerList[i].Rot;
            sticker.transform.localScale = stickerList[i].Scale;
            sticker.GetComponent<Image>().sprite = GetStickerImage(stickerList[i].Id);
            sticker.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stickerList[i].Id;
            sticker.transform.GetChild(0).gameObject.SetActive(false); //tool ²ô±â
        }
    }


    private void OnEnable()
    {
        UpdateSlots();
    }


    public void Save()
    {
        List<ServerStickerData> stickerList = DatabaseManager.Instance.UserInfo.BookUserData.GetStickerList();

        stickerList.Clear();
        for (int i = 0, count = _stickerZone.transform.childCount; i < count; i++)
        {
            Transform child = _stickerZone.transform.GetChild(i);
            string id = child.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            Vector3 pos = child.localPosition;
            Quaternion rot = child.localRotation;
            Vector3 scale = child.localScale;

            ServerStickerData data = new ServerStickerData(id, pos, rot, scale);
            stickerList.Add(data);
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
            Transform child = _spawnPoint.GetChild(i);
            if (i < GameManager.Instance.Player.StickerInventory.Count)
            {
                child.GetChild(0).gameObject.SetActive(true);
                child.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Image;
                child.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.Player.StickerInventory.GetStickerList()[i].Id;
            }
            else
            {
                child.GetChild(0).gameObject.SetActive(false);
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
