using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SideStoryDialogueManager : MonoBehaviour
{
    public Dictionary<string, Dictionary<string, SideStoryDialogue>> SSDic = new Dictionary<string, Dictionary<string, SideStoryDialogue>>();
    private Dictionary<string, List<Dictionary<string, object>>> _dataSideStory = new Dictionary<string, List<Dictionary<string, object>>>();

    private int _goTo = 0;

    public void Register()
    { 
        for(int i = 0; i < DatabaseManager.Instance.GetNPCList().Count; i++)
        {
            string id = "SS" + string.Format("{0:00}", (i + 1));
            try
            {
                _dataSideStory.Add(id, CSVReader.Read("SideStory/" + id)); //SS01 ~ 

                Dictionary<string, SideStoryDialogue> dictionary = new Dictionary<string, SideStoryDialogue>();
                for (int j = 0; j < _dataSideStory[id].Count; j++)
                {
                    dictionary.Add(_dataSideStory[id][j]["���丮ID"].ToString(),
                        new SideStoryDialogue(_dataSideStory[id][j]["���丮ID"].ToString(),
                        _dataSideStory[id][j]["���丮 �̸�"].ToString(),
                        _dataSideStory[id][j]["���� ���丮ID"].ToString(),
                        _dataSideStory[id][j]["���� ���丮ID"].ToString(),
                        GetIntType(_dataSideStory[id][j]["ģ�е�"]),
                        GetEventType(_dataSideStory[id][j]["�̺�Ʈ Ÿ��"].ToString()),
                        _dataSideStory[id][j]["�̺�Ʈ Ÿ�� ����"].ToString(),
                        GetIntType(_dataSideStory[id][j]["�̺�Ʈ Ÿ�� ����"]),
                        GetContext(id, j),
                        GetEventType(_dataSideStory[id][j]["Ŭ���� ���� Ÿ��"].ToString()),
                        _dataSideStory[id][j]["���� ������ ID"].ToString(),
                        GetIntType(_dataSideStory[id][j]["���� ����"]),
                        GetIntType(_dataSideStory[id][j]["Ŭ���� ���� ģ�е�"])));
                    j = _goTo; //��ȭ �پ� �ѱ�
                }

                SSDic.Add(id, dictionary);
            }
            catch {} //���̵彺�丮�� ���� NPC
        }
    }

    private List<SideDialogueData> GetContext(string i, int j)
    {
        List<SideDialogueData> dialogue = new List<SideDialogueData>();
        string current = _dataSideStory[i][j]["���丮ID"].ToString();

        while(true)
        {
            if (j==_dataSideStory[i].Count || !_dataSideStory[i][j]["���丮ID"].ToString().Equals(current))
            {
                _goTo = j - 1;
                break;
            }
            dialogue.Add(new SideDialogueData(_dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["�Ϲ� ��ȭ"].ToString(),
            _dataSideStory[i][j]["���丮 ���� ���� üũ"].ToString(),
            _dataSideStory[i][j]["��ȭ ������ ID"].ToString(),
            _dataSideStory[i][j]["����A_�ؽ�Ʈ"].ToString(),
            _dataSideStory[i][j]["������A_��ȭ �̵� ID"].ToString(),
            _dataSideStory[i][j]["����B_�ؽ�Ʈ"].ToString(),
            _dataSideStory[i][j]["������B_��ȭ �̵� ID"].ToString()));
            j++;
        } 

         
        return dialogue;
    }

    private EventType GetEventType(string str)
    {
        if (str.Equals(""))
        {
            return EventType.None;
        }
        return (EventType)Enum.Parse(typeof(EventType), str);
    }

    private int GetIntType(object obj)
    {
        if (obj.Equals(""))
            return 0;
        return (int)obj;
    }
}
