using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISideDialogue : UIView
{
    public static event Action<string> OnAddRewardHandler;

    [SerializeField] private Image _pandaImage;
    [SerializeField] private Sprite _starterImage;
    [SerializeField] private UIDialogueButton _leftButton;
    [SerializeField] private UIDialogueButton _rightButton;

    private Vector2 _tempPos;
    private DialogueState _state;
    private int _currentIndex;

    private SideStoryController _currentStoryController;
    private SideStoryDialogue _dialogue;

    private bool _isStoryStart;
    private bool _isSkipEnabled;
    private Coroutine _contextAnimeRoutine;
    private Coroutine _skipDisableRoutine;
    private bool _isFail;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        RectTransform = GetComponent<RectTransform>();
        _tempPos = RectTransform.anchoredPosition;

        SideStoryController.OnStartInteractionHandler += StartStory;
        DataBind.SetTextValue("SideDialogueName", " ");
        DataBind.SetTextValue("SideDialogueContexts", " ");
        DataBind.SetButtonValue("SideDialogueNextButton", OnNextButtonClicked);

        _leftButton.Init();
        _rightButton.Init();
    }

    public void OnDestroy()
    {
        SideStoryController.OnStartInteractionHandler -= StartStory;
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
            _isStoryStart = false;
            _state = DialogueState.None;
            VisibleState = VisibleState.Disappeared;

            _uiNav.ShowMainUI();
        });
    }


    public override void Show()
    {
        if (_isStoryStart)
        {
            Debug.LogError("이미 대화가 진행중 입니다.");
            return;
        }

        gameObject.SetActive(true);
        _isSkipEnabled = true;
        _uiNav.HideMainUI();

        DataBind.SetTextValue("SideDialogueName", " ");
        DataBind.SetTextValue("SideDialogueContexts", " ");
        _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 0);
        _currentIndex = 0;

        VisibleState = VisibleState.Appearing;
        Tween.RectTransfromAnchoredPosition(transform.gameObject, new Vector2(0, 10), 1f, TweenMode.EaseInOutBack, () =>
        {
            VisibleState = VisibleState.Appeared;
            OnNextButtonClicked();
        });
    }

    private void StartStory(SideStoryDialogue storyDialogue)
    {
        _dialogue = storyDialogue;
        _uiNav.Push("SideDialogue");
        _isStoryStart = true;
        _isFail = false;
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

        if (_currentIndex < _dialogue.DialogueData.Count)
        {
            if (_contextAnimeRoutine != null)
                StopCoroutine(_contextAnimeRoutine);

            _contextAnimeRoutine = StartCoroutine(ContextAnime(_dialogue.DialogueData[_currentIndex]));

            if (_dialogue.DialogueData[_currentIndex].IsComplete.Equals("FAIL"))
            {
                _isFail = true;
            }

            if (_dialogue.DialogueData[_currentIndex].IsComplete.Equals("END"))
            {
                _currentIndex = _dialogue.DialogueData.Count;
                if (!_isFail) //실패가 아닐 때만 보상
                {
                    OnAddRewardHandler?.Invoke(_dialogue.StoryID);
                }
            }
            _currentIndex++;
        }

        else
        {
            //OnComplateHandler?.Invoke(_dialogue.StoryID);
            _uiNav.Pop();
        }

    }

    private IEnumerator ContextAnime(SideDialogueData data)
    {
        _state = DialogueState.Context;

        if (_skipDisableRoutine != null)
            StopCoroutine(_skipDisableRoutine);
        _skipDisableRoutine = StartCoroutine(SkipDisable(0.2f));

        _pandaImage.sprite = DatabaseManager.Instance.GetPandaImage(1).NomalImage;
        _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 1);

        DataBind.SetTextValue("SideDialogueName", DatabaseManager.Instance.GetNPCNameById(data.TalkPandaID));
        if (data.TalkPandaID.Equals("POYA00"))
        {
            DataBind.SetSpriteValue("SideDialoguePandaImage", _starterImage);
        }
        else
        {
            DataBind.SetSpriteValue("SideDialoguePandaImage", DatabaseManager.Instance.GetNPCImageById(data.TalkPandaID));
        }

        char[] tempChars = data.Contexts.ToCharArray();
        string tempString = string.Empty;

        for (int j = 0, count = tempChars.Length; j < count; j++)
        {
            tempString += tempChars[j];
            DataBind.SetTextValue("SideDialogueContexts", tempString);

            yield return new WaitForSeconds(0.05f);

            if (_state == DialogueState.None)
            {
                DataBind.SetTextValue("SideDialogueContexts", data.Contexts);
                break;
            }
        }

        if (data.CanChoice)
        {
            _state = DialogueState.Choice;
            _leftButton.ShowButton(data.ChoiceContextA, () => { _leftButton.Button.onClick.AddListener(()=>OnButtonClicked(data.ChoiceAID)); });
            _rightButton.ShowButton(data.ChoiceContextB, () => { _rightButton.Button.onClick.AddListener(()=>OnButtonClicked(data.ChoiceBID)); });
        }

        else
        {
            _state = DialogueState.None;
        }
    }

    private void OnButtonClicked(string choice)
    {
        _leftButton.HideButton();
        _rightButton.HideButton();

        GoToBranch(choice);
        _state = DialogueState.None;
        _leftButton.Button.onClick.RemoveListener(()=>OnButtonClicked(choice));
        _rightButton.Button.onClick.RemoveListener(()=>OnButtonClicked(choice));
        _isSkipEnabled = true;
        OnNextButtonClicked();

    }

    private IEnumerator SkipDisable(float value)
    {
        _isSkipEnabled = false;
        yield return new WaitForSeconds(value);
        _isSkipEnabled = true;
    }

    private void GoToBranch(string branch)
    {
        for(int i = _currentIndex; i < _dialogue.DialogueData.Count; i++)
        {
            if (_dialogue.DialogueData[i].ChoiceID == branch)
            {
                _currentIndex = i;
            }
        }
    }
}
