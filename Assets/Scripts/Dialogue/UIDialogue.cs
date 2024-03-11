using UnityEngine;
using Muks.Tween;
using System.Collections;
using Muks.DataBind;
using System.Linq;
using System;
using UnityEngine.UI;


public enum DialogueState
{
    None,
    Context,
    Event,
    Choice
}

public class UIDialogue : UIView
{
    [SerializeField] private Image _pandaImage;
    [SerializeField] private Sprite _starterImage;
    [SerializeField] private UIDialogueButton _leftButton;
    [SerializeField] private UIDialogueButton _rightButton;

    private Vector2 _tempPos;
    private DialogueState _state;
    private int _currentIndex;

    private PandaStoryController _currentStoryController;
    private StoryDialogue _dialogue;
    private StoryEventData[] _eventDatas;

    private bool _isStoryStart;
    private bool _isSkipEnabled;
    private Coroutine _contextAnimeRoutine;
    private Coroutine _skipDisableRoutine;

    public static Action<string> OnComplateHandler;


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

    public void OnDestroy()
    {
        PandaStoryController.OnStartInteractionHandler -= StartStory;
    }



    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;

        _leftButton.Disabled();
        _rightButton.Disabled();

        _currentIndex = 0;
        _isSkipEnabled = false;

        if (_contextAnimeRoutine != null)
            StopCoroutine(_contextAnimeRoutine);

        if (_skipDisableRoutine != null)
            StopCoroutine(_skipDisableRoutine);

        Tween.RectTransfromAnchoredPosition(gameObject, _tempPos, 1f, TweenMode.EaseInOutBack, () => 
        {
            gameObject.SetActive(false);
            _currentStoryController.FollowButton.gameObject.SetActive(true);
            _isStoryStart = false;
            _state = DialogueState.None;
            VisibleState = VisibleState.Disappeared;

            _uiNav.ShowMainUI();
        });

        if (!StoryManager.Instance.CheckCompletedStoryById(_dialogue.StoryID))
        {
            foreach (StoryEventData data in _eventDatas)
            {
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
        _uiNav.HideMainUI();

        DataBind.SetTextValue("DialogueName", " ");
        DataBind.SetTextValue("DialogueContexts", " ");
        _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 0);
        _currentStoryController.FollowButton.gameObject.SetActive(false);
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
        gameObject.SetActive(true);

        if (!_isSkipEnabled)
            return;

        switch (_state)
        {
            case DialogueState.Context:
                _state = DialogueState.None;

                if (_skipDisableRoutine != null)
                    StopCoroutine(_skipDisableRoutine);
                _skipDisableRoutine = StartCoroutine(SkipDisable(0.3f));
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


    private void StartStory(PandaStoryController stroyController, StoryDialogue storyDialogue, StoryEventData[] eventDatas)
    {
        if (_isStoryStart || StoryManager.Instance.CheckCompletedStoryById(storyDialogue.StoryID))
        {
            Debug.Log("이미 시작중이거나 완료된 퀘스트 입니다.");
            return;
        }

        _currentStoryController = stroyController;
        _dialogue = storyDialogue;
        _eventDatas = eventDatas;
        _uiNav.Push("Dialogue");
        _isStoryStart = true;

        //판다 도감에 추가
        for(int i=0;i<DatabaseManager.Instance.GetNPCList().Count;i++)
        {
            if (storyDialogue.PandaID.Equals(DatabaseManager.Instance.GetNPCList()[i].Id) && !DatabaseManager.Instance.GetNPCList()[i].IsReceived)
            {
                Debug.Log(storyDialogue.PandaID + "를 만났다");
                if(DatabaseManager.Instance.GetNPCList()[i].IsReceived != true)
                {
                    DatabaseManager.Instance.GetNPCList()[i].IsReceived = true;
                    DatabaseManager.Instance.Challenges.UnlockingBook("NPC"); // 도전 과제 달성 체크
                }
            }
        }
    }


    private IEnumerator ContextAnime(DialogData data)
    {
        _state = DialogueState.Context;

        if (_skipDisableRoutine != null)
            StopCoroutine(_skipDisableRoutine);
        _skipDisableRoutine = StartCoroutine(SkipDisable(0.2f));

        _pandaImage.sprite = DatabaseManager.Instance.GetPandaImage(1).NomalImage;
        _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 1);

        DataBind.SetTextValue("DialogueName", DatabaseManager.Instance.GetNPCNameById(data.TalkPandaID));
        if (data.TalkPandaID.Equals("스타터"))
        {
            DataBind.SetSpriteValue("DialoguePandaImage", _starterImage);
        }
        else
        {
            DataBind.SetSpriteValue("DialoguePandaImage", DatabaseManager.Instance.GetNPCImageById(data.TalkPandaID));
        }

        char[] tempChars = data.Contexts.ToCharArray();
        string tempString = string.Empty;

        for (int j = 0, count = tempChars.Length; j < count; j++)
        {
            tempString += tempChars[j];
            DataBind.SetTextValue("DialogueContexts", tempString);

            yield return new WaitForSeconds(0.05f);

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

    private IEnumerator SkipDisable(float value)
    {
        _isSkipEnabled = false;
        yield return new WaitForSeconds(value);
        _isSkipEnabled = true;
    }
}
