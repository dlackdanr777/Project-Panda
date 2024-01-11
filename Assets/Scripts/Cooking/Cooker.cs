using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{

    private void Awake()
    {
        DataBind.SetButtonValue("ShowCookButton", OnCookButtonClicked);
    }

    private void OnCookButtonClicked()
    {
        LoadingSceneManager.LoadScene("CookingScene");
    }
}
