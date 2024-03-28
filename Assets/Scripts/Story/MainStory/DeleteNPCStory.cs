using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNPCStory : MonoBehaviour
{
    private MainStoryController _mainStoryController;
    [SerializeField] private string _storyId;

    SpriteRenderer sr;
    private void Awake()
    {
        _mainStoryController = gameObject.transform.GetComponent<MainStoryController>();

        MainStoryController.OnFinishStoryHandler += CheckDelete;
    }

    private void OnDestroy()
    {
        MainStoryController.OnFinishStoryHandler -= CheckDelete;
    }

    private void CheckDelete()
    {
        // ���� ���丮�� �Ǵ� ���� ��
        if (_mainStoryController.NextStory.Contains(_storyId))
        {
            gameObject.SetActive(true);

        }
        // ���丮 ���� ��
        else if (DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyId))
        {
            gameObject.SetActive(false);

        }
    }
}
