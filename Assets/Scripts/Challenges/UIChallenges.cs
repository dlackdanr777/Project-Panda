using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.DataBind;

public class UIChallenges : MonoBehaviour
{

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

        DatabaseManager.Instance.Challenges.ChallengeDone += ChallengeDone;

        _challengeDonePfDic = new Dictionary<string, Image>();

        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        // 개수만큼 프리팹 생성
        foreach (string key in challengesDic.Keys)
        {
            GameObject challengeSlot = Instantiate(_challengesSlotPf, _content.transform);
            challengeSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = challengesDic[key].Name;
            challengeSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = challengesDic[key].Description;
            _challengeDonePfDic.Add(key, challengeSlot.transform.GetChild(2).GetComponent<Image>());

            // 성공한 도전 과제라면 완료 이미지로 변경
            if (challengesDic[key].IsSuccess == true)
            {
                _challengeDonePfDic[key].sprite = _doneImage;
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
