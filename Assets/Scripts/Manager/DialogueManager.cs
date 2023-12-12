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
        Debug.Log("����");
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
