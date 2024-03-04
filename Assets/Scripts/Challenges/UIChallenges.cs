using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.DataBind;
using Unity.VisualScripting;

public class UIChallenges : UIView
{
    [SerializeField] private BambooFieldSystem _bambooFieldSystem; // 대나무 획득 위해 필요

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
        // 개수만큼 프리팹 생성
        foreach (string key in challengesDic.Keys)
        {
            UIChallengeSlot slot = Instantiate(_slotPrefab, _content.transform);
            slot.Init(challengesDic[key], ChallengeClear);

            _slotDic.Add(key, slot);

            // 성공한 도전 과제라면 완료 이미지로 변경
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
    /// 도전과제 완료 </summary>
    private void ChallengeDone(string id)
    {
        _slotDic[id].IsDone();
    }

    /// <summary>
    /// 도전과제 완료 후 클릭 </summary>
    private void ChallengeClear(string id)
    {
        if (DatabaseManager.Instance.GetChallengesDic()[id].IsDone == true)
        {
            
            DatabaseManager.Instance.Challenges.EarningRewards(id);

            // 대나무 증가하는 애니메이션 실행
            //EarningBamboo(id);
            int getBoombooAmount = DatabaseManager.Instance.GetChallengesDic()[id].BambooCount;
            GameManager.Instance.Player.GainBamboo(getBoombooAmount);

            _clearChallenges.Add(id); // 완료된 도전과제 저장 후 창 종료 시 맨 아래로 이동
        }
    }


    private void CloseChallenges()
    {
        // 도전과제를 리스트의 맨 아래로 이동
        for(int i = 0; i < _clearChallenges.Count; i++)
        {
            _slotDic[_clearChallenges[i]].CloseSlot();
        }

        _clearChallenges.Clear();

        // 스크롤 뷰 맨 위가 보이도록 설정
        _scrollRect.verticalNormalizedPosition = 1f;
    }


    private void EarningBamboo(string id)
    {
        _bambooFieldSystem.HarvestBamboo(0, DatabaseManager.Instance.GetChallengesDic()[id].BambooCount, _slotDic[id].GetBambooTransform);
    }
}
