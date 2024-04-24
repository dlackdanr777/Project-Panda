using BT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStoryController : MonoBehaviour
{
    public static event Action<MainStoryDialogue> OnStartInteractionHandler;

    public static event Action OnFinishStoryHandler;
    public static event Action OnCheckConditionHandler;

    //[SerializeField] private SpriteRenderer _npcRenderer;
    [SerializeField] private string _npcID;
    [SerializeField] private GameObject _questMark;
    [SerializeField] private Transform _transform;

    private NPCButton _npcButton;
    private Dictionary<string, MainStoryDialogue> _storyDatabase;
    private List<string> _storyKey = new List<string>();
    private string _storyIndex;
    private bool _isInitialized = false;

    public static List<string> NextStory = new List<string>();

    private GameObject _poyaAnimControll;
    private GameObject _jijiAnimControll;
    private SpriteRenderer _jiji;

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
        FadeInOutManager.Instance.OnFadeOutHandler += NpcButtonSetSibling;
    }
    private void OnDestroy()
    {
        UIMainDialogue.OnAddRewardHandler -= AddReward;
        UIMainDialogue.OnFinishStoryHandler -= FinishStory;
        OnFinishStoryHandler -= CheckNectStory;
        SceneManager.sceneLoaded -= ChangeScene;
        TimeManager.OnChangedTimeHandler -= CheckMap;
        UIMainDialogue.OnCheckConditionHandler -= CheckCondition;
        FadeInOutManager.Instance.OnFadeOutHandler -= NpcButtonSetSibling;
    }

    private void OnEnable()
    {
        if(_npcButton != null)
        {
            _npcButton.gameObject.SetActive(true);
            NpcButtonSetSibling();
        }
    }

    private void OnDisable()
    {
        if(_npcButton != null) { _npcButton.gameObject.SetActive(false); }

    }

    private void Start()
    {
        _poyaAnimControll = GameObject.Find("Poya Anime ControllCenter");
        _jijiAnimControll = GameObject.Find("JiJi Anime ControllCenter"); 
        if (_jiji == null)
            _jiji = GameObject.Find("NPC01").transform.GetComponent<SpriteRenderer>();

        Init();
        //Invoke("SetNPCButton", 1f);
        CheckNectStory();
        SetNPCButton();
        NpcButtonSetSibling();
    }


    // NPC ��ư Ŭ������ ��
    public void OnClickStartButton()
    {

        // ���� ���ν��丮�� ���� NPC�� Ŭ���ߴٸ� ���丮 ����
        foreach (string key in NextStory)
        {
            //DatabaseManager.Instance.GetNPC(_storyDatabase[key].StoryStartPanda).Intimacy = 0;
            if (_storyDatabase[key].StoryStartPanda.Equals(_npcID))
            {
                if (!DatabaseManager.Instance.GetNPC(_npcID).IsReceived)
                {
                    DatabaseManager.Instance.GetNPC(_npcID).IsReceived = true; //NPC ����
                }
                _storyIndex = key;
                StartMainStory();
                _isInitialized = true;
                break;
            }
        }
    }


    // �ʱ� ����
    private void Init()
    {
        _npcID = gameObject.name;

        _storyDatabase = DatabaseManager.Instance.MainDialogueDatabase.MSDic;
        _storyKey.Clear();

        foreach(var key in _storyDatabase.Keys)
        {
            _storyKey.Add(key);
        }

        _storyIndex = _storyKey[0];

        //DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Clear();
        // ����� �� �ҷ�����
        for(int i = 0; i < _storyDatabase.Count; i++)
        {
            //_storyDatabase[_storyKey[i]].IsSuccess = false;

            // �Ϸ�� id�� ���
            if (!string.IsNullOrEmpty(DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Find(x => x == _storyKey[i]))){
                _storyDatabase[_storyKey[i]].IsSuccess = true;
            }
        }
        if(NextStory.Count == 0) //NextStory�� ������� ��쿡�� �߰�
        {
            for (int j = 0; j < _storyKey.Count; j++)
            {
                // ���� ���丮 �̿Ϸ� && ���� ���丮�� ���ų� �̹� �Ϸ��� ���
                if (_storyDatabase[_storyKey[j]].IsSuccess == false && 
                    (!_storyDatabase.ContainsKey(_storyDatabase[_storyKey[j]].PriorStoryID) || _storyDatabase[_storyDatabase[_storyKey[j]].PriorStoryID].IsSuccess))
                {
                    if (!NextStory.Contains(_storyKey[j]))
                    {
                        NextStory.Add(_storyKey[j]);
                    }
                }
            }
            SortingNextStory();
        }
        //if(NextStory.Count == 0 && !DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyKey[0])) 
        //{
        //    NextStory.Add(_storyKey[0]);
        //}

        _isInitialized = true;
        _currentMap = TimeManager.Instance.CurrentMap;
        //PoyaSetTrue();
        //JijiSetTrue();
    }

    private void SetNPCButton()
    {
        Transform parent = GameObject.Find("NPC Button Parent").transform;
        NPCButton npcButton = Resources.Load<NPCButton>("Button/NPC Button");

        // ��ư ũ�� ����
        UnityEngine.Vector2 rendererSize = DatabaseManager.Instance.GetNPCImageById(_npcID).rect.size;
        //if (rendererSize.x > 1000 || rendererSize.x < 1000)
        //{
        //    rendererSize *= 0.5f;
        //}
        rendererSize *= transform.localScale;
        if (rendererSize.x < 0)
        {
            rendererSize.x = -rendererSize.x;
        }
        _npcButton = Instantiate(npcButton, transform.position, UnityEngine.Quaternion.identity, parent);
        _npcButton.Init(transform, rendererSize, DatabaseManager.Instance.GetNPCIntimacyImageById(_npcID), () => OnClickStartButton(), _transform);
        _npcButton.gameObject.SetActive(true);
    }


    private void CheckMap()
    {
        if (string.IsNullOrWhiteSpace(_npcID) || string.IsNullOrWhiteSpace(_currentMap))
        {
            return;
        }
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
        if (string.IsNullOrWhiteSpace(_npcID))
        {
            return;
        }
        foreach (string key in NextStory)
        {
            if (_storyDatabase[key].StoryStartPanda.Equals(_npcID) && _storyDatabase[key].RequiredIntimacy <= StarterPanda.Instance.Intimacy)
            {
                if(key.Substring(0,2) == "MS") // ���ν��丮�� ��쿡�� ����ǥ ǥ��
                {
                    _questMark.SetActive(true);
                }

            }
        }
        // ������ ���߰� ���� �̾߱⿡ ���ԵǾ� �ִٸ� �ִϸ��̼� ����
        PoyaSetTrue();
        JijiSetTrue();
        SetPosition();
    }

    private void StartMainStory()
    {
        MainStoryDialogue currentStory = _storyDatabase[_storyIndex];

        if (!currentStory.IsSuccess || currentStory.StoryID.Substring(0, 2) == "SS") // ���丮�� �Ϸ����� �ʾҴٸ� || ���꽺�丮
        {
            //���� ���丮 ���� ���丮 ��
            bool priorCheck = CheckPrior(currentStory.PriorStoryID); //ó�� �����̸� ������ true�� �Ѿ �� �ֵ���

            if (priorCheck) //���� ���丮 �����ߴ��� �� && StarterPanda.Instance.Intimacy >= nextCheck) //���� ���丮�� ģ�е� ��
            {
                ShowContext(currentStory);
            }
            //���� ���丮�� ģ�е��� ������ ���� ���丮�� �Ѿ
        }
    }

    private bool CheckPrior(string priorStory)
    {
        if (priorStory != null)
        {
            if (_storyDatabase.ContainsKey(priorStory)) //�ش� �����ͺ��̽��� �ִ� id���� Ȯ�� -> ������ ó�� => ������ ���
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
            if (_storyDatabase.ContainsKey(nextStory)) //�ش� �����ͺ��̽��� �ִ� id���� Ȯ�� -> ������ ������ => �Ѿ�� ���ϵ��� MaxValue
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
            if (currentStory.EventType != MainEventType.None)// �̺�Ʈ ������ ������
            {
                //���� ��
                switch (currentStory.EventType)
                {
                    case MainEventType.HOLDITEM:
                        isTrue = GameManager.Instance.Player.FindItemById(currentStory.EventTypeCondition, currentStory.EventTypeAmount);
                        break;
                    case MainEventType.GIVEITEM:
                        isTrue = GameManager.Instance.Player.RemoveItemById(currentStory.EventTypeCondition, currentStory.EventTypeAmount);
                        break;
                    case MainEventType.LOVEAMOUNT:
                        //isTrue = DatabaseManager.Instance.GetNPC(currentStory.EventTypeCondition).Intimacy >= currentStory.EventTypeAmount;
                        isTrue = true; // ���� ģ�е� üũ ����
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
        OnStartInteractionHandler?.Invoke(data); //��ȭ ���
    }

    private void AddReward(string id, string npcId)
    {

        if (!_isInitialized || npcId != _npcID)
        {
            return;
        }

        NPC currentNPC = DatabaseManager.Instance.GetNPC(_npcID);
        MainStoryDialogue currentStory = _storyDatabase[id];

        //int nextStoryInti = CheckNext(currentStory.NextStoryID);
        if(id.Substring(0, 2) == "MS")
        {
            StarterPanda.Instance.Intimacy += currentStory.RewardIntimacy; // ���� ģ�е�
        }
        else
        {
            currentNPC.Intimacy += currentStory.RewardIntimacy;
        }

        //����
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

        //���� ��
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
        if (_storyDatabase[id].StoryStartPanda == _npcID) // �� ���� ����
        {
            //foreach (string key in NextStory)
            //{
            //    if (key == id)
            //    {
            if (NextStory.Contains(id))
            {
                string key = id;
                // ���� ���丮
                if (_questMark != null && key.Substring(0, 2) == "MS")
                {
                    _questMark.SetActive(false);

                    _storyDatabase[key].IsSuccess = true;
                    if (!DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(key))
                    {
                        Debug.Log("���丮 �Ϸ�: " + key);
                        DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Add(key);
                        DatabaseManager.Instance.Challenges.MainStoryDone(id);
                    }
                    NextStory.Remove(key);


                    _storyIndex = key;
                    DatabaseManager.Instance.MainDialogueDatabase.CurrentStoryID = key;
                }
                // ���� ���丮
                else if (key.Substring(0, 2) == "SS")
                {
                    _storyDatabase[key].IsSuccess = true;
                }

                // NextStory �߰�
                AddNextStory(key, id);

                // ä���� �ִٸ� ä�� Ȱ��ȭ
                foreach (string nextStoryId in NextStory)
                {
                    string name = nextStoryId + "Collection";
                    GameObject gameObject = GameObject.Find(name);
                    if (gameObject != null)
                    {
                        MainStoryCollection msCollection = gameObject.transform.GetComponent<MainStoryCollection>();
                        msCollection.CollectionID = _storyDatabase[nextStoryId].EventTypeCondition;
                    }
                }

                // ������ ���� �ִϸ��̼� �ѱ�
                SetAnimControll();
                PoyaSetFalse();
                JijiSetFalse();
                //break;
            }
            //    }   
            //}
            SortingNextStory();
        }
        NpcButtonSetSibling();
        OnFinishStoryHandler?.Invoke();
    }


    private void NpcButtonSetSibling()
    {
        if(string.IsNullOrWhiteSpace(_npcID))
        {
            return;
        }
        foreach (string key in NextStory)
        {
            if (_storyDatabase[key].StoryStartPanda.Equals(_npcID))
            {
                //Debug.LogFormat("{0}����", _npcID);
                _npcButton?.transform.SetAsLastSibling();
                return;
            }
        }
    }

    private void SetAnimControll()
    {
        if(_poyaAnimControll == null)
        {
            _poyaAnimControll = GameObject.Find("Poya Anime ControllCenter");
        }
        if( _jijiAnimControll == null)
        {
            _jijiAnimControll = GameObject.Find("JiJi Anime ControllCenter");
        }
    }

    // �ִϸ��̼� ���� ���� �ѱ�
    private void PoyaSetTrue()
    {
        _poyaTransform.Clear();
        // ���� ���丮�� ���߰� ���ԵǾ� �ִٸ�
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

    // �ִϸ��̼� ���� ���� �ѱ�
    private void JijiSetTrue()
    {
        _jijiTransform.Clear();
        foreach (string key in NextStory)
        {
            // ���� ���丮�� ������ �ִ� ���
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
                if(_jiji == null)
                {
                    _jiji = GameObject.Find("NPC01").transform.GetComponent<SpriteRenderer>();
                }
                UnityEngine.Vector3 jijiScale = _jiji.gameObject.transform.localScale;
                // ���� �ٶ󺸴� ���� ����
                if (key == "MS01A" && jijiScale.x < 0)
                {
                    Debug.Log("MS01A");
                    _jiji.gameObject.transform.localScale = new UnityEngine.Vector3(-jijiScale.x, jijiScale.y, jijiScale.z);
                }
                else if(jijiScale.x > 0 && key != "MS01A")
                {
                    Debug.Log("jijiScale.x > 0");

                    _jiji.gameObject.transform.localScale = new UnityEngine.Vector3(-jijiScale.x, jijiScale.y, jijiScale.z);
                }
                _jiji.enabled = true;
            }

        }
    }
    private void JijiSetFalse()
    {
        foreach (Transform child in _jijiAnimControll.transform)
        {
            child.gameObject.SetActive(true);
        }
        //GameObject.Find("NPC01").transform.GetComponent<SpriteRenderer>().enabled = false;
        _jiji.enabled = false;
    }

    private void SetPosition()
    {
        GameObject poya = StarterPanda.Instance.gameObject;
        GameObject jiji = _jiji.gameObject;
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
        SetAnimControll();
    }

    private void AddNextStory(string key, string id)
    {
        //Debug.Log(string.Join(", ", DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList));
        for (int j = 0; j < _storyKey.Count; j++)
        {
            // ���� ���丮 �Ϸ� üũ
            if (_storyDatabase[_storyKey[j]].IsSuccess == false && 
                (!_storyDatabase.ContainsKey(_storyDatabase[_storyKey[j]].PriorStoryID) || _storyDatabase[_storyDatabase[_storyKey[j]].PriorStoryID].IsSuccess))
            {
                // ���ν��丮�� �� ���� ���� && ģ�е� ���� üũ
                if (_storyKey[j].Substring(0, 2) == "MS" 
                    && _storyDatabase[_storyKey[j]].RequiredIntimacy <= StarterPanda.Instance.Intimacy)
                {
                    if (!NextStory.Contains(_storyKey[j]))
                    {
                        NextStory.Add(_storyKey[j]);
                    }
                }
                // ���꽺�丮 && �Ϸ� üũ && ģ�е� ���� üũ
                else if (_storyKey[j].Substring(0, 2) == "SS" && !DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyKey[j]) 
                    && _storyDatabase[_storyKey[j]].RequiredIntimacy <= DatabaseManager.Instance.GetNPC("NPC" + _storyKey[j].Substring(2, 2)).Intimacy)
                {
                    // �� ���꽺�丮 ����
                    foreach (string k in NextStory)
                    {
                        if (k.StartsWith(_storyKey[j].Substring(0, 4)) && !k.Equals(_storyKey[j]))
                        {
                            _storyDatabase[k].IsSuccess = true;
                            if (!DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(k))
                            {
                                Debug.Log("���丮 �Ϸ�: " + k);
                                DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Add(k);
                                DatabaseManager.Instance.Challenges.MainStoryDone(id);
                            }
                            NextStory.Remove(k);
                            break;
                        }
                    }
                    if (!NextStory.Contains(_storyKey[j]))
                    {
                        NextStory.Add(_storyKey[j]);
                    }
                }
            }
        }
    }

    public static void SortingNextStory()
    {
        MainStoryDialogue currentStory;
        bool isTrue;
        List<string> conditionCompleteMainStory = new List<string>();
        List<string> conditionCompleteSideStory = new List<string>();
        List<string> conditionIncompleteMainStory = new List<string>();
        List<string> conditionIncompleteSideStory = new List<string>();

        foreach (string key in NextStory)
        {
            currentStory = DatabaseManager.Instance.MainDialogueDatabase.MSDic[key];
            if (currentStory.EventType != MainEventType.None)// �̺�Ʈ ������ ������
            {
                //���� ��
                switch (currentStory.EventType)
                {
                    case MainEventType.HOLDITEM:
                        isTrue = GameManager.Instance.Player.FindItemById(currentStory.EventTypeCondition, currentStory.EventTypeAmount);
                        break;
                    case MainEventType.GIVEITEM:
                        isTrue = GameManager.Instance.Player.RemoveItemById(currentStory.EventTypeCondition, currentStory.EventTypeAmount);
                        break;
                    case MainEventType.LOVEAMOUNT:
                        //isTrue = DatabaseManager.Instance.GetNPC(currentStory.EventTypeCondition).Intimacy >= currentStory.EventTypeAmount;
                        isTrue = true; // ���� ģ�е� üũ ����
                        break;
                    default:
                        isTrue = false;
                        break;
                }
                // ���� ������ ���
                if (isTrue && key.Substring(0,2) == "MS")
                {
                    conditionCompleteMainStory.Add(key);
                }
                else if(isTrue)
                {
                    conditionCompleteSideStory.Add(key);
                }
                // ���� �� ������ ���
                else if(key.Substring(0, 2) == "MS")
                {
                    conditionIncompleteMainStory.Add(key);
                }
                else
                {
                    conditionIncompleteSideStory.Add(key);
                }
            }
            // ������ ���� ���
            else if(key.Substring(0, 2) == "MS")
            {
                conditionCompleteMainStory.Add(key);
            }
            else
            {
                conditionCompleteSideStory.Add(key);
            }
        }

        // ���� ���丮�� ���̵� ���丮���� ���� ����
        NextStory = conditionCompleteMainStory.OrderBy(x => x).ToList();
        conditionCompleteSideStory = conditionCompleteSideStory.OrderBy(x => x).ToList();
        conditionIncompleteMainStory = conditionIncompleteMainStory.OrderBy(x => x).ToList();
        conditionIncompleteSideStory = conditionIncompleteSideStory.OrderBy(x => x).ToList();
        NextStory.AddRange(conditionIncompleteMainStory);
        NextStory.AddRange(conditionCompleteSideStory);
        NextStory.AddRange(conditionIncompleteSideStory);

        //Debug.Log(string.Join(", ", NextStory));
    }
}
