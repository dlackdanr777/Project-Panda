using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EChallenges
{
    Reward_MainStory,
    Reward_Beginner,
    Reward_Middle
}


public class ChallengesDatabase : SingletonHandler<ChallengesDatabase>
{
    public Dictionary<string, ChallengesData>[] ChallengesDic = new Dictionary<string, ChallengesData>[System.Enum.GetValues(typeof(EChallenges)).Length];
    //public Dictionary<string, ChallengesData> ChallengesBeginnerDic;
    //public Dictionary<string, ChallengesData> ChallengesMiddleDic;

    public override void Awake()
    {
        Register();
    }

    public void Register()
    {
        for (int i = 0; i < ChallengesDic.Length; i++)
        {
            ChallengesDic[i] = ChallengesParse(((EChallenges)i).ToString());
        }
    }


    private Dictionary<string, ChallengesData> ChallengesParse(string CSVFileName)
    {
        Dictionary<string, ChallengesData> challengesDic = new Dictionary<string, ChallengesData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            challengesDic.Add(row[0], new ChallengesData(row[0], row[1], row[2], int.Parse(row[3]), "IFR15")); // 나중에 아이템 받아오는 것으로 수정
        }
        return challengesDic;
    }
}
