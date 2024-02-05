using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

public class SideStoryController : MonoBehaviour, IInteraction
{
    public static event Action<SideStoryDialogue> OnStartInteractionHandler;

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
        
        _storyDatabase = DatabaseManager.Instance.SideDialogueDatabase.SSDic[_storyCode];
        foreach (var key in _storyDatabase.Keys)
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
            SideStoryDialogue currentStory = _storyDatabase[_storyKey[i]];
            if (!currentStory.IsSuccess) //스토리를 완료하지 않았다면
            {
                //이전 스토리 다음 스토리 비교
                bool priorCheck = CheckPrior(currentStory.PriorStoryID); //처음 시작이면 무조건 true로 넘어갈 수 있도록
                int nextCheck = CheckNext(currentStory.NextStoryID); //마지막이면 친밀도를 max로 해서 성공여부를 true로 할 수 없도록

                if (priorCheck && //이전 스토리 성공했는지 비교
                    DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= nextCheck) //다음 스토리와 친밀도 비교
                {
                    _storyDatabase[_storyKey[i]].IsSuccess = true; 

                    continue; //성공한 스토리면 다음 스토리로 이동
                }

                //성공한 스토리가 아니면
                if (DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy < CheckNext(currentStory.NextStoryID)) //다음 스토리보다 친밀도 작은지 비교
                {       
                    if (currentStory.EventType != EventType.None)// 이벤트 조건이 있으면
                    {
                        //조건 비교
                        if (!CheckCondition(currentStory.EventType, currentStory.EventTypeCondition, currentStory.EventTypeAmount))
                        {
                            i--;
                            currentStory = _storyDatabase[_storyKey[i]];
                        }
                    }

                    string printResult = PrintContext(currentStory.DialogueData);
                    //GetContext(currentStory);
                    if (printResult.Equals("DONE") || printResult.Equals("")) //대화 출력
                    {
                        AddReward(i); //보상       
                    }
                    break;
                }
            }

        }
    }

    private bool CheckPrior(string priorStory)
    {
        if (priorStory != null)
        {
            if (_storyDatabase.ContainsKey(priorStory)) //해당 데이터베이스에 있는 id인지 확인 -> 없으면 처음 => 무조건 통과
            {
                return _storyDatabase[priorStory].IsSuccess;
            }
        }
        else
        {
            return false;
        }
       
        return true;
    }

    private int CheckNext(string nextStory)
    {
        if (nextStory != null)
        { 
            if (_storyDatabase.ContainsKey(nextStory)) //해당 데이터베이스에 있는 id인지 확인 -> 없으면 마지막 => 넘어가지 못하도록 MaxValue
            {
                return _storyDatabase[nextStory].RequiredIntimacy;
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
        switch (type)
        {
            case EventType.GISP:
                return GameManager.Instance.Player.GetGISP(condition, count);
            case EventType.INM:
                return GameManager.Instance.Player.GetINM(condition);
            case EventType.MONEY:
                return GameManager.Instance.Player.GetMONEY(count);
            case EventType.REIT:
                return DatabaseManager.Instance.GetNPC(condition).Intimacy >= count;
            case EventType.IVGI:
                return GameManager.Instance.Player.GetIVGI(condition, count);
            case EventType.IVCK:
                return GameManager.Instance.Player.GetIVCK(condition, count);
            default:
                return false;
        }
    }

    private string PrintContext(List<SideDialogueData> data)
    {

        for (int i = 0; i < data.Count; i++) //대화 출력
        {
            string result = "";
            Debug.Log(data[i].Contexts);

            //선택지 텍스트
            if (!data[i].ChoiceContextA.Equals("") && !data[i].ChoiceContextB.Equals(""))
            {
                Debug.Log(data[i].ChoiceContextA);
                Debug.Log(data[i].ChoiceContextB);

                if (Input.GetKeyDown(KeyCode.A)) //왼쪽 누르면
                {
                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (data[j].ChoiceID.Equals(data[i].ChoiceAID)) //대화지 ID와 선택지 ID가 같다면 그곳으로 이동
                        {
                            i = j;
                            continue;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S)) //오른쪽 누르면
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

            //성공 여부 확인
            if (data[i].IsComplete.Equals("FAIL"))
            {
                result = "FAIL";
            }
            else if (data[i].IsComplete.Equals("DONE"))
            {
                result = "DONE";
            }

            if (data[i].IsComplete.Equals("END"))
            {
                return result;
            }
        }

        return null;
    }

    private void GetContext(SideStoryDialogue data)
    {
        OnStartInteractionHandler?.Invoke(data);
    }

    private void AddReward(int index)
    {
        var npcList = DatabaseManager.Instance.GetNPCList();
        var currentNPC = npcList[_npcCode - 1];
        var nextStory = _storyDatabase[_storyKey[index + 1]];

        DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy += _storyDatabase[_storyKey[index]].RewardIntimacy; //보상 친밀도

        if (currentNPC.Intimacy > nextStory.RequiredIntimacy)
        {
            DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy = nextStory.RequiredIntimacy;
        }

        Debug.Log(currentNPC.Name + " " + currentNPC.Intimacy);
    }
}
