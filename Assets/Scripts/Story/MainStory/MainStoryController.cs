using BT;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStoryController : MonoBehaviour
{
    public static event Action<MainStoryDialogue> OnStartInteractionHandler;

    public static event Action OnFinishStoryHandler;
    public static event Action OnCheckConditionHandler;

    [SerializeField] private SpriteRenderer _npcRenderer;
    [SerializeField] private string _npcID;
    [SerializeField] private GameObject _questMark;

    private NPCButton _npcButton;
    private Dictionary<string, MainStoryDialogue> _storyDatabase;
    private List<string> _storyKey = new List<string>();
    private string _storyIndex;
    private bool _isInitialized = false;

    public List<string> NextStory = new List<string>();

    private GameObject _poyaAnimControll;
    private GameObject _jijiAnimControll;

    private Dictionary<string, Transform> _poyaTransform = new Dictionary<string, Transform>();
    private Dictionary<string, Transform> _jijiTransform = new Dictionary<string, Transform>();

    private string _currentMap;

    private void Awake()
    {
        UIMainDialogue.OnAddRewardHandler += AddReward;
        UIMainDialogue.OnFinishStoryHandler += FinishStory;
        UIMainDialogue.OnCheckConditionHandler += CheckCondition;
        OnFinishStoryHandler += CheckNectStory;
        SceneManager.sceneLoaded += ChangeScene;
        TimeManager.OnChangedTimeHandler += CheckMap;
    }
    private void OnDestroy()
    {
        UIMainDialogue.OnAddRewardHandler -= AddReward;
        UIMainDialogue.OnFinishStoryHandler -= FinishStory;
        OnFinishStoryHandler -= CheckNectStory;
        SceneManager.sceneLoaded -= ChangeScene;
        TimeManager.OnChangedTimeHandler -= CheckMap;
        UIMainDialogue.OnCheckConditionHandler += CheckCondition;
    }

    private void Start()
    {
        _npcID = gameObject.name;
        SetNPCButton();

        _poyaAnimControll = GameObject.Find("Poya Anime ControllCenter");
        _jijiAnimControll = GameObject.Find("JiJi Anime ControllCenter");

        Init();

        CheckNectStory();
    }


    //private void Update()
    //{
    //    foreach(string key in NextStory)
    //    {
    //        if (_storyDatabase[key].StoryStartPanda.Equals(_npcID) && !_questMark.activeSelf)
    //        {
    //            if (_storyDatabase[key].EventType == MainEventType.None || CheckCondition(_storyDatabase[key].EventType,
    //                _storyDatabase[key].EventTypeCondition, _storyDatabase[key].EventTypeAmount))
    //            {
    //                _questMark.SetActive(true);
    //                Debug.Log("_npcID _questMarktrue" + _npcID);

    //                if(_isStartStory == false)
    //                {
    //                    // 지지와 포야가 다음 이야기에 포함되어 있다면 애니메이션 끄기
    //                    PoyaSetTrue();
    //                    JijiSetTrue();
    //                    SetPosition();
    //                }
    //            }
    //        }

    //        if (_storyDatabase[key].StoryStartPanda.Equals(_npcID) && _currentMap != TimeManager.Instance.CurrentMap)
    //        {
    //            _currentMap = TimeManager.Instance.CurrentMap;
    //            SetPosition();
    //        }
    //    }


    //}


    // NPC 버튼 클릭했을 때
    public void OnClickStartButton()
    {
        // 현재 메인스토리의 시작 NPC를 클릭했다면 스토리 진행
        foreach(string key in NextStory)
        {
            if (_storyDatabase[key].StoryStartPanda.Equals(_npcID))
            {
                if (!DatabaseManager.Instance.GetNPC(_npcID).IsReceived)
                {
                    DatabaseManager.Instance.GetNPC(_npcID).IsReceived = true; //NPC 만남
                }
                _storyIndex = key;
                StartMainStory();
                _isInitialized = true;
                break;
            }
        }

    }

    // 초기 설정
    private void Init()
    {
        _storyDatabase = DatabaseManager.Instance.MainDialogueDatabase.MSDic;
        _storyKey.Clear();

        foreach(var key in _storyDatabase.Keys)
        {
            _storyKey.Add(key);
        }

        _storyIndex = _storyKey[0];
        // 저장된 값 불러오기
        for(int i = 0; i < _storyDatabase.Count; i++)
        {
            // 완료된 id일 경우
            if(!string.IsNullOrEmpty(DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Find(x => x == _storyKey[i]))){
                _storyDatabase[_storyKey[i]].IsSuccess = true;
            }
        }

        for (int j = 1; j < _storyKey.Count; j++)
        {
            if (_storyDatabase[_storyKey[j]].IsSuccess == false && _storyDatabase[_storyDatabase[_storyKey[j]].PriorStoryID].IsSuccess)
            {
                if (!NextStory.Contains(_storyKey[j]))
                {
                    NextStory.Add(_storyKey[j]);
                }
            }
        }

        if(NextStory.Count ==0 && !DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyKey[0])) 
        {
            NextStory.Add(_storyKey[0]);
        }

        _isInitialized = true;
        _currentMap = TimeManager.Instance.CurrentMap;
        //PoyaSetTrue();
        //JijiSetTrue();
    }

    private void SetNPCButton()
    {
        Transform parent = GameObject.Find("NPC Button Parent").transform;
        NPCButton npcButton = Resources.Load<NPCButton>("Button/NPC Button");
        Vector2 rendererSize = _npcRenderer.sprite.rect.size * transform.localScale;
        if (rendererSize.x < 0)
        {
            rendererSize.x = -rendererSize.x;
        }
        if (rendererSize.x < 200)
        {
            rendererSize.x *= 2;
            rendererSize.y *= 2;
        }
        if (rendererSize.x < 300)
        {
            rendererSize.x *= 1.5f;
            rendererSize.y *= 1.5f;
        }
        if (rendererSize.x > 500)
        {
            rendererSize.x /= 1.5f;
        }
        _npcButton = Instantiate(npcButton, transform.position, Quaternion.identity, parent);
        _npcButton.Init(transform, rendererSize, DatabaseManager.Instance.GetNPCIntimacyImageById(_npcID), () => OnClickStartButton());
        _npcButton.gameObject.SetActive(true);
    }

    private void CheckMap()
    {
        foreach (string key in NextStory)
        {
            if (_storyDatabase[key].StoryStartPanda.Equals(_npcID) && _currentMap != TimeManager.Instance.CurrentMap)
            {
                _currentMap = TimeManager.Instance.CurrentMap;
                SetPosition();
            }
        }
    }

    private void CheckNectStory()
    {
        foreach (string key in NextStory)
        {
            if (_storyDatabase[key].StoryStartPanda.Equals(_npcID) && !_questMark.activeSelf && _storyDatabase[key].RequiredIntimacy <= StarterPanda.Instance.Intimacy)
            {
                //if (_storyDatabase[key].EventType == MainEventType.None || CheckCondition(_storyDatabase[key].EventType,
                //    _storyDatabase[key].EventTypeCondition, _storyDatabase[key].EventTypeAmount))
                //{
                    _questMark.SetActive(true);
 
                //}
            }
        }
        // 지지와 포야가 다음 이야기에 포함되어 있다면 애니메이션 끄기
        PoyaSetTrue();
        JijiSetTrue();
        SetPosition();
    }

    private void StartMainStory()
    {
        MainStoryDialogue currentStory = _storyDatabase[_storyIndex];

        if (!currentStory.IsSuccess) // 스토리를 완료하지 않았다면
        {
            //이전 스토리 다음 스토리 비교
            bool priorCheck = CheckPrior(currentStory.PriorStoryID); //처음 시작이면 무조건 true로 넘어갈 수 있도록
            //int nextCheck = CheckNext(currentStory.NextStoryID); //마지막이면 친밀도를 max로 해서 성공여부를 true로 할 수 없도록

            if (priorCheck) //이전 스토리 성공했는지 비교 && StarterPanda.Instance.Intimacy >= nextCheck) //다음 스토리와 친밀도 비교
            {

                if (!currentStory.IsSuccess) // 스토리를 완료하지 않았다면
                {
                    ShowContext(currentStory);
                }
            }
            //다음 스토리와 친밀도가 같으면 다음 스토리로 넘어감
        }
    }

    private bool CheckPrior(string priorStory)
    {
        if (priorStory != null)
        {
            if (_storyDatabase.ContainsKey(priorStory)) //해당 데이터베이스에 있는 id인지 확인 -> 없으면 처음 => 무조건 통과
            {
                return _storyDatabase[priorStory].IsSuccess;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    private int CheckNext(string nextStory)
    {
        if (nextStory != null)
        {
            if (_storyDatabase.ContainsKey(nextStory)) //해당 데이터베이스에 있는 id인지 확인 -> 없으면 마지막 => 넘어가지 못하도록 MaxValue
            {
                return _storyDatabase[nextStory].RequiredIntimacy;
            }
        }
        else
        {
            return -1;
        }
        return int.MaxValue;
    }

    private void CheckCondition(string storyId)
    {
        if(_storyDatabase == null)
        {
            _storyDatabase = DatabaseManager.Instance.MainDialogueDatabase.MSDic;
        }
        MainStoryDialogue currentStory = _storyDatabase[storyId];
        if(currentStory.StoryStartPanda.Equals(_npcID))
        {
            bool isTrue;
            if (currentStory.EventType != MainEventType.None)// 이벤트 조건이 있으면
            {
                //조건 비교
                switch (currentStory.EventType)
                {
                    case MainEventType.HOLDITEM:
                        isTrue = GameManager.Instance.Player.FindItemById(currentStory.EventTypeCondition, currentStory.EventTypeAmount);
                        break;
                    case MainEventType.GIVEITEM:
                        isTrue = GameManager.Instance.Player.RemoveItemById(currentStory.EventTypeCondition, currentStory.EventTypeAmount);
                        break;
                    case MainEventType.LOVEMOUNT:
                        isTrue = DatabaseManager.Instance.GetNPC(currentStory.EventTypeCondition).Intimacy >= currentStory.EventTypeAmount;
                        break;
                    default:
                        isTrue = false;
                        break;
                }
            }
            else
            {
                isTrue = true;
            }
            if(isTrue == false)
            {
                OnCheckConditionHandler?.Invoke();
            }
        }
}

    private void ShowContext(MainStoryDialogue data)
    {
        List<MainDialogueData> dialogueData = data.DialogueData;
        OnStartInteractionHandler?.Invoke(data); //대화 출력
    }

    private void AddReward(string id, string npcId)
    {

        if (!_isInitialized || npcId != _npcID)
        {
            return;
        }

        NPC currentNPC = DatabaseManager.Instance.GetNPC(_npcID);
        MainStoryDialogue currentStory = _storyDatabase[id];

        int nextStoryInti = CheckNext(currentStory.NextStoryID);

        StarterPanda.Instance.Intimacy += currentStory.RewardIntimacy; // 보상 친밀도

        //보상
        RewardItem(currentStory.RewardType, currentStory.RewardID, currentStory.RewardCount);
        MainDialogueData lastMainDialogue = currentStory.DialogueData[currentStory.DialogueData.Count - 1];

        if (lastMainDialogue.EventType != MainEventType.None)
        {
            RewardItem(lastMainDialogue.EventType, lastMainDialogue.EventTypeCondition, lastMainDialogue.EventTypeAmount);
        }

        _isInitialized = false;
    }

    private bool RewardItem(MainEventType type, string condition, int count)
    {

        //조건 비교
        switch (type)
        {
            case MainEventType.MONEY:
                return GameManager.Instance.Player.GainBamboo(count);
            case MainEventType.QUESTITEM:
                return GameManager.Instance.Player.AddItemById(condition, count);
            case MainEventType.GIVEITEM:
                return GameManager.Instance.Player.RemoveItemById(condition, count);
            default:
                return false;
        }
    }

    private void FinishStory(string id)
    {
        foreach(string key in NextStory)
        {
            if(key == id)
            {
                if(_questMark != null)
                {
                    _questMark.SetActive(false);
                }
                _storyDatabase[key].IsSuccess = true;
                if (!DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(key))
                {
                    Debug.Log("스토리 완료: " + key);
                    DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Add(key);
                    DatabaseManager.Instance.Challenges.MainStoryDone(id);
                }
                NextStory.Remove(key);


                _storyIndex = key;
                DatabaseManager.Instance.MainDialogueDatabase.CurrentStoryID = key;

                for(int j = 0;  j < _storyKey.Count; j++)
                {
                    if (_storyDatabase[_storyKey[j]].IsSuccess == false && _storyDatabase[_storyDatabase[_storyKey[j]].PriorStoryID].IsSuccess)
                    {
                        if (!NextStory.Contains(_storyKey[j]))
                        {
                            NextStory.Add(_storyKey[j]);
                        }
                    }
                }

                // 채집이 있다면 채집 활성화
                foreach(string nextStoryId in NextStory)
                {
                    string name = nextStoryId + "Collection";
                    GameObject gameObject = GameObject.Find(name);
                    if (gameObject != null)
                    {
                        MainStoryCollection msCollection = gameObject.transform.GetComponent<MainStoryCollection>();
                        msCollection.CollectionID = _storyDatabase[nextStoryId].EventTypeCondition;
                    }
                }

                // 지지와 포야 애니메이션 켜기
                _poyaAnimControll = GameObject.Find("Poya Anime ControllCenter");
                _jijiAnimControll = GameObject.Find("JiJi Anime ControllCenter");
                PoyaSetFalse();
                JijiSetFalse();
                break;
            }
        }
        OnFinishStoryHandler?.Invoke();
    }

    // 애니메이션 끄고 포야 켜기
    private void PoyaSetTrue()
    {
        _poyaTransform.Clear();
        // 다음 스토리에 포야가 포함되어 있다면
        foreach (string key in NextStory)
        {
            if (DatabaseManager.Instance.MainDialogueDatabase.PoyaStoryList.Contains(key))
            {
                foreach (Transform child in _poyaAnimControll.transform)
                {
                    if(child.name == _storyDatabase[key].MapID)
                    {
                        child.gameObject.SetActive(false);
                        if (!_poyaTransform.Keys.Contains(_storyDatabase[key].MapID))
                        {
                            _poyaTransform.Add(_storyDatabase[key].MapID, child.GetChild(0));
                            break;
                        }
                    }
                }
                StarterPanda.Instance.GetComponent<SpriteRenderer>().enabled = true;

            }
        }
    }

    private void PoyaSetFalse()
    {
        foreach (Transform child in _poyaAnimControll.transform)
        {
            child.gameObject.SetActive(true);
        }
        StarterPanda.Instance.GetComponent<SpriteRenderer>().enabled = false;
    }

    // 애니메이션 끄고 지지 켜기
    private void JijiSetTrue()
    {
        _jijiTransform.Clear();
        foreach (string key in NextStory)
        {
            if (DatabaseManager.Instance.MainDialogueDatabase.JijiStoryList.Contains(key))
            {
                foreach (Transform child in _jijiAnimControll.transform)
                {
                    if (child.name == _storyDatabase[key].MapID)
                    {
                        child.gameObject.SetActive(false);
                        if (!_jijiTransform.Keys.Contains(_storyDatabase[key].MapID))
                        {
                            _jijiTransform.Add(_storyDatabase[key].MapID, child.GetChild(0));
                            break;
                        }
                    }
                }
            }
            GameObject.Find("NPC01").transform.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void JijiSetFalse()
    {
        foreach (Transform child in _jijiAnimControll.transform)
        {
            child.gameObject.SetActive(true);
        }
        GameObject.Find("NPC01").transform.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void SetPosition()
    {
        GameObject poya = StarterPanda.Instance.gameObject;
        GameObject jiji = GameObject.Find("NPC01");
        foreach (string key in _poyaTransform.Keys)
        {
            if (key == _currentMap)
            {
                poya.transform.position = _poyaTransform[key].position;

                break;
            }
            poya.transform.position = _poyaTransform[key].position;
        }

        foreach (string key in _jijiTransform.Keys)
        {
            if (key == _currentMap)
            {
                jiji.transform.position = _jijiTransform[key].position;
                break;
            }
            jiji.transform.position = _jijiTransform[key].position;
        }
    }

    private void ChangeScene(Scene scene, LoadSceneMode loadscene)
    {
        _poyaAnimControll = GameObject.Find("Poya Anime ControllCenter");
        _jijiAnimControll = GameObject.Find("JiJi Anime ControllCenter");
    }
}
