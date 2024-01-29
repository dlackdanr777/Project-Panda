using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideStoyDialogueManager : MonoBehaviour
{
    private Dictionary<string, SideStoryDialogue> SSDic = new Dictionary<string, SideStoryDialogue>();
    private List<List<Dictionary<string, object>>> _dataSideStory;

    public void Register()
    {
        for(int i = 0; i < DatabaseManager.Instance.GetNPCList().Count; i++)
        {
            _dataSideStory[i] = CSVReader.Read("SS" + string.Format("{0:00}", (i+1))); //SS01 ~ SS14
            
            for(int j = 0; j < _dataSideStory[i].Count; i++)
            {
                SSDic.Add(_dataSideStory[i][j]["���丮ID"].ToString(),
                    new SideStoryDialogue(_dataSideStory[i][j]["���丮ID"].ToString(),
                    _dataSideStory[i][j]["���丮 �̸�"].ToString(),
                    (int)_dataSideStory[i][j]["ģ�е�"],
                    (EventType)_dataSideStory[i][j]["�̺�Ʈ Ÿ��"],
                    _dataSideStory[i][j]["�̺�Ʈ Ÿ�� ����"].ToString(),
                    _dataSideStory[i][j]["�̺�Ʈ Ÿ��"],
                    (EventType)_dataSideStory[i][j]["Ŭ���� ���� Ÿ��"],
                    _dataSideStory[i][j]["���� ������ ID"].ToString(),
                    (int)_dataSideStory[i][j]["���� ����"],
                    (int)_dataSideStory[i][j]["Ŭ���� ���� ģ�е�"]
                    ));
            }
        }
    }

    private SideDialogueData GetContext(int i, int j)
    {
        return new SideDialogueData(_dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["�Ϲ� ��ȭ"].ToString(),
            _dataSideStory[i][j]["���丮 ���� ���� üũ"].ToString(),
            _dataSideStory[i][j]["��ȭ ������ ID"].ToString(),
            _dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["talkPandaID"].ToString(),);
    }
}
