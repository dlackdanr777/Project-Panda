using Muks.DataBind;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

public class SideStoryController : MonoBehaviour
{
    public static event Action<SideStoryDialogue> OnStartInteractionHandler;

    [SerializeField] private string NPCID;
    [SerializeField] private FollowButton _followButtonPrefab;
    [SerializeField] private Vector3 _buttonPos;
    public FollowButton FollowButton => _followButton;
    private FollowButton _followButton;

    private Dictionary<string, SideStoryDialogue> _storyDatabase;
    private List<string> _storyKey = new List<string>();
    private string _storyCode;
    private int _storyIndex;
    private bool _isInitialized = false;


    private void Awake()
    {
        UISideDialogue.OnAddRewardHandler += AddReward;    
    }
    private void OnDestroy()
    {
        UISideDialogue.OnAddRewardHandler -= AddReward;    
    }

    private void Start()
    {
        Transform parent = GameObject.Find("Follow Button Parent").transform;
        _followButton = Instantiate(_followButtonPrefab, transform.position + Vector3.up, Quaternion.identity, parent);
        _followButton.Init(gameObject, _buttonPos, new Vector2(120, 120), DatabaseManager.Instance.GetNPCIntimacyImageById(NPCID), () => OnClickStartButton());
        _followButton.gameObject.SetActive(gameObject.activeSelf);
    }

    public void OnClickStartButton()
    {   
        Init();
        StartSideStory();
    }

    private void Init()
    {
        _storyCode = "SS" + NPCID.Substring(3);
        _storyDatabase = DatabaseManager.Instance.SideDialogueDatabase.SSDic[_storyCode];
        _storyKey.Clear();
        _storyIndex = -1;

        foreach (var key in _storyDatabase.Keys)
        {
            _storyKey.Add(key);
        }
        for (int i = 0; i < _storyKey.Count; i++)
        {
            if (_storyKey[i].Equals(DatabaseManager.Instance.GetNPC(NPCID).SSId)) //����� ���̵彺�丮 ���̵�� ��
            {
                _storyIndex = i;
                break;
            }
        }

        if (!DatabaseManager.Instance.GetNPC(NPCID).IsReceived)
        {
            DatabaseManager.Instance.GetNPC(NPCID).IsReceived = true; //NPC ����
        }

        _isInitialized = true;
    }
    private void StartSideStory()
    {
        for (int i = _storyIndex+1; i < _storyKey.Count; i++)
        {
            SideStoryDialogue currentStory = _storyDatabase[_storyKey[i]];
            if (!currentStory.IsSuccess) //���丮�� �Ϸ����� �ʾҴٸ�
            {
                //���� ���丮 ���� ���丮 ��
                bool priorCheck = CheckPrior(currentStory.PriorStoryID); //ó�� �����̸� ������ true�� �Ѿ �� �ֵ���
                int nextCheck = CheckNext(currentStory.NextStoryID); //�������̸� ģ�е��� max�� �ؼ� �������θ� true�� �� �� ������

                if (priorCheck && //���� ���丮 �����ߴ��� ��
                    DatabaseManager.Instance.GetNPC(NPCID).Intimacy >= nextCheck) //���� ���丮�� ģ�е� ��
                {
                    _storyDatabase[_storyKey[i]].IsSuccess = true;
                    DatabaseManager.Instance.GetNPC(NPCID).SSId = _storyKey[i];

                    continue; //������ ���丮�� ���� ���丮�� �̵�
                }

                //������ ���丮�� �ƴϸ�
                if (DatabaseManager.Instance.GetNPC(NPCID).Intimacy < CheckNext(currentStory.NextStoryID)) //���� ���丮���� ģ�е� ������ ��
                {       
                    if (currentStory.EventType != EventType.None)// �̺�Ʈ ������ ������
                    {
                        //���� ��
                        if (!CheckCondition(currentStory.EventType, currentStory.EventTypeCondition, currentStory.EventTypeAmount))
                        {
                            i--;
                            currentStory = _storyDatabase[_storyKey[i]];
                        }
                    }

                    ShowContext(currentStory);

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

    private bool CheckCondition(EventType type, string condition, int count)
    {
        //���� ��
        switch (type)
        {
            case EventType.GISP:
                return GameManager.Instance.Player.GetGISP(condition, count);
            case EventType.INM:
                return GameManager.Instance.Player.GetINM(condition);
            case EventType.MONEY:
                return GameManager.Instance.Player.GetMONEY(count);
            case EventType.REIT:
                return DatabaseManager.Instance.GetNPC(condition).Intimacy >= count;
            case EventType.IVGI:
                return GameManager.Instance.Player.GetIVGI(condition, count);
            case EventType.IVCK:
                return GameManager.Instance.Player.GetIVCK(condition, count);
            case EventType.IVFU:
                return GameManager.Instance.Player.GetIVFU(condition);
            default:
                return false;
        }
    }

    private void ShowContext(SideStoryDialogue data)
    {
        List<SideDialogueData> dialogueData = data.DialogueData;
        OnStartInteractionHandler?.Invoke(data); //��ȭ ���
    }

    private void AddReward(string id)
    {
        if (!_isInitialized)
        {
            return;
        }

        NPC currentNPC = DatabaseManager.Instance.GetNPC(NPCID);
        SideStoryDialogue currentStory = _storyDatabase[id];

        int nextStoryInti = CheckNext(currentStory.NextStoryID);
        DatabaseManager.Instance.AddIntimacy(NPCID, currentStory.RewardIntimacy); //���� ģ�е�

        if (currentNPC.Intimacy > nextStoryInti)
        {
            DatabaseManager.Instance.GetNPC(NPCID).Intimacy = nextStoryInti;
        }
        _followButton._image.sprite = DatabaseManager.Instance.GetNPCIntimacyImageById(NPCID);

        //����
        RewardItem(currentStory.RewardType, currentStory.RewardID, currentStory.RewardCount);
        _isInitialized = false;
    }

    private bool RewardItem(EventType type, string condition, int count)
    {
        //���� ��
        switch (type)
        {
            case EventType.MONEY:
                return GameManager.Instance.Player.GainBamboo(count);
            case EventType.IVGI:
                GameManager.Instance.Player.AddIVGI(condition, count);
                return true;
            case EventType.IVCK:
            case EventType.IVFU:
                DatabaseManager.Instance.StartPandaInfo.AddFurniture(condition);
                return true;
            default:
                return false;
        }
    }
}
