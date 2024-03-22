using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public enum MainEventType
{
    None = -1,
    HOLDITEM, // 아이템 소지 확인
    GIVEITEM, // 소지한 아이템 NPC에게 줌
    QUESTITEM, // 퀘스트 아이템 획득
    LOVEMOUNT, // 친밀도가 일정량 이상일 때
    MONEY

}

public class MainStoryDialogue
{
    public string StoryID { get; private set; }
    public string StroyName { get; private set; }
    public string PriorStoryID { get; private set; }
    public string NextStoryID { get; private set; }
    public int RequiredIntimacy { get; private set; }
    public MainEventType EventType { get; private set; }
    public string EventTypeCondition { get; private set; }
    public int EventTypeAmount { get; private set; }
    public List<MainDialogueData> DialogueData { get; private set; }
    public MainEventType RewardType { get; private set; }
    public string RewardID { get; private set; }
    public int RewardCount { get; private set; }
    public int RewardIntimacy { get; private set; }
    public bool IsSuccess { get; set; }

    public MainStoryDialogue(string storyID, string stroyName, string priorStoryID, string nextStoryID, int requiredIntimacy, MainEventType eventType, string eventTypeCondition, int eventTypeAmount, List<MainDialogueData> dialogueData, MainEventType rewardType, string rewardID, int rewardCount, int rewardIntimacy)
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

public class MainDialogueData
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
    public MainEventType EventType { get; private set; }
    public string EventTypeCondition { get; private set; }
    public int EventTypeAmount { get; private set; }

    public MainDialogueData(string talkPandaID, string contexts, string isComplete, string choiceID, string choiceContextA, string choiceAID, string choiceContextB, string choiceBID, MainEventType eventType, string eventTypeCondition, int eventTypeAmount)
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
        EventType = eventType;
        EventTypeCondition = eventTypeCondition;
        EventTypeAmount = eventTypeAmount;
    }
}
