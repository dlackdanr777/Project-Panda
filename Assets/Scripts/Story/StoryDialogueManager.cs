using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDialogueManager : MonoBehaviour
{
    [SerializeField] private UIDialogue _uiDialogue;

    public static StoryDialogue CurrentDialogue { get; private set; }

    private static int _currentID;

    public static int CurrentID
    {
        get
        {
            return _currentID;
        }
        set
        {
            _currentID = value;
            CurrentDialogue = DialogueManager.Instance.GetStoryDialogue(value);
        }
    }


}
