using Muks.Tween;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIIntroScene : MonoBehaviour
{
    [SerializeField] private RectTransform _introDialogue;
    [SerializeField] private TextMeshProUGUI _dialogueContext;
    [SerializeField] private TextMeshProUGUI _dialogueNameText;
    [SerializeField] private Image _pandaImage;

    private CanvasGroup _canvasGroup;

    public void Init()
    {
        _canvasGroup = _introDialogue.GetComponent<CanvasGroup>();
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
        Tween.CanvasGroupAlpha(_canvasGroup.gameObject, 1, 0.3f, TweenMode.Constant, onCompleted);
    }


    public void EndDialogue(Action onCompleted = null)
    {
        Tween.CanvasGroupAlpha(_canvasGroup.gameObject, 0, 0.3f, TweenMode.Constant, onCompleted);
    }


    public void SetDialogueContext(string text, float fontSize = 35)
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
        if(sprite != null)
        {
            _pandaImage.gameObject.SetActive(true);
            _pandaImage.sprite = sprite;
            return;
        }

        _pandaImage.gameObject.SetActive(false);
    }

    public void ShakeDialogue(float totalDuration)
    {
        float duration = totalDuration / 14f;

        Vector2 tmpPos = _introDialogue.anchoredPosition;
        Vector2 targetPos1 = tmpPos + new Vector2(5, 0);
        Vector2 targetPos2 = tmpPos + new Vector2(-5, 0);

        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos2, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos2, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos2, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos2, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos2, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos2, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, targetPos1, duration, TweenMode.Constant);
        Tween.RectTransfromAnchoredPosition(_introDialogue.gameObject, tmpPos, duration, TweenMode.EaseOutBack);
    }

}
