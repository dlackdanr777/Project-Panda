using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    IFRSP, //���� ������ ����� Ȯ��
    IBGSP, //���� ������ ����� Ȯ��
    IFISP, //����� ������ ����� Ȯ��
    INM, //�Ϲ� ������ ����(ä������, �Ϲݾ����� ���� ����)
    NPCTK, //NPC ��ȭ Ȯ��
    REIT, //ģ�е� ��ġ Ȯ��
    IVFR, //���� ������ ���� ����
    IVBG, //���� ������ ���� ����
    IVFI, //����� ������ ���� ����
    IVCK, //�丮 ������ �ϼ� ������ ��
    IVFU //���� ������ ���� Ȯ��
}

public class SideStoryDialogue
{ 
    public string StoryID { get; private set; }
    public string StroyName { get; private set; }
    public int RequiredIntimacy { get; private set; }
    public EventType EventType { get; private set; }
    public string EventTypeCondition { get; private set; }
    public SideDialogueData[] DialogueData { get; private set; }
    public EventType RewardType { get; private set; }
    public string RewardID { get; private set; }
    public int RewardCount { get; private set; }
    public int RewardIntimacy { get; private set; }

    public SideStoryDialogue(string storyID, string stroyName, int requiredIntimacy, EventType eventType, string eventTypeCondition, SideDialogueData[] dialogueData, EventType rewardType, string rewardID, int rewardCount, int rewardIntimacy)
    {
        StoryID = storyID;
        StroyName = stroyName;
        RequiredIntimacy = requiredIntimacy;
        EventType = eventType;
        EventTypeCondition = eventTypeCondition;
        DialogueData = dialogueData;
        RewardType = rewardType;
        RewardID = rewardID;
        RewardCount = rewardCount;
        RewardIntimacy = rewardIntimacy;
    }
}

public class SideDialogueData
{
    public string TalkPandaID { get; private set; }
    public string Contexts { get; private set; }
    public bool isComplete { get; private set; }
    public string ChoiceContextA { get; private set; }
    public string ChoiceAID { get; private set; }
    public string ChoiceContextB { get; private set; }
    public string ChoiceBID { get; private set; }

    public SideDialogueData(string talkPandaID, string contexts, bool isComplete, string choiceContextA, string choiceAID, string choiceContextB, string choiceBID)
    {
        TalkPandaID = talkPandaID;
        Contexts = contexts;
        this.isComplete = isComplete;
        ChoiceContextA = choiceContextA;
        ChoiceAID = choiceAID;
        ChoiceContextB = choiceContextB;
        ChoiceBID = choiceBID;
    }
}
