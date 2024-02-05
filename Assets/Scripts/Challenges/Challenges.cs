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
    public int[] ChallengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� ��������

    // �ٸ� ���� ���� �� �Ϸ�� ���� ���� Id ���� �� ���� ���� ���ƿͼ� �Ѳ����� ����
    //private List<string> _doneChallenges = new List<string>();

    #region ����
    // ä�� ������ ������ ����
    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��

    // ���� ����
    private int[] _unlockingBookCount = new int[System.Enum.GetValues(typeof(EUnlockingBook)).Length - 1];

    // �볪�� ���� ����
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

    // ����
    public int PurchaseCount;
    public int SalesCount;

    // ����
    public int FurnitureCount;

    // �丮 ���� Ƚ��
    public int CookingCount;

    // ����
    public int TakePhotoCount;
    public int SharingPhotoCount;
    #endregion

    public void Register()
    {
        LoadData();
        CheckIsDone();

    }

    public void LoadData()
    {
        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        for (int i = 0; i < System.Enum.GetValues(typeof(EChallengesKategorie)).Length; i++)
        {
            _challengesDatas[i] = new List<ChallengesData>();
        }

        // ī�װ����� ���� ���� �з�
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

        // ����� ���� �ҷ�����
        ChallengesNum = DatabaseManager.Instance.UserInfo.ChallengesNum;
        GatheringSuccessCount = DatabaseManager.Instance.UserInfo.GatheringSuccessCount;
        _stackedBambooCount = DatabaseManager.Instance.UserInfo.ChallengesCount[0];
        PurchaseCount = DatabaseManager.Instance.UserInfo.ChallengesCount[1];
        SalesCount = DatabaseManager.Instance.UserInfo.ChallengesCount[2];
        FurnitureCount = DatabaseManager.Instance.UserInfo.ChallengesCount[3];
        CookingCount = DatabaseManager.Instance.UserInfo.ChallengesCount[4];
        TakePhotoCount = DatabaseManager.Instance.UserInfo.ChallengesCount[5];
        SharingPhotoCount = DatabaseManager.Instance.UserInfo.ChallengesCount[6];

        // ���� Count �ʱ�ȭ
        _unlockingBookCount[(int)EUnlockingBook.NPC] = DatabaseManager.Instance.GetNPCList().Where(n => n.IsReceived == true).Count();
        _unlockingBookCount[(int)EUnlockingBook.Bug] = DatabaseManager.Instance.GetBugItemList().Where(n => n.IsReceived == true).Count();
        _unlockingBookCount[(int)EUnlockingBook.Fish] = DatabaseManager.Instance.GetFishItemList().Where(n => n.IsReceived == true).Count();
        _unlockingBookCount[(int)EUnlockingBook.Fruit] = DatabaseManager.Instance.GetFruitItemList().Where(n => n.IsReceived == true).Count();

        // ������ - �߰��ϱ�
    }

    /// <summary>
    /// IsDone�� ����� ����Ǿ����� Ȯ�� </summary>
    private void CheckIsDone()
    {
        bool isDone = true;

        int count = 0;
        string challengesId = "";
        int challengesNum = 0;


        // ���ν��丮
        for (int i = 0; i < StoryManager.Instance._storyCompletedList.Count - 1; i++)
        {
            string id = StoryManager.Instance._storyCompletedList[i];
            if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4))
            {
                challengesId = "RW" + id.Substring(0, 4);
                DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
            }
        }


        // ä��
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


        // ���� ó�� ����
        for (int i = 0; i < _unlockingBookCount.Length; i++)
        {
            if (_unlockingBookCount[i] > 0)
            {
                DatabaseManager.Instance.GetChallengesDic()["RWBG03"].IsDone = true;
                break;
            }
        }

        // ���� ����
        string type = "";
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


        // �볪��
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


        // ����
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


        // ����
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


        // �丮
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


        // ����
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
        Debug.Log(id);
        if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // ���� ���丮 �� ���� ������
        {
            Debug.Log("�Ѿ��");
            string storyChallengeId = "RW" + id.Substring(0, 4);
            SuccessChallenge(storyChallengeId); // �������� �޼�
        }
    }

    /// <summary>
    /// ä�� ���� nȸ �̻� ���� </summary>
    public void GatheringSuccess(int gatheringType) // ä�� ������ ������ �ҷ�����
    {
        GatheringSuccessCount[gatheringType]++;

        // �� �� �����ؾ� �ϴ���
        int successNum = _challengesDatas[(int)EChallengesKategorie.gathering][ChallengesNum[(int)EChallengesKategorie.gathering]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.gathering][ChallengesNum[(int)EChallengesKategorie.gathering]].Id;

        for (int i = 0; i < GatheringSuccessCount.Length; i++)
        {
            if (GatheringSuccessCount[i] < successNum)
            {
                return;
            }
        }

        // ���� ���� �޼� ��
        SuccessChallenge(challengesId);
        ChallengesNum[(int)EChallengesKategorie.gathering]++; // ���� �������� Ȯ���� �� �ֵ��� ����
    }


    /// <summary>
    /// ���� ���� </summary>
    public void UnlockingBook(string type) // ó���� ī��Ʈ �޾ƿ� �� ���� ������ ������ �����ϱ�... ? -> IsReceived ����� ������ �����ϱ�
    {
        EUnlockingBook eUnlockingBook = EUnlockingBook.None;

        // ù ���� ����
        if (DatabaseManager.Instance.GetChallengesDic()["RWBG03"].IsDone == false)
        {
            SuccessChallenge(_challengesDatas[(int)EChallengesKategorie.book][0].Id);
        }

        // NPC
        if(type == "NPC")
        {
            // ó�� �ʱ�ȭ �� ���� �̷��� �޾ƿ��� �� �Ŀ� ++ ���ֱ�
            _unlockingBookCount[(int)EUnlockingBook.NPC]++;

            eUnlockingBook = EUnlockingBook.NPC;
        }
        // ����
        else if(type == "IBG")
        {
            _unlockingBookCount[(int)EUnlockingBook.Bug]++;

            eUnlockingBook = EUnlockingBook.Bug;
            type = "Bug";
        }
        // �����
        else if(type == "IFI")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fish]++;

            eUnlockingBook = EUnlockingBook.Fish;
            type = "Fish";
        }
        // ����
        else if(type == "IFR")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fruit]++;

            eUnlockingBook = EUnlockingBook.Fruit;
            type = "Fruit";
        }
        // ������
        else if(type == "Recipe")
        {
            // ���� �̿ϼ� - �߰��ϱ�
            eUnlockingBook = EUnlockingBook.Recipe;
            type = "Recipe";
        }

        // �� ���� ���� ���� �޼� üũ
        int challengesNum = _challengesDatas[(int)EChallengesKategorie.book].FindIndex(n => n.Type == type || n.IsDone == false);
        int count = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Id;
        if (_unlockingBookCount[(int)eUnlockingBook] >= count)
        {
            SuccessChallenge(challengesId);
        }

        // ������ ���� n�� �����ߴ��� üũ
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
    /// ���� �볪�� �� �޼� </summary>
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
    /// ���� ���� ���� </summary>
    /// <param name="isPurchase">����: true �Ǹ�: false</param>
    public void UsingShop(bool isPurchase) // ��� �����Ǹ� ����
    {
        // ����
        if (isPurchase)
        {
            PurchaseCount++;

            // ����Ʈ���� �޼����� ���� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.item].Find(n => n.Type == "Purchase" || n.IsDone == false).Id;

            // �޼��ߴٸ�
            if (PurchaseCount >= DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // �Ǹ�
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
    /// n���� ���� �׸� �ϼ� </summary>
    public void FurnitureArrangement(string id)
    {
        int count = _challengesDatas[(int)EChallengesKategorie.furniture][ChallengesNum[(int)EChallengesKategorie.furniture]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.furniture][ChallengesNum[(int)EChallengesKategorie.furniture]].Id;

        // �߰��� ������ ���� �׸��� ������ 8���� �� �׸� �ϼ�
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
    /// �丮 nȸ �̻� ���� </summary>
    public void Cooking(string grade)
    {
        int Count = _challengesDatas[(int)EChallengesKategorie.cook][ChallengesNum[(int)EChallengesKategorie.cook]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.cook][ChallengesNum[(int)EChallengesKategorie.cook]].Id;

        // A��� �̻����� Ȯ��
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
    /// ���� ���&�����ϱ� </summary>
    public void Photo(bool isTakePhoto)
    {
        // ���� ���
        if(isTakePhoto)
        {
            TakePhotoCount++;

            // ����Ʈ���� �޼����� ���� ������� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Take" || n.IsDone == false).Id;
            
            // �޼��ߴٸ�
            if (TakePhotoCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // ���� �����ϱ�
        else
        {
            SharingPhotoCount++;

            // ����Ʈ���� �޼����� ���� �������� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Sharing" || n.IsDone == false).Id;

            if (SharingPhotoCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
    }


    private void SuccessChallenge(string challengesId)
    {
        Debug.Log("�������� �Ϸ�: id" +  challengesId);
        DatabaseManager.Instance.GetChallengesDic()[challengesId].IsDone = true;
        DatabaseManager.Instance.UserInfo.ChallengeDoneId.Add(challengesId);

        // ���� ���� ���̸� �ٷ� �������� UI�� �ݿ�
        //if (SceneManager.GetActiveScene().name == "ChallengesTest") // ���� �� �̸����� ����
        //{
            ChallengeDone?.Invoke(challengesId);
        //}
        //else // �ƴ϶�� �Ϸ��� �������� ���� �� ���ξ����� ���ƿ��� �� UI�� �ݿ�
        //{
            //_doneChallenges.Add(challengesId);
            //EarningRewards(challengesId);
        //}
    }

    public void EarningRewards(string challengesId)
    {
        // �볪�� ȹ��


        // ������ ȹ�� - ����
        GameManager.Instance.Player.ToolItemInventory[0].AddById
            (InventoryItemField.Tool, DatabaseManager.Instance.GetChallengesDic()[challengesId].Item);

        DatabaseManager.Instance.GetChallengesDic()[challengesId].IsClear = true;
        DatabaseManager.Instance.UserInfo.ChallengeClearId.Add(challengesId);
    }
}