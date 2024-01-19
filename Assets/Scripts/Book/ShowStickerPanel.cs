using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStickerPanel : MonoBehaviour
{
    [SerializeField] private GameObject _stickerPanel;
    [SerializeField] private GameObject _stickerZone;
    [SerializeField] private Button _clearButton;

    private Vector2 _tempPos;

    private void Awake()
    {
        _tempPos = _stickerPanel.GetComponent<RectTransform>().anchoredPosition;
        _clearButton.onClick.AddListener(OnClickClearButton);
    }

    private void OnEnable()
    {
        Anime1_Show();
        Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        Hide();
        Debug.Log("OnDisable");
    }

    private void Anime1_Show()
    {
        _stickerPanel.SetActive(true);
        Tween.RectTransfromAnchoredPosition(_stickerPanel, new Vector2(0, -545), 1f, TweenMode.EaseInOutBack);

    }

    private void Hide()
    {
        _stickerPanel.GetComponent<RectTransform>().anchoredPosition = _tempPos;
        _stickerPanel.SetActive(false);
    }

    private void OnClickClearButton()
    {
        foreach(GameObject sticker in _stickerZone.GetComponentsInChildren<GameObject>())
        {
            Destroy(sticker);
        }
    }
}
