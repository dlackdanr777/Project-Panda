using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStickerPanel : MonoBehaviour
{
    [SerializeField] private GameObject _stickerPanel;
    [SerializeField] private GameObject _dontTouchArea;

    private void OnEnable()
    {
        Anime1_Show();
    }

    private void OnDisable()
    {
        Hide();
    }

    private void Anime1_Show()
    {
        _stickerPanel.SetActive(true);
        _dontTouchArea.SetActive(true);
        Tween.RectTransfromAnchoredPosition(_stickerPanel, new Vector2(0, -545), 1.2f, TweenMode.EaseInOutBack, () =>
        {
            _dontTouchArea.SetActive(false);
        });
    }

    private void Hide()
    { 
        _stickerPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1043);
        _stickerPanel.SetActive(false);
    }
}
