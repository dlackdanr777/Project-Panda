
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
    public int TalkPandaID { get; private set; }

    public string Contexts { get; private set; }

    public DialogData(int talkPandaID, string contexts)
    {
        TalkPandaID = talkPandaID;
        Contexts = contexts;
    }
}
