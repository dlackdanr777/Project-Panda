using System.Collections.Generic;
using UnityEngine;


public class StoryManager : SingletonHandler<StoryManager>
{
    public List<int> _storyCompleteList { get; private set; }

    private Dictionary<int, PandaStoryController> _pandaStoryControllerDic = new Dictionary<int, PandaStoryController>();


    public override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Init()
    {
        _storyCompleteList = new List<int>();
        UIDialogue.OnComplateHandler += AddComplateStory;
        PandaStoryController.OnStartHandler += SetStroyDic;
        PandaStoryController.OnCheckActivateHandler += CheckStoryActivate;
    }


    private void AddComplateStory(int id)
    {
        if (_storyCompleteList.Contains(id))
        {
            Debug.Log("�̹� �ִ� �ε��� �Դϴ�.");
            return;
        }

        _storyCompleteList.Add(id);
        CheckStoryActivates();

        //�޽��� ����
        //GameManager.Instance.Player.Messages[0].AddById(id, MessageField.Mail); //���丮 ID�� ���� �޽��� ����
        //�ϱ��� �߰�
    }


    private void SetStroyDic(int id, PandaStoryController pandaStoryController)
    {

        if (_pandaStoryControllerDic.ContainsKey(id))
        {
            Debug.LogError("�̹� ��ųʸ��� �����մϴ�.");
            return;
        }
        _pandaStoryControllerDic.Add(id, pandaStoryController);
    }


    private void CheckStoryActivates()
    {
        foreach(PandaStoryController panda in  _pandaStoryControllerDic.Values)
        {
            if (panda == null)
            {
                Debug.Log(panda.name);
                continue;
            }

            CheckStoryActivate(panda);
        }
    }


    private void CheckStoryActivate(PandaStoryController panda)
    {
        bool checkClear = !_storyCompleteList.Contains(panda.StoryDialogue.StoryID);
        bool checkPriorStoryID = _storyCompleteList.Contains(panda.StoryDialogue.PriorStoryID) || panda.StoryDialogue.PriorStoryID == 9999;

        if (checkClear && checkPriorStoryID)
        {
            panda.gameObject.SetActive(true);
            return;
        }

        panda.gameObject.SetActive(false);
    }

}
