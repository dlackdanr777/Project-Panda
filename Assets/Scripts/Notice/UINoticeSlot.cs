using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UINoticeSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Button _descriptionButton;
    private Notice _notice;

    
    public void Init(Notice notice, UnityAction onButtonClicked = null)
    {
        _notice = notice;
        _titleText.text = notice.Title;
        _descriptionButton.onClick.AddListener(onButtonClicked);

        transform.localScale = Vector3.one;
    }

}
