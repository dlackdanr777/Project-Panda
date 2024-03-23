using BT;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainStoryController : MonoBehaviour
{
    public static event Action<MainStoryDialogue> OnStartInteractionHandler;

    [SerializeField] private SpriteRenderer _npcRenderer;
    [SerializeField] private string _npcID;
    [SerializeField] private GameObject _questMark;

    private NPCButton _npcButton;
    private Dictionary<string, MainStoryDialogue> _storyDatabase;
    private List<string> _storyKey = new List<string>();
    private int _storyIndex;
    private bool _isInitialized = false;
    private bool _isStartStory = false;

    private void Awake()
    {
        UIMainDialogue.OnAddRewardHandler += AddReward;
        UIMainDialogue.OnFinishStoryHandler += FinishStory;
    }
    private void OnDestroy()
    {
        UIMainDialogue.OnAddRewardHandler -= AddReward;
        UIMainDialogue.OnFinishStoryHandler -= FinishStory;
    }

    private void Start()
    {
        _npcID = gameObject.name;

        Transform parent = GameObject.Find("NPC Button Parent").transform;
        NPCButton npcButton = Resources.Load<NPCButton>("Button/NPC Button");
        Vector2 rendererSize = _npcRenderer.sprite.rect.size * transform.localScale;
        _npcButton = Instantiate(npcButton, transform.position, Quaternion.identity, parent);
        _npcButton.Init(transform, rendererSize, DatabaseManager.Instance.GetNPCIntimacyImageById(_npcID), () => OnClickStartButton());
        _npcButton.gameObject.SetActive(gameObject.activeSelf);

        Init();
    }

    private void Update()
    {
        if(_storyDatabase[_storyKey[_storyIndex + 1]].DialogueData[0].TalkPandaID.Equals(_npcID) && _isStartStory == false)
        {
            if (_storyDatabase[_storyKey[_storyIndex + 1]].EventType == MainEventType.None || CheckCondition(_storyDatabase[_storyKey[_storyIndex + 1]].EventType, 
                _storyDatabase[_storyKey[_storyIndex + 1]].EventTypeCondition, _storyDatabase[_storyKey[_storyIndex + 1]].EventTypeAmount))
            {
                _questMark.SetActive(true);
            }
        }
    }

    // NPC ��ư Ŭ������ ��
    public void OnClickStartButton()
    {
        _questMark.SetActive(false);
        // ���� ���ν��丮�� ���� NPC�� Ŭ���ߴٸ� ���丮 ����
        if (_storyDatabase[_storyKey[_storyIndex + 1]].DialogueData[0].TalkPandaID.Equals(_npcID))
        {
            if (!DatabaseManager.Instance.GetNPC(_npcID).IsReceived)
            {
                DatabaseManager.Instance.GetNPC(_npcID).IsReceived = true; //NPC ����
            }
            StartMainStory();
            _isInitialized = true;
        }
    }

    // �ʱ� ����
    private void Init()
    {
        _storyDatabase = DatabaseManager.Instance.MainDialogueDatabase.MSDic;
        _storyKey.Clear();
        _storyIndex = -1;

        foreach(var key in _storyDatabase.Keys)
        {
            _storyKey.Add(key);
        }

        // ����� �� �ҷ�����
        for(int i = 0; i < _storyDatabase.Count; i++)
        {
            // �Ϸ�� id�� ���
            if(!string.IsNullOrEmpty(DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Find(x => x == _storyKey[i]))){
                _storyDatabase[_storyKey[i]].IsSuccess = true;
                _storyIndex = i;
            }
            else
            {
                break;
            }
        }


        _isInitialized = true;
    }

    private void StartMainStory()
    {
        for(int i = _storyIndex + 1; i < _storyKey.Count; i++)
        {
            MainStoryDialogue currentStory = _storyDatabase[_storyKey[i]];

            if (!currentStory.IsSuccess) // ���丮�� �Ϸ����� �ʾҴٸ�
            {
                //���� ���丮 ���� ���丮 ��
                bool priorCheck = CheckPrior(currentStory.PriorStoryID); //ó�� �����̸� ������ true�� �Ѿ �� �ֵ���
                int nextCheck = CheckNext(currentStory.NextStoryID); //�������̸� ģ�е��� max�� �ؼ� �������θ� true�� �� �� ������

                if (priorCheck && //���� ���丮 �����ߴ��� ��
                    StarterPanda.Instance.Intimacy >= nextCheck) //���� ���丮�� ģ�е� ��
                {
                    if (currentStory.EventType != MainEventType.None)// �̺�Ʈ ������ ������
                    {
                        //���� ��
                        if (!CheckCondition(currentStory.EventType, currentStory.EventTypeCondition, currentStory.EventTypeAmount))
                        {
                            i--;
                            currentStory = _storyDatabase[_storyKey[i]];
                        }
                    }
                    if (!currentStory.IsSuccess) // ���丮�� �Ϸ����� �ʾҴٸ�
                    {
                        _isStartStory = true;
                        ShowContext(currentStory);
                    }

                    break;
                }
                //���� ���丮�� ģ�е��� ������ ���� ���丮�� �Ѿ
            }
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

    private bool CheckCondition(MainEventType type, string condition, int count)
    {
        //���� ��
        switch (type)
        {
            case MainEventType.HOLDITEM:
                return GameManager.Instance.Player.FindItemById(condition, count);
            case MainEventType.GIVEITEM:
                return GameManager.Instance.Player.RemoveItemById(condition, count);
            case MainEventType.LOVEMOUNT:
                return DatabaseManager.Instance.GetNPC(condition).Intimacy >= count;
            default:
                return false;
        }
    }

    private void ShowContext(MainStoryDialogue data)
    {
        List<MainDialogueData> dialogueData = data.DialogueData;
        OnStartInteractionHandler?.Invoke(data); //��ȭ ���
    }

    private void AddReward(string id)
    {
        if (!_isInitialized)
        {
            return;
        }

        NPC currentNPC = DatabaseManager.Instance.GetNPC(_npcID);
        MainStoryDialogue currentStory = _storyDatabase[id];

        int nextStoryInti = CheckNext(currentStory.NextStoryID);

        StarterPanda.Instance.Intimacy += currentStory.RewardIntimacy; // ���� ģ�е�

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
        for(int i = _storyIndex + 1; i < _storyKey.Count; i++) 
        {
            if(_storyKey[i] == id)
            {
                _storyDatabase[_storyKey[i]].IsSuccess = true;
                if (string.IsNullOrEmpty(DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Find(x => x == _storyKey[i])))
                {
                    DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Add(_storyKey[i]);
                }
                _storyIndex = i;

                // ä���� �ִٸ� ä�� Ȱ��ȭ
                string name = id + "Collection";
                GameObject gameObject = GameObject.Find(name);
                if(gameObject != null)
                {
                    MainStoryCollection msCollection = gameObject.transform.GetComponent<MainStoryCollection>();
                    msCollection.CollectionID = _storyDatabase[_storyKey[i+1]].EventTypeCondition;
                    msCollection.NextMainStoryID = _storyDatabase[_storyKey[i]].NextStoryID;

                }
                break;
            }
        }
        _isStartStory = false;
    }

}
