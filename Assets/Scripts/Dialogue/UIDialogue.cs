
using UnityEngine;
using Muks.Tween;
using System.Collections;
using Muks.DataBind;

public class UIDialogue : UIView
{
    private StoryDialogueManager _manager;

    private RectTransform _rectTransform;

    private Vector2 _tempPos;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _rectTransform = GetComponent<RectTransform>();
        _tempPos = _rectTransform.anchoredPosition;

        DataBind.SetTextValue("DialogueName", " ");
        DataBind.SetTextValue("DialogueContexts", " ");
        DataBind.SetButtonValue("DialogueNextButton", OnNextButtonClicked);
    }



    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;
        Tween.RectTransfromAnchoredPosition(gameObject, _tempPos, 1f, TweenMode.EaseInOutBack, () => 
        {
            gameObject.SetActive(false);
            VisibleState = VisibleState.Disappeared;
            _currentIndex = 0;
        });
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        _currentIndex = 0;
        


        VisibleState = VisibleState.Appearing;
        Tween.RectTransfromAnchoredPosition(transform.gameObject, new Vector2(0, -700), 1f, TweenMode.EaseInOutBack, () => 
        {
            VisibleState = VisibleState.Appeared;
            dialogue = StoryDialogueManager.CurrentDialogue;
            Debug.Log(dialogue);
            OnNextButtonClicked();
        });
    }


    private bool _isContextRunning;
    private int _currentIndex;
    private StoryDialogue dialogue;


    private void OnNextButtonClicked()
    {
        if (_isContextRunning)
            _isContextRunning = false;

        else
        {
            if(_currentIndex >= dialogue.DialogDatas.Length - 1)
            {
                _uiNav.Pop();
            }
            else
            {
                StartCoroutine(ContextAnime(dialogue.DialogDatas[_currentIndex]));
               _currentIndex++;

            }
        }
    }

    private IEnumerator ContextAnime(DialogData data)
    {
        _isContextRunning = true;
        DataBind.SetTextValue("DialogueName", data.TalkPandaID.ToString());

        char[] tempChars = data.Contexts.ToCharArray();
        string tempString = string.Empty;

        for (int j = 0, count = tempChars.Length; j < count; j++)
        {
            tempString += tempChars[j];
            DataBind.SetTextValue("DialogueContexts", tempString);

            yield return new WaitForSeconds(0.1f);

            if (!_isContextRunning)
            {
                DataBind.SetTextValue("DialogueContexts", data.Contexts);
                break;
            }
        }

        _isContextRunning = false;
    }



}
