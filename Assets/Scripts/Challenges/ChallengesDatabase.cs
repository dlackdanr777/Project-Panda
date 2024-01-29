using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EChallenges
{
    Reward_MainStory,
    Reward_Beginner,
    Reward_Middle
}

public enum EChallengesKategorie
{
    story,
    gathering,
    book,
    bamboo,
    item,
    furniture,
    cook,
    camera
}

public class ChallengesDatabase
{
    //private Dictionary<string, ChallengesData>[] ChallengesDic = new Dictionary<string, ChallengesData>[System.Enum.GetValues(typeof(EChallenges)).Length];
    private Dictionary<string, ChallengesData> ChallengesDic = new Dictionary<string, ChallengesData>();


    public void Register()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EChallenges)).Length; i++)
        {
            //ChallengesDic[i] = ChallengesParse(((EChallenges)i).ToString());
            ChallengesDic.AddRange(ChallengesParse(((EChallenges)i).ToString()));
        }

        DatabaseManager.Instance.UserInfo.LoadUserChallenges();
    }


    private Dictionary<string, ChallengesData> ChallengesParse(string CSVFileName)
    {
        Dictionary<string, ChallengesData> challengesDic = new Dictionary<string, ChallengesData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            challengesDic.Add(row[0], new ChallengesData(row[0], row[1], row[2], int.Parse(row[3]), "ITG01", (EChallengesKategorie)Enum.Parse(typeof(EChallengesKategorie), row[5]), row[6], int.Parse(row[7]))); // 나중에 아이템 받아오는 것으로 수정
        }
        return challengesDic;
    }

    public Dictionary<string, ChallengesData> GetChallengesDic()
    {
        return ChallengesDic;
    }
}
