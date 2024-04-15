using System.Collections.Generic;
using UnityEngine;
using System;


public class StoryManager : SingletonHandler<StoryManager>
{
    private List<string> _storyCompletedList = new List<string>();
    public List<string> StoryCompletedList => _storyCompletedList;

    private Dictionary<string, PandaStoryController> _pandaStoryControllerDic = new Dictionary<string, PandaStoryController>();

    public event Action OnAddCompletedStoryHandler;

    public override void Awake()
    {
        base.Awake();
        Init();
    }


    private void Init()
    {
        _storyCompletedList.Clear();
        //_storyCompletedList = DatabaseManager.Instance.UserInfo.StoryCompletedList;

        UIDialogue.OnComplateHandler = AddComplatedStory;
        PandaStoryController.OnStartHandler = SetStroyDic;
        PandaStoryController.OnCheckActivateHandler = CheckStoryActivated;

    }


    public void SetStoryCompletedList(List<string> storyCompletedList)
    {
        _storyCompletedList = storyCompletedList;
    }


    public List<string> GetStoryCompletedList()
    {
        return _storyCompletedList;
    }


    /// <summary> 스토리 완료 목록에 해당 stroyId가 존재하면 true, 아니면 false를 반환하는 함수 </summary>
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
            Debug.Log("이미 있는 인덱스 입니다.");
            return;
        }

        _storyCompletedList.Add(id);
        CheckStoryActivateds();

        //메시지 전송
        Debug.Log("스토리 끝 메시지 전송, 일기장 추가 : " + id);
        GameManager.Instance.Player.GetMailList(Player.MailType.Mail).AddByStoryId(id, MessageField.Mail); //스토리 ID에 따른 메시지 전송
        //일기장 추가
        DatabaseManager.Instance.AlbumDatabase.SetReceiveAlbumById(id);
        DatabaseManager.Instance.Challenges.MainStoryDone(id);

        OnAddCompletedStoryHandler?.Invoke();

        DatabaseManager.Instance.UserInfo.StoryUserData.AsyncSaveStoryData(10);
    }


    private void SetStroyDic(string id, PandaStoryController pandaStoryController)
    {
        if (_pandaStoryControllerDic.ContainsKey(id))
        {
            Debug.Log("이미 딕셔너리에 존재합니다.");
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
                Debug.LogError("판다 데이터가 존재하지 않습니다.");
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
