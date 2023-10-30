using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv파일 로드
        string[] data = csvData.text.Split(new char[] { '\n' }); //줄마다 나눈다

        for(int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //콤마단위로 나눈다.

            Dialogue dialogue = new Dialogue(); //대사 리스트 생성

            dialogue.Contexts = row[1];
            dialogue.LeftButtonContexts = row[2];
            dialogue.RightButtonContexts = row[3];
            dialogue.LeftButtonOutput = row[4];
            dialogue.RightButtonOutput = row[5];

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }

}
