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
        //그 스토리를 완료하였는가?
        //완료하였다면 다음 스토리의 친밀도 비교
        //아직 다음 스토리의 친밀도를 채우지 못했다면 이전 스토리 진행
        //다음 스토리의 친밀도를 채웠다면 반복
        for(int i=0;i<_storyKey.Count;i++) 
        {
            if (!_storyDatabase[_storyKey[i]].IsSuccess) //스토리를 완료하지 않았다면
            {
                if (DatabaseManager.Instance.GetNPCList()[_npcCode - 1].Intimacy >= _storyDatabase[_storyKey[i]].RequiredIntimacy) //친밀도 비교
                {
                    List<SideDialogueData> data = _storyDatabase[_storyKey[i]].DialogueData;
                    Debug.Log(data.Count);
                    for (int j =0;j<data.Count; j++) //대화 출력
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
