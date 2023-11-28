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
            Debug.Log("이미 있는 인덱스 입니다.");
            return;
        }
            
        _storyCompleteList.Add(id);
        CheckStoryActivate();
    }


    private void SetStroyDic(int id, PandaStoryController pandaStoryController)
    {

        if (_pandaStoryControllerDic.ContainsKey(id))
        {
            Debug.LogError("이미 딕셔너리에 존재합니다.");
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

            //현재 panda스토리 스크립트의 등록된 storyID가 클리어되지 않았거나, 선행 스토리를 진행했을경우 오브젝트를 킨다.
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
