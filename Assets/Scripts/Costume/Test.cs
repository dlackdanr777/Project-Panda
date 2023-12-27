using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    void Awake()
    {
        DataBind.SetButtonValue("CostumeButton", OnCostumeButtonClicked);
    }

    private void OnCostumeButtonClicked()
    {
        SceneManager.LoadScene("CostumeTest");
    }
}
