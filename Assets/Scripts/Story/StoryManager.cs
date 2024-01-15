using System.Collections.Generic;
using UnityEngine;


public class StoryManager : SingletonHandler<StoryManager>
{
    public List<string> _storyCompleteList { get; private set; }

    private Dictionary<string, PandaStoryController> _pandaStoryControllerDic = new Dictionary<string, PandaStoryController>();


    public override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Init()
    {
        _storyCompleteList = new List<string>();
        Debug.Log("���丮 �Ŵ��� ����");
        UIDialogue.OnComplateHandler += AddComplateStory;
        PandaStoryController.OnStartHandler += SetStroyDic;
        PandaStoryController.OnCheckActivateHandler += CheckStoryActivate;
    }


    private void AddComplateStory(string id)
    {
        if (_storyCompleteList.Contains(id))
        {
            Debug.Log("�̹� �ִ� �ε��� �Դϴ�.");
            return;
        }

        _storyCompleteList.Add(id);
        CheckStoryActivates();

        //�޽��� ����
        Debug.Log("���丮 �� �޽��� ���� : " + id);
        GameManager.Instance.Player.Messages[0].AddByStoryId(id, MessageField.Mail); //���丮 ID�� ���� �޽��� ����
        //�ϱ��� �߰�
    }


    private void SetStroyDic(string id, PandaStoryController pandaStoryController)
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
        bool checkPriorStoryID = _storyCompleteList.Contains(panda.StoryDialogue.PriorStoryID) || panda.StoryDialogue.PriorStoryID == "MS99Z";

        if (checkClear && checkPriorStoryID)
        {
            panda.gameObject.SetActive(true);
            return;
        }

        panda.gameObject.SetActive(false);
    }

}
