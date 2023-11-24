using Muks.DataBind;
using System.Collections.Generic;
using UnityEngine;
using System;




public class StoryManager : SingletonHandler<StoryManager>
{
    public static int CurrentDialogueID { get; private set; }

    public StoryDialogue Dialogue { get; private set; }

    public bool IsStoryStart { get; private set; }

    private List<int> _storyCompleteList = new List<int>();

    private Action _startHandler;

    private Action _exitHandler;

    private ActionGetter _startActionGetter;

    private ActionGetter _exitActionGetter;


    public override void Awake()
    {
        UIDialogue.AddComplateStory.AddListener(AddComplateStory);
        PandaStoryController.StartStroy.AddListener(StartStory);
    }


    private void Start()
    {
        _startActionGetter = new ActionGetter("ShowDialogue", ref _startHandler);
        _exitActionGetter = new ActionGetter("HideDialogue", ref _exitHandler);
    }


    private void StartStory(int id)
    {
        if(IsStoryStart || _storyCompleteList.Contains(id))
        {
            Debug.Log("이미 시작중이거나 완료된 퀘스트 입니다.");
            return;
        }

        Dialogue = DialogueManager.Instance.GetStoryDialogue(id);
        CurrentDialogueID = id;
        _startHandler?.Invoke();
        IsStoryStart = true;
    }


    private void AddComplateStory(int id)
    {
        if (!_storyCompleteList.Contains(id))
        {
            Debug.Log("이미 있는 인덱스 입니다.");
            return;
        }
            
        _storyCompleteList.Add(id);
        _exitHandler?.Invoke();
        IsStoryStart = false;
    }

}
