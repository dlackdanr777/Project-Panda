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
        // 다음 스토리에 쓰러진 판다 나올 때
        if (_mainStoryController.NextStory.Contains("MS01C"))
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            sr.enabled = true;

        }
        // 스토리 종료 후
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
