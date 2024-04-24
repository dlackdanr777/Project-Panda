using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstLoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadLoginScene", 0.5f);
    }


    private void LoadLoginScene()
    {
        TmpNoticeManagement notice = new TmpNoticeManagement();
        if (notice.TmpNoticeCheck(() => LoadingSceneManager.LoadScene("LoginScene")))
            return;

        LoadingSceneManager.LoadScene("LoginScene");
    }

/*    public InputField input;

    public void GetGoogleHash()
    {
        string googleHashKey = Backend.Utils.GetGoogleHash();

        if (!string.IsNullOrEmpty(googleHashKey))
        {
            Debug.Log(googleHashKey);
            if (input != null)
                input.text = googleHashKey;

        }
    }*/
}
