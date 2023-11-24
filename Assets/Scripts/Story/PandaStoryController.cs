using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Muks.DataBind;

public class PandaStoryController : MonoBehaviour, IInteraction
{
    [SerializeField] private Panda _panda;

    [SerializeField] private int _storyID;

    private StoryDialogue _storyDialogue;

    private Action _startHandler;

    private Action _exitHandler;

    private ActionGetter _startActionGetter;

    private ActionGetter _exitActionGetter;

    private void Start()
    {
        _storyDialogue = DialogueManager.Instance.GetStoryDialogue(_storyID);

        _startActionGetter = new ActionGetter("ShowDialogue", ref _startHandler);

        _exitActionGetter = new ActionGetter("HideDialogue", ref _exitHandler);


        bool isActive = 0 <= _storyDialogue.RequiredIntimacy ? true : false;

        if (!isActive)
        {
            gameObject.SetActive(false);
            return;
        }
    }




    public void StartInteraction()
    {
        StoryDialogueManager.CurrentID = _storyID;
        _startHandler?.Invoke();
    }


    public void UpdateInteraction()
    {

    }


    public void ExitInteraction()
    {
        Debug.Log("³¡!");
        _exitHandler?.Invoke();
    }
}
