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
                    dictionary.Add(_dataSideStory[id][j]["스토리ID"].ToString(),
                        new SideStoryDialogue(_dataSideStory[id][j]["스토리ID"].ToString(),
                        _dataSideStory[id][j]["스토리 이름"].ToString(),
                        _dataSideStory[id][j]["이전 스토리ID"].ToString(),
                        _dataSideStory[id][j]["다음 스토리ID"].ToString(),
                        GetIntType(_dataSideStory[id][j]["친밀도"]),
                        GetEventType(_dataSideStory[id][j]["이벤트 타입"].ToString()),
                        _dataSideStory[id][j]["이벤트 타입 조건"].ToString(),
                        GetContext(id, j),
                        GetEventType(_dataSideStory[id][j]["클리어 보상 타입"].ToString()),
                        _dataSideStory[id][j]["보상 아이템 ID"].ToString(),
                        GetIntType(_dataSideStory[id][j]["보상 수량"]),
                        GetIntType(_dataSideStory[id][j]["클리어 보상 친밀도"])));
                    j = _goTo; //대화 뛰어 넘기
                }

                SSDic.Add(id, dictionary);
            }
            catch (Exception e) //사이드스토리가 없는 NPC
            {
                Debug.Log(e.ToString());
            }
        }
    }

    private List<SideDialogueData> GetContext(string i, int j)
    {
        List<SideDialogueData> dialogue = new List<SideDialogueData>();
        string current = _dataSideStory[i][j]["스토리ID"].ToString();

        while(true)
        {
            Debug.Log("j" + j);
            if (j==_dataSideStory[i].Count || !_dataSideStory[i][j]["스토리ID"].ToString().Equals(current))
            {
                _goTo = j - 1;
                break;
            }
            Debug.Log(current  + _dataSideStory[i][j]["스토리ID"].ToString());
            Debug.Log(j+_dataSideStory[i][j]["일반 대화"].ToString());
            dialogue.Add(new SideDialogueData(_dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["일반 대화"].ToString(),
            GetComplete(_dataSideStory[i][j]["스토리 성공 실패 체크"].ToString()),
            _dataSideStory[i][j]["대화 선택지 ID"].ToString(),
            _dataSideStory[i][j]["선택A_텍스트"].ToString(),
            _dataSideStory[i][j]["선택지A_대화 이동 ID"].ToString(),
            _dataSideStory[i][j]["선택B_텍스트"].ToString(),
            _dataSideStory[i][j]["선택지B_대화 이동 ID"].ToString()));
            j++;
        } 

         
        return dialogue;
    }

    private bool GetComplete(string result)
    {
        if (result.Equals("FAIL"))
        {
            return false;
        }
        else //DONE
        {
            return true;
        }
        
    }

    private EventType GetEventType(string str)
    {
        if (str.Equals(""))
        {
            return EventType.None;
        }
        return (EventType)Enum.Parse(typeof(EventType), str);
    }

    private int? GetIntType(object obj)
    {
        if (obj.Equals(""))
            return null;
        return (int?)obj;
    }
}
