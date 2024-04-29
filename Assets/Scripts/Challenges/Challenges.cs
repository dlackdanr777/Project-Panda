using Muks.BackEnd;
using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EUnlockingBook
{
    None = -1,
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
    public int[] ChallengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // 현재 도전과제

    // 다른 씬에 있을 때 완료된 도전 과제 Id 저장 후 메인 씬에 돌아와서 한꺼번에 실행
    //private List<string> _doneChallenges = new List<string>();

    #region 개수
    private Dictionary<string, int> _mainStoryCount = new Dictionary<string, int>(); // 메인스토리 완료체크
    private List<string> _checkStoryCompleteList = new List<string>(); // 메인스토리 완료체크

    // 채집 성공할 때마다 저장
    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // 채집 성공 횟수

    // 도감 관련
    private int[] _unlockingBookCount = new int[System.Enum.GetValues(typeof(EUnlockingBook)).Length - 1];

    // 대나무 누적 개수
    private int _stackedBambooCount;
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
    public int PurchaseCount;
    public int SalesCount;

    // 가구
    public int FurnitureCount;

    // 요리 제작 횟수
    public int CookingCount;

    // 사진
    public int TakePhotoCount;
    public int SharingPhotoCount;
    #endregion

    public void Register()
    {
        LoadMyData();
    }

    public void LoadMyData()
    {
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

        // 도감 Count 초기화
        _unlockingBookCount[(int)EUnlockingBook.NPC] = DatabaseManager.Instance.GetNPCList().Where(n => n.IsReceived == true).Count();
        _unlockingBookCount[(int)EUnlockingBook.Bug] = DatabaseManager.Instance.ItemDatabase.ItemBugList.Where(n => n.IsReceived == true).Count();
        _unlockingBookCount[(int)EUnlockingBook.Fish] = DatabaseManager.Instance.ItemDatabase.ItemFishList.Where(n => n.IsReceived == true).Count();
        _unlockingBookCount[(int)EUnlockingBook.Fruit] = DatabaseManager.Instance.ItemDatabase.ItemFruitList.Where(n => n.IsReceived == true).Count();

        // 레시피 - 추가하기
    }

    /// <summary>
    /// IsDone이 제대로 저장되었는지 확인 </summary>
    public void CheckIsDone()
    {
        bool isDone = true;

        int count = 0;
        string challengesId = "";
        int challengesNum = 0;


        // 메인스토리
        Dictionary<string, MainStoryDialogue> msDic = DatabaseManager.Instance.MainDialogueDatabase.MSDic;
        _mainStoryCount.Clear();
        _checkStoryCompleteList.Clear();
        foreach (string key in msDic.Keys)
        {
            if (key.Substring(0, 2) != "MS") // 메인 스토리가 아닐 경우
            {
                break;
            }
            if (_mainStoryCount.Count == 0)
            {
                _mainStoryCount.Add(key.Substring(0, 4), 1);
            }
            else if (!_mainStoryCount.Keys.Contains(key.Substring(0, 4)))
            {
                _mainStoryCount.Add(key.Substring(0, 4), 1);
            }
            else
            {
                _mainStoryCount[key.Substring(0, 4)]++;
            }
            
        }
        foreach (string key in DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList)
        {
            if (key.Substring(0, 2) != "MS") // 메인 스토리가 아닐 경우
            {
                continue;
            }
            if (!_checkStoryCompleteList.Contains(key) && _mainStoryCount.Keys.Contains(key.Substring(0, 4)))
            {
                _checkStoryCompleteList.Add(key);
                _mainStoryCount[key.Substring(0, 4)]--;

                if (_mainStoryCount[key.Substring(0, 4)] == 0)
                {

                    challengesId = "RW" + key.Substring(0, 4);
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                }
            }
        }

        // 채집
        for (int i = 0; i < _challengesDatas[(int)EChallengesKategorie.gathering].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.gathering][i].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.gathering][i].Id;
            for (int j = 0; j < GatheringSuccessCount.Length; j++)
            {
                if (GatheringSuccessCount[j] < count)
                {
                    isDone = false;
                    break;
                }
            }
            if (!isDone)
            {
                break;
            }
            DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
            challengesNum++;
        }
        if(challengesNum > ChallengesNum[(int)EChallengesKategorie.gathering])
        {
            ChallengesNum[(int)EChallengesKategorie.gathering] = challengesNum;
        }
        isDone = true;
        challengesNum = 0;


        // 도감 처음 해제
        for (int i = 0; i < _unlockingBookCount.Length; i++)
        {
            if (_unlockingBookCount[i] > 0)
            {
                DatabaseManager.Instance.GetChallengesDic()["RWBG03"].IsDone = true;
                break;
            }
        }

        // 도감 해제
        string type;
        for (int i = 1; i < _challengesDatas[(int)EChallengesKategorie.book].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.book][i].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.book][i].Id;
            type = _challengesDatas[(int)EChallengesKategorie.book][i].Type;
            if(type == "All")
            {
                for (int j = 0; j < _unlockingBookCount.Length; j++)
                {
                    if (_unlockingBookCount[j] < count)
                    {
                        isDone = false;
                        break;
                    }
                }
                if(isDone)
                {
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                    challengesNum++;
                }
            }
            else
            {
                EUnlockingBook bookType = (EUnlockingBook)Enum.Parse(typeof(EUnlockingBook), type);
                if (_unlockingBookCount[(int)bookType] >= count)
                {
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                }
            }
            
        }
        if(challengesNum > ChallengesNum[(int)EChallengesKategorie.book])
        {
            ChallengesNum[(int)EChallengesKategorie.book] = challengesNum;
        }
        challengesNum = 0;


        // 대나무
        for (int i = 0; i < _challengesDatas[(int)EChallengesKategorie.bamboo].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.bamboo][ChallengesNum[(int)EChallengesKategorie.bamboo]].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.bamboo][ChallengesNum[(int)EChallengesKategorie.bamboo]].Id;
            if (_stackedBambooCount > count)
            {
                DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                challengesNum++;
            }
            else
            {
                break;
            }
        }
        if (challengesNum > ChallengesNum[(int)EChallengesKategorie.bamboo])
        {
            ChallengesNum[(int)EChallengesKategorie.bamboo] = challengesNum;
        }
        challengesNum = 0;


        // 상점
        for (int i = 0; i < _challengesDatas[(int)EChallengesKategorie.item].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.item][i].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.item][i].Id;
            type = _challengesDatas[(int)EChallengesKategorie.item][i].Type;

            if (type == "Purchase")
            {
                if (PurchaseCount >= count)
                {
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                }
            }
            else if(type == "Sales")
            {
                if (SalesCount >= count)
                {
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                }
            }
        }


        // 가구
        for (int i = 0; i < _challengesDatas[(int)EChallengesKategorie.furniture].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.furniture][i].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.furniture][i].Id;
            if (FurnitureCount >= count)
            {
                DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                challengesNum++;
            }
            else
            {
                break;
            }
        }
        if (challengesNum > ChallengesNum[(int)EChallengesKategorie.furniture])
        {
            ChallengesNum[(int)EChallengesKategorie.furniture] = challengesNum;
        }
        challengesNum = 0;


        // 요리
        for (int i = 0; i < _challengesDatas[(int)EChallengesKategorie.cook].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.cook][i].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.cook][i].Id;
            if (CookingCount >= count)
            {
                DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                challengesNum++;
            }
            else
            {
                break;
            }
        }
        if (challengesNum > ChallengesNum[(int)EChallengesKategorie.cook])
        {
            ChallengesNum[(int)EChallengesKategorie.cook] = challengesNum;
        }
        challengesNum = 0;


        // 사진
        for (int i = 0; i < _challengesDatas[(int)EChallengesKategorie.camera].Count; i++)
        {
            count = _challengesDatas[(int)EChallengesKategorie.camera][i].Count;
            challengesId = _challengesDatas[(int)EChallengesKategorie.camera][i].Id;
            type = _challengesDatas[(int)EChallengesKategorie.camera][i].Type;
            if (type == "Take")
            {
                if (TakePhotoCount >= count)
                {
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                }
            }
            else if (type == "Sharing")
            {
                if (SharingPhotoCount >= count)
                {
                    DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
                }
            }
        }

    }




    public void MainStoryDone(string id)
    {
        //if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // 메인 스토리 한 장이 끝나면
        //{
        //    Debug.Log("넘어옴");
        //    string storyChallengeId = "RW" + id.Substring(0, 4);
        //    SuccessChallenge(storyChallengeId); // 도전과제 달성
        //}
        //foreach (string key in DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList)
        //{
        //    if (key.Substring(0, 2) != "MS") // 메인 스토리가 아닐 경우
        //    {
        //        continue;
        //    }
        //    if (!_checkStoryCompleteList.Contains(key) && _mainStoryCount.Keys.Contains(key.Substring(0, 4)))
        //    {
        //        _checkStoryCompleteList.Add(key);
        //        _mainStoryCount[key.Substring(0, 4)]--;
        //        if (_mainStoryCount[key.Substring(0, 4)] == 0)
        //        {
        //            string storyChallengeId = "RW" + key.Substring(0, 4);
        //            SuccessChallenge(storyChallengeId); // 도전과제 달성
        //        }
        //    }
        //}

        if (id.Substring(0, 2) != "MS") // 메인 스토리가 아닐 경우
        {
            return;
        }
        if (!_checkStoryCompleteList.Contains(id) && _mainStoryCount.Keys.Contains(id.Substring(0, 4)))
        {
            _checkStoryCompleteList.Add(id);
            _mainStoryCount[id.Substring(0, 4)]--;
            if (_mainStoryCount[id.Substring(0, 4)] == 0)
            {
                string storyChallengeId = "RW" + id.Substring(0, 4);
                SuccessChallenge(storyChallengeId); // 도전과제 달성
            }
        }
    }

    /// <summary>
    /// 채집 각각 n회 이상 성공 </summary>
    public void GatheringSuccess(int gatheringType) // 채집 성공할 때마다 불러오기
    {
        GatheringSuccessCount[gatheringType]++;

        // 몇 번 성공해야 하는지
        int successNum = _challengesDatas[(int)EChallengesKategorie.gathering][ChallengesNum[(int)EChallengesKategorie.gathering]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.gathering][ChallengesNum[(int)EChallengesKategorie.gathering]].Id;

        for (int i = 0; i < GatheringSuccessCount.Length; i++)
        {
            if (GatheringSuccessCount[i] < successNum)
            {
                return;
            }
        }

        // 도전 과제 달성 시
        SuccessChallenge(challengesId);
        ChallengesNum[(int)EChallengesKategorie.gathering]++; // 다음 도전과제 확인할 수 있도록 저장
    }


    /// <summary>
    /// 도감 해제 </summary>
    public void UnlockingBook(string type) // 처음에 카운트 받아온 후 도감 해제될 때마다 실행하기... ? -> IsReceived 변경될 때마다 실행하기
    {
        EUnlockingBook eUnlockingBook = EUnlockingBook.None;


        // NPC
        if(type == "NPC")
        {
            // 처음 초기화 할 때만 이렇게 받아오고 그 후에 ++ 해주기
            _unlockingBookCount[(int)EUnlockingBook.NPC]++;

            eUnlockingBook = EUnlockingBook.NPC;
        }
        // 곤충
        else if(type == "IBG")
        {
            _unlockingBookCount[(int)EUnlockingBook.Bug]++;

            eUnlockingBook = EUnlockingBook.Bug;
            type = "Bug";
        }
        // 물고기
        else if(type == "IFI")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fish]++;

            eUnlockingBook = EUnlockingBook.Fish;
            type = "Fish";
        }
        // 과일
        else if(type == "IFR")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fruit]++;

            eUnlockingBook = EUnlockingBook.Fruit;
            type = "Fruit";
        }
        // 레시피
        else if(type == "Coo")
        {
            _unlockingBookCount[(int)EUnlockingBook.Recipe]++;
            // 아직 미완성 - 추가하기
            eUnlockingBook = EUnlockingBook.Recipe;
            type = "Recipe";
        }
        else
        {
            Debug.Log("해당하는 도전과제가 없습니다.");
            return;
        }

        // 첫 도감 해제
        if (DatabaseManager.Instance.GetChallengesDic()["RWBG03"].IsDone == false)
        {
            SuccessChallenge(_challengesDatas[(int)EChallengesKategorie.book][0].Id);
        }

        // 한 가지 종류 도감 달성 체크
        int challengesNum = _challengesDatas[(int)EChallengesKategorie.book].FindIndex(n => n.Type == type && n.IsDone == false);
        int count = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Id;
        if (_unlockingBookCount[(int)eUnlockingBook] >= count)
        {
            SuccessChallenge(challengesId);
        }

        // 각각의 도감 n개 해제했는지 체크
        if (ChallengesNum[(int)EChallengesKategorie.book] == -1)
        {
            ChallengesNum[(int)EChallengesKategorie.book] = _challengesDatas[(int)EChallengesKategorie.book].FindIndex(n => n.Type == "All" || n.IsDone == false);
        }

        count = _challengesDatas[(int)EChallengesKategorie.book][ChallengesNum[(int)EChallengesKategorie.book]].Count;
        challengesId = _challengesDatas[(int)EChallengesKategorie.book][ChallengesNum[(int)EChallengesKategorie.book]].Id;

        for (int i = 0; i< _unlockingBookCount.Length; i++)
        {
            if (_unlockingBookCount[i] < count)
            {
                return;
            }
        }
        SuccessChallenge(challengesId);
        ChallengesNum[(int)EChallengesKategorie.book] = -1;
    }

    /// <summary>
    /// 누적 대나무 수 달성 </summary>
    private void StackedBamboo()
    {
        int count = _challengesDatas[(int)EChallengesKategorie.bamboo][ChallengesNum[(int)EChallengesKategorie.bamboo]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.bamboo][ChallengesNum[(int)EChallengesKategorie.bamboo]].Id;

        if(_stackedBambooCount >= count)
        {
            SuccessChallenge(challengesId);
            ChallengesNum[(int)EChallengesKategorie.bamboo]++;
        }
    }

    /// <summary>
    /// 상점 사용시 실행 </summary>
    /// <param name="isPurchase">구매: true 판매: false</param>
    public void UsingShop(bool isPurchase) // 기능 구현되면 수정
    {
        // 구매
        if (isPurchase)
        {
            PurchaseCount++;

            // 리스트에서 달성하지 못한 과제 중 첫 번째 찾기
            string challengesId = _challengesDatas[(int)EChallengesKategorie.item].Find(n => n.Type == "Purchase" || n.IsDone == false).Id;

            // 달성했다면
            if (PurchaseCount >= DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // 판매
        else
        {
            SalesCount++;

            string challengesId = _challengesDatas[(int)EChallengesKategorie.item].Find(n => n.Type == "Sales" || n.IsDone == false).Id;

            if (SalesCount >= DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
    }

    /// <summary>
    /// n개의 가구 테마 완성 </summary>
    public void FurnitureArrangement(string id)
    {
        int count = _challengesDatas[(int)EChallengesKategorie.furniture][ChallengesNum[(int)EChallengesKategorie.furniture]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.furniture][ChallengesNum[(int)EChallengesKategorie.furniture]].Id;

        // 추가된 가구와 같은 테마의 가구가 8개면 한 테마 완성
        if (DatabaseManager.Instance.StartPandaInfo.FurnitureInventoryID.Where(furnitureId => furnitureId.Substring(0,4) == id.Substring(0,4)).Count() == 8)
        {
            FurnitureCount++;
            if (FurnitureCount >= count)
            {
                SuccessChallenge(challengesId);
                ChallengesNum[(int)EChallengesKategorie.furniture]++;
            }
        }
    }

    /// <summary>
    /// 요리 n회 이상 제작 </summary>
    public void Cooking(string grade)
    {
        int Count = _challengesDatas[(int)EChallengesKategorie.cook][ChallengesNum[(int)EChallengesKategorie.cook]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.cook][ChallengesNum[(int)EChallengesKategorie.cook]].Id;

        // A등급 이상인지 확인
        if (grade == "A" || grade == "S")
        {
            CookingCount++;
            if(CookingCount >= Count)
            {
                SuccessChallenge(challengesId);
                ChallengesNum[(int)EChallengesKategorie.cook]++;
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
            TakePhotoCount++;

            // 리스트에서 달성하지 못한 사진찍는 과제 중 첫 번째 찾기
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Take" || n.IsDone == false).Id;
            
            // 달성했다면
            if (TakePhotoCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // 사진 공유하기
        else
        {
            SharingPhotoCount++;

            // 리스트에서 달성하지 못한 사진공유 과제 중 첫 번째 찾기
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Sharing" || n.IsDone == false).Id;

            if (SharingPhotoCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
    }


    private void SuccessChallenge(string challengesId)
    {
        Debug.Log("도전과제 완료: id" +  challengesId);
        DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;


        ChallengeDone?.Invoke(challengesId);
        DatabaseManager.Instance.UserInfo.ChallengesUserData.AsyncSaveChallengesData(10);
        BackendManager.Instance.LogUpload("ChallengeLog", "Success" + "(" + challengesId + ")");
    }

    public void EarningRewards(string challengesId)
    {
        // 대나무 획득

        // 아이템 획득 - 도구
        if (!string.IsNullOrWhiteSpace(DatabaseManager.Instance.GetChallengesDic()[challengesId].Item))
        {
            GameManager.Instance.Player.AddItemById(DatabaseManager.Instance.GetChallengesDic()[challengesId].Item);
        }

        DatabaseManager.Instance.GetChallengesDic()[challengesId].IsClear = true;
        DatabaseManager.Instance.UserInfo.ChallengesUserData.AsyncSaveChallengesData(10);
        BackendManager.Instance.LogUpload("ChallengeLog", "Success" + "(" + challengesId + ")");
    }
}