using BackEnd;
using LitJson;
using Muks.BackEnd;
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
    private Dictionary<string, ChallengesData> _challengesDic = new Dictionary<string, ChallengesData>();


    public void Register()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(EChallenges)).Length; i++)
        {
            _challengesDic.AddRange(ChallengesParseByLocal(((EChallenges)i).ToString()));
        }
    }


    public Dictionary<string, ChallengesData> GetChallengesDic()
    {
        return _challengesDic;
    }


    #region LoadChallengesData

    public void LoadData()
    {
        _challengesDic.Clear();
        BackendManager.Instance.GetChartData("114027", 10, LoadChallengesByServer);
        BackendManager.Instance.GetChartData("114028", 10, LoadChallengesByServer);
        BackendManager.Instance.GetChartData("114029", 10, LoadChallengesByServer);
    }


    private Dictionary<string, ChallengesData> ChallengesParseByLocal(string CSVFileName)
    {
        Dictionary<string, ChallengesData> challengesDic = new Dictionary<string, ChallengesData>();

        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            string itemId = !string.IsNullOrWhiteSpace(row[4].ToString()) ? row[4].ToString() : string.Empty;
            challengesDic.Add(row[0], new ChallengesData(row[0], row[1], row[2], int.Parse(row[3]), itemId, (EChallengesKategorie)Enum.Parse(typeof(EChallengesKategorie), row[5]), row[6], int.Parse(row[7]))); // 나중에 아이템 받아오는 것으로 수정
        }
        return challengesDic;
    }


    private void LoadChallengesByServer(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        for(int i = 0, count = json.Count; i < count; i++)
        {
            string challengeId = json[i]["ChallengeID"].ToString();
            string name = json[i]["Name"].ToString();
            string description = json[i]["Description"].ToString();
            int rewardBamboo = int.Parse(json[i]["RewardBamboo"].ToString());
            string rewardItemId = !string.IsNullOrWhiteSpace(json[i]["RewardItem"].ToString()) ? json[i]["RewardItem"].ToString() : string.Empty;
            EChallengesKategorie category = (EChallengesKategorie)Enum.Parse(typeof(EChallengesKategorie), json[i]["Category"].ToString());
            string type = json[i]["Type"].ToString();
            int rewardCount = int.Parse(json[i]["Count"].ToString());

            _challengesDic.Add(challengeId, new ChallengesData(challengeId, name, description, rewardBamboo, rewardItemId, category, type, rewardCount)); 
        }

        Debug.Log("도전과제 데이터 받아오기 성공!");
    }
    #endregion

}
