using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>UI 메일 슬롯 관리 클래스</summary>
public class UIMailSlot : MonoBehaviour
{
    [SerializeField] private Image _mailImage;

    [SerializeField] private Image _checkImage;

    [SerializeField] private Button _button;

    [SerializeField] private TextMeshProUGUI _npcNameText;


    public void Init(UnityAction onButtonClicked)
    {
        _checkImage.gameObject.SetActive(false);
        _button.onClick.AddListener(onButtonClicked);
    }


    public void SetMailImage(Sprite sprite)
    {
        _mailImage.sprite = sprite;
    }


    public void SetActiveCheckImage(bool value)
    {
        _checkImage.gameObject.SetActive(value);
    }


    public void SetNpcNameText(string npcName)
    {
        _npcNameText.text = npcName;
    }
}
