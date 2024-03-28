using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNPCStory : MonoBehaviour
{
    private MainStoryController _mainStoryController;
    [SerializeField] private string _storyId;
    [Tooltip("게임 처음 시작할 때 보이면 true")]
    [SerializeField] private bool _setActive;

    private void Awake()
    {
        MainStoryController.OnFinishStoryHandler += CheckDelete;
        _mainStoryController = gameObject.transform.GetComponent<MainStoryController>();
        gameObject.SetActive(_setActive);
    }

    private void OnDestroy()
    {
        MainStoryController.OnFinishStoryHandler -= CheckDelete;
    }

    private void CheckDelete()
    {
        // 다음 스토리에 판다 나올 때
        if (_mainStoryController.NextStory.Contains(_storyId))
        {
            gameObject.SetActive(true);

        }
        // 스토리 종료 후
        else if (DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyId))
        {
            gameObject.SetActive(false);

        }
    }
}
