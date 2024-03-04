using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.DataBind;
using Unity.VisualScripting;

public class UIChallenges : UIView
{
    [SerializeField] private BambooFieldSystem _bambooFieldSystem; // �볪�� ȹ�� ���� �ʿ�

    [SerializeField] private GameObject _uiChallengesPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _backGroundImage;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Sprite _doneImage;

    [SerializeField] private GameObject _challengesSlotPf;

    [SerializeField] private UIChallengeSlot _slotPrefab;
    private Dictionary<string, UIChallengeSlot> _slotDic = new Dictionary<string, UIChallengeSlot>();

    private List<string> _clearChallenges = new List<string>();


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        DatabaseManager.Instance.Challenges.ChallengeDone += ChallengeDone;
        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        _slotDic.Clear();
        // ������ŭ ������ ����
        foreach (string key in challengesDic.Keys)
        {
            UIChallengeSlot slot = Instantiate(_slotPrefab, _content.transform);
            slot.Init(challengesDic[key], ChallengeClear);

            _slotDic.Add(key, slot);

            // ������ ���� ������� �Ϸ� �̹����� ����
            if (challengesDic[key].IsDone == true)
            {
                slot.IsDone();
                if (challengesDic[key].IsClear == true)
                {
                    slot.IsClear();
                    _clearChallenges.Add(key);
                }
            }
        }
        CloseChallenges();
    }

    private void OnDestroy()
    {
            DatabaseManager.Instance.Challenges.ChallengeDone -= ChallengeDone;
    }


    public override void Show()
    {
        gameObject.SetActive(true);
    }


    public override void Hide()
    {
        gameObject.SetActive(false);
        CloseChallenges();
    }


    /// <summary>
    /// �������� �Ϸ� </summary>
    private void ChallengeDone(string id)
    {
        _slotDic[id].IsDone();
    }

    /// <summary>
    /// �������� �Ϸ� �� Ŭ�� </summary>
    private void ChallengeClear(string id)
    {
        if (DatabaseManager.Instance.GetChallengesDic()[id].IsDone == true)
        {
            
            DatabaseManager.Instance.Challenges.EarningRewards(id);

            // �볪�� �����ϴ� �ִϸ��̼� ����
            //EarningBamboo(id);
            int getBoombooAmount = DatabaseManager.Instance.GetChallengesDic()[id].BambooCount;
            GameManager.Instance.Player.GainBamboo(getBoombooAmount);

            _clearChallenges.Add(id); // �Ϸ�� �������� ���� �� â ���� �� �� �Ʒ��� �̵�
        }
    }


    private void CloseChallenges()
    {
        // ���������� ����Ʈ�� �� �Ʒ��� �̵�
        for(int i = 0; i < _clearChallenges.Count; i++)
        {
            _slotDic[_clearChallenges[i]].CloseSlot();
        }

        _clearChallenges.Clear();

        // ��ũ�� �� �� ���� ���̵��� ����
        _scrollRect.verticalNormalizedPosition = 1f;
    }


    private void EarningBamboo(string id)
    {
        _bambooFieldSystem.HarvestBamboo(0, DatabaseManager.Instance.GetChallengesDic()[id].BambooCount, _slotDic[id].GetBambooTransform);
    }
}
