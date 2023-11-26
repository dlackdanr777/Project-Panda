using Muks.DataBind;
using System.Collections.Generic;
using UnityEngine;
using System;




public class StoryManager : SingletonHandler<StoryManager>
{
    public  int CurrentDialogueID { get; private set; }

    public StoryDialogue CurrentDialogue { get; private set; }

    public PandaStoryController CurrentStroyController { get; private set; }

    public bool IsStoryStart { get; private set; }

    [SerializeField]
    private List<int> _storyCompleteList = new List<int>();

    private Dictionary<int, PandaStoryController> _pandaStoryControllerDic = new Dictionary<int, PandaStoryController>();

    private Action _startHandler;

    private Action _exitHandler;

    private ActionGetter _startActionGetter;

    private ActionGetter _exitActionGetter;


    public override void Awake()
    {
        UIDialogue.AddComplateStory.AddListener(AddComplateStory);
        PandaStoryController.StartStroy.AddListener(StartStory);
        PandaStoryController.SetStroyData.AddListener(SetStroyDic);
    }


    private void Start()
    {
        _startActionGetter = new ActionGetter("ShowDialogue", ref _startHandler);
        _exitActionGetter = new ActionGetter("HideDialogue", ref _exitHandler);
    }


    private void StartStory(int id, PandaStoryController _storyController)
    {
        if(IsStoryStart || _storyCompleteList.Contains(id))
        {
            Debug.Log("�̹� �������̰ų� �Ϸ�� ����Ʈ �Դϴ�.");
            return;
        }

        CurrentDialogue = DialogueManager.Instance.GetStoryDialogue(id);
        CurrentStroyController = _storyController;
        CurrentDialogueID = id;
        _startHandler?.Invoke();
        IsStoryStart = true;
    }


    private void AddComplateStory(int id)
    {
        if (_storyCompleteList.Contains(id))
        {
            Debug.Log("�̹� �ִ� �ε��� �Դϴ�.");
            return;
        }
            
        _storyCompleteList.Add(id);
        _exitHandler?.Invoke();
        CheckStoryActivate();
        IsStoryStart = false;
    }

    private void SetStroyDic(int id, PandaStoryController pandaStoryController)
    {

        if (_pandaStoryControllerDic.ContainsKey(id))
        {
            Debug.LogError("�̹� ��ųʸ��� �����մϴ�.");
            return;
        }
        _pandaStoryControllerDic.Add(id, pandaStoryController);
    }


    private void CheckStoryActivate()
    {
        foreach(int pandaID in  _pandaStoryControllerDic.Keys)
        {
            if (_storyCompleteList.Contains(pandaID))
            {
                _pandaStoryControllerDic[pandaID].DisableStory();
                continue;
            }

            _pandaStoryControllerDic[pandaID].CheckActivateStory();
        }
    }

}
