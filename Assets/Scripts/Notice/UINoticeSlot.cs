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
    [SerializeField] private Image _image;
    [SerializeField] private Button _descriptionButton;
    private Notice _notice;

    
    public void Init(Notice notice, UnityAction onButtonClicked = null)
    {
        _notice = notice;
        _titleText.text = notice.Title;
        _descriptionButton.onClick.AddListener(onButtonClicked);
        transform.localScale = Vector3.one;

        SetSprite();
    }


    public void SetSprite()
    {
        if (_notice.Sprite != null)
        {
            _image.gameObject.SetActive(true);
            _image.sprite = _notice.Sprite;

            //그림 비율에 맞게 _image의 비율을 조정한다. 
            float heightMul = (float)_notice.Sprite.textureRect.height / (float)_notice.Sprite.textureRect.width;
            Vector2 sizeDelta = _image.rectTransform.sizeDelta;
            _image.rectTransform.sizeDelta = 1 < heightMul
                ? new Vector2(sizeDelta.x, sizeDelta.y * heightMul)
                : new Vector2(sizeDelta.x / heightMul, sizeDelta.y);
        }

        else
        {
            _image.gameObject.SetActive(false);
        }
    }

}
