using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC02Story : MonoBehaviour
{
    private MainStoryController _mainStoryController;
    [SerializeField] private Sprite _image;

    SpriteRenderer sr;
    void Start()
    {
        _mainStoryController = gameObject.transform.GetComponent<MainStoryController>();   
    }

    void Update()
    {
        // ���� ���丮�� ������ �Ǵ� ���� ��
        if (_mainStoryController.NextStory.Contains("MS01C"))
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            sr.enabled = true;

        }
        // ���丮 ���� ��
        else if (DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains("MS01C"))
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            sr.enabled = true;
            sr.sprite = _image;
            this.enabled = false;
            gameObject.GetComponent<Animator>().enabled = true;

        }

    }
}
