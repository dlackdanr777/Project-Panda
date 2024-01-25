using Muks.DataBind;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� ��������

    // �ٸ� ���� ���� �� �Ϸ�� ���� ���� Id ���� �� ���� ���� ���ƿͼ� �Ѳ����� ����
    private List<string> _doneChallenges = new List<string>();

    #region ����
    // ä�� ������ ������ ����
    public int[] GatheringSuccessCount = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��

    // ���� ����
    private int[] _unlockingBookCount = new int[System.Enum.GetValues(typeof(EUnlockingBook)).Length];

    // �볪�� ���� ����
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

    // ����
    private int _purchaseCount;
    private int _salesCount;

    // ����
    private int _furnitureCount;

    // �丮 ���� Ƚ��
    private int CookingCount = 0;

    // ����
    private int _takePhotoCount;
    private int _sharingPhotoCount;
    #endregion

    public void Register()
    {
        ChallengeDone += EarningRewards;

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
    }

    public void MainStoryDone(string id)
    {
        if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // ���� ���丮 �� ���� ������
        {
            string storyChallengeId = "RW" + id.Substring(0, 4);
            ChallengeDone?.Invoke(storyChallengeId); // �������� �޼�
        }
    }

    /// <summary>
    /// ä�� ���� nȸ �̻� ���� </summary>
    public void GatheringSuccess(int gatheringType) // ä�� ������ ������ �ҷ�����
    {
        GatheringSuccessCount[gatheringType]++;

        // �� �� �����ؾ� �ϴ���
        int successNum = _challengesDatas[(int)EChallengesKategorie.gathering][_challengesNum[(int)EChallengesKategorie.gathering]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.gathering][_challengesNum[(int)EChallengesKategorie.gathering]].Id;

        for (int i = 0; i < GatheringSuccessCount.Length; i++)
        {
            if (GatheringSuccessCount[i] < successNum)
            {
                return;
            }
        }

        // ���� ���� �޼� ��
        SuccessChallenge(challengesId);
        _challengesNum[(int)EChallengesKategorie.gathering]++; // ���� �������� Ȯ���� �� �ֵ��� ����
    }

    /// <summary>
    /// ���� ���� </summary>
    public void UnlockingBook(string type) // ó���� ī��Ʈ �޾ƿ� �� ���� ������ ������ �����ϱ�... ? -> IsReceived ����� ������ �����ϱ�
    {
        EUnlockingBook eUnlockingBook = EUnlockingBook.NPC;

        // NPC
        if(type == "NPC")
        {
            // ó�� �ʱ�ȭ �� ���� �̷��� �޾ƿ��� �� �Ŀ� ++ ���ֱ�
            _unlockingBookCount[(int)EUnlockingBook.NPC] = DatabaseManager.Instance.GetNPCList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.NPC;
        }
        // ����
        else if(type == "Bug")
        {
            _unlockingBookCount[(int)EUnlockingBook.Bug] = DatabaseManager.Instance.GetBugItemList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.Bug;
        }
        // �����
        else if(type == "Fish")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fish] = DatabaseManager.Instance.GetFishItemList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.Fish;
        }
        // ����
        else if(type == "Fruit")
        {
            _unlockingBookCount[(int)EUnlockingBook.Fruit] = DatabaseManager.Instance.GetFruitItemList().Where(n => n.IsReceived == true).Count();

            eUnlockingBook = EUnlockingBook.Fruit;
        }
        // ������
        else if(type == "Recipe")
        {
            // �߰�
            eUnlockingBook = EUnlockingBook.Recipe;
        }

        // �� ���� ���� ���� �޼� üũ
        int challengesNum = _challengesDatas[(int)EChallengesKategorie.book].FindIndex(n => n.Type == type || n.IsSuccess == false);
        int count = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.book][challengesNum].Id;
        if (_unlockingBookCount[(int)eUnlockingBook] > count)
        {
            SuccessChallenge(challengesId);
        }

        // ������ ���� n�� �����ߴ��� üũ
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
        SuccessChallenge(challengesId);
        _challengesNum[(int)EChallengesKategorie.book] = -1;
    }

    /// <summary>
    /// ���� �볪�� �� �޼� </summary>
    private void StackedBamboo()
    {
        int count = _challengesDatas[(int)EChallengesKategorie.bamboo][_challengesNum[(int)EChallengesKategorie.bamboo]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.bamboo][_challengesNum[(int)EChallengesKategorie.bamboo]].Id;

        if(_stackedBambooCount > count)
        {
            SuccessChallenge(challengesId);
            _challengesNum[(int)EChallengesKategorie.bamboo]++;
        }
    }

    /// <summary>
    /// ���� ���� ���� </summary>
    private void UsingShop(bool isPurchase) // ��� �����Ǹ� ����
    {
        // ����
        if (isPurchase)
        {
            _purchaseCount++;

            // ����Ʈ���� �޼����� ���� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.item].Find(n => n.Type == "Purchase" || n.IsSuccess == false).Id;

            // �޼��ߴٸ�
            if (_purchaseCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // �Ǹ�
        else
        {
            _salesCount++;

            string challengesId = _challengesDatas[(int)EChallengesKategorie.item].Find(n => n.Type == "Sales" || n.IsSuccess == false).Id;

            if (_salesCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
    }

    /// <summary>
    /// n���� ���� �׸� �ϼ� </summary>
    public void FurnitureArrangement(string id)
    {
        int count = _challengesDatas[(int)EChallengesKategorie.furniture][_challengesNum[(int)EChallengesKategorie.furniture]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.furniture][_challengesNum[(int)EChallengesKategorie.furniture]].Id;

        // �߰��� ������ ���� �׸��� ������ 8���� �� �׸� �ϼ�
        if (DatabaseManager.Instance.StartPandaInfo.FurnitureInventoryID.Where(furnitureId => furnitureId.Substring(0,4) == id.Substring(0,4)).Count() == 8)
        {
            _furnitureCount++;
            if (_furnitureCount > count)
            {
                SuccessChallenge(challengesId);
                _challengesNum[(int)EChallengesKategorie.furniture]++;
            }
        }
    }

    /// <summary>
    /// �丮 nȸ �̻� ���� </summary>
    public void Cooking(string grade)
    {
        int Count = _challengesDatas[(int)EChallengesKategorie.cook][_challengesNum[(int)EChallengesKategorie.cook]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.cook][_challengesNum[(int)EChallengesKategorie.cook]].Id;

        // A��� �̻����� Ȯ��
        if (grade == "A" || grade == "S")
        {
            CookingCount++;
            if(CookingCount >= Count)
            {
                SuccessChallenge(challengesId);
                _challengesNum[(int)EChallengesKategorie.cook]++;
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
            _takePhotoCount++;

            // ����Ʈ���� �޼����� ���� ������� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Take" || n.IsSuccess == false).Id;
            
            // �޼��ߴٸ�
            if (_takePhotoCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
        // ���� �����ϱ�
        else
        {
            _sharingPhotoCount++;

            // ����Ʈ���� �޼����� ���� �������� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Sharing" || n.IsSuccess == false).Id;

            if (_sharingPhotoCount > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                SuccessChallenge(challengesId);
            }
        }
    }


    private void SuccessChallenge(string challengesId)
    {
        DatabaseManager.Instance.GetChallengesDic()[challengesId].IsSuccess = true;

        // ���� ���� ���̸� �ٷ� �������� �Ϸ�
        if (SceneManager.GetActiveScene().name == "ChallengesTest") // ���� �� �̸����� ����
        {
            ChallengeDone?.Invoke(challengesId);
        }
        else // �ƴ϶�� �Ϸ��� �������� ���� �� ���ξ����� ���ƿ��� �� �Ϸ�
        {
            _doneChallenges.Add(challengesId);
        }
    }

    private void EarningRewards(string challengesId)
    {
        // �볪�� ȹ��
        GameManager.Instance.Player.GainBamboo(DatabaseManager.Instance.GetChallengesDic()[challengesId].BambooCount);

        // ������ ȹ�� - ����
        GameManager.Instance.Player.ToolItemInventory[0].AddById
            (InventoryItemField.Tool, (int)ToolItemType.GatheringTool, DatabaseManager.Instance.GetChallengesDic()[challengesId].Item);



    }
}
