using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //����Ʈ ����
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�
        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������

        for(int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.

            Dialogue dialogue = new Dialogue(); //��� ����Ʈ ����

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
