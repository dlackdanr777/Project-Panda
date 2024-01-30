using System.Collections;
using System.Collections.Generic;
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
        for(int i=0;i<_storyKey.Count;i++) 
        {
            if (!_storyDatabase[_storyKey[i]].IsSuccess) //���丮�� �Ϸ����� �ʾҴٸ�
            {
                if (DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= _storyDatabase[_storyKey[i]].RequiredIntimacy) //ģ�е� ��
                {
                    List<SideDialogueData> data = _storyDatabase[_storyKey[i]].DialogueData;
                    Debug.Log(data.Count);
                    for (int j =0;j<data.Count; j++) //��ȭ ���
                    {
                        Debug.Log(_storyDatabase[_storyKey[i]].DialogueData[j].Contexts);

                    }
                    DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy += (int)_storyDatabase[_storyKey[i]].RewardIntimacy;
                    Debug.Log(DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Name + " " + DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy);

                }
                
            }

        }

    }

    public void UpdateInteraction()
    {
    }
    public void ExitInteraction()
    {
    }
}
