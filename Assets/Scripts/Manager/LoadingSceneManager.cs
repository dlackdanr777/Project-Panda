using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string _nextScene;

    [SerializeField] private Text _progressText;

    void Start()
    {
        StartCoroutine(LoadScene());
    }


    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        ChangeSceneManager.Instance.ChangeScene(1, () => SceneManager.LoadScene("LoadingScene"));
    }


    private IEnumerator LoadScene()
    {
        ChangeSceneManager.Instance.ResetFadeIamge();
        yield return null;
        AsyncOperation op;
        op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;
        float timer = 0f;

        while(!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if(op.progress < 0.9f)
            {
                _progressText.text = Mathf.RoundToInt(op.progress * 100).ToString();
            }
            else
            {
                _progressText.text = Mathf.RoundToInt(Mathf.Lerp(int.Parse(_progressText.text), 100, timer * 0.5f)).ToString();

                if(timer > 2)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
