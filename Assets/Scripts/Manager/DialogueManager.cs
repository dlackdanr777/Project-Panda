using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonHandler<DialogueManager>
{
    private Dictionary<int, StoryDialogue> _storyDialogueDic;

    //완료된 스토리정보를 모아놓는 곳
    public List<int> CompleteStoryIndex { get; private set; }

    private DialogueParser _parser = new DialogueParser();


    public override void Awake()
    {
        base.Awake();

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
