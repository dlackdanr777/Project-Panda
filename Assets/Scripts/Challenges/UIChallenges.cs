using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Muks.DataBind;

public class UIChallenges : UIView
{
    [SerializeField] private BambooFieldSystem _bambooFieldSystem; // �볪�� ȹ�� ���� �ʿ�

    [SerializeField] private GameObject _uiChallengesPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _backGroundImage;
    [SerializeField] private Sprite _doneImage;
    [SerializeField] private ScrollRect _scrollRect;

    [SerializeField] private GameObject _challengesSlotPf;
    private Dictionary<string, Image> _challengesSlotImageDic;
    private Dictionary<string, Image> _challengeDoneImageDic;
    private Dictionary<string, GameObject> _challengeClearImageDic;
    private Dictionary<string, Button> _challengeButtonDic;

    private List<string> _clearChallenges = new List<string>();


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        DatabaseManager.Instance.Challenges.ChallengeDone += ChallengeDone;

        _challengesSlotImageDic = new Dictionary<string, Image>();
        _challengeDoneImageDic = new Dictionary<string, Image>();
        _challengeClearImageDic = new Dictionary<string, GameObject>();
        _challengeButtonDic = new Dictionary<string, Button>();

        Dictionary<string, ChallengesData> challengesDic = DatabaseManager.Instance.GetChallengesDic();

        // ������ŭ ������ ����
        foreach (string key in challengesDic.Keys)
        {
            GameObject challengeSlot = Instantiate(_challengesSlotPf, _content.transform);
            challengeSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = challengesDic[key].Name;
            challengeSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = challengesDic[key].Description;

            _challengesSlotImageDic.Add(key, challengeSlot.GetComponent<Image>());
            _challengeDoneImageDic.Add(key, challengeSlot.transform.GetChild(2).GetComponent<Image>());
            _challengeClearImageDic.Add(key, challengeSlot.transform.GetChild(3).gameObject);

            _challengeButtonDic[key] = challengeSlot.GetComponent<Button>();
            if (_challengeButtonDic[key] != null)
            {
                _challengeButtonDic[key].onClick.AddListener(() => ChallengeClear(key));
            }


            // ������ ���� ������� �Ϸ� �̹����� ����
            if (challengesDic[key].IsDone == true)
            {
                ChallengeDone(key);
                if (challengesDic[key].IsClear == true)
                {
                    //ChallengeClear(key);

                    Destroy(_challengeButtonDic[key].GetComponent<Button>());
                    _challengeClearImageDic[key].SetActive(true);
                    _challengesSlotImageDic[key].color = new Color(0.5f, 0.5f, 0.5f, 1);

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
        _challengeDoneImageDic[id].sprite = _doneImage;

        // �Ϸ��� ���������� ����Ʈ�� �� ���� �̵�
        RectTransform slotTransform = _challengesSlotImageDic[id].GetComponent<RectTransform>();
        slotTransform.SetAsFirstSibling();
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

            Destroy(_challengeButtonDic[id].GetComponent<Button>());
            _challengeClearImageDic[id].SetActive(true);
            _challengesSlotImageDic[id].color = new Color(0.5f, 0.5f, 0.5f, 1);


            _clearChallenges.Add(id); // �Ϸ�� �������� ���� �� â ���� �� �� �Ʒ��� �̵�
        }
    }

    private void CloseChallenges()
    {
        // ���������� ����Ʈ�� �� �Ʒ��� �̵�
        for(int i = 0; i < _clearChallenges.Count; i++)
        {
            RectTransform slotTransform = _challengesSlotImageDic[_clearChallenges[i]].GetComponent<RectTransform>();
            slotTransform.SetAsLastSibling();
        }

        _clearChallenges.Clear();

        // ��ũ�� �� �� ���� ���̵��� ����
        _scrollRect.verticalNormalizedPosition = 1f;
    }


    private void EarningBamboo(string id)
    {
        _bambooFieldSystem.HarvestBamboo(0, DatabaseManager.Instance.GetChallengesDic()[id].BambooCount, _challengeClearImageDic[id].transform);
    }
}
