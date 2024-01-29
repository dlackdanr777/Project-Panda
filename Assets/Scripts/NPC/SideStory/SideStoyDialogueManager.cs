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
                SSDic.Add(_dataSideStory[i][j]["스토리ID"].ToString(),
                    new SideStoryDialogue(_dataSideStory[i][j]["스토리ID"].ToString(),
                    _dataSideStory[i][j]["스토리 이름"].ToString(),
                    (int)_dataSideStory[i][j]["친밀도"],
                    (EventType)_dataSideStory[i][j]["이벤트 타입"],
                    _dataSideStory[i][j]["이벤트 타입 조건"].ToString(),
                    _dataSideStory[i][j]["이벤트 타입"],
                    (EventType)_dataSideStory[i][j]["클리어 보상 타입"],
                    _dataSideStory[i][j]["보상 아이템 ID"].ToString(),
                    (int)_dataSideStory[i][j]["보상 수량"],
                    (int)_dataSideStory[i][j]["클리어 보상 친밀도"]
                    ));
            }
        }
    }

    private SideDialogueData GetContext(int i, int j)
    {
        return new SideDialogueData(_dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["일반 대화"].ToString(),
            _dataSideStory[i][j]["스토리 성공 실패 체크"].ToString(),
            _dataSideStory[i][j]["대화 선택지 ID"].ToString(),
            _dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["talkPandaID"].ToString(),
            _dataSideStory[i][j]["talkPandaID"].ToString(),);
    }
}
