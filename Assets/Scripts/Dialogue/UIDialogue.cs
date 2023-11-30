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
    [SerializeField] private UIDialogueButton _leftButton;
    [SerializeField] private UIDialogueButton _rightButton;

    private Vector2 _tempPos;
    private DialogueState _state;
    private int _currentIndex;

    private StoryDialogue _dialogue;
    private StoryEventData[] _eventDatas;

    private bool _isStoryStart;
    private bool _isSkipEnabled;
    private Coroutine _contextAnimeRoutine;

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

        _leftButton.Init();
        _rightButton.Init();
    }



    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;
        CancelInvoke("SkipDisable");
        if (_contextAnimeRoutine != null)
            StopCoroutine(_contextAnimeRoutine);

        Tween.RectTransfromAnchoredPosition(gameObject, _tempPos, 1f, TweenMode.EaseInOutBack, () => 
        {
            gameObject.SetActive(false);
            _isStoryStart = false;
            VisibleState = VisibleState.Disappeared;
            _currentIndex = 0;
        });

        if (!StoryManager.Instance._storyCompleteList.Contains(_dialogue.StoryID))
        {
            foreach (StoryEventData data in _eventDatas)
            {
                if(data.StoryEvent.IsComplate)
                    data.StoryEvent.EventCancel();

                data.StoryEvent.IsComplate = false;
            }
        }
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

        DataBind.SetTextValue("DialogueName", " ");
        DataBind.SetTextValue("DialogueContexts", " ");

        _currentIndex = 0;
        
        VisibleState = VisibleState.Appearing;
        Tween.RectTransfromAnchoredPosition(transform.gameObject, new Vector2(0, 10), 1f, TweenMode.EaseInOutBack, () => 
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
            if (_currentIndex != data.InsertIndex)
                continue;

            if (!data.StoryEvent.IsComplate)
            {
                StartStoryEvent(data.StoryEvent);
                return;
            }
        }

        if (_currentIndex < _dialogue.DialogDatas.Length)
        {
            if (_contextAnimeRoutine != null)
                StopCoroutine(_contextAnimeRoutine);

            _contextAnimeRoutine = StartCoroutine(ContextAnime(_dialogue.DialogDatas[_currentIndex]));
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

        _state = DialogueState.Event;

        storyEvent.EventStart(() =>
        {
            _state = DialogueState.None;
            storyEvent.IsComplate = true;
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
        _isSkipEnabled = false;
        Invoke("SkipDisable", 0.5f);
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

        if (data.CanChoice)
        {
            _state = DialogueState.Choice;
            _leftButton.ShowButton(data.ChoiceContext1, () => { _leftButton.Button.onClick.AddListener(OnButtonClicked); });
            _rightButton.ShowButton(data.ChoiceContext2, () => { _rightButton.Button.onClick.AddListener(OnButtonClicked); });
        }

        else
        {
            _state = DialogueState.None;
        }
    }

    private void OnButtonClicked()
    {
        _leftButton.HideButton();
        _rightButton.HideButton(() => 
        {
            _state = DialogueState.None;
            _leftButton.Button.onClick.RemoveListener(OnButtonClicked);
            _rightButton.Button.onClick.RemoveListener(OnButtonClicked);
            _isSkipEnabled = true;
            OnNextButtonClicked();
        });

    }


    private void SkipDisable()
    {
        _isSkipEnabled = true;
    }

}
