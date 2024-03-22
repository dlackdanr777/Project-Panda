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

    //�ڳ� ��Ʈ�� ���� ID
    private string _chartID = "104969";

    public void Register()
    {
        _storyDialogueDic = _parser.StroyParse("StoryDialogue");
    }

    public void LoadData()
    {
        //���丮 ������ �������� ���÷� ����
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
        //���� �������� �����͸� �޾ƿ��� �������� �ӽ÷� �����͸� �ҷ��´�
        //���� ���� ����
        if (_storyDialogueDic == null) 
        {
            _storyDialogueDic = _parser.StroyParse("StoryDialogue");
        }

        if (!_storyDialogueDic.TryGetValue(index, out StoryDialogue storyDialogue))
        {
            Debug.LogError("�ش� ���丮 ID�� �������� �ʽ��ϴ�.");
            return default;
        }
            
        return storyDialogue;
    }


}
