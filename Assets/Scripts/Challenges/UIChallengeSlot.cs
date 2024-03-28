using Muks.BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>도전과제 슬롯 클래스</summary>
public class UIChallengeSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Image _clearImage;
    [SerializeField] private Sprite _doneSprite;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _buttonClickSound;

    private ChallengesData _data;
    private Color _clearBackgroundColor;
    private RectTransform _rectTransform;
    public Transform GetBambooTransform => _clearButton.transform;


    public void Init(ChallengesData data, Action<string> onButtonClicked)
    {
        _rectTransform = GetComponent<RectTransform>();
        _clearBackgroundColor = new Color(0.4f, 0.4f, 0.4f, 1);

        _data = data;
        _nameText.text = data.Name;
        _descriptionText.text = data.Description;
        _clearButton.onClick.AddListener(() =>
        {
            onButtonClicked?.Invoke(_data.Id);
            Clear();
            SoundManager.Instance.PlayEffectAudio(_buttonClickSound);
        });

        _clearButton.gameObject.SetActive(false);
        _clearImage.gameObject.SetActive(false);
    }


    public void Done(bool isServerUploaded = true)
    {
        _clearButton.gameObject.SetActive(true);
        _backgroundImage.sprite = _doneSprite;

        _rectTransform.SetAsFirstSibling();

        if (isServerUploaded)
            DatabaseManager.Instance.UserInfo.SaveChallengesData(10);
    }


    public void Clear(bool isServerUploaded = true)
    {
        _clearButton.gameObject.SetActive(true);
        _clearButton.interactable = false;

        _clearImage.gameObject.SetActive(true);

        _backgroundImage.sprite = _doneSprite;
        _backgroundImage.color = _clearBackgroundColor;

        if (isServerUploaded)
            DatabaseManager.Instance.UserInfo.SaveChallengesData(10);
    }

    public void CloseSlot()
    {
        Clear(false);
        _rectTransform.SetAsLastSibling();
    }
}
