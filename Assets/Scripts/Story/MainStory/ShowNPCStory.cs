using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNPCStory : MonoBehaviour
{
    private MainStoryController _mainStoryController;
    [SerializeField] private Sprite _image;
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
            sr = gameObject.GetComponent<SpriteRenderer>();
            sr.enabled = true;
            sr.sprite = _image;
            this.enabled = false;
            gameObject.GetComponent<Animator>().enabled = true;

        }

    }
}
