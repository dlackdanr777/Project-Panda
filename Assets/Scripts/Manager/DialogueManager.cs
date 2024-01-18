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

    private DialogueParser _parser = new DialogueParser();

    //뒤끝 차트의 파일 ID
    private string _chartID = "104969";

    public void Register()
    {
        //임시로 주석처리. 뒤끝테스트를 위해 로컬 Csv는 꺼놓습니다.
        //_storyDialogueDic = _parser.StroyParse("StoryDialogue");
    }

    public void LoadData()
    {
        BackendManager.Instance.GetChartData(_chartID, 10, DialogueParse);
    }

    private void DialogueParse(BackendReturnObject callback)
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


    private void DialogueParse(int maxRepeatCount = 10)
    {
        if (maxRepeatCount <= 0)
            return;

        Backend.Chart.GetOneChartAndSave(_chartID, callback =>
        {
            if (callback.IsServerError() || callback.IsClientRequestFailError() || callback.GetMessage().Contains("signature"))
            {
                Debug.LogErrorFormat("다이얼로그 받아오기 실패 : {0}", callback);
                DialogueParse(maxRepeatCount - 1);
                return;
            }

            if (!callback.IsSuccess())
            {
                Debug.Log("정보를 받아오지 못했습니다.");
                DialogueParse(maxRepeatCount - 1);
                return;
            }

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
        });
    }


    public StoryDialogue GetStoryDialogue(string index)
    {
        if (!_storyDialogueDic.TryGetValue(index, out StoryDialogue storyDialogue))
        {
            Debug.LogError("해당 스토리 ID가 존재하지 않습니다.");
            return default;
        }
            

        return storyDialogue;
    }


}
