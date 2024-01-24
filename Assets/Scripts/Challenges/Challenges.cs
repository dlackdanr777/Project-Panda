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
    private int[] _challengesNum = new int[System.Enum.GetValues(typeof(EChallengesKategorie)).Length]; // ���� ��������

    // ä�� ������ ������ ����
    public int[] GatheringSuccessNum = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length - 1]; // ä�� ���� Ƚ��

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
    public void UnlockingBook() // ó���� ī��Ʈ �޾ƿ� �� ���� ������ ������ �����ϱ�... ?
    {
        // ���(+����, �߿��� ����)


        // NPC
        int npcNum = DatabaseManager.Instance.GetNPCList().Where(n => n.IsReceived == true).Count();

        // ����
        int bugNum = DatabaseManager.Instance.GetBugItemList().Where(n => n.IsReceived == true).Count();

        // �����
        int fishNum = DatabaseManager.Instance.GetFishItemList().Where(n => n.IsReceived == true).Count();

        // ����
        int fruitNum = DatabaseManager.Instance.GetFruitItemList().Where(n => n.IsReceived== true).Count();

        // ������
        
    }

    /// <summary>
    /// ���� �볪�� �� �޼� </summary>
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
    /// ���� ���� ���� </summary>
    private void UsingShop()
    {
        // ����

        // �Ǹ�
    }

    /// <summary>
    /// ������ ��ġ�� n���� ���� �׸� �ϼ� </summary>
    private void FurnitureArrangement()
    {

    }

    /// <summary>
    /// �丮 nȸ �̻� ���� </summary>
    private void Cooking()
    {
        // A��� �̻����� Ȯ��
    }

    /// <summary>
    /// ���� ���&�����ϱ� </summary>
    private void Photo()
    {
        // ���� ���

        // ���� �����ϱ�
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
