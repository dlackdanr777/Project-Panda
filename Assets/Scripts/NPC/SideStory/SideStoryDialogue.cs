using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None = -1,
    GISP, //과일 아이템 스페셜 확인
    INM, //일반 아이템 소지(채집도구, 일반아이템 전부 포함)
    MONEY, //돈
    NPCTK, //NPC 대화 확인
    REIT, //친밀도 수치 확인
    IVGI, //채집 아이템 소지 개수
    IVCK, //요리 레시피 완성 아이템 비교
    IVFU //가구 아이템 소지 확인
}

public class SideStoryDialogue
{ 
    public string StoryID { get; private set; }
    public string StroyName { get; private set; }
    public string PriorStoryID { get; private set; }
    public string NextStoryID { get; private set; }
    public int RequiredIntimacy { get; private set; }
    public EventType EventType { get; private set; }
    public string EventTypeCondition { get; private set; }
    public int EventTypeAmount { get; private set; }   
    public List<SideDialogueData> DialogueData { get; private set; }
    public EventType RewardType { get; private set; }
    public string RewardID { get; private set; }
    public int RewardCount { get; private set; }
    public int RewardIntimacy { get; private set; }
    public bool IsSuccess;

    public SideStoryDialogue(string storyID, string stroyName, string priorStoryID, string nextStoryID, int requiredIntimacy, EventType eventType, string eventTypeCondition, int eventTypeAmount, List<SideDialogueData> dialogueData, EventType rewardType, string rewardID, int rewardCount, int rewardIntimacy)
    {
        StoryID = storyID;
        StroyName = stroyName;
        PriorStoryID = priorStoryID;
        NextStoryID = nextStoryID;
        RequiredIntimacy = requiredIntimacy;
        EventType = eventType;
        EventTypeCondition = eventTypeCondition;
        EventTypeAmount = eventTypeAmount;
        DialogueData = dialogueData;
        RewardType = rewardType;
        RewardID = rewardID;
        RewardCount = rewardCount;
        RewardIntimacy = rewardIntimacy;
        IsSuccess = false;
    }
}

public class SideDialogueData
{
    public string TalkPandaID { get; private set; }
    public string Contexts { get; private set; }
    public string IsComplete { get; private set; }
    public string ChoiceID { get; private set; }
    public string ChoiceContextA { get; private set; }
    public string ChoiceAID { get; private set; }
    public string ChoiceContextB { get; private set; }
    public string ChoiceBID { get; private set; }
    public bool CanChoice { get; private set; }

    public SideDialogueData(string talkPandaID, string contexts, string isComplete, string choiceID, string choiceContextA, string choiceAID, string choiceContextB, string choiceBID)
    {
        TalkPandaID = talkPandaID;
        Contexts = contexts;
        IsComplete = isComplete;
        ChoiceID = choiceID;
        ChoiceContextA = choiceContextA;
        ChoiceAID = choiceAID;
        ChoiceContextB = choiceContextB;
        ChoiceBID = choiceBID;
        if (!ChoiceContextA.Equals("") && !ChoiceContextB.Equals(""))
        {
            CanChoice = true;
        }
    }
}
