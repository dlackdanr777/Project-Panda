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
    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� ��������

    // ä�� ������ ������ ����
    public int[] GatheringSuccessNum = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��

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

    // �丮 ���� Ƚ��
    private int CookingCount = 0;

    // ����
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
        GatheringSuccessNum[gatheringType]++;

        // �� �� �����ؾ� �ϴ���
        int successNum = _challengesDatas[(int)EChallengesKategorie.gathering][_challengesNum[(int)EChallengesKategorie.gathering]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.gathering][_challengesNum[(int)EChallengesKategorie.gathering]].Id;

        for (int i = 0; i < GatheringSuccessNum.Length; i++)
        {
            if (GatheringSuccessNum[i] < successNum)
            {
                return;
            }
        }

        // ���� ���� �޼� ��
        ChallengeDone?.Invoke(challengesId);
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
            ChallengeDone?.Invoke(challengesId);
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
        ChallengeDone?.Invoke(challengesId);
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
            ChallengeDone?.Invoke(challengesId);
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

        }
        // �Ǹ�
        else
        {

        }
    }

    /// <summary>
    /// n���� ���� �׸� �ϼ� </summary>
    private void FurnitureArrangement()
    {

    }

    /// <summary>
    /// �丮 nȸ �̻� ���� </summary>
    public void Cooking(string id, string grade)
    {
        int Count = _challengesDatas[(int)EChallengesKategorie.cook][_challengesNum[(int)EChallengesKategorie.cook]].Count;
        string challengesId = _challengesDatas[(int)EChallengesKategorie.cook][_challengesNum[(int)EChallengesKategorie.cook]].Id;

        // A��� �̻����� Ȯ��
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
    /// ���� ���&�����ϱ� </summary>
    public void Photo(bool isTakePhoto)
    {
        // ���� ���
        if(isTakePhoto)
        {
            _takePhoto++;

            // ����Ʈ���� �޼����� ���� ������� ���� �� ù ��° ã��
            string challengesId = _challengesDatas[(int)EChallengesKategorie.camera].Find(n => n.Type == "Take" || n.IsSuccess == false).Id;
            
            // �޼��ߴٸ�
            if (_takePhoto > DatabaseManager.Instance.GetChallengesDic()[challengesId].Count)
            {
                ChallengeDone?.Invoke(challengesId);
            }
        }
        // ���� �����ϱ�
        else
        {
            _shatingPhoto++;

            // ����Ʈ���� �޼����� ���� �������� ���� �� ù ��° ã��
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

        // �볪�� ȹ��
        GameManager.Instance.Player.GainBamboo(DatabaseManager.Instance.GetChallengesDic()[id].BambooCount);

        // ������ ȹ��
        //GameManager.Instance.Player.GetItemInventory(InventoryItemField.GatheringItem)[(int)_cookingSystem.InventoryType].Add(_currentRecipeData.Item);
    }
}
