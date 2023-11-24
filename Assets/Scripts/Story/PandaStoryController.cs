using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartStory : UnityEngine.Events.UnityEvent<int> { }


public class PandaStoryController : MonoBehaviour, IInteraction
{
    [SerializeField] private int _storyID;

    private StoryDialogue _storyDialogue;

    public static StartStory StartStroy = new StartStory();


    private void Start()
    {
        _storyDialogue = DialogueManager.Instance.GetStoryDialogue(_storyID);

        bool isActive = 0 <= _storyDialogue.RequiredIntimacy ? true : false;

        if (!isActive)
        {
            gameObject.SetActive(false);
            return;
        }
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
