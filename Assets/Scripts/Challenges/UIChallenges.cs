using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.DataBind;

public class UIChallenges : MonoBehaviour
{
    private Challenges _challenges;

    [SerializeField] private GameObject _uiChallengesPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private Sprite _doneImage;

    [SerializeField] private GameObject _challengesSlotPf;
    private Dictionary<string, Image> _challengeDonePfDic;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _challenges = Challenges.Instance;
        _challenges.ChallengeDone += ChallengeDone;

        _challengeDonePfDic = new Dictionary<string, Image>();

        // 개수만큼 프리팹 생성
        for(int i = 0; i < System.Enum.GetValues(typeof(EChallenges)).Length; i++) // 도전과제 종류
        {
            foreach (string key in ChallengesDatabase.Instance.ChallengesDic[i].Keys)
            {
                GameObject challengeSlot = Instantiate(_challengesSlotPf, _content.transform);
                challengeSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ChallengesDatabase.Instance.ChallengesDic[i][key].Name;
                challengeSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ChallengesDatabase.Instance.ChallengesDic[i][key].Description;
                _challengeDonePfDic.Add(key, challengeSlot.transform.GetChild(2).GetComponent<Image>());
            }
        }
        
        DataBind.SetButtonValue("ShowChallengesButton", ()=>_uiChallengesPanel.SetActive(true));
        DataBind.SetButtonValue("CloseChallengesButton", ()=>_uiChallengesPanel.SetActive(false));

    }

    private void ChallengeDone(string id)
    {
        _challengeDonePfDic[id].sprite = _doneImage;
    }
}
