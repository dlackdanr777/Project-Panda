using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class Parser
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


    public Dictionary<string, StoryDialogue> StroyParse(string CSVFileName)
    {
        Dictionary<string, StoryDialogue> dialogueDic = new Dictionary<string, StoryDialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�
        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������
        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.
            string storyID = row[0];
            string storyName = row[1];
            int requiredIntimacy = int.Parse(row[2]);
            string priorStoryID = row[3];
            string nextStoryID = row[4];
            string pandaID = row[5];

            List<DialogData> dialogDataList = new List<DialogData>();

            do
            {
                string talkPandaID = row[6];
                string contexts = row[7];
                string choiceContext1 = row[8];
                string choiceContext2 = row[9];

                dialogDataList.Add(new DialogData(talkPandaID, contexts, choiceContext1, choiceContext2));
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

    /// <summary>
    /// �Ǵ� ������ �޾ƿ� ����
    /// </summary>
    public Dictionary<int, PandaData> PandaParse(string CSVFileName)
    {
        Dictionary<int, PandaData> pandaDic = new Dictionary<int, PandaData>(); // �Ǵ� ��ųʸ� ����

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {

            string[] row = data[i].Split(new char[] { ',' });
            pandaDic.Add(int.Parse(row[0]), new PandaData(int.Parse(row[0]), row[1], row[2], float.Parse(row[3]), float.Parse(row[4])));
        }
        return pandaDic;
    }

    public RecipeData[] RecipeDataParse(string CSVFileName)
    {
        List<RecipeData> recipeDataList = new List<RecipeData>(); //����Ʈ ����
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�

        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.

            List<KeyValuePair<string, int>> itemList = new List<KeyValuePair<string, int>>();

            if (!string.IsNullOrWhiteSpace(row[0]))
            {
                string materialItemID = row[0];
                int materialValue = int.Parse(row[1]);

               itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }
            else
            {
                string materialItemID = "";
                int materialValue = 0;

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }

            if (!string.IsNullOrWhiteSpace(row[2]))
            {
                string materialItemID = row[2];
                int materialValue = int.Parse(row[3]);

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }
            else
            {
                string materialItemID = "";
                int materialValue = 0;

                itemList.Add(new KeyValuePair<string, int>(materialItemID, materialValue));
            }


            string successItemID = row[4];
            float successLocation = float.Parse(row[5]);
            float successRangeLevel_S = float.Parse(row[6]);
            float successRangeLevel_A = float.Parse(row[7]);
            float successRangeLevel_B = float.Parse(row[8]);

            RecipeData recipeData = new RecipeData(itemList, successItemID, successLocation,
                successRangeLevel_S, successRangeLevel_A, successRangeLevel_B); //������ Ŭ���� ����

            recipeDataList.Add(recipeData);
        }

        return recipeDataList.ToArray();
    }


    public Dictionary<int, WeatherRewardData> WeatherParse(string CSVFileName)
    {
        Dictionary<int, WeatherRewardData> dic = new Dictionary<int, WeatherRewardData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            int day = int.Parse(row[0]);
            string weather = row[1];
            string itemId = row[2];
            int amount = int.Parse(row[3]);
            Item item = DatabaseManager.Instance.ItemDatabase.GetGatheringItemById(itemId);
            Sprite sprite = DatabaseManager.Instance.WeatherImage.GetWeatherImage(weather);

            WeatherRewardData weatherData = new WeatherRewardData(day, weather, amount, item, sprite);

            dic.Add(day, weatherData);
        }
        return dic;
    }


    /*public RecipeData[] RecipeDataParse(string CSVFileName)
    {
        List<RecipeData> recipeDataList = new List<RecipeData>(); //����Ʈ ����
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);//csv���� �ε�

        string[] data = csvData.text.Split(new char[] { '\n' }); //�ٸ��� ������

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); //�޸������� ������.

            string materialItemID = row[0];
            int materialValue = int.Parse(row[1]);
            string cookingItemID_B = row[2];
            string cookingItemID_A = row[3];
            string cookingItemID_S = row[4];

            RecipeData recipeData = new RecipeData(materialItemID, materialValue, cookingItemID_B,
                cookingItemID_A, cookingItemID_S); //������ Ŭ���� ����

            recipeDataList.Add(recipeData);
        }

        return recipeDataList.ToArray();
    }*/
}
