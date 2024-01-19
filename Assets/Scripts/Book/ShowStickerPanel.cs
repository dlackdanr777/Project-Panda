using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStickerPanel : MonoBehaviour
{
    [SerializeField] private GameObject _stickerPanel;
    private Vector2 _tempPos;

    private void Awake()
    {
        _tempPos = _stickerPanel.GetComponent<RectTransform>().anchoredPosition;
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
}
