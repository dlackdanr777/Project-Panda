using UnityEngine;
using Muks.Tween;
using System.Collections;
using Muks.DataBind;
using System.Linq;
using System;

public class AddComplateStory : UnityEngine.Events.UnityEvent<int> { }

public enum DialogueState
{
    None,
    Context,
    Event,
    Choice
}

public class UIDialogue : UIView
{
    private Vector2 _tempPos;
    private DialogueState _state;
    private int _currentIndex;

    private StoryDialogue _dialogue;
    private StoryEventData[] _eventDatas;

    private bool _isStoryStart;
    private bool _isSkipEnabled;

    public static event Action<int> OnComplateHandler;


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        RectTransform = GetComponent<RectTransform>();
        _tempPos = RectTransform.anchoredPosition;

        PandaStoryController.OnStartInteractionHandler += StartStory;
        DataBind.SetTextValue("DialogueName", " ");
        DataBind.SetTextValue("DialogueContexts", " ");
        DataBind.SetButtonValue("DialogueNextButton", OnNextButtonClicked);
    }



    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;
        CancelInvoke("SkipDisable");

        foreach (StoryEventData data in _eventDatas)
        {
            data.StoryEvent.IsComplate = false;
        }

        Tween.RectTransfromAnchoredPosition(gameObject, _tempPos, 1f, TweenMode.EaseInOutBack, () => 
        {
            CameraController.FriezePos = false;
            CameraController.FriezeZoom = false;
            gameObject.SetActive(false);

            VisibleState = VisibleState.Disappeared;
            _currentIndex = 0;
        });
    }

    public override void Show()
    {
        if(_isStoryStart)
        {
            Debug.LogError("이미 대화가 진행중 입니다.");
            return;
        }

        gameObject.SetActive(true);
        _isSkipEnabled = true;

        CameraController.FriezePos = true;
        CameraController.FriezeZoom = true;

        _currentIndex = 0;
        
        VisibleState = VisibleState.Appearing;
        Tween.RectTransfromAnchoredPosition(transform.gameObject, new Vector2(0, -700), 1f, TweenMode.EaseInOutBack, () => 
        {
            VisibleState = VisibleState.Appeared;
            OnNextButtonClicked();
        });
    }




    private void OnNextButtonClicked()
    {
        if (!_isSkipEnabled)
            return;


        switch (_state)
        {
            case DialogueState.Context:
                _state = DialogueState.None;
                return;

            case DialogueState.Choice: return;

            case DialogueState.Event: return;
        }


        foreach (StoryEventData data in _eventDatas)
        {
            if (_currentIndex == data.InsertIndex)
            {
                if (data.StoryEvent.IsComplate)
                    continue;

                StartStoryEvent(data.StoryEvent);
                return;
            }
        }

        if (_currentIndex < _dialogue.DialogDatas.Length)
        {
            StartCoroutine(ContextAnime(_dialogue.DialogDatas[_currentIndex]));
            _currentIndex++;
        }

        else
        {
            OnComplateHandler?.Invoke(_dialogue.StoryID);
            _uiNav.Pop();
        }

    }


    private void StartStoryEvent(StoryEvent storyEvent)
    {
        if (_state != DialogueState.None)
            return;

        storyEvent.EventStart(() =>
        {
            _currentIndex++;
            _state = DialogueState.None;
            OnNextButtonClicked();
        });

    }


    private void StartStory(StoryDialogue storyDialogue, StoryEventData[] eventDatas)
    {
        if (_isStoryStart || StoryManager.Instance._storyCompleteList.Contains(storyDialogue.StoryID))
        {
            Debug.Log("이미 시작중이거나 완료된 퀘스트 입니다.");
            return;
        }

        _dialogue = storyDialogue;
        _eventDatas = eventDatas;
        _uiNav.Push("Dialogue");
        _isStoryStart = true;
    }


    private IEnumerator ContextAnime(DialogData data)
    {
        _state = DialogueState.Context;
        DataBind.SetTextValue("DialogueName", data.TalkPandaID.ToString());

        char[] tempChars = data.Contexts.ToCharArray();
        string tempString = string.Empty;

        for (int j = 0, count = tempChars.Length; j < count; j++)
        {
            tempString += tempChars[j];
            DataBind.SetTextValue("DialogueContexts", tempString);

            yield return new WaitForSeconds(0.1f);

            if (_state == DialogueState.None)
            {
                DataBind.SetTextValue("DialogueContexts", data.Contexts);
                break;
            }
        }
        _isSkipEnabled = false;
        Invoke("SkipDisable", 0.5f);
        _state = DialogueState.None;
    }


    private void SkipDisable()
    {
        _isSkipEnabled = true;
    }

}
