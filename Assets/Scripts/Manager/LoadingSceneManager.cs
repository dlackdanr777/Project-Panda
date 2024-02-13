using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadingSceneManager : MonoBehaviour
{


    [Tooltip("로딩이 최소 몇 초가 걸리게 할지 설정")]
    [SerializeField] private float _changeSceneTime;

    private static string _nextScene;

    private static LoadingType _loadingType;


    private void Start()
    {
        StartCoroutine(LoadScene());
    }


    public static void LoadScene(string sceneName, LoadingType type = LoadingType.SceneChange)
    {
        _nextScene = sceneName;
        _loadingType = type;
        FadeInOutManager.Instance.FadeIn( onComplete: () => SceneManager.LoadScene("LoadingScene") );
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

                    switch (_loadingType)
                    {
                        case LoadingType.FirstLoading:
                            FadeInOutManager.Instance.FirstFadeInOut();
                            break;

                        case LoadingType.SceneChange:
                            FadeInOutManager.Instance.FadeOut();
                            break;
                    }
                    
                    yield break;
                }
            }
        }

        
    }
}
