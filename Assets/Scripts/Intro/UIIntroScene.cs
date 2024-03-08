using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIIntroScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueContext;
    [SerializeField] private TextMeshProUGUI _dialogueNameText;
    [SerializeField] private Image _pandaImage;


    private CanvasGroup _canvasGroup;

    public void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _dialogueContext.text = string.Empty;
        _dialogueNameText.text = string.Empty;
        _pandaImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }


    public void StartDialogue(Action onCompleted = null)
    {
        gameObject.SetActive(true);
        _pandaImage.gameObject.SetActive(false);
        _dialogueContext.text = string.Empty;
        _dialogueNameText.text= string.Empty;
        Tween.CanvasGroupAlpha(gameObject, 1, 0.3f, TweenMode.Constant, onCompleted);
    }


    public void EndDialogue(Action onCompleted = null)
    {
        Tween.CanvasGroupAlpha(gameObject, 0, 0.3f, TweenMode.Constant, onCompleted);
    }


    public void SetDialogueContext(string text, float fontSize = 40)
    {
        _dialogueContext.fontSize = fontSize;
        _dialogueContext.text = text;
    }


    public void SetDialogueNameText(string name)
    {
        _dialogueNameText.text = name;
    }

    public void SetDialogueImage(Sprite sprite)
    {
        _pandaImage.gameObject.SetActive(true);
        _pandaImage.sprite = sprite;
    }

}
