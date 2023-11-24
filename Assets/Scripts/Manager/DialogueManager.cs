using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonHandler<DialogueManager>
{
    private Dictionary<int, StoryDialogue> _storyDialogueDic;

    //�Ϸ�� ���丮������ ��Ƴ��� ��
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
            Debug.LogError("�ش� ���丮 ID�� �������� �ʽ��ϴ�.");
            return default;
        }
            

        return storyDialogue;
    }


}
