using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class DialogueParser
{
    public MBTIDialogue[] MBTIParse(string CSVFileName)
    {
        List<MBTIDialogue> dialogueList = new List<MBTIDialogue>(); //리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv파일 로드
        string[] data = csvData.text.Split(new char[] { '\n' }); //줄마다 나눈다

        for(int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //콤마단위로 나눈다.

            MBTIDialogue dialogue = new MBTIDialogue(); //대사 리스트 생성

            dialogue.Contexts = row[1];
            dialogue.LeftButtonContexts = row[2];
            dialogue.RightButtonContexts = row[3];
            dialogue.LeftButtonOutput = row[4];
            dialogue.RightButtonOutput = row[5];

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }


/*    //스토리 정보를 변환하여 반환하는 함수
    public StoryDialogue[] StroyParse(string CSVFileName)
    {
        List<StoryDialogue> dialogueList = new List<StoryDialogue>(); //리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv파일 로드
        string[] data = csvData.text.Split(new char[] { '\n' }); //줄마다 나눈다

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //콤마단위로 나눈다.

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
                    row = data[i].Split(new char[] { ',' }); //콤마단위로 나눈다.
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
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv파일 로드
        string[] data = csvData.text.Split(new char[] { '\n' }); //줄마다 나눈다

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //콤마단위로 나눈다.

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
                    row = data[i].Split(new char[] { ',' }); //콤마단위로 나눈다.
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
