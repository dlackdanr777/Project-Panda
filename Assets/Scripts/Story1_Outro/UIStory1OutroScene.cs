using Muks.Tween;
using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStory1OutroScene : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform _introDialogue;
    [SerializeField] private TextMeshProUGUI _dialogueContext;
    [SerializeField] private TextMeshProUGUI _dialogueNameText;
    [SerializeField] private TextMeshProUGUI _nextText;
    [SerializeField] private Image _pandaImage;
    [SerializeField] private Button _dialogueSkipButton;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private TextMeshProUGUI _endText;

    private CanvasGroup _canvasGroup;
    private bool _isSkipButtonClicked;

    public void Init()
    {
        _canvasGroup = _introDialogue.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;

        _dialogueSkipButton.onClick.AddListener(OnSkipButtonClicked);

        _dialogueContext.text = string.Empty;
        _dialogueNameText.text = string.Empty;
        _pandaImage.gameObject.SetActive(false);
        _dialogueSkipButton.gameObject.SetActive(false);
        _fadeImage.gameObject.SetActive(false);
        _endText.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }


    public void StartDialogue(Action onCompleted = null)
    {
        gameObject.SetActive(true);
        _pandaImage.gameObject.SetActive(false);
        _nextText.gameObject.SetActive(false);
        _dialogueContext.text = string.Empty;
        _dialogueNameText.text = string.Empty;
        _isSkipButtonClicked = false;
        Tween.CanvasGroupAlpha(_canvasGroup.gameObject, 1, 0.3f, TweenMode.Constant, onCompleted);
    }


    public IEnumerator StartContext(string context, float fontSize = 35, float contextTimeInterval = 0.08f, float contextEndWaitTime = 1f)
    {
        _isSkipButtonClicked = false;
        HideNextText();
        Tween.TransformMove(gameObject, transform.position, 0.25f, TweenMode.Constant, () => _dialogueSkipButton.gameObject.SetActive(true));

        char[] tempChars = context.ToCharArray();
        string tempString = string.Empty;

        for (int i = 0, count = tempChars.Length; i < count; i++)
        {
            SetDialogueContext(tempString, fontSize);
            tempString += tempChars[i];

            yield return YieldCache.WaitForSeconds(contextTimeInterval);

            //버튼을 눌렀을땐
            if (_isSkipButtonClicked)
            {
                //순차적으로 나오는 대사를 한번에 출력한다.
                SetDialogueContext(context, fontSize);
                break;
            }
        }

        yield return YieldCache.WaitForSeconds(contextEndWaitTime);

        ShowNextText();

        _isSkipButtonClicked = false;
        while (!_isSkipButtonClicked)
        {
            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _dialogueSkipButton.gameObject.SetActive(false);
        HideNextText();
    }


    public void EndDialogue(Action onCompleted = null)
    {
        _isSkipButtonClicked = false;
        _dialogueSkipButton.gameObject.SetActive(false);
        _nextText.gameObject.SetActive(false);
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
        if (sprite != null)
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



    private void ShowNextText()
    {
        _nextText.gameObject.SetActive(true);
        _nextText.color = new Color(_nextText.color.r, _nextText.color.g, _nextText.color.b, 0.1f);

        Tween.TMPAlpha(_nextText.gameObject, 1, 2).Loop(LoopType.Yoyo);
    }


    private void HideNextText()
    {
        Tween.Stop(_nextText.gameObject);
        _nextText.gameObject.SetActive(false);
    }

    private void OnSkipButtonClicked()
    {
        _isSkipButtonClicked = true;
    }


    public void StartFadeIn(float duration)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 0);

        Tween.IamgeAlpha(_fadeImage.gameObject, 1, duration, TweenMode.Constant);
    }


    public void StartFadeOut(float duration)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(0, 0, 0, 1);

        Tween.IamgeAlpha(_fadeImage.gameObject, 0, duration, TweenMode.Constant, () => _fadeImage.gameObject.SetActive(false)); ;
    }


    public void StartEndText(float duration)
    {
        _endText.gameObject.SetActive(true);
        _endText.color = new Color(_endText.color.r, _endText.color.g, _endText.color.b, 0);

        Tween.TMPAlpha(_endText.gameObject, 1, duration);
    }


    public void EndEndText(float duration)
    {
        _endText.color = new Color(_endText.color.r, _endText.color.g, _endText.color.b, 1);
        Tween.TMPAlpha(_endText.gameObject, 0, duration, TweenMode.Constant, () => _endText.gameObject.SetActive(false));
    }
}
