using System.Collections;
using System.Collections.Generic;
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
        //�� ���丮�� �Ϸ��Ͽ��°�?
        //�Ϸ��Ͽ��ٸ� ���� ���丮�� ģ�е� ��
        //���� ���� ���丮�� ģ�е��� ä���� ���ߴٸ� ���� ���丮 ����
        //���� ���丮�� ģ�е��� ä���ٸ� �ݺ�
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
                        if (!CheckCondition(i))
                        {
                            i--; //������ �������� �������Ƿ� �� ��ȭ ���
                        }
                    }

                    PrintContext(i); //��ȭ ���
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

    private bool CheckCondition(int index)
    {
        //���� ��
        bool result = false;
        switch (_storyDatabase[_storyKey[index]].EventType)
        {
            case EventType.GISP:
                result = GameManager.Instance.Player.GetGISP(_storyDatabase[_storyKey[index]].EventTypeCondition); //������� �ִ��� Ȯ��
                break;
            case EventType.INM:
                result = GameManager.Instance.Player.GetINM(_storyDatabase[_storyKey[index]].EventTypeCondition);
                break;
            case EventType.MONEY:
                result = GameManager.Instance.Player.GetMONEY(_storyDatabase[_storyKey[index]].EventTypeCondition);
                break;
            case EventType.NPCTK: //� �������̳� �׷� ���� �ʿ� �׳� ��ȭ�� ���ϱ⿣ ������� ����
                break;
            case EventType.REIT: //�󸶸�ŭ ������ �ʿ� => ���� ��ȭ�ϴ� npc���� ģ�е��� ���ƾ���
                if (DatabaseManager.Instance.GetNPC(_storyDatabase[_storyKey[index]].EventTypeCondition).Intimacy >= DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy)
                {
                    result = true;
                }
                break;
            case EventType.IVGI:
                result = GameManager.Instance.Player.GetIVGI(_storyDatabase[_storyKey[index]].EventTypeCondition);
                break;
            case EventType.IVCK:
                result = GameManager.Instance.Player.GetIVCK(_storyDatabase[_storyKey[index]].EventTypeCondition);
                break;
            case EventType.IVFU:
                break;

        }

        return result;
    }

    private void PrintContext(int index)
    {
        List<SideDialogueData> data = _storyDatabase[_storyKey[index]].DialogueData;

        for (int j = 0; j < data.Count; j++) //��ȭ ���
        {
            Debug.Log(_storyDatabase[_storyKey[index]].DialogueData[j].Contexts);

        }
        DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy += (int)_storyDatabase[_storyKey[index]].RewardIntimacy; //���� ģ�е�
        Debug.Log(DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Name + " " + DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy);
    }
}
