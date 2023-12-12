using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{
    private Dictionary<int, StoryDialogue> _storyDialogueDic;

    private DialogueParser _parser = new DialogueParser();



    public void Register()
    {
        Debug.Log("시작");
        _storyDialogueDic = _parser.StroyParse("StoryDialogue");
    }


    public StoryDialogue GetStoryDialogue(int index)
    {
        if (!_storyDialogueDic.TryGetValue(index, out StoryDialogue storyDialogue))
        {
            Debug.LogError("해당 스토리 ID가 존재하지 않습니다.");
            return default;
        }
            

        return storyDialogue;
    }


}
