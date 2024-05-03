using UnityEngine;

public class FirstStartStory : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MainStoryController _stroyController;


    private void Awake()
    {
        FadeInOutManager.Instance.OnEndFadeOutHandler += FadeOutEvent;
        LoadingSceneManager.OnLoadSceneHandler += ChangeSceneEvent;
    }


    private void FadeOutEvent()
    {
        if (!_stroyController.gameObject.activeSelf)
            return;

        Debug.Log("½ÇÇà");
        _stroyController.OnClickStartButton();
    }

    private void ChangeSceneEvent()
    {
        FadeInOutManager.Instance.OnEndFadeOutHandler -= FadeOutEvent;
        LoadingSceneManager.OnLoadSceneHandler -= ChangeSceneEvent;
    }

}
