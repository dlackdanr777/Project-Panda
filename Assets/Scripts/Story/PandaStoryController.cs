using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartStory : UnityEvent<int, PandaStoryController> { }

public class SetStroyData : UnityEvent<int, PandaStoryController> { }

[Serializable]
public class StoryEventData
{
    [Tooltip("몇번 째 대화 창에 이벤트가 들어가는 것인지?")]
    [SerializeField] private int _insertIndex;
    [SerializeField] private StoryEvent _storyEvent;

    public int InsertIndex => _insertIndex;
    public StoryEvent StoryEvent => _storyEvent;


}


public class PandaStoryController : MonoBehaviour, IInteraction
{
    [SerializeField] private int _storyID;

    [SerializeField] private StoryEventData[] _storyEvents;

    private StoryDialogue _storyDialogue;

    public StoryEventData[] StoryEvents => _storyEvents;

    public static StartStory StartStroy = new StartStory();

    public static SetStroyData SetStroyData = new SetStroyData();


    private void Start()
    {
        _storyDialogue = DialogueManager.Instance.GetStoryDialogue(_storyID);

        SetStroyData?.Invoke(_storyID, this);

        CheckActivateStory();
    }


    //판다 조건을 확인한 후 활성화 하는 함수
    public void CheckActivateStory()
    {
        bool isActive = 0 <= _storyDialogue.RequiredIntimacy ? true : false;

        gameObject.SetActive(isActive);
    }


    public void DisableStory()
    {
        gameObject.SetActive(false);
    }


    public void EnableStory()
    {
        gameObject.SetActive(true);
    }


    public void StartInteraction()
    {
        StartStroy?.Invoke(_storyID, this);
    }


    public void UpdateInteraction()
    {

    }


    public void ExitInteraction()
    {

    }
}
