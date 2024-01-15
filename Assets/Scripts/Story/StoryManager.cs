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
        Debug.Log("스토리 매니저 실행");
        UIDialogue.OnComplateHandler += AddComplateStory;
        PandaStoryController.OnStartHandler += SetStroyDic;
        PandaStoryController.OnCheckActivateHandler += CheckStoryActivate;
    }


    private void AddComplateStory(string id)
    {
        if (_storyCompleteList.Contains(id))
        {
            Debug.Log("이미 있는 인덱스 입니다.");
            return;
        }

        _storyCompleteList.Add(id);
        CheckStoryActivates();

        //메시지 전송
        Debug.Log("스토리 끝 메시지 전송 : " + id);
        GameManager.Instance.Player.Messages[0].AddByStoryId(id, MessageField.Mail); //스토리 ID에 따른 메시지 전송
        //일기장 추가
    }


    private void SetStroyDic(string id, PandaStoryController pandaStoryController)
    {

        if (_pandaStoryControllerDic.ContainsKey(id))
        {
            Debug.LogError("이미 딕셔너리에 존재합니다.");
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
