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
            if (!_storyDatabase[_storyKey[i]].IsSuccess) //���丮�� �Ϸ����� �ʾҴٸ�
            {
                //���� ���丮 ���� ���丮 ��
                bool priorCheck = CheckPrior(i, _storyDatabase[_storyKey[i]].PriorStoryID); //ó�� �����̸� ������ true�� �Ѿ �� �ֵ���
                int? nextCheck = CheckNext(i, _storyDatabase[_storyKey[i]].NextStoryID); //�������̸� ģ�е��� max�� �ؼ� �������θ� true�� �� �� ������

                if (priorCheck && //���� ���丮 �����ߴ��� ��
                    DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= nextCheck) //���� ���丮�� ģ�е� ��
                {
                    _storyDatabase[_storyKey[i]].IsSuccess = true;

                    continue; //������ ���丮�� ���� ���丮�� �̵�
                }

                //������ ���丮�� �ƴϸ�
                if (DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= _storyDatabase[_storyKey[i]].RequiredIntimacy) //ģ�е� ��
                {
                    if (_storyDatabase[_storyKey[i]].EventType != EventType.None)// �̺�Ʈ ������ ������
                    {
                        //���� ��
                        if (!CheckCondition(_storyDatabase[_storyKey[i]].EventType, _storyDatabase[_storyKey[i]].EventTypeCondition, _storyDatabase[_storyKey[i]].EventTypeAmount))
                        {
                            i--; //������ �������� �������Ƿ� �� ��ȭ ���
                        }
                    }

                    if(PrintContext(_storyDatabase[_storyKey[i]].DialogueData)) //��ȭ ���
                    {
                        AddReward(i); //����
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
            if (_storyDatabase.ContainsKey(priorStory)) //�ش� �����ͺ��̽��� �ִ� id���� Ȯ�� -> ������ ó�� => ������ ���
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
            if (_storyDatabase.ContainsKey(nextStory)) //�ش� �����ͺ��̽��� �ִ� id���� Ȯ�� -> ������ ������ => �Ѿ�� ���ϵ��� MaxValue
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
        //���� ��
        bool result = false;
        switch (type)
        {
            case EventType.GISP:
                result = GameManager.Instance.Player.GetGISP(condition, count); //������� ������ŭ �ִ��� Ȯ��
                break;
            case EventType.INM:
                result = GameManager.Instance.Player.GetINM(condition);
                break;
            case EventType.MONEY:
                result = GameManager.Instance.Player.GetMONEY(count);
                break;
            case EventType.NPCTK: //� �������̳� �׷� ���� �ʿ� �׳� ��ȭ�� ���ϱ⿣ ������� ����
                break;
            case EventType.REIT: //�󸶸�ŭ ������ �ʿ� => ���� ��ȭ�ϴ� npc���� ģ�е��� ���ƾ���
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

        for (int i = 0; i < data.Count; i++) //��ȭ ���
        {
            Debug.Log(data[i].Contexts);
            
            //���� ���� Ȯ��
            if (data[i].IsComplete.Equals("FAIL"))
            {
                return false;
            }
            else if (data[i].IsComplete.Equals("Done"))
            {
                return true;
            }

            //������ �ؽ�Ʈ
            if (!data[i].ChoiceContextA.Equals("") && !data[i].ChoiceContextB.Equals(""))
            {
                Debug.Log(data[i].ChoiceContextA);
                Debug.Log(data[i].ChoiceContextB);

                if (Input.GetKeyDown(KeyCode.A)) //���� ������
                {
                    for(int j = i+1; j < data.Count; j++)
                    {
                        if(data[j].ChoiceID.Equals(data[i].ChoiceAID)) //��ȭ�� ID�� ������ ID�� ���ٸ� �װ����� �̵�
                        {
                            i = j;
                            continue;
                        }
                    }
                }
                else if(Input.GetKeyDown(KeyCode.S)) //������ ������
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

        }
        return true;
    }

    private void AddReward(int index)
    {
        DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy += _storyDatabase[_storyKey[index]].RewardIntimacy; //���� ģ�е�
        if(DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy > _storyDatabase[_storyKey[index + 1]].RequiredIntimacy)
        {
            DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy = _storyDatabase[_storyKey[index + 1]].RequiredIntimacy;
        }
        Debug.Log(DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Name + " " + DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy);

    }
}
