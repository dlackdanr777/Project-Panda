using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;
using BT;

public class Test : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnCostumeButtonClicked);
        //DataBind.SetButtonValue("CostumeButton", OnCostumeButtonClicked);
    }

    private void OnCostumeButtonClicked()
    {
        if (StarterPanda.Instance.SetFalseUI())
        {
            SceneManager.LoadScene("CostumeTest");
        }
    }
}
