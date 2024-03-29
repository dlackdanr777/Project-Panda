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
        _dataMainStory = CSVReader.Read("01__TEST_1_1"); // 이름 나중에 수정
        for(int i = 0; i< _dataMainStory.Count; i++)
        {
            _isExistPoya = false;
            _isExistJiji = false;
            MSDic.Add(_dataMainStory[i]["스토리ID"].ToString(),
                        new MainStoryDialogue(_dataMainStory[i]["스토리ID"].ToString(),
                        _dataMainStory[i]["스토리 이름"].ToString(),
                        _dataMainStory[i]["이전 스토리ID"].ToString(),
                        _dataMainStory[i]["다음 스토리ID"].ToString(),
                        GetIntType(_dataMainStory[i]["친밀도"]),
                        GetMainEventType(_dataMainStory[i]["이벤트 타입"].ToString()),
                        _dataMainStory[i]["이벤트 타입 조건"].ToString(),
                        GetIntType(_dataMainStory[i]["이벤트 타입 수량"]),
                        _dataMainStory[i]["스토리 스타트 포인트"].ToString(),
                        _dataMainStory[i]["맵 ID"].ToString(),
                        GetContext(i),
                        GetMainEventType(_dataMainStory[i]["클리어 보상 타입"].ToString()),
                        _dataMainStory[i]["보상 아이템 ID"].ToString(),
                        GetIntType(_dataMainStory[i]["보상 수량"]),
                        GetIntType(_dataMainStory[i]["클리어 보상 친밀도"])));
            if(_isExistPoya)
            {
                PoyaStoryList.Add(_dataMainStory[i]["스토리ID"].ToString());
            }
            if(_isExistJiji)
            {
                JijiStoryList.Add(_dataMainStory[i]["스토리ID"].ToString());
            }
            i = _goTo; //대화 뛰어 넘기
        }
    }

    private List<MainDialogueData> GetContext(int i)
    {
        List<MainDialogueData> dialogue = new List<MainDialogueData>();
        string current = _dataMainStory[i]["스토리ID"].ToString();

        while (true)
        {
            if (i == _dataMainStory.Count || !_dataMainStory[i]["스토리ID"].ToString().Equals(current))
            {
                _goTo = i - 1;
                break;
            }
            dialogue.Add(new MainDialogueData(_dataMainStory[i]["talkPandaID"].ToString(),
            _dataMainStory[i]["일반 대화"].ToString(),
            _dataMainStory[i]["스토리 성공 실패 체크"].ToString(),
            _dataMainStory[i]["대화 선택지 ID"].ToString(),
            _dataMainStory[i]["선택A_텍스트"].ToString(),
            _dataMainStory[i]["선택지A_대화 이동 ID"].ToString(),
            _dataMainStory[i]["선택B_텍스트"].ToString(),
            _dataMainStory[i]["선택지B_대화 이동 ID"].ToString(),
            GetMainEventType(_dataMainStory[i]["이벤트 타입"].ToString()),
            _dataMainStory[i]["이벤트 타입 조건"].ToString(),
            GetIntType(_dataMainStory[i]["이벤트 타입 수량"])));

            if(_dataMainStory[i]["talkPandaID"].ToString() == "POYA00"){
                _isExistPoya = true;
            }
            else if(_dataMainStory[i]["talkPandaID"].ToString() == "NPC01")
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
