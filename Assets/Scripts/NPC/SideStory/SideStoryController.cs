using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

public class SideStoryController : MonoBehaviour, IInteraction
{
    [SerializeField] private string NPCID;

    private Dictionary<string, SideStoryDialogue> _storyDatabase;
    private List<string> _storyKey = new List<string>();
    private string _storyCode;
    private int _npcCode;

    // Start is called before the first frame update
    void Start()
    {
        _npcCode = int.Parse(NPCID.Substring(3));
        _storyCode = "SS" + NPCID.Substring(3);
        foreach (var key in DatabaseManager.Instance.SideDialogueDatabase.SSDic.Keys)
        {
            Debug.Log(key);
        }
        _storyDatabase = DatabaseManager.Instance.SideDialogueDatabase.SSDic[_storyCode];

        foreach (var key in DatabaseManager.Instance.SideDialogueDatabase.SSDic[_storyCode].Keys)
        {
            _storyKey.Add(key);
        }
    }

    public void StartInteraction()
    {
        StartSideStory();
    }

    public void UpdateInteraction()
    {
    }
    public void ExitInteraction()
    {
    }

    private void StartSideStory()
    {
        for (int i = 0; i < _storyKey.Count; i++)
        {
            if (!_storyDatabase[_storyKey[i]].IsSuccess) //스토리를 완료하지 않았다면
            {
                //이전 스토리 다음 스토리 비교
                bool priorCheck = CheckPrior(i, _storyDatabase[_storyKey[i]].PriorStoryID); //처음 시작이면 무조건 true로 넘어갈 수 있도록
                int? nextCheck = CheckNext(i, _storyDatabase[_storyKey[i]].NextStoryID); //마지막이면 친밀도를 max로 해서 성공여부를 true로 할 수 없도록

                if (priorCheck && //이전 스토리 성공했는지 비교
                    DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= nextCheck) //다음 스토리와 친밀도 비교
                {
                    _storyDatabase[_storyKey[i]].IsSuccess = true;

                    continue; //성공한 스토리면 다음 스토리로 이동
                }

                //성공한 스토리가 아니면
                if (DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= _storyDatabase[_storyKey[i]].RequiredIntimacy) //친밀도 비교
                {
                    if (_storyDatabase[_storyKey[i]].EventType != EventType.None)// 이벤트 조건이 있으면
                    {
                        //조건 비교
                        if (!CheckCondition(_storyDatabase[_storyKey[i]].EventType, _storyDatabase[_storyKey[i]].EventTypeCondition, _storyDatabase[_storyKey[i]].EventTypeAmount))
                        {
                            i--; //조건이 만족하지 못했으므로 전 대화 출력
                        }
                    }

                    if(PrintContext(_storyDatabase[_storyKey[i]].DialogueData)) //대화 출력
                    {
                        AddReward(i); //보상
                    }
                    break;
                }

            }

        }
    }

    private bool CheckPrior(int index, string priorStory)
    {
        if (priorStory != null)
        {
            if (_storyDatabase.ContainsKey(priorStory)) //해당 데이터베이스에 있는 id인지 확인 -> 없으면 처음 => 무조건 통과
            {
                return _storyDatabase[_storyDatabase[_storyKey[index]].PriorStoryID].IsSuccess;
            }
        }
        else
        {
            return false;
        }
       
        return true;
    }

    private int? CheckNext(int index, string nextStory)
    {
        if (nextStory != null)
        { 
            if (_storyDatabase.ContainsKey(nextStory)) //해당 데이터베이스에 있는 id인지 확인 -> 없으면 마지막 => 넘어가지 못하도록 MaxValue
            {
                return _storyDatabase[_storyDatabase[_storyKey[index]].NextStoryID].RequiredIntimacy;
            }
        }
        else
        {
            return -1;
        }
        return int.MaxValue;
    }

    private bool CheckCondition(EventType type, string condition, int count)
    {
        //조건 비교
        bool result = false;
        switch (type)
        {
            case EventType.GISP:
                result = GameManager.Instance.Player.GetGISP(condition, count); //스페셜이 갯수만큼 있는지 확인
                break;
            case EventType.INM:
                result = GameManager.Instance.Player.GetINM(condition);
                break;
            case EventType.MONEY:
                result = GameManager.Instance.Player.GetMONEY(count);
                break;
            case EventType.NPCTK: //어떤 아이템이나 그런 것이 필요 그냥 대화만 비교하기엔 어려움이 있음
                break;
            case EventType.REIT: //얼마만큼 비교할지 필요 => 현재 대화하는 npc보다 친밀도가 높아야함
                if (DatabaseManager.Instance.GetNPC(condition).Intimacy >= count)
                {
                    result = true;
                }
                break;
            case EventType.IVGI:
                result = GameManager.Instance.Player.GetIVGI(condition, count);
                break;
            case EventType.IVCK:
                result = GameManager.Instance.Player.GetIVCK(condition, count);
                break;
            case EventType.IVFU:
                break;

        }

        return result;
    }

    private bool PrintContext(List<SideDialogueData> data)
    {

        for (int i = 0; i < data.Count; i++) //대화 출력
        {
            Debug.Log(data[i].Contexts);
            
            //성공 여부 확인
            if (data[i].IsComplete.Equals("FAIL"))
            {
                return false;
            }
            else if (data[i].IsComplete.Equals("Done"))
            {
                return true;
            }

            //선택지 텍스트
            if (!data[i].ChoiceContextA.Equals("") && !data[i].ChoiceContextB.Equals(""))
            {
                Debug.Log(data[i].ChoiceContextA);
                Debug.Log(data[i].ChoiceContextB);

                if (Input.GetKeyDown(KeyCode.A)) //왼쪽 누르면
                {
                    for(int j = i+1; j < data.Count; j++)
                    {
                        if(data[j].ChoiceID.Equals(data[i].ChoiceAID)) //대화지 ID와 선택지 ID가 같다면 그곳으로 이동
                        {
                            i = j;
                            continue;
                        }
                    }
                }
                else if(Input.GetKeyDown(KeyCode.S)) //오른쪽 누르면
                {
                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (data[j].ChoiceID.Equals(data[i].ChoiceBID)) //대화지 ID와 선택지 ID가 같다면 그곳으로 이동
                        {
                            i = j;
                            continue;
                        }
                    }
                }
            }

        }
        return true;
    }

    private void AddReward(int index)
    {
        DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy += _storyDatabase[_storyKey[index]].RewardIntimacy; //보상 친밀도
        if(DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy > _storyDatabase[_storyKey[index + 1]].RequiredIntimacy)
        {
            DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy = _storyDatabase[_storyKey[index + 1]].RequiredIntimacy;
        }
        Debug.Log(DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Name + " " + DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy);

    }
}
