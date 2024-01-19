using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnCostumeButtonClicked);
        //DataBind.SetButtonValue("CostumeButton", OnCostumeButtonClicked);
    }

    private void OnCostumeButtonClicked()
    {
        if (DatabaseManager.Instance.StartPandaInfo.StarterPanda.SetFalseUI())
        {
            SceneManager.LoadScene("CostumeTest");
        }
    }
}
