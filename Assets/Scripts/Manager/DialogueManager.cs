using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonHandler<DialogueManager>
{
    private Dictionary<int, StoryDialogue> _storyDialogue;

    //�Ϸ�� ���丮������ ��Ƴ��� ��
    public List<int> CompleteStoryIndex { get; private set; }

    private DialogueParser _parser = new DialogueParser();

    public override void Awake()
    {
        base.Awake();

        _storyDialogue = _parser.StroyParse("StoryDialogue");
    }

    public StoryDialogue GetStoryDialogue(int index)
    {
        if (!_storyDialogue.TryGetValue(index, out StoryDialogue storyDialogue))
        {
            Debug.LogError("�ش� ���丮 ID�� �������� �ʽ��ϴ�.");
            return default;
        }
            

        return storyDialogue;
    }
}
