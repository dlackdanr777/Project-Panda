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

    public StoryDialogue StoryDialogue { get; private set; }

    public static event Action<StoryDialogue, StoryEventData[]> OnStartInteractionHandler;

    public static event Action<int, PandaStoryController> OnStartHandler;

    public static event Action<PandaStoryController> OnCheckActivateHandler;


    private void Start()
    {
        StoryDialogue = DatabaseManager.Instance.GetDialogueData(_storyID);
        OnStartHandler?.Invoke(_storyID, this);
        OnCheckActivateHandler?.Invoke(this);
    }


    public void StartInteraction()
    {
        OnStartInteractionHandler?.Invoke(StoryDialogue, _storyEvents);
    }


    public void UpdateInteraction()
    {

    }


    public void ExitInteraction()
    {

    }
}
