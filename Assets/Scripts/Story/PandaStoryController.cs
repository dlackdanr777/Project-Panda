using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaStoryController : MonoBehaviour, IInteraction
{
    [SerializeField] private Panda _panda;

    [SerializeField] private int _storyID;

    private StoryDialogue _storyDialogue;


    private void Start()
    {
        _storyDialogue = DialogueManager.Instance.GetStoryDialogue(_storyID);

        bool isActive = 0 <= _storyDialogue.RequiredIntimacy ? true : false;

        if (!isActive)
        {
            gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < _storyDialogue.DialogDatas.Length; i++)
        {
            Debug.Log(_storyDialogue.DialogDatas[i].TalkPandaID);
            Debug.Log(_storyDialogue.DialogDatas[i].Contexts);
        }
    }


    public void StartInteraction()
    {
        throw new System.NotImplementedException();
    }


    public void UpdateInteraction()
    {
        throw new System.NotImplementedException();
    }


    public void ExitInteraction()
    {
        throw new System.NotImplementedException();
    }

}
