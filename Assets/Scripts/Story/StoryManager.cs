using System.Collections.Generic;
using UnityEngine;


public class StoryManager : SingletonHandler<StoryManager>
{
    private List<string> _storyCompletedList = new List<string>();
    public List<string> StoryCompletedList => _storyCompletedList;

    private Dictionary<string, PandaStoryController> _pandaStoryControllerDic = new Dictionary<string, PandaStoryController>();


    public override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Init()
    {
        UIDialogue.OnComplateHandler += AddComplatedStory;
        PandaStoryController.OnStartHandler += SetStroyDic;
        PandaStoryController.OnCheckActivateHandler += CheckStoryActivated;
    }


    public void SetStoryCompletedList(List<string> storyCompletedList)
    {
        _storyCompletedList = storyCompletedList;
    }


    public List<string> GetStoryCompletedList()
    {
        return _storyCompletedList;
    }


    /// <summary> ���丮 �Ϸ� ��Ͽ� �ش� stroyId�� �����ϸ� true, �ƴϸ� false�� ��ȯ�ϴ� �Լ� </summary>
    public bool CheckCompletedStoryById(string storyId)
    {
        if(_storyCompletedList.Contains(storyId))
            return true;

        return false;
    }


    private void AddComplatedStory(string id)
    {
        if (_storyCompletedList.Contains(id))
        {
            Debug.Log("�̹� �ִ� �ε��� �Դϴ�.");
            return;
        }

        _storyCompletedList.Add(id);
        CheckStoryActivateds();

        //�޽��� ����
        Debug.Log("���丮 �� �޽��� ����, �ϱ��� �߰� : " + id);
        GameManager.Instance.Player.Messages[0].AddByStoryId(id, MessageField.Mail); //���丮 ID�� ���� �޽��� ����
        //�ϱ��� �߰�
        DatabaseManager.Instance.SetReceiveAlbumById(id);

        DatabaseManager.Instance.Challenges.MainStoryDone(id);
    }


    private void SetStroyDic(string id, PandaStoryController pandaStoryController)
    {
        if (_pandaStoryControllerDic.ContainsKey(id))
        {
            Debug.Log("�̹� ��ųʸ��� �����մϴ�.");
            return;
        }
        _pandaStoryControllerDic.Add(id, pandaStoryController);
    }


    private void CheckStoryActivateds()
    {
        foreach(PandaStoryController panda in  _pandaStoryControllerDic.Values)
        {
            if (panda == null)
            {
                continue;
            }

            CheckStoryActivated(panda);
        }
    }


    private void CheckStoryActivated(PandaStoryController panda)
    {
        bool checkClear = !_storyCompletedList.Contains(panda.StoryDialogue.StoryID);
        bool checkPriorStoryID = _storyCompletedList.Contains(panda.StoryDialogue.PriorStoryID) || panda.StoryDialogue.PriorStoryID == "MS99Z" || string.IsNullOrWhiteSpace(panda.StoryDialogue.PriorStoryID);
        if (checkClear && checkPriorStoryID)
        {
            panda.gameObject.SetActive(true);
            return;
        }

        panda.gameObject.SetActive(false);
    }

}
