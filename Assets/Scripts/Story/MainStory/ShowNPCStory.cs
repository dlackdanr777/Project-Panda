using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ShowNPCStory : MonoBehaviour
{
    private MainStoryController _mainStoryController;
    [SerializeField] private Sprite _image;
    [SerializeField] private string _storyId;
    [Tooltip("게임 처음 시작할 때 보이면 true")]
    [SerializeField] private bool _setActive;

    SpriteRenderer sr;

    private void Awake()
    {
        MainStoryController.OnFinishStoryHandler += CheckShow;

    }
    private void Start()
    {
        _mainStoryController = gameObject.transform.GetComponent<MainStoryController>();
        //gameObject.SetActive(_setActive);
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = _setActive;
        CheckShow();

    }

    private void OnDestroy()
    {
        MainStoryController.OnFinishStoryHandler -= CheckShow;
    }

    private void CheckShow()
    {
        // 다음 스토리에 판다 나올 때
        if (MainStoryController.NextStory.Contains(_storyId))
        {
            sr.enabled = true;

        }
        // 스토리 종료 후
        else if (DatabaseManager.Instance.MainDialogueDatabase.StoryCompletedList.Contains(_storyId))
        {
            sr.enabled = true;
            //gameObject.SetActive(true);
            sr.sprite = _image;
            gameObject.GetComponent<Animator>().enabled = true;
            this.enabled = false;

        }
    }
}
