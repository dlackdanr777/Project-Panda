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
    None,
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
    private int[] _unlockingBookCount = new int[System.Enum.GetValues(typeof(EUnlockingBook)).Length];

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
    public int PurchaseCount { get; private set; }
    public int SalesCount { get; private set; }

    // ����
    public int FurnitureCount { get; private set; }

    // �丮 ���� Ƚ��
    public int CookingCount { get; private set; }

    // ����
    public int TakePhotoCount { get; private set; }
    public int SharingPhotoCount { get; private set; }
    #endregion

    public void Register()
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


    public void MainStoryDone(string id)
    {
        if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // ���� ���丮 �� ���� ������
        {
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
        if (_unlockingBookCount[(int)EUnlockingBook.None] == 0)
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
        if (_unlockingBookCount[(int)eUnlockingBook] > count)
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

        for (int i = 0; i< System.Enum.GetValues(typeof(EUnlockingBook)).Length; i++ )
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

        if(_stackedBambooCount > count)
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
            if (PurchaseCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // �Ǹ�
        else
        {
            SalesCount++;

            string challengesId = _challengesDatas[(int)EChallengesKategorie.item].Find(n => n.Type == "Sales" || n.IsDone == false).Id;

            if (SalesCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
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
            if (FurnitureCount > count)
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
        DatabaseManager.Instance.UserInfo.ChallengeDoneId.Remove(challengesId);
        DatabaseManager.Instance.UserInfo.ChallengeClearId.Add(challengesId);
    }
}