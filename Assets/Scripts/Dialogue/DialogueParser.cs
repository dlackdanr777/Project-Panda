using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class DialogueParser
{
    public MBTIDialogue[] MBTIParse(string CSVFileName)
    {
        List<MBTIDialogue> dialogueList = new List<MBTIDialogue>(); //����Ʈ ����
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�
        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������

        for(int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.

            MBTIDialogue dialogue = new MBTIDialogue(); //��� ����Ʈ ����

            dialogue.Contexts = row[1];
            dialogue.LeftButtonContexts = row[2];
            dialogue.RightButtonContexts = row[3];
            dialogue.LeftButtonOutput = row[4];
            dialogue.RightButtonOutput = row[5];

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }


/*    //���丮 ������ ��ȯ�Ͽ� ��ȯ�ϴ� �Լ�
    public StoryDialogue[] StroyParse(string CSVFileName)
    {
        List<StoryDialogue> dialogueList = new List<StoryDialogue>(); //����Ʈ ����
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�
        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.

            int storyID = int.Parse(row[1]);
            string storyName = row[1];
            int requiredIntimacy = int.Parse(row[2]);
            int priorStoryID = int.Parse(row[3]);
            int nextStoryID = int.Parse(row[4]);
            int pandaID = int.Parse(row[5]);

            List<DialogData> dialogDataList = new List<DialogData>();

            do
            {
                int talkPandaID = int.Parse(row[6]);
                string contexts = row[7];

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' }); //�޸������� ������.
                }
                else
                {
                    break;
                }
                dialogDataList.Add(new DialogData(talkPandaID, contexts));

            } while (row[0].ToString() == "");

            StoryDialogue dialogue = new StoryDialogue(storyID, storyName, requiredIntimacy, priorStoryID, nextStoryID, pandaID, dialogDataList.ToArray());
            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }*/


    public Dictionary<int, StoryDialogue> StroyParse(string CSVFileName)
    {
        Dictionary<int, StoryDialogue> dialogueDic = new Dictionary<int, StoryDialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�
        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.

            int storyID = int.Parse(row[0]);
            string storyName = row[1];
            int requiredIntimacy = int.Parse(row[2]);
            int priorStoryID = int.Parse(row[3]);
            int nextStoryID = int.Parse(row[4]);
            int pandaID = int.Parse(row[5]);

            List<DialogData> dialogDataList = new List<DialogData>();

            do
            {
                int talkPandaID = int.Parse(row[6]);
                string contexts = row[7];
                dialogDataList.Add(new DialogData(talkPandaID, contexts));
                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' }); //�޸������� ������.
                }
                else
                {
                    break;
                }
                

            } while (row[0].ToString() == "");

            StoryDialogue dialogue = new StoryDialogue(storyID, storyName, requiredIntimacy, priorStoryID, nextStoryID, pandaID, dialogDataList.ToArray());
            dialogueDic.Add(storyID, dialogue);
        }

        return dialogueDic;
    }

}
