using System.Collections.Generic;
using UnityEngine;


public class StoryManager : SingletonHandler<StoryManager>
{
    public List<int> _storyCompleteList { get; private set; }

    private Dictionary<int, PandaStoryController> _pandaStoryControllerDic = new Dictionary<int, PandaStoryController>();


    public override void Awake()
    {
        Init();

    }


    private void Init()
    {
        _storyCompleteList = new List<int>();
        UIDialogue.OnComplateHandler += AddComplateStory;
        PandaStoryController.OnStartHandler += SetStroyDic;
    }


    private void Start()
    {
        CheckStoryActivate();
    }


    private void AddComplateStory(int id)
    {
        if (_storyCompleteList.Contains(id))
        {
            Debug.Log("�̹� �ִ� �ε��� �Դϴ�.");
            return;
        }
            
        _storyCompleteList.Add(id);
        CheckStoryActivate();
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


    private void CheckStoryActivate()
    {
        foreach(PandaStoryController panda in  _pandaStoryControllerDic.Values)
        {
            if (panda == null)
            {
                Debug.Log(panda.name);
                continue;
            }

            //���� panda���丮 ��ũ��Ʈ�� ��ϵ� storyID�� Ŭ������� �ʾҰų�, ���� ���丮�� ����������� ������Ʈ�� Ų��.
            bool checkClear = !_storyCompleteList.Contains(panda.StoryDialogue.StoryID);
            bool checkPriorStoryID = _storyCompleteList.Contains(panda.StoryDialogue.PriorStoryID) || panda.StoryDialogue.PriorStoryID == 9999;

            if (checkClear && checkPriorStoryID)
            {
                panda.gameObject.SetActive(true);
                continue;
            }   

            panda.gameObject.SetActive(false);
        }
    }

}
