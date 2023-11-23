
public class StoryDialogue
{
    //스토리 ID
    public int StoryID { get; private set; }

    //스토리 제목
    public string StoryName { get; private set; }

    //필요 친밀도 
    public int RequiredIntimacy { get; private set; }

    //이전 스토리
    public int PriorStoryID { get; private set; }

    //다음 스토리
    public int NextStoryID { get; private set; }

    //판다 ID
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
