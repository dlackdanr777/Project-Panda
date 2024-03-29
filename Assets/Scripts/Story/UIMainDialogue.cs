using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMainDialogue : UIView
{
    public static event Action<string, string> OnAddRewardHandler;
    public static event Action<string> OnFinishStoryHandler;
    public static event Action<string> OnCheckConditionHandler;

    [SerializeField] private Image _pandaImage;
    [SerializeField] private Sprite _starterImage;
    [SerializeField] private Sprite[] _intimacyImage;
    [SerializeField] private GameObject _intimacyobj;
    [SerializeField] private GameObject _itemReward;
    [SerializeField] private GameObject _moneyReward;
    [SerializeField] private Transform _targetPos;
    [SerializeField] private UIDialogueButton _leftButton;
    [SerializeField] private UIDialogueButton _rightButton;
    [SerializeField] private Button _nextButton;

    private Vector2 _tempPos;
    private DialogueState _state;
    private int _currentIndex;

    private MainStoryDialogue _dialogue;

    private bool _isStoryStart;
    private bool _isSkipEnabled;
    private bool _isConditionTrue;
    private bool _isCheckCondition;
    private Coroutine _contextAnimeRoutine;
    private Coroutine _skipDisableRoutine;
    private Coroutine _itemRewardRoutine;
    private Coroutine _moneyRewardRoutine;
    private bool _isFail;
    private bool _isEnd;
    private string _isReward;

    private string _currentNPC;
    private const int MinIntimacy = 0;
    private const int MaxIntimacy = 5;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        RectTransform = GetComponent<RectTransform>();
        _tempPos = RectTransform.anchoredPosition;

        MainStoryController.OnStartInteractionHandler += StartStory;
        MainStoryController.OnCheckConditionHandler += CheckCondition;
        DataBind.SetTextValue("MainDialogueName", " ");
        DataBind.SetTextValue("MainDialogueContexts", " ");

        _nextButton.onClick.AddListener(OnNextButtonClicked);
        //DataBind.SetButtonValue("MainDialogueNextButton", OnNextButtonClicked);

        _leftButton.Init();
        _rightButton.Init();

        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        MainStoryController.OnStartInteractionHandler -= StartStory;
        MainStoryController.OnCheckConditionHandler -= CheckCondition;
    }

    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;

        _leftButton.Disabled();
        _rightButton.Disabled();
        _itemReward.SetActive(false);

        _currentIndex = 0;
        _isSkipEnabled = false;

        if (_contextAnimeRoutine != null)
            StopCoroutine(_contextAnimeRoutine);

        if (_skipDisableRoutine != null)
            StopCoroutine(_skipDisableRoutine);

        if (_itemRewardRoutine != null)
            StopCoroutine(_itemRewardRoutine);

        if (_moneyRewardRoutine != null)
            StopCoroutine(_moneyRewardRoutine);


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
        _isCheckCondition = false;
        _isConditionTrue = true;
        
        _uiNav.HideMainUI();

        DataBind.SetTextValue("MainDialogueName", " ");
        DataBind.SetTextValue("MainDialogueContexts", " ");
        int intimacy = GetIntimacy(DatabaseManager.Instance.GetNPC(_currentNPC).Intimacy);
        DataBind.SetSpriteValue("MainDialogueIntimacyImage", _intimacyImage[intimacy]);
        _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 0);
        _currentIndex = 0;

        VisibleState = VisibleState.Appearing;
        Tween.RectTransfromAnchoredPosition(transform.gameObject, new Vector2(0, 10), 1f, TweenMode.EaseInOutBack, () =>
        {
            _isSkipEnabled = true;
            VisibleState = VisibleState.Appeared;
            OnNextButtonClicked();
        });
    }

    private void StartStory(MainStoryDialogue storyDialogue)
    {
        if (_isStoryStart)
        {
            Debug.Log("이미 시작중이거나 완료된 퀘스트 입니다.");
            return;
        }

        _dialogue = storyDialogue;
        _currentNPC = DatabaseManager.Instance.GetNPCList()[int.Parse(_dialogue.StoryID.Substring(2, 2)) - 1].Id;
        _uiNav.Push("MainDialogue");
        _isStoryStart = true;
        _isEnd = false;
        _isFail = false;
        _isReward = null;
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

        if (!_isConditionTrue)
        {
            _uiNav.Pop();
        }
        else if (_currentIndex < _dialogue.DialogueData.Count)
        {
            if (_contextAnimeRoutine != null)
                StopCoroutine(_contextAnimeRoutine);

            _contextAnimeRoutine = StartCoroutine(ContextAnime(_dialogue.DialogueData[_currentIndex]));

            if (_dialogue.DialogueData[_currentIndex].IsComplete.Equals("FAIL"))
            {
                _isFail = true;
            }

            else if (_dialogue.DialogueData[_currentIndex].IsComplete.Equals("REWARD") && _dialogue.RewardCount > 0)
            {
                _isReward = _dialogue.RewardType.ToString();

                RewardItemContent(_dialogue.RewardType, _dialogue.RewardID, _dialogue.RewardCount);
            }
            else if (_dialogue.DialogueData[_currentIndex].IsComplete.Equals("END"))
            {
                _currentIndex = _dialogue.DialogueData.Count;

                if (!_isFail) //실패가 아닐 때만 보상
                {
                    _isEnd = true;
                    //OnAddRewardHandler.Invoke(_dialogue.StoryID, _dialogue.StoryStartPanda);
                }
            }
            _currentIndex++;
        }
        else
        {
            _uiNav.Pop();
            if (_isEnd == true)
            {
                OnAddRewardHandler.Invoke(_dialogue.StoryID, _dialogue.StoryStartPanda);
            }
            if (!_isFail) //실패가 아닐 때만
            {
                OnFinishStoryHandler?.Invoke(_dialogue.StoryID);
                DatabaseManager.Instance.UserInfo.SaveStoryData(3);
            }
        }
        if (!_isCheckCondition)
        {
            _isCheckCondition = true;
            OnCheckConditionHandler?.Invoke(_dialogue.StoryID); // 조건 체크
        }
    }

    private IEnumerator ContextAnime(MainDialogueData data)
    {
        _state = DialogueState.Context;

        if (_skipDisableRoutine != null)
            StopCoroutine(_skipDisableRoutine);
        _skipDisableRoutine = StartCoroutine(SkipDisable(0.2f));

        _pandaImage.sprite = DatabaseManager.Instance.GetPandaImage(1).NomalImage;
        _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 1);

        DataBind.SetTextValue("MainDialogueName", DatabaseManager.Instance.GetNPCNameById(data.TalkPandaID));
        if (data.TalkPandaID.Equals("POYA00"))
        {
            DataBind.SetSpriteValue("MainDialoguePandaImage", _starterImage);
            DataBind.SetTextValue("MainDialogueName", "포야");
            _intimacyobj.SetActive(false);
        }
        else if (data.TalkPandaID.Equals("SYSTEM"))
        {
            _pandaImage.color = new Color(_pandaImage.color.r, _pandaImage.color.g, _pandaImage.color.b, 0);
            _intimacyobj.SetActive(false);
        }
        else
        {
            DataBind.SetSpriteValue("MainDialoguePandaImage", DatabaseManager.Instance.GetNPCImageById(data.TalkPandaID));

            int intimacy = GetIntimacy(DatabaseManager.Instance.GetNPC(data.TalkPandaID).Intimacy);
            DataBind.SetSpriteValue("MainDialogueIntimacyImage", _intimacyImage[intimacy]);
            _intimacyobj.SetActive(true);
        }

        char[] tempChars = data.Contexts.ToCharArray();
        string tempString = string.Empty;

        for (int j = 0, count = tempChars.Length; j < count; j++)
        {
            tempString += tempChars[j];
            DataBind.SetTextValue("MainDialogueContexts", tempString);

            yield return new WaitForSeconds(0.05f);

            if (_state == DialogueState.None)
            {
                DataBind.SetTextValue("MainDialogueContexts", data.Contexts);
                break;
            }
        }

        if (data.CanChoice)
        {
            _state = DialogueState.Choice;
            _leftButton.ShowButton(data.ChoiceContextA, () => { _leftButton.Button.onClick.AddListener(() => OnButtonClicked(data.ChoiceAID)); });
            _rightButton.ShowButton(data.ChoiceContextB, () => { _rightButton.Button.onClick.AddListener(() => OnButtonClicked(data.ChoiceBID)); });
        }
        else
        {
            _state = DialogueState.None;
        }
    }

    private void OnButtonClicked(string choice)
    {
        _leftButton.HideButton();
        _rightButton.HideButton(() =>
        {
            GoToBranch(choice);
            _state = DialogueState.None;
            _leftButton.Button.onClick.RemoveListener(() => OnButtonClicked(choice));
            _rightButton.Button.onClick.RemoveListener(() => OnButtonClicked(choice));
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

    private void GoToBranch(string branch)
    {
        for (int i = _currentIndex + 1; i < _dialogue.DialogueData.Count; i++)
        {
            if (_dialogue.DialogueData[i].ChoiceID.Equals(branch))
            {
                _currentIndex = i;
            }
        }
    }

    private int GetIntimacy(int amount)
    {
        amount /= 20;

        if (amount < MinIntimacy)
        {
            return MinIntimacy;
        }
        else if (amount >= MaxIntimacy)
        {
            return MaxIntimacy;
        }

        return amount;
    }

    private IEnumerator ItemRewardRoutine()
    {
        _itemReward.SetActive(true);
        yield return new WaitForSeconds(2f);
        _itemReward.SetActive(false);
    }

    private IEnumerator MoneyRewardRoutine()
    {
        _moneyReward.SetActive(true);
        foreach (var item in _moneyReward.transform.GetChild(0).GetComponentsInChildren<Image>())
        {
            Tween.TransformMove(item.gameObject, _targetPos.position, 0.5f, TweenMode.Quadratic);
        }
        yield return new WaitForSeconds(2f);
        _moneyReward.SetActive(false);
    }


    private void RewardItemContent(MainEventType type, string condition, int count)
    {
        //조건 비교
        switch (type)
        {
            case MainEventType.MONEY:
                if (_moneyRewardRoutine != null)
                {
                    StopCoroutine(_moneyRewardRoutine);
                }
                _moneyRewardRoutine = StartCoroutine(MoneyRewardRoutine());
                break;
            case MainEventType.QUESTITEM:
                ShowItemReward(DatabaseManager.Instance.GetGIImageById(condition).Image,
                    DatabaseManager.Instance.GetGIImageById(condition).Name,
                    count.ToString() + " 획득!");
                break;
                //case EventType.IVGI:
                //    ShowItemReward(DatabaseManager.Instance.GetGIImageById(condition).Image,
                //        DatabaseManager.Instance.GetGIImageById(condition).Name,
                //        count.ToString() + " 획득!");
                //    break;
                //case EventType.IVCK:
                //    break;
                //case EventType.IVFU:
                //    ShowItemReward
                //        (DatabaseManager.Instance.GetFurnitureItem()[condition].RoomImage,
                //        DatabaseManager.Instance.GetFurnitureItem()[condition].Name,
                //        "획득!");
                //    break;
        }
    }

    private void ShowItemReward(Sprite image, string name, string count)
    {
        DataBind.SetSpriteValue("MSRewardDetailImage", image);
        DataBind.SetTextValue("MSRewardDetailName", name);
        //DataBind.SetTextValue("MSRewardDetailCount", count);

        if (_itemRewardRoutine != null)
        {
            StopCoroutine(_itemRewardRoutine);
        }
        _itemRewardRoutine = StartCoroutine(ItemRewardRoutine());
    }

    private void CheckCondition()
    {
        _isConditionTrue = false;
    }
}
