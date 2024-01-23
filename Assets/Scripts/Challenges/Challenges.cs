using System;
using UnityEngine;

public class Challenges
{
    public Action<string> ChallengeDone;

    // 채집 성공할 때마다 저장
    public int[] CollectionSuccessNum = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length -1]; // 채집 성공 횟수

    public void Register()
    {

    }

    public void MainStoryDone(string id)
    {
        if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // 메인 스토리 한 장이 끝나면
        {
            string storyChallengeId = "RW" + id.Substring(0, 4);
            ChallengeDone?.Invoke(storyChallengeId); // 도전과제 달성
        }
    }

    public void GatheringSuccess()
    {
        int successNum = 1; // 몇 번 성공해야 하는지 .. 받아와야 함
        string challengesId = "RWBG01"; // 이걸 어떻게 결정.. 해야할지 .. 고민
        for (int i = 0; i < CollectionSuccessNum.Length; i++)
        {
            if (CollectionSuccessNum[i] < successNum)
            {
                return;
            }
        }
        ChallengeDone?.Invoke(challengesId);
    }

    private void EarningRewards()
    {
        // 대나무 획득

        // 아이템 획득
    }
}
