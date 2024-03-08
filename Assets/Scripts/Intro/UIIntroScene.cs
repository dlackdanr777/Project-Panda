using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIIntroScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;


    private CanvasGroup _canvasGroup;

    public void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _dialogueText.text = string.Empty;
        gameObject.SetActive(false);
    }


    public void StartDialogue(Action onCompleted)
    {
        gameObject.SetActive(true);
        Tween.CanvasGroupAlpha(gameObject, 1, 0.5f, TweenMode.Constant, onCompleted);
    }


    public void SetDialogueText(string text)
    {
        _dialogueText.text = text;
    }
}
