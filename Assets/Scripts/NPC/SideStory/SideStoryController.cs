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
            if (!currentStory.IsSuccess) //���丮�� �Ϸ����� �ʾҴٸ�
            {
                //���� ���丮 ���� ���丮 ��
                bool priorCheck = CheckPrior(currentStory.PriorStoryID); //ó�� �����̸� ������ true�� �Ѿ �� �ֵ���
                int nextCheck = CheckNext(currentStory.NextStoryID); //�������̸� ģ�е��� max�� �ؼ� �������θ� true�� �� �� ������

                if (priorCheck && //���� ���丮 �����ߴ��� ��
                    DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= nextCheck) //���� ���丮�� ģ�е� ��
                {
                    _storyDatabase[_storyKey[i]].IsSuccess = true; 

                    continue; //������ ���丮�� ���� ���丮�� �̵�
                }

                //������ ���丮�� �ƴϸ�
                if (DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy < CheckNext(currentStory.NextStoryID)) //���� ���丮���� ģ�е� ������ ��
                {       
                    if (currentStory.EventType != EventType.None)// �̺�Ʈ ������ ������
                    {
                        //���� ��
                        if (!CheckCondition(currentStory.EventType, currentStory.EventTypeCondition, currentStory.EventTypeAmount))
                        {
                            i--;
                            currentStory = _storyDatabase[_storyKey[i]];
                        }
                    }

                    string printResult = PrintContext(currentStory.DialogueData);
                    //GetContext(currentStory);
                    if (printResult.Equals("DONE") || printResult.Equals("")) //��ȭ ���
                    {
                        AddReward(i); //����       
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
            if (_storyDatabase.ContainsKey(priorStory)) //�ش� �����ͺ��̽��� �ִ� id���� Ȯ�� -> ������ ó�� => ������ ���
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
            if (_storyDatabase.ContainsKey(nextStory)) //�ش� �����ͺ��̽��� �ִ� id���� Ȯ�� -> ������ ������ => �Ѿ�� ���ϵ��� MaxValue
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
        //���� ��
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

        for (int i = 0; i < data.Count; i++) //��ȭ ���
        {
            string result = "";
            Debug.Log(data[i].Contexts);

            //������ �ؽ�Ʈ
            if (!data[i].ChoiceContextA.Equals("") && !data[i].ChoiceContextB.Equals(""))
            {
                Debug.Log(data[i].ChoiceContextA);
                Debug.Log(data[i].ChoiceContextB);

                if (Input.GetKeyDown(KeyCode.A)) //���� ������
                {
                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (data[j].ChoiceID.Equals(data[i].ChoiceAID)) //��ȭ�� ID�� ������ ID�� ���ٸ� �װ����� �̵�
                        {
                            i = j;
                            continue;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S)) //������ ������
                {
                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (data[j].ChoiceID.Equals(data[i].ChoiceBID)) //��ȭ�� ID�� ������ ID�� ���ٸ� �װ����� �̵�
                        {
                            i = j;
                            continue;
                        }
                    }
                }
            }

            //���� ���� Ȯ��
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

        DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy += _storyDatabase[_storyKey[index]].RewardIntimacy; //���� ģ�е�

        if (currentNPC.Intimacy > nextStory.RequiredIntimacy)
        {
            DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy = nextStory.RequiredIntimacy;
        }

        Debug.Log(currentNPC.Name + " " + currentNPC.Intimacy);
    }
}
