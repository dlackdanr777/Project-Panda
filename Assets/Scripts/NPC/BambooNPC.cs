using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooNPC : MonoBehaviour
{
    private int _currentHour;

    private void Start()
    {
        _currentHour = TimeManager.Instance.GameHour;
        UIMainDialogue.OnFinishStoryHandler += CheckCompletedStory;
        MapButton.OnMoveMapHandler += ShowNPC;
        LoadingSceneManager.OnLoadSceneHandler += ActionDisabled;
        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        ActionDisabled();
    }


    private void ShowNPC()
    {
        if( _currentHour != TimeManager.Instance.GameHour)
        {
            _currentHour = TimeManager.Instance.GameHour;
            gameObject.SetActive(true);
        }
    }

    // ��ȭ ���� �� �����
    private void CheckCompletedStory(string id)
    {
        if(DatabaseManager.Instance.MainDialogueDatabase.MSDic[id].StoryStartPanda == gameObject.name)
        {
            gameObject.SetActive(false);
        }
    }


    private void ActionDisabled()
    {
        UIMainDialogue.OnFinishStoryHandler -= CheckCompletedStory;
        MapButton.OnMoveMapHandler -= ShowNPC;
        LoadingSceneManager.OnLoadSceneHandler -= ActionDisabled;
    }

}