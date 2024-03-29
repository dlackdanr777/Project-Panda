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
        _dataMainStory = CSVReader.Read("01__TEST_1_1"); // �̸� ���߿� ����
        for(int i = 0; i< _dataMainStory.Count; i++)
        {
            _isExistPoya = false;
            _isExistJiji = false;
            MSDic.Add(_dataMainStory[i]["���丮ID"].ToString(),
                        new MainStoryDialogue(_dataMainStory[i]["���丮ID"].ToString(),
                        _dataMainStory[i]["���丮 �̸�"].ToString(),
                        _dataMainStory[i]["���� ���丮ID"].ToString(),
                        _dataMainStory[i]["���� ���丮ID"].ToString(),
                        GetIntType(_dataMainStory[i]["ģ�е�"]),
                        GetMainEventType(_dataMainStory[i]["�̺�Ʈ Ÿ��"].ToString()),
                        _dataMainStory[i]["�̺�Ʈ Ÿ�� ����"].ToString(),
                        GetIntType(_dataMainStory[i]["�̺�Ʈ Ÿ�� ����"]),
                        _dataMainStory[i]["���丮 ��ŸƮ ����Ʈ"].ToString(),
                        _dataMainStory[i]["�� ID"].ToString(),
                        GetContext(i),
                        GetMainEventType(_dataMainStory[i]["Ŭ���� ���� Ÿ��"].ToString()),
                        _dataMainStory[i]["���� ������ ID"].ToString(),
                        GetIntType(_dataMainStory[i]["���� ����"]),
                        GetIntType(_dataMainStory[i]["Ŭ���� ���� ģ�е�"])));
            if(_isExistPoya)
            {
                PoyaStoryList.Add(_dataMainStory[i]["���丮ID"].ToString());
            }
            if(_isExistJiji)
            {
                JijiStoryList.Add(_dataMainStory[i]["���丮ID"].ToString());
            }
            i = _goTo; //��ȭ �پ� �ѱ�
        }
    }

    private List<MainDialogueData> GetContext(int i)
    {
        List<MainDialogueData> dialogue = new List<MainDialogueData>();
        string current = _dataMainStory[i]["���丮ID"].ToString();

        while (true)
        {
            if (i == _dataMainStory.Count || !_dataMainStory[i]["���丮ID"].ToString().Equals(current))
            {
                _goTo = i - 1;
                break;
            }
            dialogue.Add(new MainDialogueData(_dataMainStory[i]["talkPandaID"].ToString(),
            _dataMainStory[i]["�Ϲ� ��ȭ"].ToString(),
            _dataMainStory[i]["���丮 ���� ���� üũ"].ToString(),
            _dataMainStory[i]["��ȭ ������ ID"].ToString(),
            _dataMainStory[i]["����A_�ؽ�Ʈ"].ToString(),
            _dataMainStory[i]["������A_��ȭ �̵� ID"].ToString(),
            _dataMainStory[i]["����B_�ؽ�Ʈ"].ToString(),
            _dataMainStory[i]["������B_��ȭ �̵� ID"].ToString(),
            GetMainEventType(_dataMainStory[i]["�̺�Ʈ Ÿ��"].ToString()),
            _dataMainStory[i]["�̺�Ʈ Ÿ�� ����"].ToString(),
            GetIntType(_dataMainStory[i]["�̺�Ʈ Ÿ�� ����"])));

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
