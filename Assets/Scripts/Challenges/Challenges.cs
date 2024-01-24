using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;



public class Challenges
{
    public Action<string> ChallengeDone;

    private List<ChallengesData>[] _challengesDatas = new List<ChallengesData>[System.Enum.GetValues(typeof(EChallengesKategorie)).Length];
    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // 현재 도전과제

    // 채집 성공할 때마다 저장
    public int[] GatheringSuccessNum = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // 채집 성공 횟수

    // 대나무 누적 개수
    private int _stackedBambooCount = 0;
    public int StackedBambooCount 
    {
        get { return _stackedBambooCount; }
        set 
        { 
            _stackedBambooCount = value;
            StackedBamboo();
        }
    }

    public void Register()
    {
        ChallengeDone += EarningRewards;

        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        for (int i = 0; i < System.Enum.GetValues(typeof(EChallengesKategorie)).Length; i++)
        {
            _challengesDatas[i] = new List<ChallengesData>();
        }

            // 카테고리별로 도전 과제 분류
            foreach (string key in challengesDic.Keys)
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(EChallengesKategorie)).Length; i++)
            {
                if (challengesDic[key].Kategorie == (EChallengesKategorie)i)
                {
                    _challengesDatas[i].Add(challengesDic[key]);
                    break;
                }
            }
        }
    }

    public void MainStoryDone(string id)
    {
        if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // 메인 스토리 한 장이 끝나면
        {
            string storyChallengeId = "RW" + id.Substring(0, 4);
            ChallengeDone?.Invoke(storyChallengeId); // 도전과제 달성
        }
    }

    /// <summary>
    /// 채집 각각 n회 이상 성공 </summary>
    public void GatheringSuccess(int gatheringType) // 채집 성공할 때마다 불러오기
    {
        GatheringSuccessNum[gatheringType]++;

        // 몇 번 성공해야 하는지
        int successNum = _challengesDatas[(int)EChallengesKategorie.gathering][_challengesNum[(int)EChallengesKategorie.gathering]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.gathering][_challengesNum[(int)EChallengesKategorie.gathering]].Id;

        for (int i = 0; i < GatheringSuccessNum.Length; i++)
        {
            if (GatheringSuccessNum[i] < successNum)
            {
                return;
            }
        }

        // 도전 과제 달성 시
        ChallengeDone?.Invoke(challengesId);
        _challengesNum[(int)EChallengesKategorie.gathering]++; // 다음 도전과제 확인할 수 있도록 저장
    }

    /// <summary>
    /// 도감 해제 </summary>
    public void UnlockingBook() // 처음에 카운트 받아온 후 도감 해제될 때마다 실행하기... ?
    {
        // 모두(+가구, 중요한 도구)


        // NPC
        int npcNum = DatabaseManager.Instance.GetNPCList().Where(n => n.IsReceived == true).Count();

        // 곤충
        int bugNum = DatabaseManager.Instance.GetBugItemList().Where(n => n.IsReceived == true).Count();

        // 물고기
        int fishNum = DatabaseManager.Instance.GetFishItemList().Where(n => n.IsReceived == true).Count();

        // 과일
        int fruitNum = DatabaseManager.Instance.GetFruitItemList().Where(n => n.IsReceived== true).Count();

        // 레시피
        
    }

    /// <summary>
    /// 누적 대나무 수 달성 </summary>
    private void StackedBamboo()
    {
        int Count = _challengesDatas[(int)EChallengesKategorie.bamboo][_challengesNum[(int)EChallengesKategorie.bamboo]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.bamboo][_challengesNum[(int)EChallengesKategorie.bamboo]].Id;

        if(_stackedBambooCount > Count)
        {
            ChallengeDone?.Invoke(challengesId);
            _challengesNum[(int)EChallengesKategorie.bamboo]++;
        }
    }

    /// <summary>
    /// 상점 사용시 실행 </summary>
    private void UsingShop()
    {
        // 구매

        // 판매
    }

    /// <summary>
    /// 가구를 배치해 n개의 가구 테마 완성 </summary>
    private void FurnitureArrangement()
    {

    }

    /// <summary>
    /// 요리 n회 이상 제작 </summary>
    private void Cooking()
    {
        // A등급 이상인지 확인
    }

    /// <summary>
    /// 사진 찍기&공유하기 </summary>
    private void Photo()
    {
        // 사진 찍기

        // 사진 공유하기
    }


    private void EarningRewards(string id)
    {
        DatabaseManager.Instance.GetChallengesDic()[id].IsSuccess = true;

        // 대나무 획득
        GameManager.Instance.Player.GainBamboo(DatabaseManager.Instance.GetChallengesDic()[id].BambooCount);

        // 아이템 획득
        //GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem)[(int)_cookingSystem.InventoryType].Add(_currentRecipeData.Item);
    }
}
