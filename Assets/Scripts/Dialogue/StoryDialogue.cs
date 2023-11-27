
public class StoryDialogue
{
    //���丮 ID
    public int StoryID { get; private set; }

    //���丮 ����
    public string StoryName { get; private set; }

    //�ʿ� ģ�е� 
    public int RequiredIntimacy { get; private set; }

    //���� ���丮
    public int PriorStoryID { get; private set; }

    //���� ���丮
    public int NextStoryID { get; private set; }

    //�Ǵ� ID
    public int PandaID { get; private set; }

    public DialogData[] DialogDatas { get; private set; }

    public StoryDialogue(int storyID, string storyName, int requiredIntimacy, int priorStoryID, int nextStoryID, int pandaID, DialogData[] dialogDatas)
    {
        StoryID = storyID;
        StoryName = storyName;
        RequiredIntimacy = requiredIntimacy;
        PriorStoryID = priorStoryID;
        NextStoryID = nextStoryID;
        PandaID = pandaID;
        DialogDatas = dialogDatas;
    }

}

public class DialogData
{
    public string TalkPandaID { get; private set; }

    public string Contexts { get; private set; }

    public string ChoiceContext1 { get; private set; }

    public string ChoiceContext2 { get; private set; }

    public bool CanChoice { get; private set; }


    public DialogData(string talkPandaID, string contexts, string choiceContext1, string choiceContext2)
    {
        TalkPandaID = talkPandaID;
        Contexts = contexts;
        ChoiceContext1 = choiceContext1;
        ChoiceContext2 = choiceContext2;

        if (!ChoiceContext1.Equals("") || !ChoiceContext1.Equals(string.Empty))
            CanChoice = true;
    }
}
