using Muks.DataBind;
using Muks.Tween;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIChallenges : UIView
{

    [Header("ShowUI Animation Setting")]
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private float _startAlpha = 0;
    [SerializeField] private float _targetAlpha = 1;
    [SerializeField] private float _duration;
    [SerializeField] private TweenMode _tweenMode;


    [Space]
    [Header("Components")]
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _backGroundImage;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private UIChallengeSlot _slotPrefab;
    [SerializeField] private Button _backgroundButton;


    private CanvasGroup _canvasGroup;
    private Vector3 _tmpPos;
    private Vector3 _movePos => new Vector3(0, 50, 0);

    private Dictionary<string, UIChallengeSlot> _slotDic = new Dictionary<string, UIChallengeSlot>();

    private List<string> _clearChallenges = new List<string>();


    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);

        _canvasGroup = GetComponent<CanvasGroup>(); 
        _tmpPos = _targetRect.anchoredPosition;

        _backgroundButton.onClick.AddListener(OnBackgroundButtonClicked);
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
                slot.Done(false);
                if (challengesDic[key].IsClear == true)
                {
                    slot.Clear(false);
                    _clearChallenges.Add(key);
                }
            }
        }
        CloseChallenges();
        AlarmCheck();
        DatabaseManager.Instance.Challenges.ChallengeDone += ChallengeDone;
        LoadingSceneManager.OnLoadSceneHandler += ChangeSceneEvent;
        GameManager.Instance.Player.OnAddItemHandler += AlarmCheck;

        gameObject.SetActive(false);
    }


    public override void Show()
    {
        VisibleState = VisibleState.Appearing;
        gameObject.SetActive(true);

        CloseChallenges();
        _targetRect.anchoredPosition = _tmpPos + _movePos;
        _canvasGroup.alpha = _startAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos, _duration, _tweenMode);
        Tween.CanvasGroupAlpha(gameObject, _targetAlpha, _duration, _tweenMode, () =>
        {
            VisibleState = VisibleState.Appeared;
            _canvasGroup.blocksRaycasts = true;
        });
    }


    public override void Hide()
    {
        VisibleState = VisibleState.Disappearing;

        _targetRect.anchoredPosition = _tmpPos;
        _canvasGroup.alpha = _targetAlpha;
        _canvasGroup.blocksRaycasts = false;

        Tween.RectTransfromAnchoredPosition(_targetRect.gameObject, _tmpPos - _movePos, _duration, _tweenMode);
        Tween.CanvasGroupAlpha(gameObject, _startAlpha, _duration, _tweenMode, () =>
        {
            VisibleState = VisibleState.Disappeared;
            _canvasGroup.blocksRaycasts = true;

            gameObject.SetActive(false);
        });
    }


    /// <summary>
    /// 도전과제 완료 </summary>
    private void ChallengeDone(string id)
    {
        _slotDic[id].Done();
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
            GameManager.Instance.Player.AsyncSaveBambooData(3);
            _clearChallenges.Add(id); // 완료된 도전과제 저장 후 창 종료 시 맨 아래로 이동
            AlarmCheck();
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


    private void AlarmCheck()
    {
        bool alarmCheck = false;
        foreach(string key in _slotDic.Keys)
        {
            alarmCheck = _slotDic[key].AlarmCheck() || alarmCheck;
        }

        DataBind.SetBoolValue("ChallengesAlarm", alarmCheck);
    }




    private void OnBackgroundButtonClicked()
    {
        SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonExit);
        _uiNav.Pop("DropdownMenuButton");
        _uiNav.Pop("Challenges");
    }


    private void ChangeSceneEvent()
    {
        DatabaseManager.Instance.Challenges.ChallengeDone -= ChallengeDone;
        GameManager.Instance.Player.OnAddItemHandler -= AlarmCheck;
        LoadingSceneManager.OnLoadSceneHandler -= ChangeSceneEvent;
    }

}
