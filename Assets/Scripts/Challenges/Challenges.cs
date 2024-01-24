using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public enum EUnlockingBook
{
    NPC,
    Bug,
    Fish,
    Fruit,
    Recipe,
}

public class Challenges
{
    public Action<string> ChallengeDone;

    private List<ChallengesData>[] _challengesDatas = new List<ChallengesData>[System.Enum.GetValues(typeof(EChallengesKategorie)).Length];
    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // 현재 도전과제

    // 채집 성공할 때마다 저장
    public int[] GatheringSuccessNum = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // 채집 성공 횟수

    // 도감 관련
    private int[] _unlockingBookCount = new int[System.Enum.GetValues(typeof(EUnlockingBook)).Length];


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

    // 상점
    private int _purchaseCount;
    private int _salesCount;

    // 요리 제작 횟수
    private int CookingCount = 0;

    // 사진
    private int _takePhoto;
    private int _shatingPhoto;

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
    public void UnlockingBook(string type) // 처음에 카운트 받아온 후 도감 해제될 때마다 실행하기... ? -> IsReceived 변경될 때마다 실행하기
    {
        EUnlockingBook eUnlockingBook = EUnlockingBook.NPC;

        // NPC
        if(type == "NPC")
        {
            // 처음 초기화 할 때만 이렇게 받아오고 그 후에 ++ 해주기
            _unlockingBookCount[(int)EUnlockingBook.NPC] = DatabaseManager.Instance.GetNPCList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.NPC;
        }
        // 곤충
        else if(type == "Bug")
        {
            _unlockingBookCount[(int)EUnlockingBook.Bug] = DatabaseManager.Instance.GetBugItemList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.Bug;
        }
        // 물고기
        else if(type == "Fish")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fish] = DatabaseManager.Instance.GetFishItemList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.Fish;
        }
        // 과일
        else if(type == "Fruit")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fruit] = DatabaseManager.Instance.GetFruitItemList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.Fruit;
        }
        // 레시피
        else if(type == "Recipe")
        {
            // 추가
            eUnlockingBook = EUnlockingBook.Recipe;
        }

        // 한 가지 종류 도감 달성 체크
        int challengesNum = _challengesDatas[(int)EChallengesKategorie.book].FindIndex(n => n.Type == type || n.IsSuccess == false);
        int count = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Id;
        if (_unlockingBookCount[(int)eUnlockingBook] > count)
        {
            ChallengeDone?.Invoke(challengesId);
        }

        // 각각의 도감 n개 해제했는지 체크
        if (_challengesNum[(int)EChallengesKategorie.book] == -1)
        {
            _challengesNum[(int)EChallengesKategorie.book] = _challengesDatas[(int)EChallengesKategorie.book].FindIndex(n => n.Type == "All" || n.IsSuccess == false);
        }

        count = _challengesDatas[(int)EChallengesKategorie.book][_challengesNum[(int)EChallengesKategorie.book]].Count;
        challengesId = _challengesDatas[(int)EChallengesKategorie.book][_challengesNum[(int)EChallengesKategorie.book]].Id;

        for (int i = 0; i< System.Enum.GetValues(typeof(EUnlockingBook)).Length; i++ )
        {
            if (_unlockingBookCount[i] < count)
            {
                return;
            }
        }
        ChallengeDone?.Invoke(challengesId);
        _challengesNum[(int)EChallengesKategorie.book] = -1;
    }

    /// <summary>
    /// 누적 대나무 수 달성 </summary>
    private void StackedBamboo()
    {
        int count = _challengesDatas[(int)EChallengesKategorie.bamboo][_challengesNum[(int)EChallengesKategorie.bamboo]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.bamboo][_challengesNum[(int)EChallengesKategorie.bamboo]].Id;

        if(_stackedBambooCount > count)
        {
            ChallengeDone?.Invoke(challengesId);
            _challengesNum[(int)EChallengesKategorie.bamboo]++;
        }
    }

    /// <summary>
    /// 상점 사용시 실행 </summary>
    private void UsingShop(bool isPurchase) // 기능 구현되면 수정
    {
        // 구매
        if (isPurchase)
        {

        }
        // 판매
        else
        {

        }
    }

    /// <summary>
    /// n개의 가구 테마 완성 </summary>
    private void FurnitureArrangement()
    {

    }

    /// <summary>
    /// 요리 n회 이상 제작 </summary>
    public void Cooking(string id, string grade)
    {
        int Count = _challengesDatas[(int)EChallengesKategorie.cook][_challengesNum[(int)EChallengesKategorie.cook]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.cook][_challengesNum[(int)EChallengesKategorie.cook]].Id;

        // A등급 이상인지 확인
        if (grade == "A" || grade == "S")
        {
            CookingCount++;
            if(CookingCount >= Count)
            {
                ChallengeDone?.Invoke(id);
                _challengesNum[(int)EChallengesKategorie.cook]++;
            }
        }
    }

    /// <summary>
    /// 사진 찍기&공유하기 </summary>
    public void Photo(bool isTakePhoto)
    {
        // 사진 찍기
        if(isTakePhoto)
        {
            _takePhoto++;

            // 리스트에서 달성하지 못한 사진찍는 과제 중 첫 번째 찾기
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Take" || n.IsSuccess == false).Id;
            
            // 달성했다면
            if (_takePhoto > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                ChallengeDone?.Invoke(challengesId);
            }
        }
        // 사진 공유하기
        else
        {
            _shatingPhoto++;

            // 리스트에서 달성하지 못한 사진공유 과제 중 첫 번째 찾기
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Sharing" || n.IsSuccess == false).Id;

            if (_shatingPhoto > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                ChallengeDone?.Invoke(challengesId);
            }
        }
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
