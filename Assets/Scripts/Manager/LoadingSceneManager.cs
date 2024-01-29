using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadingSceneManager : MonoBehaviour
{


    [Tooltip("�ε��� �ּ� �� �ʰ� �ɸ��� ���� ����")]
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


                    switch (_loadingType)
                    {
                        case LoadingType.FirstLoading:
                            ChangeSceneManager.Instance.FirstLoading();
                            break;

                        case LoadingType.SceneChange:
                            ChangeSceneManager.Instance.ResetFadeImage();
                            break;
                    }
                    
                    yield break;
                }
            }
        }

        
    }
}
