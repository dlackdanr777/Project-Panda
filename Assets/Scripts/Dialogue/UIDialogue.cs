using UnityEngine;
using Muks.Tween;
using System.Collections;
using Muks.DataBind;

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

    private RectTransform _rectTransform;
    private Vector2 _tempPos;
    private DialogueState _state;
    private int _currentIndex;
    private StoryDialogue _dialogue;
    private PandaStoryController _pandaController;
    public static AddComplateStory AddComplateStory = new AddComplateStory();
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

        foreach (StoryEventData data in _pandaController.StoryEvents)
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
        if(StoryManager.Instance.IsStoryStart)
        {
            Debug.LogError("이미 대화가 진행중 입니다.");
            return;
        }

        gameObject.SetActive(true);
        CameraController.FriezePos = true;
        CameraController.FriezeZoom = true;

        _currentIndex = 0;
        
        VisibleState = VisibleState.Appearing;
        Tween.RectTransfromAnchoredPosition(transform.gameObject, new Vector2(0, -700), 1f, TweenMode.EaseInOutBack, () => 
        {
            VisibleState = VisibleState.Appeared;
            _dialogue = StoryManager.Instance.CurrentDialogue;
            _pandaController = StoryManager.Instance.CurrentStroyController;
            OnNextButtonClicked();
        });
    }




    private void OnNextButtonClicked()
    {

        switch (_state)
        {
            case DialogueState.Context:
                _state = DialogueState.None;
                return;

            case DialogueState.Choice: return;

            case DialogueState.Event: return;
        }


        foreach (StoryEventData data in _pandaController.StoryEvents)
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
            AddComplateStory?.Invoke(StoryManager.Instance.CurrentDialogueID);
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

        _state = DialogueState.None;
    }



}
