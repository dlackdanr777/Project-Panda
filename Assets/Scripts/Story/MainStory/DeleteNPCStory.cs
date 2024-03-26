using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteNPCStory : MonoBehaviour
{
    private MainStoryController _mainStoryController;
    [SerializeField] private string _storyId;

    SpriteRenderer sr;
    void Start()
    {
        _mainStoryController = gameObject.transform.GetComponent<MainStoryController>();
    }

    void Update()
    {
        // ���� ���丮�� �Ǵ� ���� ��
        if (_mainStoryController.NextStory.Contains(_storyId))
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            sr.enabled = true;

        }
        // ���丮 ���� ��
        else if (DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyId))
        {
            gameObject.SetActive(false);

        }

    }
}
