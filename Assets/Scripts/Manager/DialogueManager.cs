using BackEnd;
using LitJson;
using Muks.BackEnd;
using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{
    private Dictionary<string, StoryDialogue> _storyDialogueDic;

    private Parser _parser = new Parser();

    //뒤끝 차트의 파일 ID
    private string _chartID = "104969";

    public void Register()
    {
        _storyDialogueDic = _parser.StroyParse("StoryDialogue");
    }

    public void LoadData()
    {
        //스토리 데이터 서버에서 로컬로 변경
        //BackendManager.Instance.GetChartData(_chartID, 10, DialogueParseByServer);
    }

    private void DialogueParseByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();
        Dictionary<string, StoryDialogue> dialogueDic = new Dictionary<string, StoryDialogue>();

        for (int i = 0; i < json.Count;)
        {

            string storyID = json[i]["StoryID"].ToString();
            string storyName = json[i]["StoryName"].ToString();
            int requiredIntimacy = int.Parse(json[i]["RequiredIntimacy"].ToString());
            string priorStoryID = json[i]["PriorStoryID"].ToString();
            string nextStoryID = json[i]["NextStoryID"].ToString();
            string pandaID = json[i]["PandaIDTxtStartPoint"].ToString();

            List<DialogData> dialogDataList = new List<DialogData>();

            do
            {
                string talkPandaID = json[i]["TalkPandaID"].ToString();
                string contexts = json[i]["Contexts"].ToString();
                string choiceContext1 = json[i]["Choice1"].ToString();
                string choiceContext2 = json[i]["Choice2"].ToString();

                dialogDataList.Add(new DialogData(talkPandaID, contexts, choiceContext1, choiceContext2));
                if (++i < json.Count)
                {
                }
                else
                {
                    break;
                }

            } while (json[i]["StoryID"].ToString() == "");
            StoryDialogue dialogue = new StoryDialogue(storyID, storyName, requiredIntimacy, priorStoryID, nextStoryID, pandaID, dialogDataList.ToArray());
            dialogueDic.Add(storyID, dialogue);

        }
        _storyDialogueDic = dialogueDic;
    }


    public StoryDialogue GetStoryDialogue(string index)
    {
        //만약 서버에서 데이터를 받아오지 못했으면 임시로 데이터를 불러온다
        //차후 제거 예정
        if (_storyDialogueDic == null) 
        {
            _storyDialogueDic = _parser.StroyParse("StoryDialogue");
        }

        if (!_storyDialogueDic.TryGetValue(index, out StoryDialogue storyDialogue))
        {
            Debug.LogError("해당 스토리 ID가 존재하지 않습니다.");
            return default;
        }
            
        return storyDialogue;
    }


}
