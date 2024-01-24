using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string _nextScene;

    [Tooltip("로딩이 최소 몇 초가 걸리게 할지 설정")]
    [SerializeField] private float _changeSceneTime;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }


    public static void LoadScene(string sceneName)
    {

        _nextScene = sceneName;
        ChangeSceneManager.Instance.ChangeScene(() => SceneManager.LoadScene("LoadingScene"));
    }


    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);

        op.allowSceneActivation = false;
        float timer = 0f;
        
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if(0.9f <= op.progress)
            {
                if(_changeSceneTime < timer)
                {
                    op.allowSceneActivation = true;
                    ChangeSceneManager.Instance.ResetFadeImage();
                    yield break;
                }
            }
        }

        
    }
}
