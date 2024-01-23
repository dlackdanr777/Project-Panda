using System;
using UnityEngine;

public class Challenges
{
    public Action<string> ChallengeDone;

    // ä�� ������ ������ ����
    public int[] CollectionSuccessNum = new int[System.Enum.GetValues(typeof(GatheringItemType)).Length -1]; // ä�� ���� Ƚ��

    public void Register()
    {

    }

    public void MainStoryDone(string id)
    {
        if (id.Substring(0, 4) != DatabaseManager.Instance.DialogueDatabase.GetStoryDialogue(id).NextStoryID.Substring(0, 4)) // ���� ���丮 �� ���� ������
        {
            string storyChallengeId = "RW" + id.Substring(0, 4);
            ChallengeDone?.Invoke(storyChallengeId); // �������� �޼�
        }
    }

    public void GatheringSuccess()
    {
        int successNum = 1; // �� �� �����ؾ� �ϴ��� .. �޾ƿ;� ��
        string challengesId = "RWBG01"; // �̰� ��� ����.. �ؾ����� .. ���
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
        // �볪�� ȹ��

        // ������ ȹ��
    }
}
