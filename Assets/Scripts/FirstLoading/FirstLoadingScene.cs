using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadLoginScene", 1);
    }


    private void LoadLoginScene()
    {
        LoadingSceneManager.LoadScene("LoginScene");
    }
}
