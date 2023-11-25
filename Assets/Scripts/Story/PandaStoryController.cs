using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartStory : UnityEngine.Events.UnityEvent<int> { }

public class SetStroyData : UnityEngine.Events.UnityEvent<int, PandaStoryController> { }


public class PandaStoryController : MonoBehaviour, IInteraction
{
    [SerializeField] private int _storyID;

    [SerializeField] private UnityEvent _onComplate;

    private StoryDialogue _storyDialogue;

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
        StartStroy?.Invoke(_storyID);
    }


    public void UpdateInteraction()
    {

    }


    public void ExitInteraction()
    {

    }
}
