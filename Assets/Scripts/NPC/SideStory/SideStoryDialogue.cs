using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    IFRSP, //과일 아이템 스페셜 확인
    IBGSP, //곤충 아이템 스페셜 확인
    IFISP, //물고기 아이템 스페셜 확인
    INM, //일반 아이템 소지(채집도구, 일반아이템 전부 포함)
    NPCTK, //NPC 대화 확인
    REIT, //친밀도 수치 확인
    IVFR, //과일 아이템 소지 개수
    IVBG, //곤충 아이템 소지 개수
    IVFI, //물고기 아이템 소지 개수
    IVCK, //요리 레시피 완성 아이템 비교
    IVFU //가구 아이템 소지 확인
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
