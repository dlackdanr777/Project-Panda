using System;
using System.Collections.Generic;
using UnityEngine;

public class MainStoryDialogueManager : MonoBehaviour
{
    public Dictionary<string, MainStoryDialogue> MSDic = new Dictionary<string, MainStoryDialogue>();
    private List<Dictionary<string, object>> _dataMainStory = new List<Dictionary<string, object>>();

    private List<string> _storyCompletedList = new List<string>();
    public List<string> StoryCompletedList => _storyCompletedList;

    public List<string> PoyaStoryList = new List<string>();
    public List<string> JijiStoryList = new List<string>();

    private bool _isExistPoya;
    private bool _isExistJiji;

    public string CurrentStoryID;

    private int _goTo = 0;


    public void SetCompletedStoryList(List<string> storyCompletedList)
    {
        _storyCompletedList.Clear();
        _storyCompletedList = storyCompletedList;
    }


    public void Register()
    {
        string csv;
        ReadCSV("01__TEST_1_1_2 (1)");
        for (int i = 2; i <= 36; i++)
        {
            if (i == 4 || i == 22 || i == 23) // 현재 없는 파일
            {
                continue;
            }
            if (i < 10)
            {
                csv = "NPCSideStory/NPC0" + i;
            }
            else
            {
                csv = "NPCSideStory/NPC" + i;
            }
            ReadCSV(csv);
        }
        ReadCSV("NPCSideStory/NPC46");

    }

    private void ReadCSV(string csv)
    {
        _dataMainStory = CSVReader.Read(csv); // 이름 나중에 수정
        for (int i = 0; i < _dataMainStory.Count; i++)
        {
            _isExistPoya = false;
            _isExistJiji = false;
            MSDic.Add(_dataMainStory[i]["StoryID"].ToString(),
                        new MainStoryDialogue(_dataMainStory[i]["StoryID"].ToString(),
                        _dataMainStory[i]["StoryName"].ToString(),
                        _dataMainStory[i]["PreviousStoryID"].ToString(),
                        _dataMainStory[i]["NextStoryID"].ToString(),
                        GetIntType(_dataMainStory[i]["Intimacy"]),
                        GetMainEventType(_dataMainStory[i]["EventType"].ToString()),
                        _dataMainStory[i]["EventTypeCondition"].ToString(),
                        GetIntType(_dataMainStory[i]["EventTypeQuantity"]),
                        _dataMainStory[i]["StoryStartPoint"].ToString(),
                        _dataMainStory[i]["MapID"].ToString(),
                        GetContext(i),
                        GetMainEventType(_dataMainStory[i]["ClearRewardType"].ToString()),
                        _dataMainStory[i]["RewardItemID"].ToString(),
                        GetIntType(_dataMainStory[i]["RewardQuantity"]),
                        GetIntType(_dataMainStory[i]["ClearRewardIntimacy"])));
            if (_isExistPoya)
            {
                PoyaStoryList.Add(_dataMainStory[i]["StoryID"].ToString());
            }
            if (_isExistJiji)
            {
                JijiStoryList.Add(_dataMainStory[i]["StoryID"].ToString());
            }
            i = _goTo; //대화 뛰어 넘기
        }
    }

    private List<MainDialogueData> GetContext(int i)
    {
        List<MainDialogueData> dialogue = new List<MainDialogueData>();
        string current = _dataMainStory[i]["StoryID"].ToString();

        while (true)
        {
            if (i == _dataMainStory.Count || !_dataMainStory[i]["StoryID"].ToString().Equals(current))
            {
                _goTo = i - 1;
                break;
            }
            dialogue.Add(new MainDialogueData(_dataMainStory[i]["TalkPandaID"].ToString(),
            _dataMainStory[i]["Conversation"].ToString(),
            _dataMainStory[i]["CheckStorySuccessOrFailure"].ToString(),
            _dataMainStory[i]["ConversationOptionID"].ToString(),
            _dataMainStory[i]["OptionA_Text"].ToString(),
            _dataMainStory[i]["OptionA_ConversationOptionID"].ToString(),
            _dataMainStory[i]["OptionB_Text"].ToString(),
            _dataMainStory[i]["OptionB_ConversationOptionID"].ToString(),
            GetMainEventType(_dataMainStory[i]["EventType"].ToString()),
            _dataMainStory[i]["EventTypeCondition"].ToString(),
            GetIntType(_dataMainStory[i]["EventTypeQuantity"])));

            if(_dataMainStory[i]["TalkPandaID"].ToString() == "POYA00"){
                _isExistPoya = true;
            }
            else if(_dataMainStory[i]["TalkPandaID"].ToString() == "NPC01")
            {
                _isExistJiji = true;
            }

            i++;
        }


        return dialogue;
    }

    private MainEventType GetMainEventType(string str)
    {
        if (str.Equals(""))
        {
            return MainEventType.None;
        }
        return (MainEventType)Enum.Parse(typeof(MainEventType), str);
    }

    //private EventType GetEventType(string str)
    //{
    //    if (str.Equals(""))
    //    {
    //        return EventType.None;
    //    }
    //    return (EventType)Enum.Parse(typeof(EventType), str);
    //}

    private int GetIntType(object obj)
    {
        if (obj.Equals(""))
            return 0;
        return (int)obj;
    }
}
